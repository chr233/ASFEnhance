using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Data;
using ASFEnhance.Data;
using SteamKit2;

namespace ASFEnhance.Wishlist;

internal static class WebRequest
{
    /// <summary>
    /// 添加愿望单
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="gameId"></param>
    /// <returns></returns>
    internal static async Task<bool> AddWishlist(this Bot bot, uint gameId)
    {
        var request = new Uri(SteamStoreURL, "/api/addtowishlist");
        var referer = new Uri(SteamStoreURL, "/app/" + gameId);

        var data = new Dictionary<string, string>(2, StringComparer.Ordinal)
        {
            { "appid", gameId.ToString() },
        };

        var response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<ResultResponse>(request, data: data, referer: referer).ConfigureAwait(false);

        if (response == null)
        {
            return false;
        }

        if (response?.Content?.Result != EResult.OK)
        {
            bot.ArchiLogger.LogGenericWarning(Strings.WarningFailed);
            return false;
        }
        return true;
    }

    /// <summary>
    /// 删除愿望单
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="gameId"></param>
    /// <returns></returns>
    internal static async Task<bool> RemoveWishlist(this Bot bot, uint gameId)
    {
        var request = new Uri(SteamStoreURL, "/api/removefromwishlist");
        var referer = new Uri(SteamStoreURL, $"/app/{gameId}");

        var data = new Dictionary<string, string>(2, StringComparer.Ordinal)
        {
            { "appid", gameId.ToString() },
        };

        var response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<ResultResponse>(request, data: data, referer: referer).ConfigureAwait(false);

        if (response == null)
        {
            return false;
        }

        if (response?.Content?.Result != EResult.OK)
        {
            bot.ArchiLogger.LogGenericWarning(Strings.WarningFailed);
            return false;
        }
        return true;
    }

    /// <summary>
    /// 关注游戏
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="gameId"></param>
    /// <param name="isFollow"></param>
    /// <returns></returns>
    internal static async Task<bool> FollowGame(this Bot bot, uint gameId, bool isFollow)
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
    internal static async Task<CheckGameResponse> CheckGame(this Bot bot, uint gameId)
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
}
