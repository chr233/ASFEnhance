using ArchiSteamFarm.Core;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;
using ASFEnhance.Data.Common;
using ASFEnhance.Data.IStoreBrowseService;
using ASFEnhance.Data.Plugin;
using System.Text.Json;

namespace ASFEnhance.Store;

internal static class WebRequest
{
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
    internal static async Task<RecommendGameResponse?> PublishReview(this Bot bot, uint gameId, string comment, bool rateUp = true, bool isPublic = true, bool enComment = true, bool forFree = false)
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
    internal static async Task<bool> DeleteRecommend(this Bot bot, uint gameId)
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
    internal static async Task<string?> SearchGame(this Bot bot, string keyWord)
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
    internal static async Task<AjaxRequestAccessResponse?> RequestAccess(this Bot bot, ulong gameId)
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
    internal static async Task<string?> FetchPage(this Bot bot, Uri request)
    {
        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);
        if (response?.Content == null)
        {
            return null;
        }

        return $"[{response.StatusCode}] {response.Content.Title}";
    }

    /// <summary>
    /// 兑换点数徽章
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="defId"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    internal static async Task<string> RedeemPointsForBadgeLevel(this Bot bot, uint defId, uint level)
    {
        var request = new Uri(SteamApiURL, "/ILoyaltyRewardsService/RedeemPointsForBadgeLevel/v1/");
        var data = new Dictionary<string, string>(3) {
            { "access_token", bot.AccessToken ?? throw new AccessTokenNullException() },
            { "defid", defId.ToString() },
            { "num_levels", level.ToString() },
        };

        var response = await bot.ArchiWebHandler.UrlPost(request, data, SteamStoreURL).ConfigureAwait(false);

        return response ? Langs.Done : Langs.Failure;
    }

    /// <summary>
    /// 兑换点数物品
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="defId"></param>
    /// <returns></returns>
    internal static async Task<string> RedeemPoints(this Bot bot, uint defId)
    {
        var request = new Uri(SteamApiURL, "/ILoyaltyRewardsService/RedeemPoints/v1/");
        var data = new Dictionary<string, string>(2) {
            { "access_token", bot.AccessToken ?? throw new AccessTokenNullException() },
            { "defid", defId.ToString() }
        };

        var response = await bot.ArchiWebHandler.UrlPost(request, data, SteamStoreURL).ConfigureAwait(false);

        return response ? Langs.Done : Langs.Failure;
    }

    /// <summary>
    /// 获取游戏信息
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="gameIds"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    internal static Task<GetItemsResponse?> GetStoreItems(this Bot bot, IEnumerable<SteamGameId> gameIds)
    {
        var ids = new List<IdData>(gameIds.Count());
        foreach (var gameId in gameIds)
        {
            var id = gameId.Type switch
            {
                ESteamGameIdType.App => new IdData { AppId = gameId.Id },
                ESteamGameIdType.Sub => new IdData { PackageId = gameId.Id },
                ESteamGameIdType.Bundle => new IdData { BundleId = gameId.Id },
                _ => throw new NotImplementedException(),
            };
            ids.Add(id);
        }

        return bot.GetStoreItems(ids);
    }

    /// <summary>
    /// 获取游戏信息
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="gameIds"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    internal static async Task<GetItemsResponse?> GetStoreItems(this Bot bot, IEnumerable<IdData> gameIds)
    {
        if (!gameIds.Any())
        {
            return null;
        }

        var payload = new GetItemsRequest
        {
            Ids = gameIds.ToList(),
            Context = new GetItemsRequest.ContextData
            {
                Language = Langs.Language,
                CountryCode = Langs.CountryCode,
                SteamRealm = "1",
            },
            DataRequest = new GetItemsRequest.DataRequestData
            {
                IncludeAllPurchaseOptions = true,
                IncludeFullDescription = true,
            },
        };

        var json = JsonSerializer.Serialize(payload, JsonOptions);
        var encJson = UrlEncode(json);
        var token = bot.AccessToken ?? throw new AccessTokenNullException();
        var request = new Uri(SteamApiURL, $"/IStoreBrowseService/GetItems/v1/?access_token={token}&input_json={encJson}");

        var response = await bot.ArchiWebHandler.UrlGetToJsonObjectWithSession<AbstractResponse<GetItemsResponse>>(request).ConfigureAwait(false);

        return response?.Content?.Response;
    }
}
