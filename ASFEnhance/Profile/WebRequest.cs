#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;
using ArchiSteamFarm.Web.Responses;
using System;
using System.Threading.Tasks;

namespace Chrxw.ASFEnhance.Profile

{
    internal static class WebRequest
    {
        //读取个人资料
        internal static async Task<string?> GetSteamProfile(Bot bot)
        {
            Uri request = new(SteamCommunityURL, "/profiles/" + bot.SteamID + "/?l=english");

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL).ConfigureAwait(false);

            return HtmlParser.ParseProfilePage(response);
        }
        static private Uri SteamStoreURL => ArchiWebHandler.SteamStoreURL;
        static private Uri SteamCommunityURL => ArchiWebHandler.SteamCommunityURL;
    }
}
