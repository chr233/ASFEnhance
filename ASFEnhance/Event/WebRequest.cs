using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;
using ProtoBuf;
using System.Text;

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
    /// 提名载荷
    /// </summary>
    [ProtoContract]
    internal sealed record NominatePayload
    {
        [ProtoMember(1)]
        public int CategoryId { get; set; }
        [ProtoMember(2)]
        public int NominatedId { get; set; }
        [ProtoMember(3)]
        public int Source { get; set; }
    }

    /// <summary>
    /// 获取AccessToken
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> FetchWebApiToken(Bot bot)
    {
        var request = new Uri(SteamStoreURL, "/steamawards/nominations");
        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL).ConfigureAwait(false);

        if (response == null)
        {
            return null;
        }

        var configEle = response?.Content?.QuerySelector<IElement>("#application_config");
        string community = configEle?.GetAttribute("data-store_user_config") ?? "";
        var match = RegexUtils.MatchWebApiToken().Match(community);

        return match.Success ? match.Groups[1].Value : null;
    }

    /// <summary>
    /// 秋促投票
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="gameID"></param>
    /// <param name="categoryID"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    internal static async Task<bool?> MakeVote(Bot bot, uint gameID, int categoryID, string token)
    {
        var payload = new NominatePayload
        {
            CategoryId = categoryID,
            NominatedId = (int)gameID,
            Source = 3,
        };
        var enc = ProtoBufEncode(payload);

        var request = new Uri(SteamApiURL, $"ISteamAwardsService/Nominate/v1?access_token={token}&origin=https://store.steampowered.com&input_protobuf_encoded={enc}");

        await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL).ConfigureAwait(false);

        return true;
    }

    /// <summary>
    /// 检查秋促徽章
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string> CheckSummerBadge(Bot bot)
    {
        var request = new Uri(SteamCommunityURL, "/profiles/" + bot.SteamID.ToString() + "/badges/65");

        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamCommunityURL).ConfigureAwait(false);

        if (response?.Content == null)
        {
            return Langs.EventReadBadgeStatusFailed;
        }

        var taskStatus = new string[] {
            Langs.VoteNominateAtLeastOne,
            Langs.VoteNominateEachCategory,
            Langs.VotePlayGameYouNominated,
            Langs.VoteReviewGameYouNominated
        };

        var sb = new StringBuilder();

        for (int i = 0; i < 4; i++)
        {
            var xpath = $"//div[@class='badge_task'][{i + 1}]/img";
            var eleTask = response.Content.SelectSingleNode<IElement>(xpath);
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

            sb.AppendLineFormat(taskStatus[i], status);
        }
        return sb.ToString();
    }
}
