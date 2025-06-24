using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Data;
using ASFEnhance.Data.Plugin;
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
    internal static async Task<string?> ResponseAccountHistory(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var result = await WebRequest.GetAccountHistoryDetail(bot).ConfigureAwait(false);

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
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(ResponseAccountHistory)).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 读取账号许可证列表
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="onlyFreeLicense"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetAccountLicenses(Bot bot, bool onlyFreeLicense)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var result = await WebRequest.GetOwnedLicenses(bot, onlyFreeLicense).ConfigureAwait(false);

        if (result == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        var sb = new StringBuilder();
        sb.AppendLine(bot.FormatBotResponse(Langs.MultipleLineResult));

        if (onlyFreeLicense)
        {
            sb.AppendLine(Langs.AccountFreeSubTitle);
            foreach (var item in result)
            {
                if (item.PackageId != 0 && item.Type == LicenseType.Complimentary)
                {
                    sb.AppendLineFormat(Langs.AccountSubItem, item.PackageId, item.Name);
                }
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
                sb.AppendLineFormat(Langs.AccountSubItem, type, item.Name);
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
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseGetAccountLicenses(bot, onlyFreelicense)))
            .ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

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

        sb.AppendLineFormat(Langs.CookieItem, Langs.EnableEmailNotification,
            result.EnableEmailNotification ? Langs.Yes : Langs.No);
        if (result.EnableEmailNotification)
        {
            sb.AppendLineFormat(Langs.StoreItemHeader, Langs.WhenWishlistDiscount,
                result.WhenWishlistDiscount ? Langs.Yes : Langs.No);
            sb.AppendLineFormat(Langs.StoreItemHeader, Langs.WhenWishlistRelease,
                result.WhenWishlistRelease ? Langs.Yes : Langs.No);
            sb.AppendLineFormat(Langs.StoreItemHeader, Langs.WhenGreenLightRelease,
                result.WhenGreenLightRelease ? Langs.Yes : Langs.No);
            sb.AppendLineFormat(Langs.StoreItemHeader, Langs.WhenFollowPublisherRelease,
                result.WhenFollowPublisherRelease ? Langs.Yes : Langs.No);
            sb.AppendLineFormat(Langs.StoreItemHeader, Langs.WhenSaleEvent,
                result.WhenSaleEvent ? Langs.Yes : Langs.No);
            sb.AppendLineFormat(Langs.StoreItemHeader, Langs.WhenReceiveCuratorReview,
                result.WhenReceiveCuratorReview ? Langs.Yes : Langs.No);
            sb.AppendLineFormat(Langs.StoreItemHeader, Langs.WhenReceiveCommunityReward,
                result.WhenReceiveCommunityReward ? Langs.Yes : Langs.No);
            sb.AppendLineFormat(Langs.StoreItemHeader, Langs.WhenGameEventNotification,
                result.WhenGameEventNotification ? Langs.Yes : Langs.No);
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
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(ResponseGetEmailOptions))
            .ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

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

        var entries = query.Split(SeparatorDot, StringSplitOptions.RemoveEmptyEntries);

        var payload = new EmailOptions();

        int i = 0;

        List<string> yesStrings = ["1", "y", "yes", "true"];

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

        sb.AppendLineFormat(Langs.CookieItem, Langs.EnableEmailNotification,
            result.EnableEmailNotification ? Langs.Yes : Langs.No);
        if (result.EnableEmailNotification)
        {
            sb.AppendLineFormat(Langs.StoreItemHeader, Langs.WhenWishlistDiscount,
                result.WhenWishlistDiscount ? Langs.Yes : Langs.No);
            sb.AppendLineFormat(Langs.StoreItemHeader, Langs.WhenWishlistRelease,
                result.WhenWishlistRelease ? Langs.Yes : Langs.No);
            sb.AppendLineFormat(Langs.StoreItemHeader, Langs.WhenGreenLightRelease,
                result.WhenGreenLightRelease ? Langs.Yes : Langs.No);
            sb.AppendLineFormat(Langs.StoreItemHeader, Langs.WhenFollowPublisherRelease,
                result.WhenFollowPublisherRelease ? Langs.Yes : Langs.No);
            sb.AppendLineFormat(Langs.StoreItemHeader, Langs.WhenSaleEvent,
                result.WhenSaleEvent ? Langs.Yes : Langs.No);
            sb.AppendLineFormat(Langs.StoreItemHeader, Langs.WhenReceiveCuratorReview,
                result.WhenReceiveCuratorReview ? Langs.Yes : Langs.No);
            sb.AppendLineFormat(Langs.StoreItemHeader, Langs.WhenReceiveCommunityReward,
                result.WhenReceiveCommunityReward ? Langs.Yes : Langs.No);
            sb.AppendLineFormat(Langs.StoreItemHeader, Langs.WhenGameEventNotification,
                result.WhenGameEventNotification ? Langs.Yes : Langs.No);
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
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseSetEmailOptions(bot, query)))
            .ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

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
        sb.AppendLineFormat(Langs.StoreItemHeader, Langs.NooReceivedGift,
            NotificationTargetToString(result.ReceivedGift));
        sb.AppendLineFormat(Langs.StoreItemHeader, Langs.NooReceivedReply,
            NotificationTargetToString(result.SubscribedDissionReplyed));
        sb.AppendLineFormat(Langs.StoreItemHeader, Langs.NooNewItem,
            NotificationTargetToString(result.ReceivedNewItem));
        sb.AppendLineFormat(Langs.StoreItemHeader, Langs.NooFriendInvite,
            NotificationTargetToString(result.ReceivedFriendInvitation));
        sb.AppendLineFormat(Langs.StoreItemHeader, Langs.NooMajorSale,
            NotificationTargetToString(result.MajorSaleStart));
        sb.AppendLineFormat(Langs.StoreItemHeader, Langs.NooWishlistOnSale,
            NotificationTargetToString(result.ItemInWishlistOnSale));
        sb.AppendLineFormat(Langs.StoreItemHeader, Langs.NooNewTradeOffer,
            NotificationTargetToString(result.ReceivedTradeOffer));
        sb.AppendLineFormat(Langs.StoreItemHeader, Langs.NooSteamSupport,
            NotificationTargetToString(result.ReceivedSteamSupportReply));
        sb.AppendLineFormat(Langs.StoreItemHeader, Langs.NooSteamTurn,
            NotificationTargetToString(result.SteamTurnNotification));

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
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(ResponseGetNotificationOptions))
            .ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

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

        var entries = query.Split(SeparatorDot, StringSplitOptions.RemoveEmptyEntries);

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
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseSetNotificationOptions(bot, query)))
            .ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

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

        var token = Config.ApiKey;
        if (string.IsNullOrEmpty(token))
        {
            return await WebRequest.GetAccountBans(bot).ConfigureAwait(false);
        }

        var result = await WebRequest.GetPlayerBans(bot, token, steamId ?? bot.SteamID).ConfigureAwait(false);

        if (result == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        if (result.Players == null || result.Players.Count == 0)
        {
            return bot.FormatBotResponse(Langs.NoUserFound);
        }

        var sb = new StringBuilder();
        sb.AppendLine(bot.FormatBotResponse(Langs.MultipleLineResult));

        var player = result.Players.First();

        sb.AppendLineFormat(Langs.BanSteamId, player.SteamId);
        sb.AppendLineFormat(Langs.BanCommunity, Bool2Str(player.CommunityBanned));
        sb.AppendLineFormat(Langs.BanEconomy, player.EconomyBan == "none" ? "×" : player.EconomyBan);
        sb.Append(string.Format(Langs.BanVAC, Bool2Str(player.VACBanned)));
        if (player.VACBanned)
        {
            sb.AppendLineFormat(Langs.BanVACCount, player.NumberOfVACBans, player.DaysSinceLastBan);
        }
        else
        {
            sb.AppendLine();
        }

        var gameban = player.NumberOfGameBans > 0;
        sb.Append(string.Format(Langs.BanGame, Bool2Str(gameban)));
        if (gameban)
        {
            sb.AppendLineFormat(Langs.BanGameCount, player.NumberOfGameBans);
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

        if (targetBots == null || targetBots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(targetBots.Select(bot => ResponseGetAccountBanned(bot, null)))
            .ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 获取账户封禁情况 (多个Bot)
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseSteamIdAccountBanned(string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            throw new ArgumentNullException(nameof(query));
        }

        Bot? bot = null;
        var bs = Bot.GetBots("ASF");
        if (bs != null && bs.Count > 0)
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
            return FormatStaticResponse(Strings.NoBotsAreRunning);
        }

        var steamIds = new List<ulong>();

        string[] entries = query.Split(',', StringSplitOptions.RemoveEmptyEntries);

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

        if (steamIds.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, steamIds);
        }

        var results = await Utilities.InParallel(steamIds.Select(x => ResponseGetAccountBanned(bot, x)))
            .ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 手动接收礼物
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseReceiveGift(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var result = await WebRequest.GetReceivedGift(bot).ConfigureAwait(false);

        if (result == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        var giftCount = result.Count;
        var successCount = 0;

        foreach (var giftId in result)
        {
            var response = await WebRequest.AcceptReceivedGift(bot, giftId).ConfigureAwait(false);
            if (response?.Result == EResult.OK)
            {
                successCount++;
            }
        }

        return bot.FormatBotResponse(Langs.ReceiveGiftResult, giftCount, successCount);
    }

    /// <summary>
    /// 手动接收礼物 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseReceiveGift(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(ResponseReceiveGift)).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 获取游玩时间
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetPlayTime(Bot bot, string? query)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var token = bot.AccessToken;

        if (string.IsNullOrEmpty(token))
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        var result = await WebRequest.GetGamePlayTime(bot, token).ConfigureAwait(false);

        if (result == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        var sb = new StringBuilder();

        var appIds = new HashSet<uint>();
        if (!string.IsNullOrEmpty(query))
        {
            var entries = query.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var entry in entries)
            {
                if (uint.TryParse(entry, out var appId))
                {
                    appIds.Add(appId);
                }
                else
                {
                    sb.AppendFormat(Langs.InvalidAppId, entry);
                }
            }
        }

        if (sb.Length > 0)
        {
            sb.AppendLine();
        }

        long twoWeekHours = 0;
        long totalHours = 0;

        if (appIds.Count > 0)
        {
            foreach (var appId in appIds)
            {
                if (result.TryGetValue(appId, out var game))
                {
                    sb.AppendLineFormat(Langs.PlayTimeItem, appId, game.Name, game.PlayTimeForever / 60.0,
                        game.PlayTime2Weeks / 60.0);
                    totalHours += game.PlayTimeForever;
                    twoWeekHours += game.PlayTime2Weeks;
                }
                else
                {
                    sb.AppendLineFormat(Langs.NoPlayTimeInfo, appId);
                }
            }
        }
        else
        {
            sb.AppendLine(Langs.AllPlayTimeInfo);
            foreach (var game in result.Values)
            {
                totalHours += game.PlayTimeForever;
                twoWeekHours += game.PlayTime2Weeks;
            }
        }

        sb.AppendLineFormat(Langs.TotalPlayTimeItem, totalHours / 60.0, twoWeekHours / 60.0);

        return bot.FormatBotResponse(sb.ToString());
    }

    /// <summary>
    /// 获取游玩时间 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetPlayTime(string botNames, string? query)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseGetPlayTime(bot, query)))
            .ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 获取账户邮箱
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetEmail(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var email = await WebRequest.GetAccountEmail(bot).ConfigureAwait(false);

        return bot.FormatBotResponse(email ?? Langs.NetworkError);
    }

    /// <summary>
    /// 获取账户邮箱 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetEmail(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(ResponseGetEmail)).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 检查ApiKey
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseCheckApiKey(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var result = await WebRequest.CheckApiKey(bot).ConfigureAwait(false);
        if (result == null)
        {
            return Langs.NetworkError;
        }

        return bot.FormatBotResponse(result.Value ? Langs.ExistsApiKey : Langs.NoExistsApiKey);
    }

    /// <summary>
    /// 检查ApiKey (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseCheckApiKey(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(ResponseCheckApiKey)).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 吊销ApiKey
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseRevokeApiKey(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        await WebRequest.RevokeApiKey(bot).ConfigureAwait(false);

        return bot.FormatBotResponse(Langs.Success);
    }

    /// <summary>
    /// 吊销ApiKey (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseRevokeApiKey(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(ResponseRevokeApiKey)).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 获取私密应用列表
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetPrivacyAppList(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var response = await WebRequest.GetPrivateAppList(bot).ConfigureAwait(false);
        var appIds = response?.PrivateApps?.AppIds;

        if (appIds == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        if (appIds.Count == 0)
        {
            return bot.FormatBotResponse("私密应用列表为空");
        }

        var sb = new StringBuilder();
        sb.AppendLine(bot.FormatBotResponse("私密应用列表:"));
        int i = 1;
        foreach (var id in appIds)
        {
            sb.AppendLineFormat("{0} - {1}", i++, id);
        }

        return sb.ToString();
    }

    /// <summary>
    /// 获取私密应用列表 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetPrivacyAppList(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(ResponseGetPrivacyAppList))
            .ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 设置私密应用
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="query"></param>
    /// <param name="privacy"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseSetAppListPrivacy(Bot bot, string query, bool privacy)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var gameIds = FetchGameIds(query, ESteamGameIdType.App, ESteamGameIdType.App);

        if (gameIds.Count == 0)
        {
            return bot.FormatBotResponse(Langs.CanNotParseAnyGameInfo);
        }

        List<uint> appIds = [];
        foreach (var gameId in gameIds)
        {
            appIds.Add(gameId.Id);
        }

        var response = await WebRequest.ToggleAppPrivacy(bot, appIds, privacy).ConfigureAwait(false);

        return bot.FormatBotResponse(response ? Langs.Success : Langs.Failure);
    }

    /// <summary>
    /// 设置私密应用 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="query"></param>
    /// <param name="privacy"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseSetAppListPrivacy(string botNames, string query, bool privacy)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseSetAppListPrivacy(bot, query, privacy)))
            .ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 检查市场限制
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="query"></param>
    /// <param name="privacy"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseCheckMarketLimit(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var response = await WebRequest.GetIfMarketLimited(bot).ConfigureAwait(false);
        if (response == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        return bot.FormatBotResponse(response.Value ? Langs.MarketStatusNormal : Langs.MarketStatusLimited);
    }

    /// <summary>
    /// 检查市场限制 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="query"></param>
    /// <param name="privacy"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseCheckMarketLimit(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(ResponseCheckMarketLimit))
            .ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 获取手机尾号
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetPhoneSuffix(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var response = await WebRequest.GetPhoneSuffix(bot).ConfigureAwait(false);
        return bot.FormatBotResponse(response ?? Langs.GetPhoneSuffixFailed);
    }

    /// <summary>
    /// 获取手机尾号 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetPhoneSuffix(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(ResponseGetPhoneSuffix)).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 获取注册时间
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetRegisterDate(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        //if (bot.IsAccountLimited)
        //{
        //    return bot.FormatBotResponse("机器人受限, 可能无法获取到注册时间");
        //}

        var response = await WebRequest.GetRegisteDate(bot).ConfigureAwait(false);
        return bot.FormatBotResponse(response ?? Langs.NetworkError);
    }

    /// <summary>
    /// 获取注册时间 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetRegisterDate(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(ResponseGetRegisterDate)).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 移除免费许可证
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="query"></param>
    /// <param name="onlyDemos"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseRemoveFreeLicenses(Bot bot, string? query, bool onlyDemos)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var licensesOld = await WebRequest.GetOwnedLicenses(bot, true).ConfigureAwait(false);
        if (licensesOld == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        List<uint> toRemoveSubIds = [];
        if (string.IsNullOrEmpty(query))
        {
            foreach (var license in licensesOld)
            {
                if (license.Type == LicenseType.Complimentary && license.PackageId > 0)
                {
                    if (!onlyDemos || license.Name?.EndsWith(" Demo") == true)
                    {
                        toRemoveSubIds.Add(license.PackageId);
                    }
                }
            }
        }
        else
        {
            var gameIds = FetchGameIds(query, ESteamGameIdType.Sub, ESteamGameIdType.Sub);
            HashSet<uint> targetIds = [];

            foreach (var id in gameIds)
            {
                if (id.Type == ESteamGameIdType.Sub && id.Id > 0)
                {
                    targetIds.Add(id.Id);
                }
            }

            foreach (var license in licensesOld)
            {
                if (license.Type == LicenseType.Complimentary && license.PackageId > 0 && targetIds.Contains(license.PackageId))
                {
                    if (!onlyDemos || license.Name?.EndsWith(" Demo") == true)
                    {
                        toRemoveSubIds.Add(license.PackageId);
                    }
                }
            }
        }

        var semaphore = new SemaphoreSlim(3);

        async Task workThread(uint subId)
        {
            try
            {
                await semaphore.WaitAsync().ConfigureAwait(false);
                try
                {
                    await WebRequest.RemoveLicense(bot, subId).ConfigureAwait(false);
                    await Task.Delay(500).ConfigureAwait(false);
                }
                finally
                {
                    semaphore.Release();
                }
            }
            catch (Exception ex)
            {
                ASFLogger.LogGenericException(ex);
            }
        }

        if (toRemoveSubIds.Count > 0)
        {
            var tasks = toRemoveSubIds.Select(workThread);
            await Utilities.InParallel(tasks).ConfigureAwait(false);

            return bot.FormatBotResponse(Langs.AccountRemovedLicenses, toRemoveSubIds.Count);
        }
        else
        {
            return bot.FormatBotResponse(Langs.AccountRemoveLicensesFailed);
        }
    }

    /// <summary>
    /// 移除免费许可证 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="query"></param>
    /// <param name="onlyDemos"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseRemoveFreeLicenses(string botNames, string? query, bool onlyDemos)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseRemoveFreeLicenses(bot, query, onlyDemos)))
            .ConfigureAwait(false);
        return string.Join(Environment.NewLine, results);
    }

    /// <summary>
    /// 获取账号中被封禁的游戏
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetMyBans(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var result = await WebRequest.GetMyBans(bot).ConfigureAwait(false);

        if (result == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        if (result.Count == 0)
        {
            return bot.FormatBotResponse(Langs.AccountHasNoBanRecord);
        }

        var sb = new StringBuilder();
        sb.AppendLine(bot.FormatBotResponse(Langs.AccountBanRecordList));
        foreach (var ban in result)
        {
            sb.AppendLine(ban);
        }

        return sb.ToString();
    }

    /// <summary>
    /// 获取账号中被封禁的游戏 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetMyBans(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(ResponseGetMyBans)).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }
}