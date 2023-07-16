using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Data;
using SteamKit2;
using System.Text;

namespace ASFEnhance.Account;

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
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseAccountHistory(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

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
    /// <param name="onlyFreelicense"></param>
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

        var sb = new StringBuilder();
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
                string type = item.Type switch
                {
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
    /// <param name="onlyFreelicense"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetAccountLicenses(string botNames, bool onlyFreelicense)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

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
    /// <param name="query"></param>
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

        var sema = new SemaphoreSlim(3, 3);

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

        var sb = new StringBuilder();
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

        var bots = Bot.GetBots(botNames);

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

        var sema = new SemaphoreSlim(3, 3);

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

        var bots = Bot.GetBots(botNames);

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

        var sb = new StringBuilder();
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

        var bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
        }

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseGetEmailOptions(bot))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 设置邮箱偏好
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseSetEmailOptions(Bot bot, string query)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        string[] entries = query.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        var payload = new EmailOptions();

        int i = 0;

        var yesStrings = new List<string> { "1", "y", "yes", "true" };

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

        var sb = new StringBuilder();
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
    /// 设置邮箱偏好 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseSetEmailOptions(string botNames, string query)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
        }

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseSetEmailOptions(bot, query))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    private static string NotificationTargetToString(NotificationTarget target)
    {
        return target switch
        {
            NotificationTarget.On => Langs.Yes,
            NotificationTarget.OnAndSteamClient => Langs.Yes + ", 客户端通知",
            NotificationTarget.OnAndMobileApp => Langs.Yes + ", 手机应用通知",
            NotificationTarget.All => Langs.Yes + ", 客户端通知, 手机应用通知",
            _ => Langs.No,
        };
    }

    /// <summary>
    /// 获取通知偏好
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetNotificationOptions(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var result = await WebRequest.GetAccountNotificationOptions(bot).ConfigureAwait(false);

        if (result == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        var sb = new StringBuilder();
        sb.AppendLine(bot.FormatBotResponse(Langs.MultipleLineResult));
        sb.AppendLine("当前通知设定:");
        sb.AppendLine(string.Format(Langs.StoreItemHeader, "我收到了礼物", NotificationTargetToString(result.ReceivedGift)));
        sb.AppendLine(string.Format(Langs.StoreItemHeader, "我订阅的讨论区有回复", NotificationTargetToString(result.SubscribedDissionReplyed)));
        sb.AppendLine(string.Format(Langs.StoreItemHeader, "我库存中收到了新物品", NotificationTargetToString(result.ReceivedNewItem)));
        sb.AppendLine(string.Format(Langs.StoreItemHeader, "我收到了好友邀请", NotificationTargetToString(result.ReceivedFriendInvitation)));
        sb.AppendLine(string.Format(Langs.StoreItemHeader, "有大型特卖", NotificationTargetToString(result.MajorSaleStart)));
        sb.AppendLine(string.Format(Langs.StoreItemHeader, "愿望单中的某件物品有折扣", NotificationTargetToString(result.ItemInWishlistOnSale)));
        sb.AppendLine(string.Format(Langs.StoreItemHeader, "我收到了一个新的交易报价", NotificationTargetToString(result.ReceivedTradeOffer)));
        sb.AppendLine(string.Format(Langs.StoreItemHeader, "我收到了 Steam 客服的回复", NotificationTargetToString(result.ReceivedSteamSupportReply)));
        sb.AppendLine(string.Format(Langs.StoreItemHeader, "我收到了 Steam 回合通知", NotificationTargetToString(result.SteamTurnNotification)));

        return sb.ToString();
    }

    /// <summary>
    /// 获取通知偏好 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetNotificationOptions(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseGetNotificationOptions(bot))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 设置通知偏好
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseSetNotificationOptions(Bot bot, string query)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        string[] entries = query.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        var payload = new NotificationOptions();

        int i = 0;

        foreach (string entry in entries)
        {
            if (!int.TryParse(entry, out int value))
            {
                value = 0;
            }

            var option = value switch
            {
                1 => NotificationTarget.On,
                2 => NotificationTarget.OnAndSteamClient,
                3 => NotificationTarget.OnAndMobileApp,
                4 => NotificationTarget.All,
                _ => NotificationTarget.OFF,
            };

            switch (i++)
            {
                case 0:
                    payload.ReceivedGift = option;
                    break;
                case 1:
                    payload.SubscribedDissionReplyed = option;
                    break;
                case 2:
                    payload.ReceivedNewItem = option;
                    break;
                case 3:
                    payload.ReceivedFriendInvitation = option;
                    break;
                case 4:
                    payload.MajorSaleStart = option;
                    break;
                case 5:
                    payload.ItemInWishlistOnSale = option;
                    break;
                case 6:
                    payload.ReceivedTradeOffer = option;
                    break;
                case 7:
                    payload.ReceivedSteamSupportReply = option;
                    break;
                case 8:
                    payload.SteamTurnNotification = option;
                    break;
                default:
                    break;
            }
        }

        var result = await WebRequest.SetAccountNotificationOptions(bot, payload).ConfigureAwait(false);

        if (result == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }
        else
        {
            return bot.FormatBotResponse(result.Result == EResult.OK ? Langs.Success : Langs.Failure);
        }
    }

    /// <summary>
    /// 设置通知偏好 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseSetNotificationOptions(string botNames, string query)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseSetNotificationOptions(bot, query))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }
}
