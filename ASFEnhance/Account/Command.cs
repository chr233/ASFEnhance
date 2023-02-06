using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Data;
using System.Text;

namespace ASFEnhance.Account
{
    internal static class Command
    {
        /// <summary>
        /// 读取账号消费历史
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string> ResponseAccountHistory(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            string result = await WebRequest.GetAccountHistoryDetail(bot).ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// 读取账号消费历史 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseAccountHistory(string botNames)
        {
            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
            }

            IList<string> results = await Utilities.InParallel(bots.Select(bot => ResponseAccountHistory(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 读取账号许可证列表
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseGetAccountLicenses(Bot bot, bool onlyFreelicense)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            var result = await WebRequest.GetOwnedLicenses(bot).ConfigureAwait(false);

            if (result == null)
            {
                return bot.FormatBotResponse(Langs.NetworkError);
            }

            StringBuilder sb = new();
            sb.AppendLine(bot.FormatBotResponse(Langs.MultipleLineResult));

            if (onlyFreelicense)
            {
                sb.AppendLine(Langs.AccountFreeSubTitle);
                foreach (var item in result.Where(x => x.PackageId != 0 && x.Type == LicenseType.Complimentary))
                {
                    sb.AppendLine(string.Format(Langs.AccountSubItem, item.PackageId, item.Name));
                }
            }
            else
            {
                sb.AppendLine(Langs.AccountSubTitle);
                foreach (var item in result)
                {
                    string type = item.Type switch {
                        LicenseType.Retail => Langs.AccountSubRetail,
                        LicenseType.Complimentary => Langs.AccountSubFree,
                        LicenseType.SteamStore => Langs.AccountSubStore,
                        LicenseType.GiftOrGuestPass => Langs.AccountSubGift,
                        _ => Langs.AccountSubUnknown,
                    };
                    sb.AppendLine(string.Format(Langs.AccountSubItem, type, item.Name));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 读取账号许可证列表 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseGetAccountLicenses(string botNames, bool onlyFreelicense)
        {
            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseGetAccountLicenses(bot, onlyFreelicense))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 移除免费许可证
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseRemoveFreeLicenses(Bot bot, string query)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            if (string.IsNullOrEmpty(query))
            {
                return bot.FormatBotResponse(Langs.ArgsIsEmpty);
            }

            var licensesOld = await WebRequest.GetOwnedLicenses(bot).ConfigureAwait(false);

            if (licensesOld == null)
            {
                return bot.FormatBotResponse(Langs.NetworkError);
            }

            var oldSubs = licensesOld.Where(x => x.PackageId > 0 && x.Type == LicenseType.Complimentary).ToDictionary(x => x.PackageId, x => x.Name);
            var gameIds = FetchGameIds(query, SteamGameIdType.Sub, SteamGameIdType.Sub);

            SemaphoreSlim sema = new(3, 3);

            async Task workThread(uint subId)
            {
                try
                {
                    sema.Wait();
                    try
                    {
                        await WebRequest.RemoveLicense(bot, subId).ConfigureAwait(false);
                        await Task.Delay(500).ConfigureAwait(false);
                    }
                    finally
                    {
                        sema.Release();
                    }
                }
                catch (Exception ex)
                {
                    ASFLogger.LogGenericException(ex);
                }
            }

            var subIds = gameIds.Where(x => x.Type == SteamGameIdType.Sub).Select(x => x.GameId);
            var tasks = subIds.Where(x => oldSubs.ContainsKey(x)).Select(x => workThread(x));
            if (tasks.Any())
            {
                await Utilities.InParallel(gameIds.Select(x => WebRequest.RemoveLicense(bot, x.GameId))).ConfigureAwait(false);
                await Task.Delay(1000).ConfigureAwait(false);
            }

            var licensesNew = await WebRequest.GetOwnedLicenses(bot).ConfigureAwait(false);

            if (licensesNew == null)
            {
                return bot.FormatBotResponse(Langs.NetworkError);
            }

            var newSubs = licensesNew.Where(x => x.PackageId > 0 && x.Type == LicenseType.Complimentary).Select(x => x.PackageId).ToHashSet();

            StringBuilder sb = new();
            sb.AppendLine(bot.FormatBotResponse(Langs.MultipleLineResult));

            foreach (var gameId in gameIds)
            {
                string msg;
                if (gameId.Type == SteamGameIdType.Error)
                {
                    msg = Langs.AccountSubInvalidArg;
                }
                else
                {
                    uint subId = gameId.GameId;
                    if (oldSubs.TryGetValue(subId, out var name))
                    {
                        bool succ = !newSubs.Contains(subId);
                        msg = string.Format(Langs.AccountSubRemovedItem, name, succ ? Langs.Success : Langs.Failure);
                    }
                    else
                    {
                        msg = Langs.AccountSubNotOwn;
                    }
                }
                sb.AppendLine(bot.FormatBotResponse(string.Format(Langs.CookieItem, gameId.Input, msg)));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 移除免费许可证 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseRemoveFreeLicenses(string botNames, string query)
        {
            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseRemoveFreeLicenses(bot, query))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 移除所有Demo
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseRemoveAllDemos(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            var licensesOld = await WebRequest.GetOwnedLicenses(bot).ConfigureAwait(false);

            if (licensesOld == null)
            {
                return bot.FormatBotResponse(Langs.NetworkError);
            }

            var oldSubs = licensesOld.Where(x => x.PackageId > 0 && x.Type == LicenseType.Complimentary && x.Name.EndsWith("Demo")).Select(x => x.PackageId).ToHashSet();

            if (oldSubs.Count == 0)
            {
                return bot.FormatBotResponse(Langs.AccountSubDemoSubNotFount);
            }

            SemaphoreSlim sema = new(3, 3);

            async Task workThread(uint subId)
            {
                try
                {
                    sema.Wait();
                    try
                    {
                        await WebRequest.RemoveLicense(bot, subId).ConfigureAwait(false);
                        await Task.Delay(500).ConfigureAwait(false);
                    }
                    finally
                    {
                        sema.Release();
                    }
                }
                catch (Exception ex)
                {
                    ASFLogger.LogGenericException(ex);
                }
            }

            var tasks = oldSubs.Select(x => workThread(x));
            await Utilities.InParallel(tasks).ConfigureAwait(false);

            await Task.Delay(1000).ConfigureAwait(false);

            var licensesNew = await WebRequest.GetOwnedLicenses(bot).ConfigureAwait(false);

            if (licensesNew == null)
            {
                return bot.FormatBotResponse(Langs.NetworkError);
            }

            var newSubs = licensesNew.Where(x => x.PackageId > 0 && x.Type == LicenseType.Complimentary && x.Name.EndsWith("Demo")).Select(x => x.PackageId).ToHashSet();
            var count = oldSubs.Where(x => !newSubs.Contains(x)).Count();

            return bot.FormatBotResponse(string.Format(Langs.AccountSubRemovedDemos, count));
        }

        /// <summary>
        /// 移除所有Demo (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseRemoveAllDemos(string botNames)
        {
            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseRemoveAllDemos(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 获取邮箱偏好
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseGetEmailOptions(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            var result = await WebRequest.GetAccountEmailOptions(bot).ConfigureAwait(false);

            if (result == null)
            {
                return bot.FormatBotResponse(Langs.NetworkError);
            }

            StringBuilder sb = new();
            sb.AppendLine(bot.FormatBotResponse(Langs.MultipleLineResult));

            sb.AppendLine(string.Format(Langs.CookieItem, "启用邮件通知", result.EnableEmailNotification ? Langs.Yes : Langs.No));
            if (result.EnableEmailNotification)
            {
                sb.AppendLine(string.Format(Langs.StoreItemHeader, "愿望单上的一项物品享有折扣时", result.WhenWishlistDiscount ? Langs.Yes : Langs.No));
                sb.AppendLine(string.Format(Langs.StoreItemHeader, "愿望单上的一件未发行物品发行时", result.WhenWishlistRelease ? Langs.Yes : Langs.No));
                sb.AppendLine(string.Format(Langs.StoreItemHeader, "关注或收藏的青睐之光提交项目发行时", result.WhenGreenLightRelease ? Langs.Yes : Langs.No));
                sb.AppendLine(string.Format(Langs.StoreItemHeader, "关注的发行商或开发者发行了新产品时", result.WhenFollowPublisherRelease ? Langs.Yes : Langs.No));
                sb.AppendLine(string.Format(Langs.StoreItemHeader, "季节性的促销特惠开始时", result.WhenSaleEvent ? Langs.Yes : Langs.No));
                sb.AppendLine(string.Format(Langs.StoreItemHeader, "收到鉴赏家副本时", result.WhenReceiveCuratorReview ? Langs.Yes : Langs.No));
                sb.AppendLine(string.Format(Langs.StoreItemHeader, "收到社区奖励时", result.WhenReceiveCommunityReward ? Langs.Yes : Langs.No));
                sb.AppendLine(string.Format(Langs.StoreItemHeader, "收到游戏活动通知时", result.WhenGameEventNotification ? Langs.Yes : Langs.No));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取邮箱偏好 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseGetEmailOptions(string botNames)
        {
            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseGetEmailOptions(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 获取邮箱偏好
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseSetEmailOptions(Bot bot, string query)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            string[] entries = query.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            EmailOptions payload = new();

            int i = 0;

            List<string> yesStrings = new() { "1", "y", "yes", "true" };

            foreach (string entry in entries)
            {
                bool enable = yesStrings.Contains(entry.Trim().ToLowerInvariant());

                switch (i++)
                {
                    case 0:
                        payload.EnableEmailNotification = enable;
                        break;
                    case 1:
                        payload.WhenWishlistDiscount = enable;
                        break;
                    case 2:
                        payload.WhenWishlistRelease = enable;
                        break;
                    case 3:
                        payload.WhenGreenLightRelease = enable;
                        break;
                    case 4:
                        payload.WhenFollowPublisherRelease = enable;
                        break;
                    case 5:
                        payload.WhenSaleEvent = enable;
                        break;
                    case 6:
                        payload.WhenReceiveCuratorReview = enable;
                        break;
                    case 7:
                        payload.WhenReceiveCommunityReward = enable;
                        break;
                    case 8:
                        payload.WhenGameEventNotification = enable;
                        break;
                    default:
                        break;
                }
            }

            var result = await WebRequest.SetAccountEmailOptions(bot, payload).ConfigureAwait(false);

            if (result == null)
            {
                return bot.FormatBotResponse(Langs.NetworkError);
            }

            StringBuilder sb = new();
            sb.AppendLine(bot.FormatBotResponse(Langs.MultipleLineResult));

            sb.AppendLine(string.Format(Langs.CookieItem, Langs.EnableEmailNotification, result.EnableEmailNotification ? Langs.Yes : Langs.No));
            if (result.EnableEmailNotification)
            {
                sb.AppendLine(string.Format(Langs.StoreItemHeader, Langs.WhenWishlistDiscount, result.WhenWishlistDiscount ? Langs.Yes : Langs.No));
                sb.AppendLine(string.Format(Langs.StoreItemHeader, Langs.WhenWishlistRelease, result.WhenWishlistRelease ? Langs.Yes : Langs.No));
                sb.AppendLine(string.Format(Langs.StoreItemHeader, Langs.WhenGreenLightRelease, result.WhenGreenLightRelease ? Langs.Yes : Langs.No));
                sb.AppendLine(string.Format(Langs.StoreItemHeader, Langs.WhenFollowPublisherRelease, result.WhenFollowPublisherRelease ? Langs.Yes : Langs.No));
                sb.AppendLine(string.Format(Langs.StoreItemHeader, Langs.WhenSaleEvent, result.WhenSaleEvent ? Langs.Yes : Langs.No));
                sb.AppendLine(string.Format(Langs.StoreItemHeader, Langs.WhenReceiveCuratorReview, result.WhenReceiveCuratorReview ? Langs.Yes : Langs.No));
                sb.AppendLine(string.Format(Langs.StoreItemHeader, Langs.WhenReceiveCommunityReward, result.WhenReceiveCommunityReward ? Langs.Yes : Langs.No));
                sb.AppendLine(string.Format(Langs.StoreItemHeader, Langs.WhenGameEventNotification, result.WhenGameEventNotification ? Langs.Yes : Langs.No));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取邮箱偏好 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseSetEmailOptions(string botNames, string query)
        {
            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseSetEmailOptions(bot, query))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }
    }
}
