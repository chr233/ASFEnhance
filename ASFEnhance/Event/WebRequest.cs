#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Steam;
using Newtonsoft.Json;
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

            var configEle = response.Content.SelectSingleNode<IElement>("//div[@id='application_config']");
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

        /// <summary>
        /// 开始徽章任务
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        internal static async Task<AjaxOpenDoorResponse?> StartTask(Bot bot, UserInfoResponse userInfo)
        {
            Uri request = new(SteamStoreURL, "/saleaction/ajaxopendoor");
            Uri referer = new(SteamStoreURL, "/sale/clorthax_quest");

            Dictionary<string, string> data = new(4) {
                { "authwgtoken", userInfo.AuthwgToken },
                { "door_index", "0" },
                { "clan_accountid", "41316928" },
            };

            var response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<AjaxOpenDoorResponse>(request, data: data, referer: referer).ConfigureAwait(false);

            return response?.Content;
        }

        /// <summary>
        /// 解锁徽章任务
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="userInfo"></param>
        /// <param name="capsuleinsert"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        internal static async Task<AjaxOpenDoorResponse?> FinishTask(Bot bot, UserInfoResponse userInfo, CapsuleinsertResponse capsuleinsert, int index)
        {
            Uri request = new(SteamStoreURL, "/saleaction/ajaxopendoor");
            Uri referer = new(SteamStoreURL, $"/category/{UriList[index]}/");

            Dictionary<string, string> data = new(5) {
                { "authwgtoken", userInfo.AuthwgToken },
                { "datarecord", capsuleinsert.DataRecord },
                { "door_index", capsuleinsert.Payload.ToString() },
                { "clan_accountid", "41316928" },
            };

            var response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<AjaxOpenDoorResponse>(request, data: data, referer: referer).ConfigureAwait(false);

            return response?.Content;
        }

        /// <summary>
        /// 全部解锁后获取特殊主题
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        internal static async Task<AjaxOpenDoorResponse?> UnlockTheme(Bot bot, UserInfoResponse userInfo)
        {
            Uri request = new(SteamStoreURL, "/saleaction/ajaxopendoor");
            Uri referer = new(SteamStoreURL, "/sale/clorthax_quest");

            Dictionary<string, string> data = new(5) {
                { "authwgtoken", userInfo.AuthwgToken },
                { "door_index", "11" },
                { "clan_accountid", "39049601" },
            };

            var response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<AjaxOpenDoorResponse>(request, data: data, referer: referer).ConfigureAwait(false);

            return response?.Content;
        }

        private static List<string> UriList { get; } = new() {
            "category/arcade_rhythm",
            "category/strategy_cities_settlements",
            "category/sports",
            "category/simulation",
            "category/multiplayer_coop",
            "category/casual",
            "category/rpg",
            "category/horror",
            "vr",
            "category/strategy",
        };

        internal static async Task<CapsuleinsertResponse?> FetCapsuleinsert(Bot bot, int index)
        {
            if (index < 0 || index > 9)
            {
                return null;
            }

            Uri request = new(SteamStoreURL, $"/{UriList[index]}/?snr=1_614_615_clorthaxquest_1601");

            var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

            if (response == null)
            {
                return null;
            }

            var configEle = response.Content.SelectSingleNode<IElement>("//div[@id='application_config']");

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
