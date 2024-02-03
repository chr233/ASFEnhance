using ArchiSteamFarm.Steam;

namespace ASFEnhance.Community;

internal static class WebRequest
{
    /// <summary>
    /// 设置留言全部已读
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<bool> PureCommentNotifications(Bot bot)
    {
        var token = bot.AccessToken;
        if (token == null)
        {
            return false;
        }

        var request = new Uri(SteamApiURL, "/ISteamNotificationService/MarkNotificationsRead/v1/");

        var data = new Dictionary<string, string>(2) {
            { "access_token", token },
            { "mark_all_read", "true" },
        };

        await bot.ArchiWebHandler.UrlPostWithSession(request, data: data, referer: SteamCommunityURL).ConfigureAwait(false);

        return true;
    }
}
