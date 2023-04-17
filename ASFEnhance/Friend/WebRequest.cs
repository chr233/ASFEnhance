using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;
using ASFEnhance.Data;

namespace ASFEnhance.Friend
{
    internal static class WebRequest
    {
        /// <summary>
        /// 验证个人资料链接
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static async Task<ulong?> GetSteamIdByProfileLink(Bot bot, string path)
        {
            Uri request = new(SteamCommunityURL, path);

            var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

            return HtmlParser.ParseProfilePage(response);
        }
    }
}
