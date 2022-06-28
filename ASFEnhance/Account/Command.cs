#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Data;
using ASFEnhance.Localization;
using System.Text;
using static ASFEnhance.Utils;

namespace ASFEnhance.Account
{
    internal static class Command
    {
        /// <summary>
        /// 读取账号消费历史
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseAccountHistory(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            string? result = await WebRequest.GetAccountHistoryDetail(bot).ConfigureAwait(false);

            return result != null ? bot.FormatBotResponse(result) : null;
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseAccountHistory(bot))).ConfigureAwait(false);

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

            StringBuilder sb = new();
            sb.AppendLine(bot.FormatBotResponse(Langs.MultipleLineResult));

            if (onlyFreelicense)
            {
                sb.AppendLine(Langs.AccountFreeSubTitle);
                foreach (var item in result.Where(x => x.PackageID != 0 && x.Type == LicenseType.Complimentary))
                {
                    sb.AppendLine(string.Format(Langs.AccountSubItem, item.PackageID, item.Name));
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
            var oldSubs = licensesOld.Where(x => x.PackageID > 0 && x.Type == LicenseType.Complimentary).ToDictionary(x => x.PackageID, x => x.Name);
            var gameIDs = FetchGameIDs(query, SteamGameIDType.Sub, SteamGameIDType.Sub);

            SemaphoreSlim sema = new(3, 3);

            async Task workThread(uint subID)
            {
                try
                {
                    sema.Wait();
                    try
                    {
                        await WebRequest.RemoveLicense(bot, subID).ConfigureAwait(false);
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

            var subIDs = gameIDs.Where(x => x.Type == SteamGameIDType.Sub).Select(x => x.GameID);
            var tasks = subIDs.Where(x => oldSubs.ContainsKey(x)).Select(x => workThread(x));
            if (tasks.Any())
            {
                await Utilities.InParallel(gameIDs.Select(x => WebRequest.RemoveLicense(bot, x.GameID))).ConfigureAwait(false);
                await Task.Delay(1000).ConfigureAwait(false);
            }

            var licensesNew = await WebRequest.GetOwnedLicenses(bot).ConfigureAwait(false);
            var newSubs = licensesNew.Where(x => x.PackageID > 0 && x.Type == LicenseType.Complimentary).Select(x => x.PackageID).ToHashSet();

            StringBuilder sb = new();

            foreach (var gameID in gameIDs)
            {
                string msg;
                if (gameID.Type == SteamGameIDType.Error)
                {
                    msg = Langs.AccountSubInvalidArg;
                }
                else
                {
                    uint subID = gameID.GameID;
                    if (oldSubs.TryGetValue(subID, out var name))
                    {
                        bool succ = !newSubs.Contains(subID);
                        msg = string.Format(Langs.AccountSubRemovedItem, name, succ ? Langs.Success : Langs.Failure);
                    }
                    else
                    {
                        msg = Langs.AccountSubNotOwn;
                    }
                }
                sb.AppendLine(bot.FormatBotResponse(string.Format(Langs.CookieItem, gameID.Input, msg)));
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
            var oldSubs = licensesOld.Where(x => x.PackageID > 0 && x.Type == LicenseType.Complimentary && x.Name.EndsWith("Demo")).Select(x => x.PackageID).ToHashSet();

            if (oldSubs.Count == 0)
            {
                return bot.FormatBotResponse(Langs.AccountSubDemoSubNotFount);
            }

            SemaphoreSlim sema = new(3, 3);

            async Task workThread(uint subID)
            {
                try
                {
                    sema.Wait();
                    try
                    {
                        await WebRequest.RemoveLicense(bot, subID).ConfigureAwait(false);
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
            var newSubs = licensesNew.Where(x => x.PackageID > 0 && x.Type == LicenseType.Complimentary && x.Name.EndsWith("Demo")).Select(x => x.PackageID).ToHashSet();
            var count = oldSubs.Where(x => !newSubs.Contains(x)).Count();

            return bot.FormatBotResponse(string.Format(Langs.AccountSubRemovedDemos, count));
        }

        /// <summary>
        /// 移除所有Demo (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="query"></param>
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
    }
}
