using AngleSharp.Dom;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;
using ArchiSteamFarm.Web.Responses;
using System.Text.RegularExpressions;
using static ASFEnhance.Utils;

namespace ASFEnhance.Event
{
    internal static partial class WebRequest
    {
        [GeneratedRegex("\"CLANACCOUNTID\":(\\d+),")]
        private static partial Regex MatchClanaCCountId();

        /// <summary>
        /// 模拟做题
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="clan_accountid"></param>
        /// <param name="door_index"></param>
        /// <returns></returns>
        internal static async Task DoEventTask(Bot bot, string clan_accountid, uint door_index)
        {
            Uri request = new(SteamStoreURL, "/saleaction/ajaxopendoor");

            Dictionary<string, string> data = new(3) {
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
            Uri request = new(SteamStoreURL, $"/sale/{salePage}");

            var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

            if (response == null)
            {
                return null;
            }

            var configEle = response?.Content?.QuerySelector<IElement>("#application_config");
            string community = configEle?.GetAttribute("data-community") ?? "";
            var match = MatchClanaCCountId().Match(community);

            return match.Success ? match.Groups[1].Value : null;
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="salePage"></param>
        /// <returns></returns>
        internal static async Task<string?> FetchSteamDeckEventToken(Bot bot)
        {
            Uri request = new(SteamStoreURL, $"/sale/thegameawardssteamdeckdrop2022");

            var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

            if (response?.Content == null)
            {
                return null;
            }

            var token = response.Content.QuerySelector("#application_config")?.GetAttribute("data-loyalty_webapi_token");

            return token?.Replace("\"", "");
        }

        /// <summary>
        /// 领取贴纸
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="clan_accountid"></param>
        /// <param name="door_index"></param>
        /// <returns></returns>
        internal static async Task ClaimSteamDeckStick(Bot bot, string token)
        {
            Uri request = new($"https://api.steampowered.com/ISaleItemRewardsService/ClaimItem/v1?access_token={token}");

            Dictionary<string, string> data = new(1) {
                {"input_protobuf_encoded", "CghzY2hpbmVzZQ=="},
            };

            _ = await bot.ArchiWebHandler.UrlPostWithSession(request, data: data, session: ArchiSteamFarm.Steam.Integration.ArchiWebHandler.ESession.None).ConfigureAwait(false);
        }

        internal const int AllVotes = 11;

        /// <summary>
        /// 冬促投票
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="gameID"></param>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        internal static async Task<bool?> MakeVote(Bot bot, uint gameID, int categoryID)
        {
            Uri request = new(SteamStoreURL, "/salevote");
            Uri referer = new(SteamStoreURL, "/steamawards");

            Dictionary<string, string> data = new(4, StringComparer.Ordinal)
            {
                { "appid", gameID.ToString() },
                { "voteid", categoryID.ToString() },
                { "developerid", "0" },
            };

            await bot.ArchiWebHandler.UrlPostWithSession(request, data: data, referer: referer).ConfigureAwait(false);

            return true;
        }

        /// <summary>
        /// 检查冬促投票
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<int> CheckSummerBadge(Bot bot)
        {
            Uri request = new(SteamStoreURL, "/steamawards/2021");

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamCommunityURL).ConfigureAwait(false);

            if (response?.Content == null)
            {
                return -1;
            }

            int votes = AllVotes - response.Content.QuerySelectorAll("div.steamawards_shortcuts_ctn>div.steamawards_card button.award_card_btn").Length;

            return votes;
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> FetchToken(Bot bot)
        {
            Uri request = new(SteamStoreURL, "/category/sports");

            var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: request).ConfigureAwait(false);

            if (response == null)
            {
                return null;
            }

            var token = response?.Content?.QuerySelector<IElement>("#application_config")?.GetAttribute("data-loyalty_webapi_token");

            return token?.Replace("\"", "");
        }

        /// <summary>
        /// 领取冬促贴纸
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<bool> ClaimDailySticker(Bot bot, string token)
        {
            Uri request = new($"https://api.steampowered.com/ISaleItemRewardsService/ClaimItem/v1?access_token={token}");
            Uri referer = new(SteamStoreURL, "/category/sports");

            await bot.ArchiWebHandler.UrlPostWithSession(request, referer: referer, session: ArchiWebHandler.ESession.None).ConfigureAwait(false);

            return true;
        }
    }
}
