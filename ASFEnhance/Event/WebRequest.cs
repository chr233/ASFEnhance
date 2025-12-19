using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Helpers.Json;
using ArchiSteamFarm.Steam;
using ASFEnhance.Data;
using ASFEnhance.Data.Common;
using ASFEnhance.Data.ILoyaltyRewardsService;
using System.Text;

namespace ASFEnhance.Event;

internal static class WebRequest
{
    /// <summary>
    ///     模拟做题
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="clanAccountId"></param>
    /// <param name="doorIndex"></param>
    /// <returns></returns>
    internal static async Task DoEventTask(Bot bot, string clanAccountId, int doorIndex)
    {
        var request = new Uri(SteamStoreURL, "/saleaction/ajaxopendoor");

        var data = new Dictionary<string, string>(3)
        {
            { "door_index", doorIndex.ToString() }, { "clan_accountid", clanAccountId }
        };

        _ = await bot.ArchiWebHandler.UrlPostWithSession(request, data: data).ConfigureAwait(false);
    }

    /// <summary>
    ///     领取活动道具
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    internal static async Task<ClaimItemResponse?> ClaimDailySticker(Bot bot, string token)
    {
        var request = new Uri(SteamApiURL, $"/ISaleItemRewardsService/ClaimItem/v1?access_token={token}");
        var referer = new Uri(SteamStoreURL, "/sale/16212626125");

        var response = await bot.ArchiWebHandler.UrlPostToJsonObject<AbstractResponse<ClaimItemResponse>>(request, null, referer)
            .ConfigureAwait(false);

        return response?.Content?.Response;
    }

    /// <summary>
    ///     领取点数商店物品
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="token"></param>
    /// <param name="defId"></param>
    /// <returns></returns>
    internal static async Task<bool> RedeenPointShopItem(Bot bot, string token, int defId)
    {
        var request = new Uri(SteamApiURL, $"/ILoyaltyRewardsService/RedeemPoints/v1?access_token={token}");

        var data = new Dictionary<string, string>(1) { { "defId", defId.ToString() } };

        var response = await bot.ArchiWebHandler.UrlPost(request, data, SteamStoreURL).ConfigureAwait(false);

        return response;
    }

    /// <summary>
    ///     秋促投票
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="gameID"></param>
    /// <param name="categoryId"></param>
    /// <param name="token"></param>
    /// <param name="semaphore"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    internal static async Task MakeVoteForAutumnSale(Bot bot, int gameID, int categoryId, string token,
        SemaphoreSlim semaphore, uint source = 6)
    {
        try
        {
            await semaphore.WaitAsync().ConfigureAwait(false);
            var request = new Uri(SteamApiURL,
                $"/ISteamAwardsService/Nominate/v1?access_token={token}&category_id={categoryId}&nominated_id={gameID}&store_appid={gameID}&source={source}");

            await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL)
                .ConfigureAwait(false);
        }
        finally
        {
            await Task.Delay(500).ConfigureAwait(false);
            semaphore.Release();
        }
    }

    /// <summary>
    ///     检查秋促徽章
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string> CheckAutumnSaleBadge(Bot bot)
    {
        var request = new Uri(SteamCommunityURL, "/profiles/" + bot.SteamID + "/badges/70");

        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamCommunityURL)
            .ConfigureAwait(false);

        if (response?.Content == null)
        {
            return Langs.EventReadBadgeStatusFailed;
        }

        var taskStatus = new[]
        {
            Langs.VoteNominateAtLeastOne, Langs.VoteNominateEachCategory, Langs.VotePlayGameYouNominated,
            Langs.VoteReviewGameYouNominated
        };

        var sb = new StringBuilder();

        for (var i = 0; i < 4; i++)
        {
            var eleTask = response.Content.QuerySelector($"div.badge_task:nth-child({i + 1})>img");
            var taskSrc = eleTask?.GetAttribute("src") ?? "";

            char status;

            if (string.IsNullOrEmpty(taskSrc))
            {
                ASF.ArchiLogger.LogNullError(string.Format("{0}", i));
                status = '?';
            }
            else
            {
                status = taskSrc.EndsWith("_on.png") ? '√' : '×';
            }

            sb.AppendLineFormat(Langs.CookieItem, taskStatus[i], status);
        }

        return sb.ToString();
    }

    /// <summary>
    ///     冬促投票
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="appId"></param>
    /// <param name="voteId"></param>
    /// <param name="token"></param>
    /// 3
    /// <param name="semaphore"></param>
    /// <param name="saleId"></param>
    /// <returns></returns>
    internal static async Task MakeWinterSteamAwardVote(Bot bot, int appId, int voteId, string token,
        SemaphoreSlim semaphore, uint saleId)
    {
        try
        {
            await semaphore.WaitAsync().ConfigureAwait(false);
            var data = new Dictionary<string, string>(1, StringComparer.Ordinal)
            {
                { "voteid", voteId.ToString() },
                { "appid", appId.ToString() },
                { "sale_appid", saleId.ToString() },
            };
            var request = new Uri(SteamApiURL, $"/IStoreSalesService/SetVote/v1?access_token={token}");

            await bot.ArchiWebHandler.UrlPost(request, data, SteamStoreURL).ConfigureAwait(false);
        }
        finally
        {
            await Task.Delay(500).ConfigureAwait(false);
            semaphore.Release();
        }
    }

    /// <summary>
    ///     检查冬促投票
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> CheckWinterSteamAwardVote(Bot bot)
    {
        var year = DateTime.Now.Year;

        var request = new Uri(SteamStoreURL, $"/steamawards/{year}");
        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

        if (response == null)
        {
            return null;
        }

        var configEle = response.Content?.QuerySelector<IElement>("#application_config");
        var config = configEle?.GetAttribute("data-steam_awards_config") ?? "";

        var data = config.ToJsonObject<SteamAwardVoteData>();

        if (data == null)
        {
            return Langs.NetworkError;
        }

        return string.Format(Langs.CheckVote, data.UserVotes?.Count ?? -1, data.Definitions?.Votes?.Count ?? -1);
    }

    /// <summary>
    /// 获取点数商店物品
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    /// <exception cref="AccessTokenNullException"></exception>
    internal static async Task<QueryRewardItemsResponse?> QueryRewardItems(Bot bot)
    {
        var token = bot.AccessToken ?? throw new AccessTokenNullException(bot);
        var request = new Uri(SteamApiURL, $"/ILoyaltyRewardsService/QueryRewardItems/v1/?access_token={token}");

        var response = await bot.ArchiWebHandler.UrlGetToJsonObjectWithSession<AbstractResponse<QueryRewardItemsResponse>>(request).ConfigureAwait(false);

        return response?.Content?.Response;
    }

}