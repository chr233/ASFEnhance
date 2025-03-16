using ArchiSteamFarm.Steam;
using ASFEnhance.Data.ISteamNotificationService;

namespace ASFEnhance.Community;

internal static class WebRequest
{
    /// <summary>
    /// 设置留言全部已读
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<bool> MarkNotificationsRead(Bot bot)
    {
        var request = new Uri(SteamApiURL, "/ISteamNotificationService/MarkNotificationsRead/v1/");
        var data = new Dictionary<string, string>(2) {
            { "access_token",  bot.AccessToken ?? throw new AccessTokenNullException(bot) },
            { "timestamp", "0" },
            { "mark_all_read", "true" },
        };

        await bot.ArchiWebHandler.UrlPost(request, data: data, referer: SteamCommunityURL).ConfigureAwait(false);

        return true;
    }

    internal static async Task<GetSteamNotificationsResponse?> GetSteamNotificationsResponse(Bot bot)
    {
        var token = bot.AccessToken ?? throw new AccessTokenNullException(bot);

        var request = new Uri(SteamApiURL, $"/ISteamNotificationService/GetSteamNotifications/v1/?access_token={token}&include_hidden=true&language={Langs.Language}&include_confirmation_count=true&include_pinned_counts=true&include_read=true");

        var response = await bot.ArchiWebHandler.UrlGetToJsonObjectWithSession<GetSteamNotificationsResponse>(request, referer: SteamCommunityURL).ConfigureAwait(false);

        return response?.Content;
    }
}
