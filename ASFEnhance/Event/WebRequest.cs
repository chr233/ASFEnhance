using AngleSharp.Dom;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;

namespace ASFEnhance.Event;

internal static class WebRequest
{
    /// <summary>
    /// 模拟做题
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="clan_accountid"></param>
    /// <param name="door_index"></param>
    /// <returns></returns>
    internal static async Task DoEventTask(Bot bot, string clan_accountid, uint door_index)
    {
        Uri request = new(SteamStoreURL, "/saleaction/ajaxopendoor");

        Dictionary<string, string> data = new(3) {
            {"door_index", door_index.ToString()},
            {"clan_accountid", clan_accountid},
        };

        _ = await bot.ArchiWebHandler.UrlPostWithSession(request, data: data).ConfigureAwait(false);
    }

    /// <summary>
    /// 获取Token
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="salePage"></param>
    /// <returns></returns>
    internal static async Task<string?> FetchEventToken(Bot bot, string salePage)
    {
        Uri request = new(SteamStoreURL, $"/sale/{salePage}");

        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

        if (response == null)
        {
            return null;
        }

        var configEle = response?.Content?.QuerySelector<IElement>("#application_config");
        string community = configEle?.GetAttribute("data-community") ?? "";
        var match = RegexUtils.MatchClanaCCountId().Match(community);

        return match.Success ? match.Groups[1].Value : null;
    }

    /// <summary>
    /// 获取Token
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> FetchToken(Bot bot)
    {
        Uri request = new(SteamStoreURL, "/category/sports");

        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: request).ConfigureAwait(false);

        if (response == null)
        {
            return null;
        }

        var configEle = response?.Content?.QuerySelector<IElement>("#application_config");
        string community = configEle?.GetAttribute("data-loyalty_webapi_token") ?? "";
        var match = RegexUtils.MatchToken().Match(community);

        return match.Success ? match.Groups[1].Value : null;
    }

    /// <summary>
    /// 领取活动道具
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    internal static async Task<bool> ClaimDailySticker(Bot bot, string token)
    {
        Uri request = new($"https://api.steampowered.com/ISaleItemRewardsService/ClaimItem/v1?access_token={token}");
        Uri referer = new(SteamStoreURL, "/sale/16212626125");

        await bot.ArchiWebHandler.UrlPostWithSession(request, referer: referer, session: ArchiWebHandler.ESession.None).ConfigureAwait(false);

        return true;
    }
}
