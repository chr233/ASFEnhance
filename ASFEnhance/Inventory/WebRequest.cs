using ArchiSteamFarm.Steam;
using ASFEnhance.Data;
using ASFEnhance.Data.Common;
using SteamKit2;

namespace ASFEnhance.Inventory;


internal static class WebRequest
{
    /// <summary>
    /// 物品堆叠
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="appId"></param>
    /// <param name="fromAssetId"></param>
    /// <param name="destAssetId"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    internal static async Task<CombineItemStacksResponse?> CombineItemStacks(Bot bot, uint appId, ulong fromAssetId, ulong destAssetId, uint amount)
    {
        var token = bot.AccessToken ?? throw new AccessTokenNullException();
        var request = new Uri(SteamApiURL, "/IInventoryService/CombineItemStacks/v1/");

        var data = new Dictionary<string, string>(3) {
            { "access_token", token },
            { "appid", appId.ToString() },
            { "fromitemid", fromAssetId.ToString() },
            { "destitemid", destAssetId.ToString() },
            { "quantity", amount.ToString() },
            { "steamid", bot.SteamID.ToString() },
        };

        var response = await bot.ArchiWebHandler.UrlPostToJsonObject<CombineItemStacksResponse>(request, data: data, referer: SteamCommunityURL).ConfigureAwait(false);
        return response?.Content;
    }

    /// <summary>
    /// 堆叠物品拆分
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="appId"></param>
    /// <param name="itemAssetId"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    internal static async Task<CombineItemStacksResponse?> SplitItemStack(Bot bot, uint appId, ulong itemAssetId, uint amount)
    {
        var token = bot.AccessToken ?? throw new AccessTokenNullException();
        var request = new Uri(SteamApiURL, "/IInventoryService/SplitItemStack/v1/");

        var data = new Dictionary<string, string>(3) {
            { "access_token", token },
            { "appid", appId.ToString() },
            { "itemid", itemAssetId.ToString() },
            { "quantity", amount.ToString() },
            { "steamid", bot.SteamID.ToString() },
        };

        var response = await bot.ArchiWebHandler.UrlPostToJsonObject<CombineItemStacksResponse>(request, data: data, referer: SteamCommunityURL).ConfigureAwait(false);
        return response?.Content;
    }

    /// <summary>
    /// 获取待处理礼物
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<List<PendingGiftData>?> GetPendingGifts(Bot bot)
    {
        var request = new Uri(SteamCommunityURL, $"/profiles/{bot.SteamID}/inventory/?l={Langs.Language}");
        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamCommunityURL).ConfigureAwait(false);

        var body = response?.Content?.Body;
        if (body == null)
        {
            return null;
        }

        List<PendingGiftData> result = [];

        var parent = body.QuerySelector("#tabcontent_pendinggifts");

        if (parent != null)
        {
            var giftDoms = parent.QuerySelectorAll("div.pending_gift>div[id^='pending_gift']");
            var regex = RegexUtils.MatchPendingGift();
            var regex2 = RegexUtils.MatchPendingGiftAndSender();
            foreach (var giftDom in giftDoms)
            {
                var match = regex.Match(giftDom.Id ?? "");
                if (!match.Success || !ulong.TryParse(match.Groups[1].Value, out var giftId))
                {
                    continue;
                }

                var onclick = parent.QuerySelector($"#gift{giftId}_step_init>div[onclick^='ShowDeclineGiftOptions']")?.GetAttribute("onclick");
                match = regex2.Match(onclick ?? "");
                if (!match.Success || !ulong.TryParse(match.Groups[1].Value, out var senderId))
                {
                    senderId = 0;
                }

                var gameName = parent.QuerySelector($"#decline_gift_prompt_{giftId}>h2")?.TextContent;
                var senderName = giftDom.QuerySelector("div.signature_line>p")?.TextContent;

                var data = new PendingGiftData(giftId, gameName, senderName, senderId);
                result.Add(data);
            }
        }

        return result;
    }

    /// <summary>
    /// 接收指定礼物
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="giftId"></param>
    /// <param name="privaty"></param>
    /// <returns></returns>
    internal static async Task<string> AcceptGift(Bot bot, ulong giftId, bool privaty = false)
    {
        var request = new Uri(SteamCommunityURL, $"/gifts/{giftId}/validateunpack");
        var validResponse = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<BaseResultResponse>(request, data: null, referer: SteamCommunityURL).ConfigureAwait(false);
        if (validResponse?.Content == null)
        {
            return string.Format(Langs.CartItem, giftId, Langs.NetworkError);
        }

        if (validResponse.Content.Result != EResult.OK)
        {
            return string.Format(Langs.CartItem, giftId, Langs.AcceptGiftFailed);
        }

        request = new Uri(SteamCommunityURL, $"/gifts/{giftId}/unpack");
        var data = new Dictionary<string, string>
        {
            { "bPrivately", privaty ? "1" : "0" },
        };

        var unpackResponse = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<BaseResultResponse>(request, data: data, referer: SteamCommunityURL).ConfigureAwait(false);
        if (unpackResponse?.Content == null)
        {
            return string.Format(Langs.CartItem, giftId, Langs.NetworkError);
        }

        return string.Format(Langs.CartItem, giftId, unpackResponse.Content.Result == EResult.OK ? Langs.AcceptGiftSuccess : Langs.AcceptGiftFailed);
    }

    /// <summary>
    /// 接收指定礼物
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="giftId"></param>
    /// <param name="senderSteamId"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    internal static async Task<string> DeclineGift(Bot bot, ulong giftId, ulong senderSteamId, string? message)
    {
        var request = new Uri(SteamCommunityURL, $"/gifts/{giftId}/decline");
        var data = new Dictionary<string, string>
        {
            { "steamid_sender", senderSteamId.ToString() },
            { "note", message ?? "via ASFEnhance" },
        };

        var response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<BaseResultResponse>(request, data: data, referer: SteamCommunityURL).ConfigureAwait(false);
        if (response?.Content == null)
        {
            return string.Format(Langs.CartItem, giftId, Langs.NetworkError);
        }

        return string.Format(Langs.CartItem, giftId, response.Content.Result == EResult.OK ? Langs.DeclineGiftSuccess : Langs.DeclineGiftFailed);
    }
}
