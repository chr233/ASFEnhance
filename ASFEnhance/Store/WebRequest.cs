#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Core;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Web.Responses;
using Chrxw.ASFEnhance.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Chrxw.ASFEnhance.Store.Response;
using static Chrxw.ASFEnhance.Utils;

namespace Chrxw.ASFEnhance.Store
{
    internal static class WebRequest
    {
        /// <summary>
        /// 读取商店页面SUB
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="gameID"></param>
        /// <returns></returns>
        internal static async Task<StoreResponse?> GetStoreSubs(Bot bot, SteamGameID gameID)
        {
            return await GetStoreSubs(bot, gameID.Type.ToString(), gameID.GameID).ConfigureAwait(false);
        }

        /// <summary>
        /// 读取商店页面SUB
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="type"></param>
        /// <param name="gameID"></param>
        /// <returns></returns>
        internal static async Task<StoreResponse?> GetStoreSubs(Bot bot, string type, uint gameID)
        {
            Uri request = new(SteamStoreURL, "/" + type.ToLowerInvariant() + "/" + gameID.ToString() + "/?l=schinese");

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL).ConfigureAwait(false);

            return HtmlParser.ParseStorePage(response);
        }


        /// <summary>
        /// 获取App详情
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="appID"></param>
        /// <returns></returns>
        internal static async Task<AppDetailResponse?> GetAppDetails(Bot bot, uint appID)
        {
            string key = appID.ToString();

            Uri request = new(SteamStoreURL, "/api/appdetails?appids=" + key);

            ObjectResponse<Dictionary<string, AppDetailResponse>>? response = await bot.ArchiWebHandler.UrlGetToJsonObjectWithSession<Dictionary<string, AppDetailResponse>>(request, referer: SteamStoreURL).ConfigureAwait(false);

            if (response != null && response.Content.ContainsKey(key))
            {
                return response.Content[key];
            }

            return null;
        }



        /// <summary>
        /// 发布游戏评测
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="gameID"></param>
        /// <param name="comment"></param>
        /// <param name="rateUp"></param>
        /// <param name="isPublic"></param>
        /// <param name="enComment"></param>
        /// <param name="forFree"></param>
        /// <returns></returns>
        internal static async Task<RecommendGameResponse?> PublishReview(Bot bot, uint gameID, string comment, bool rateUp = true, bool isPublic = true, bool enComment = true, bool forFree = false)
        {
            Uri request = new(SteamStoreURL, "/friends/recommendgame");

            string? language = bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookieValue(SteamStoreURL, "Steam_Language") ?? "english";

            Dictionary<string, string> data = new(11, StringComparer.Ordinal)
            {
                { "appid", gameID.ToString() },
                { "steamworksappid", gameID.ToString() },
                { "comment", comment },
                { "rated_up", rateUp ? "true" : "false" },
                { "is_public", isPublic ? "true" : "false" },
                { "language", language },
                { "received_compensation", forFree ? "1" : "0" },
                { "disable_comments", enComment ? "0" : "1" },
                //{ "sessionid", "" },
            };

            ObjectResponse<RecommendGameResponse>? response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<RecommendGameResponse>(request, data: data, referer: SteamStoreURL).ConfigureAwait(false);

            return response?.Content;
        }

        /// <summary>
        /// 删除游戏评测
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="gameID"></param>
        /// <returns></returns>
        internal static async Task<bool> DeleteRecommend(Bot bot, uint gameID)
        {
            Uri request = new(SteamStoreURL, string.Format("/profiles/{0}/recommended/", bot.SteamID));

            Dictionary<string, string> data = new(4, StringComparer.Ordinal)
            {
                { "action", "delete" },
                //{ "sessionid", "" },
                { "appid", gameID.ToString() },
            };

            await bot.ArchiWebHandler.UrlPostWithSession(request, data: data, referer: SteamCommunityURL).ConfigureAwait(false);

            return true;
        }

    }
}
