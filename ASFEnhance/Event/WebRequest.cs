using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Steam;
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

            var configEle = response?.Content?.SelectSingleNode<IElement>("//div[@id='application_config']");
            string community = configEle?.GetAttribute("data-community")?? "";
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

            if (response == null)
            {
                return null;
            }

            var configEle = response.Content.QuerySelector("#application_config");
            var token = configEle?.GetAttribute("data-loyalty_webapi_token");

            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            return token.Substring(1, token.Length-2);
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
    }
}
