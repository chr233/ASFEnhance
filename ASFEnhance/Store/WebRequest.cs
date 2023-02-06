using ArchiSteamFarm.Core;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;

namespace ASFEnhance.Store
{
    internal static class WebRequest
    {
        /// <summary>
        /// 读取商店页面SUB
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        internal static async Task<GameStorePageResponse?> GetStoreSubs(Bot bot, SteamGameId gameId)
        {
            return await GetStoreSubs(bot, gameId.Type.ToString(), gameId.GameId).ConfigureAwait(false);
        }

        /// <summary>
        /// 读取商店页面SUB
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="type"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        internal static async Task<GameStorePageResponse?> GetStoreSubs(Bot bot, string type, uint gameId)
        {
            Uri request = new(SteamStoreURL, "/" + type.ToLowerInvariant() + "/" + gameId.ToString() + "/?l=schinese");

            var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL).ConfigureAwait(false);

            return HtmlParser.ParseStorePage(response);
        }

        /// <summary>
        /// 获取App详情
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        internal static async Task<AppDetailResponse?> GetAppDetails(Bot bot, uint appId)
        {
            string key = appId.ToString();

            Uri request = new(SteamStoreURL, "/api/appdetails?appids=" + key);

            var response = await bot.ArchiWebHandler.UrlGetToJsonObjectWithSession<Dictionary<string, AppDetailResponse>>(request, referer: SteamStoreURL).ConfigureAwait(false);

            return response?.Content?[key];
        }

        /// <summary>
        /// 发布游戏评测
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="gameId"></param>
        /// <param name="comment"></param>
        /// <param name="rateUp"></param>
        /// <param name="isPublic"></param>
        /// <param name="enComment"></param>
        /// <param name="forFree"></param>
        /// <returns></returns>
        internal static async Task<RecommendGameResponse?> PublishReview(Bot bot, uint gameId, string comment, bool rateUp = true, bool isPublic = true, bool enComment = true, bool forFree = false)
        {
            Uri request = new(SteamStoreURL, "/friends/recommendgame");
            Uri referer = new(SteamStoreURL, $"/app/{gameId}");

            string? language = bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookieValue(SteamStoreURL, "Steam_Language") ?? "english";

            Dictionary<string, string> data = new(11, StringComparer.Ordinal)
            {
                { "appid", gameId.ToString() },
                { "steamworksappid", gameId.ToString() },
                { "comment", comment+'\u200D' },
                { "rated_up", rateUp ? "true" : "false" },
                { "is_public", isPublic ? "true" : "false" },
                { "language", language },
                { "received_compensation", forFree ? "1" : "0" },
                { "disable_comments", enComment ? "0" : "1" },
            };

            var response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<RecommendGameResponse>(request, data: data, referer: referer).ConfigureAwait(false);

            return response?.Content;
        }

        /// <summary>
        /// 删除游戏评测
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        internal static async Task<bool> DeleteRecommend(Bot bot, uint gameId)
        {
            Uri request = new(SteamCommunityURL, $"/profiles/{bot.SteamID}/recommended/");
            Uri referer = new(request, $"/{gameId}/");

            Dictionary<string, string> data = new(3, StringComparer.Ordinal)
            {
                { "action", "delete" },
                { "appid", gameId.ToString() },
            };

            await bot.ArchiWebHandler.UrlPostWithSession(request, data: data, referer: referer).ConfigureAwait(false);

            return true;
        }

        /// <summary>
        /// 搜索游戏
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        internal static async Task<string?> SearchGame(Bot bot, string keyWord)
        {
            Uri request = new(SteamStoreURL, $"/search/?term={keyWord}");

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL).ConfigureAwait(false);

            return HtmlParser.ParseSearchPage(response);
        }

        /// <summary>
        /// 请求访问权限
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        internal static async Task<AjaxRequestAccessResponse?> RequestAccess(Bot bot, ulong gameId)
        {
            Uri request = new(SteamStoreURL, $"/ajaxrequestplaytestaccess/{gameId}");
            Uri referer = new(SteamStoreURL, $"/app/{gameId}/");

            Dictionary<string, string> data = new(1, StringComparer.Ordinal);

            var response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<AjaxRequestAccessResponse>(request, data: data, referer: referer).ConfigureAwait(false);

            return response?.Content;
        }

        /// <summary>
        /// 访问链接
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        internal static async Task<string?> FetchPage(Bot bot, Uri request)
        {
            var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);
            if (response?.Content == null)
            {
                return null;
            }

            return $"[{response.StatusCode}] {response.Content.Title}";
        }
    }
}
