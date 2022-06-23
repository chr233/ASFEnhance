#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using AngleSharp.Dom;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ArchiSteamFarm.Web.Responses;
using static ASFEnhance.Utils;

namespace ASFEnhance.Event
{

    internal static class WebRequest
    {
        internal static async Task<UserInfoResponse?> FetUserInfo(Bot bot)
        {
            Uri request = new(SteamStoreURL, "/sale/clorthax_quest");

            var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

            if (response == null)
            {
                return null;
            }

            var configEle = response.Content.SelectSingleNode("//div[@id='application_config']");

            var json = configEle?.GetAttribute("data-userinfo");

            if (json == null)
            {
                return null;
            }

            try
            {
                var result = JsonConvert.DeserializeObject<UserInfoResponse>(json);
                return result;
            }
            catch (Exception ex)
            {
                ASFLogger.LogGenericException(ex);
                return null;
            }
        }


        internal static async Task<AjaxOpenDoorResponse?> AjaxOpenDoor(Bot bot, UserInfoResponse userInfo)
        {
            Uri request = new(SteamStoreURL, "/saleaction/ajaxopendoor");

            Dictionary<string, string> data = new() {
                { "authwgtoken", userInfo.AuthwgToken },
                { "door_index", "0" },
                { "clan_accountid", userInfo.Accountid.ToString() },
            };

            var response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<AjaxOpenDoorResponse>(request, data: data, referer: SteamStoreURL).ConfigureAwait(false);

            return response?.Content;

        }
    }
}
