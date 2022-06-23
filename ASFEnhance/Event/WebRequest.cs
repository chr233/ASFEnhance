#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using AngleSharp.Dom;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ArchiSteamFarm.Web.Responses;
using System.Text.RegularExpressions;
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
            Uri referer = new(SteamStoreURL, "/sale/clorthax_quest");

            Dictionary<string, string> data = new(4) {
                { "authwgtoken", userInfo.AuthwgToken },
                { "door_index", "0" },
                { "clan_accountid", userInfo.Accountid.ToString() },
            };

            var response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<AjaxOpenDoorResponse>(request, data: data, referer: referer).ConfigureAwait(false);

            return response?.Content;
        }

        internal static async Task<AjaxOpenDoorResponse?> AjaxOpenDoor(Bot bot, UserInfoResponse userInfo, CapsuleinsertResponse capsuleinsert, int index)
        {
            Uri request = new(SteamStoreURL, "/saleaction/ajaxopendoor");
            Uri referer = new(SteamStoreURL, $"/category/{UriList[index]}/");

            Dictionary<string, string> data = new(5) {
                { "authwgtoken", userInfo.AuthwgToken },
                { "datarecord", capsuleinsert.DataRecord },
                { "door_index", (index+1).ToString() },
                { "clan_accountid", userInfo.Accountid.ToString() },
            };

            var response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<AjaxOpenDoorResponse>(request, data: data, referer: referer).ConfigureAwait(false);

            return response?.Content;
        }

        private static List<string> UriList { get; } = new() {
            "arcade_rhythm",
            "strategy_cities_settlements",
            "sports",
            "simulation",
            "multiplayer_coop",
            "casual",
            "rpg",
            "horror",
            "vr",
            "strategy",
        };

        internal static async Task<CapsuleinsertResponse?> FetCapsuleinsert(Bot bot, int index)
        {
            if (index < 0 || index > 9)
            {
                return null;
            }

            Uri request = new(SteamStoreURL, $"/category/{UriList[index]}/");

            var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

            if (response == null)
            {
                return null;
            }

            var configEle = response.Content.SelectSingleNode("//div[@id='application_config']");

            var json = configEle?.GetAttribute("data-capsuleinsert");

            if (json == null)
            {
                return null;
            }

            try
            {
                var result = JsonConvert.DeserializeObject<CapsuleinsertResponse>(json);
                return result;
            }
            catch (Exception ex)
            {
                ASFLogger.LogGenericException(ex);
                return null;
            }
        }
    }
}
