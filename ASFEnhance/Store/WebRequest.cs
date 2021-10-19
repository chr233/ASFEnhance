#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;
using ArchiSteamFarm.Web.Responses;
using System;
using System.Threading.Tasks;
using static Chrxw.ASFEnhance.Store.Response;

namespace Chrxw.ASFEnhance.Store
{
    internal static class WebRequest
    {
        //读取商店页Sub
        internal static async Task<StoreResponse?> GetStoreSubs(Bot bot, string type, uint gameID)
        {
            Uri request = new(SteamStoreURL, "/" + type.ToLowerInvariant() + "/" + gameID.ToString() + "/?l=schinese");

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL).ConfigureAwait(false);

            return HtmlParser.ParseStorePage(response);
        }
        static private Uri SteamStoreURL => ArchiWebHandler.SteamStoreURL;
    }
}
