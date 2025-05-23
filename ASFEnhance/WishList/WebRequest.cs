using ArchiSteamFarm.Steam;
using ASFEnhance.Data;
using ASFEnhance.Data.IWishlistService;
using ASFEnhance.Data.WebApi;

namespace ASFEnhance.WishList;

/// <summary>
/// 网络请求
/// </summary>
public static class WebRequest
{
    /// <summary>
    /// 添加愿望单
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="gameId"></param>
    /// <param name="isAdd"></param>
    /// <returns></returns>
    public static async Task<IgnoreGameResponse?> AddWishlist(this Bot bot, uint gameId, bool isAdd)
    {
        var request = new Uri(SteamStoreURL, isAdd ? "/api/addtowishlist" : "/api/removefromwishlist");
        var referer = new Uri(SteamStoreURL, "/app/" + gameId);

        var data = new Dictionary<string, string>(2, StringComparer.Ordinal)
        {
            { "appid", gameId.ToString() },
        };

        var response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<IgnoreGameResponse>(request, data: data, referer: referer).ConfigureAwait(false);
        return response?.Content;
    }

    /// <summary>
    /// 关注游戏
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="gameId"></param>
    /// <param name="isFollow"></param>
    /// <returns></returns>
    public static async Task<bool> FollowGame(this Bot bot, uint gameId, bool isFollow)
    {
        var request = new Uri(SteamStoreURL, "/explore/followgame/");
        var referer = new Uri(SteamStoreURL, $"/app/{gameId}");

        var data = new Dictionary<string, string>(3, StringComparer.Ordinal)
        {
            { "appid", gameId.ToString() },
        };

        if (!isFollow)
        {
            data.Add("unfollow", "1");
        }

        var response = await bot.ArchiWebHandler.UrlPostToHtmlDocumentWithSession(request, data: data, referer: referer).ConfigureAwait(false);

        if (response == null)
        {
            return false;
        }

        return response?.Content?.Body?.InnerHtml.ToLowerInvariant() == "true";
    }

    /// <summary>
    /// 检查关注/愿望单情况
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="gameId"></param>
    /// <returns></returns>
    public static async Task<CheckGameResponse> CheckGame(this Bot bot, uint gameId)
    {
        var request = new Uri(SteamStoreURL, $"/app/{gameId}");

        var response = await bot.ArchiWebHandler.UrlPostToHtmlDocumentWithSession(request).ConfigureAwait(false);

        if (response == null)
        {
            return new(false, "网络错误");
        }

        if (response.FinalUri.LocalPath.Equals("/"))
        {
            return new(false, "商店页未找到");
        }

        return HtmlParser.ParseStorePage(response);
    }

    /// <summary>
    /// 忽略指定游戏
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="gameId"></param>
    /// <param name="isIgnore"></param>
    /// <returns></returns>
    internal static async Task<bool> IgnoreGame(this Bot bot, uint gameId, bool isIgnore)
    {
        var request = new Uri(SteamStoreURL, "/recommended/ignorerecommendation/");
        var referer = new Uri(SteamStoreURL, $"/app/{gameId}");

        var data = new Dictionary<string, string>(3, StringComparer.Ordinal)
        {
            { "appid", gameId.ToString() },
        };

        if (isIgnore)
        {
            data.Add("ignore_reason", "0");
        }
        else
        {
            data.Add("remove", "1");
        }

        var response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<IgnoreGameResponse>(request, data: data, referer: referer).ConfigureAwait(false);

        if (response == null)
        {
            return false;
        }

        return response?.Content?.Result == true;
    }

    /// <summary>
    /// 获取愿望单
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    /// <exception cref="AccessTokenNullException"></exception>
    public static async Task<GetWishlistResponse?> GetWishlistGames(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return null;
        }

        var token = bot.AccessToken ?? throw new AccessTokenNullException(bot);
        var request = new Uri(SteamApiURL, $"/IWishlistService/GetWishlist/v1/?access_token={token}&steamid={bot.SteamID}");

        var response = await bot.ArchiWebHandler.UrlGetToJsonObjectWithSession<AbstractResponse<GetWishlistResponse>>(request).ConfigureAwait(false);

        return response?.Content?.Response;
    }

    /// <summary>
    /// 获取愿望单数量
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    /// <exception cref="AccessTokenNullException"></exception>
    public static async Task<int> GetWishlistGamesCount(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return -1;
        }

        var token = bot.AccessToken ?? throw new AccessTokenNullException(bot);
        var request = new Uri(SteamApiURL, $"/IWishlistService/GetWishlistItemCount/v1/?access_token={token}&steamid={bot.SteamID}");

        var response = await bot.ArchiWebHandler.UrlGetToJsonObjectWithSession<AbstractResponse<GetWishlistCountResponse>>(request).ConfigureAwait(false);

        return response?.Content?.Response?.Count ?? -1;
    }
}
