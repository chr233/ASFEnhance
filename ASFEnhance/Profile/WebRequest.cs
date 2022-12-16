#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Web.Responses;
using static ASFEnhance.Utils;

namespace ASFEnhance.Profile
{
    internal static class WebRequest
    {
        /// <summary>
        /// 读取Steam个人资料
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> GetSteamProfile(Bot bot)
        {
            Uri request = new(SteamCommunityURL, $"/profiles/{bot.SteamId}/?l=english");
            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL).ConfigureAwait(false);
            return HtmlParser.ParseProfilePage(response);
        }

        /// <summary>
        /// 读取交易链接
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> GetTradeofferPrivacyPage(Bot bot)
        {
            Uri request = new(SteamCommunityURL, $"/profiles/{bot.SteamId}/tradeoffers/privacy");
            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL).ConfigureAwait(false);
            return HtmlParser.ParseTradeofferPrivacyPage(response);
        }
    }
}
