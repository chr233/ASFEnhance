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
            NotificationTarget.OnAndSteamClient => Langs.Yes + Langs.NTSteamClient,
            NotificationTarget.OnAndMobileApp => Langs.Yes + Langs.NtMobile,
            NotificationTarget.All => Langs.Yes + Langs.NTAll,
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
        sb.AppendLine(Langs.CurrentNotificationSetting);
        sb.AppendLine(string.Format(Langs.StoreItemHeader, Langs.NooReceivedGift, NotificationTargetToString(result.ReceivedGift)));
        sb.AppendLine(string.Format(Langs.StoreItemHeader, Langs.NooReceivedReply, NotificationTargetToString(result.SubscribedDissionReplyed)));
        sb.AppendLine(string.Format(Langs.StoreItemHeader, Langs.NooNewItem, NotificationTargetToString(result.ReceivedNewItem)));
        sb.AppendLine(string.Format(Langs.StoreItemHeader, Langs.NooFriendInvite, NotificationTargetToString(result.ReceivedFriendInvitation)));
        sb.AppendLine(string.Format(Langs.StoreItemHeader, Langs.NooMajorSale, NotificationTargetToString(result.MajorSaleStart)));
        sb.AppendLine(string.Format(Langs.StoreItemHeader, Langs.NooWishlistOnSale, NotificationTargetToString(result.ItemInWishlistOnSale)));
        sb.AppendLine(string.Format(Langs.StoreItemHeader, Langs.NooNewTradeOffer, NotificationTargetToString(result.ReceivedTradeOffer)));
        sb.AppendLine(string.Format(Langs.StoreItemHeader, Langs.NooSteamSupport, NotificationTargetToString(result.ReceivedSteamSupportReply)));
        sb.AppendLine(string.Format(Langs.StoreItemHeader, Langs.NooSteamTurn, NotificationTargetToString(result.SteamTurnNotification)));

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

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseSetNotificationOptions(bot, query))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 获取账户封禁情况
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="steamId"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetAccountBanned(Bot bot, ulong? steamId)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        (_, string? apiKey) = await bot.ArchiWebHandler.CachedApiKey.GetValue().ConfigureAwait(false);

        if (string.IsNullOrEmpty(apiKey))
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        var result = await WebRequest.GetPlayerBans(bot, apiKey, steamId ?? bot.SteamID).ConfigureAwait(false);

        if (result == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        if (result.Players?.Any() != true)
        {
            return bot.FormatBotResponse(Langs.NoUserFound);
        }

        var sb = new StringBuilder();
        sb.AppendLine(bot.FormatBotResponse(Langs.MultipleLineResult));

        var player = result.Players.First();

        sb.AppendLine(string.Format(Langs.BanSteamId, player.SteamId));
        sb.AppendLine(string.Format(Langs.BanCommunity, Bool2Str(player.CommunityBanned)));
        sb.AppendLine(string.Format(Langs.BanEconomy, player.EconomyBan == "none" ? "×" : player.EconomyBan));
        sb.Append(string.Format(Langs.BanVAC, Bool2Str(player.VACBanned)));
        if (player.VACBanned)
        {
            sb.AppendLine(string.Format(Langs.BanVACCount, player.NumberOfVACBans, player.DaysSinceLastBan));
        }
        else
        {
            sb.AppendLine();
        }
        var gameban = player.NumberOfGameBans > 0;
        sb.Append(string.Format(Langs.BanGame, Bool2Str(gameban)));
        if (gameban)
        {
            sb.AppendLine(string.Format(Langs.BanGameCount, player.NumberOfGameBans));
        }
        else
        {
            sb.AppendLine();
        }

        return sb.ToString();
    }

    /// <summary>
    /// 获取账户封禁情况 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetAccountBanned(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var targetBots = Bot.GetBots(botNames);

        if (targetBots?.Any() != true)
        {
            return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
        }

        var results = await Utilities.InParallel(targetBots.Select(bot => ResponseGetAccountBanned(bot, null))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 获取账户封禁情况 (多个Bot)
    /// </summary>
    /// <param name="steamIds"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseSteamidAccountBanned(string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            throw new ArgumentNullException(nameof(query));
        }

        Bot? bot = null;
        var bs = Bot.GetBots("ASF");
        if (bs?.Any() == true)
        {
            foreach (var b in bs)
            {
                if (b.IsConnectedAndLoggedOn)
                {
                    bot = b;
                    break;
                }
            }
        }

        if (bot == null)
        {
            return FormatStaticResponse(string.Format(Strings.NoBotsAreRunning));
        }

        var steamIds = new List<ulong>();

        string[] entries = query.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string entry in entries)
        {
            if (ulong.TryParse(entry, out ulong value))
            {
                if (value < 0x110000100000000)
                {
                    value += 0x110000100000000;
                }
                steamIds.Add(value);
            }
        }

        if (steamIds?.Any() != true)
        {
            return FormatStaticResponse(string.Format(Strings.BotNotFound, steamIds));
        }

        var results = await Utilities.InParallel(steamIds.Select(x => ResponseGetAccountBanned(bot, x))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }
}
