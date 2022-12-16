using ArchiSteamFarm.Steam;
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
            Uri request = new(SteamCommunityURL, $"/profiles/{bot.SteamId}/commentnotifications/");

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
            Uri request = new(SteamCommunityURL, $"/profiles/{bot.SteamId}/inventory/");

            _ = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamCommunityURL).ConfigureAwait(false);
        }

        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="steamId"></param>
        /// <returns></returns>
        internal static async Task<bool> SendFriendRequest(Bot bot,ulong steamId)
        {
            
        }
    }
}
