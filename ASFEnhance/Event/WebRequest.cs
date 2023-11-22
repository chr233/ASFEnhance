using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;
using ArchiSteamFarm.Web.Responses;
using System.Text;
using static ASFEnhance.Event.Command;

namespace ASFEnhance.Event;

internal static class WebRequest
{
    /// <summary>
    /// 模拟做题
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="clan_accountid"></param>
    /// <param name="door_index"></param>
    /// <returns></returns>
    internal static async Task DoEventTask(Bot bot, string clan_accountid, uint door_index)
    {
        var request = new Uri(SteamStoreURL, "/saleaction/ajaxopendoor");

        var data = new Dictionary<string, string>(3) {
            {"door_index", door_index.ToString()},
            {"clan_accountid", clan_accountid},
        };

        _ = await bot.ArchiWebHandler.UrlPostWithSession(request, data: data).ConfigureAwait(false);
    }

    /// <summary>
    /// 获取Token
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="salePage"></param>
    /// <returns></returns>
    internal static async Task<string?> FetchEventToken(Bot bot, string salePage)
    {
        var request = new Uri(SteamStoreURL, $"/sale/{salePage}");

        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

        if (response == null)
        {
            return null;
        }

        var configEle = response?.Content?.QuerySelector<IElement>("#application_config");
        string community = configEle?.GetAttribute("data-community") ?? "";
        var match = RegexUtils.MatchClanaCCountId().Match(community);

        return match.Success ? match.Groups[1].Value : null;
    }

    /// <summary>
    /// 获取Token
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="developer"></param>
    /// <param name="salePage"></param>
    /// <returns></returns>
    internal static async Task<string?> FetchEventToken(Bot bot, string developer, string salePage)
    {
        var request = new Uri(SteamStoreURL, $"/developer/{developer}/sale/{salePage}");

        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

        if (response == null)
        {
            return null;
        }

        var configEle = response?.Content?.QuerySelector<IElement>("#application_config");
        string community = configEle?.GetAttribute("data-community") ?? "";
        var match = RegexUtils.MatchClanaCCountId().Match(community);

        return match.Success ? match.Groups[1].Value : null;
    }

    /// <summary>
    /// 获取Token
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> FetchToken(Bot bot)
    {
        var request = new Uri(SteamStoreURL, "/category/sports");

        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: request).ConfigureAwait(false);

        if (response == null)
        {
            return null;
        }

        var configEle = response?.Content?.QuerySelector<IElement>("#application_config");
        string community = configEle?.GetAttribute("data-loyalty_webapi_token") ?? "";
        var match = RegexUtils.MatchToken().Match(community);

        return match.Success ? match.Groups[1].Value : null;
    }

    /// <summary>
    /// 领取活动道具
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    internal static async Task<bool> ClaimDailySticker(Bot bot, string token)
    {
        var request = new Uri(SteamApiURL, $"/ISaleItemRewardsService/ClaimItem/v1?access_token={token}");
        var referer = new Uri(SteamStoreURL, "/sale/16212626125");

        await bot.ArchiWebHandler.UrlPostWithSession(request, referer: referer, session: ArchiWebHandler.ESession.None).ConfigureAwait(false);

        return true;
    }

    /// <summary>
    /// 领取点数商店物品
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="token"></param>
    /// <param name="defId"></param>
    /// <returns></returns>
    internal static async Task<bool> RedeenPointShopItem(Bot bot, string token, int defId)
    {
        var request = new Uri(SteamApiURL, $"/ILoyaltyRewardsService/RedeemPoints/v1?access_token={token}");

        var data = new Dictionary<string, string>(1) {
            {"defId", defId.ToString()},
        };

        var result = await bot.ArchiWebHandler.UrlPostWithSession(request, referer: SteamStoreURL, data: data, session: ArchiWebHandler.ESession.None).ConfigureAwait(false);

        return result;
    }

    /// <summary>
    /// 秋促投票
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="gameID"></param>
    /// <param name="categoryID"></param>
    /// <returns></returns>
    internal static async Task<bool?> MakeVote(Bot bot, uint gameID, int categoryID)
    {
        var request = new Uri(SteamStoreURL, "/steamawards/nominategame");
        var referer = new Uri(SteamStoreURL, "/steamawards/category/63");

        var sessionID = bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookieValue(SteamStoreURL, "sessionid");

        if (string.IsNullOrEmpty(sessionID))
        {
            bot.ArchiLogger.LogNullError(nameof(sessionID));
            return false;
        }

        var data = new Dictionary<string, string>(5, StringComparer.Ordinal)
            {
                { "sessionid", sessionID! },
                { "nominatedid", gameID.ToString() },
                { "categoryid", categoryID.ToString() },
                { "source", "3" },
            };

        await bot.ArchiWebHandler.UrlPostWithSession(request, data: data, referer: referer).ConfigureAwait(false);

        return true;
    }

    /// <summary>
    /// 检查秋促徽章
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<SummerBadgeResponse?> CheckSummerBadge(Bot bot)
    {
        var request = new Uri(SteamCommunityURL, "/profiles/" + bot.SteamID.ToString() + "/badges/56");

        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamCommunityURL).ConfigureAwait(false);

        if (response?.Content == null)
        {
            return null;
        }

        var taskStatus = new bool[] { false, false, false, false };

        var sb = new StringBuilder();

        for (int i = 0; i < 4; i++)
        {
            var xpath = $"//div[@class='badge_task'][{i}]/img";
            var eleTask = response.Content.SelectSingleNode<IElement>(xpath);
            var taskSrc = eleTask?.GetAttribute("src") ?? "";

            if (string.IsNullOrEmpty(taskSrc))
            {
                ASF.ArchiLogger.LogNullError(string.Format("{0}", i));
                continue;
            }

            taskStatus[i] = taskSrc.EndsWith("_on.png");
        }
        return new SummerBadgeResponse(taskStatus[0], taskStatus[1], taskStatus[2], taskStatus[3]);
    }
}

/// <summary>
/// 秋促徽章信息
/// </summary>
internal sealed record SummerBadgeResponse
{
    /// <summary>
    /// 提名一项奖项
    /// </summary>
    public bool VoteOne { get; set; }
    //提名全部奖项
    public bool VoteAll { get; set; }
    //玩一款提名游戏
    public bool PlayOne { get; set; }
    //评测一款提名游戏
    public bool ReviewOne { get; set; }
    public SummerBadgeResponse(bool VoteOne = false, bool VoteAll = false, bool PlayOne = false, bool ReviewOne = false)
    {
        this.VoteOne = VoteOne;
        this.VoteAll = VoteAll;
        this.PlayOne = PlayOne;
        this.ReviewOne = ReviewOne;
    }
}
