using ArchiSteamFarm.Steam;

namespace ASFEnhance.Community;

internal static class WebRequest
{
    /// <summary>
    /// 设置留言全部已读
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task PureCommentNotifications(Bot bot)
    {
        Uri request = new(SteamCommunityURL, $"/profiles/{bot.SteamID}/commentnotifications/");

        Dictionary<string, string> data = new(2) {
            {"action", "markallread"},
        };

        await bot.ArchiWebHandler.UrlPostWithSession(request, data: data, referer: SteamCommunityURL).ConfigureAwait(false);
    }

    /// <summary>
    /// 设置库存通知全部已读
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task PureInventoryNotifications(Bot bot)
    {
        Uri request = new(SteamCommunityURL, $"/profiles/{bot.SteamID}/inventory/");

        _ = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamCommunityURL).ConfigureAwait(false);
    }
}
