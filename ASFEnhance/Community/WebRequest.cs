using ArchiSteamFarm.Steam;
using ASFEnhance.Data;
using ArchiSteamFarm.Steam.Integration;
using static ASFEnhance.Utils;

namespace ASFEnhance.Community
{
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

        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="steamId"></param>
        /// <returns></returns>
        internal static async Task<(ulong, AjaxAddFriendResponse?)> SendFriendRequest(Bot bot, ulong steamId)
        {
            Uri request = new(SteamCommunityURL, $"/actions/AddFriendAjax");

            Dictionary<string, string> data = new(3) {
                {"steamid", steamId.ToString()},
                {"accept_invite", "0"},
            };

            var response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<AjaxAddFriendResponse>(request, data: data, referer: SteamCommunityURL, session: ArchiWebHandler.ESession.CamelCase).ConfigureAwait(false);
            return (steamId, response?.Content);
        }
    }
}
