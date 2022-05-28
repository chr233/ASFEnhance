#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Localization;
using Newtonsoft.Json;
using static ASFEnhance.Utils;

namespace ASFEnhance.Event
{
    internal static class WebRequest
    {
        /// <summary>
        /// 竞速游戏节活动
        /// 5.23-5.30
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> FetchSecrets(Bot bot)
        {
            Uri request = new(SteamStoreURL, "/category/racing/");

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL).ConfigureAwait(false);

            IElement? configELement = response?.Content.SelectSingleNode("//div[@id='application_config']");

            string? payload = configELement?.GetAttribute("data-userinfo");

            if (string.IsNullOrEmpty(payload))
            {
                return null;
            }

            try
            {
                UserInfoResponse userInfo = JsonConvert.DeserializeObject<UserInfoResponse>(payload);
                return userInfo.AuthwgToken;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取竞速节徽章
        /// 
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="authwgToken"></param>
        /// <returns></returns>
        internal static async Task<bool?> RacingFestivalBadge(Bot bot, string authwgToken)
        {
            Uri request = new(SteamStoreURL, "/saleaction/ajaxopendoor");


            Dictionary<string, string> data = new(3, StringComparer.Ordinal)
            {
                { "authwgtoken", authwgToken },
                { "door_index", "5" },
            };
            await bot.ArchiWebHandler.UrlPostWithSession(request, data: data, referer: SteamStoreURL).ConfigureAwait(false);

            return true;
        }
    }
}
