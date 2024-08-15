using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Data;
using System.Text;


namespace ASFEnhance.Community;

internal static class Command
{
    /// <summary>
    /// 清除通知小绿信
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetNotifications(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var result = await WebRequest.GetSteamNotificationsResponse(bot).ConfigureAwait(false);
        var response = result?.Response;

        if (response?.Notifications == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        var sb = new StringBuilder();
        sb.AppendLine(Langs.MultipleLineResult);

        int index = 1;
        foreach (var notification in response.Notifications)
        {
            var type = notification.NotificationType switch
            {
                (int)ENotificationType.ReceivedGift => Langs.MsgReceivedGift,
                (int)ENotificationType.SubscribedDissionReplyed => Langs.MsgReceivedReplyed,
                (int)ENotificationType.ReceivedNewItem => Langs.MsgReceivedNewItem,
                (int)ENotificationType.ReceivedFriendInvitation => Langs.MsgFriendInviation,
                (int)ENotificationType.MajorSaleStart => Langs.MsgMajorSale,
                (int)ENotificationType.ItemInWishlistOnSale => Langs.MsgWishlistItemOnSale,
                (int)ENotificationType.ReceivedTradeOffer => Langs.MsgTradeOffer,
                (int)ENotificationType.ReceivedSteamSupportReply => Langs.MsgSteamSupport,
                (int)ENotificationType.SteamTurnNotification => Langs.MsgSteamTurn,
                (int)ENotificationType.SteamCommunityMessage => Langs.MsgCommunityMessage,
                _ => notification.NotificationType.ToString(),
            };

            sb.AppendLineFormat("{0}{1}: {2} {3}", notification.Read ? "" : "*", index++, type, notification.BodyData);

            if (index >= 15)
            {
                break;
            }
        }

        if (index == 1)
        {
            sb.AppendLine(Langs.MsgNoMessage);
        }

        sb.AppendLine(Static.Line);
        sb.AppendLineFormat(Langs.NocUnreadCount, response.UnreadCount);

        if (response.ConfirmationCount > 0)
        {
            sb.AppendLineFormat(Langs.NocConfirmationCount, response.ConfirmationCount);
        }
        if (response.PendingGiftCount > 0)
        {
            sb.AppendLineFormat(Langs.NocPendingGiftCount, response.PendingGiftCount);
        }
        if (response.PendingFriendCount > 0)
        {
            sb.AppendLineFormat(Langs.NocPendingFriendCount, response.PendingFriendCount);
        }
        if (response.PendingFamilyInviteCount > 0)
        {
            sb.AppendLineFormat(Langs.NocPendingFamilyInviteCount, response.PendingFamilyInviteCount);
        }

        return bot.FormatBotResponse(sb.ToString());
    }

    /// <summary>
    /// 清除通知小绿信 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetNotifications(string botNames)
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

        var results = await Utilities.InParallel(bots.Select(bot => ResponseGetNotifications(bot))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }


    /// <summary>
    /// 清除通知小绿信
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseClearNotification(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var result = await WebRequest.MarkNotificationsRead(bot).ConfigureAwait(false);

        return bot.FormatBotResponse(result ? Langs.Success : Langs.Failure);
    }

    /// <summary>
    /// 清除通知小绿信 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseClearNotification(string botNames)
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

        var results = await Utilities.InParallel(bots.Select(bot => ResponseClearNotification(bot))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }
}
