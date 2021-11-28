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
        //读取商店页Sub
        internal static async Task<StoreResponse?> GetStoreSubs(Bot bot, string type, uint gameID)
        {
            Uri request = new(SteamStoreURL, "/" + type.ToLowerInvariant() + "/" + gameID.ToString() + "/?l=schinese");

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL).ConfigureAwait(false);

            return HtmlParser.ParseStorePage(response);
        }

        //发布评测
        internal static async Task<RecommendGameResponse?> PublishReview(Bot bot, uint gameID, string comment, bool rateUp = true, bool isPublic = true, bool enComment = true, bool forFree = false)
        {
            Uri request = new(SteamStoreURL, "/friends/recommendgame");

            string? language = bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookieValue(SteamStoreURL, "Steam_Language") ??"english";

            Dictionary<string, string> data = new(11, StringComparer.Ordinal)
            {
                { "sessionid", "" },
                { "appid", gameID.ToString() },
                { "steamworksappid", gameID.ToString() },
                { "comment", comment },
                { "rated_up", rateUp ? "true" : "false" },
                { "is_public", "true" },
                { "language", language },
                { "received_compensation", forFree ? "1" : "0" },
                { "disable_comments", enComment ? "0" : "1" },
                { "sessionid", "" },
            };

            ObjectResponse<RecommendGameResponse>? response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<RecommendGameResponse>(request, data: data, referer: SteamStoreURL).ConfigureAwait(false);

            return response?.Content;
        }

        //删除评测
        internal static async Task<bool> DeleteRecommend(Bot bot, uint gameID)
        {
            Uri request = new(SteamStoreURL, string.Format("/profiles/{0}/recommended/", bot.SteamID));

            Dictionary<string, string> data = new(4, StringComparer.Ordinal)
            {
                { "action", "delete" },
                { "sessionid", "" },
                { "appid", gameID.ToString() },
            };

            await bot.ArchiWebHandler.UrlPostWithSession(request, data: data, referer: SteamCommunityURL).ConfigureAwait(false);

            return true;
        }

    }
}
