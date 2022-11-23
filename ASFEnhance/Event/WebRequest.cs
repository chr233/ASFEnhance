#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Steam;
using ASFEnhance.Localization;
using System.Text;
using System.Text.RegularExpressions;
using static ASFEnhance.Utils;

namespace ASFEnhance.Event
{
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

            var configEle = response.Content.SelectSingleNode<IElement>("//div[@id='application_config']");
            var community = configEle?.GetAttribute("data-community");
            var match = Regex.Match(community, @"""CLANACCOUNTID"":(\d+),");

            return match.Success ? match.Groups[1].Value : null;
        }

        /// <summary>
        /// SteamAwards投票
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="gameID"></param>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        internal static async Task MakeVoteForSteamAwards(Bot bot, uint gameID, int categoryID)
        {
            Uri request = new(SteamStoreURL, "/steamawards/nominategame");
            Uri referer = new(SteamStoreURL, "/steamawards/category/72");

            Dictionary<string, string> data = new(4)
            {
                { "nominatedid", gameID.ToString() },
                { "categoryid", $"{72+categoryID}" },
                { "source", "2" },
            };

            await bot.ArchiWebHandler.UrlPostWithSession(request, data: data, referer: referer).ConfigureAwait(false);
        }

        /// <summary>
        /// 检查秋促徽章
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string> CheckSaleEventBadgeStatus(Bot bot)
        {
            Uri request = new(SteamCommunityURL, "/profiles/" + bot.SteamID.ToString() + "/badges/63");

            var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamCommunityURL).ConfigureAwait(false);

            if (response == null)
            {
                return Langs.NetworkError;
            }

            var eleTasks = response.Content.QuerySelectorAll($"div.badge_detail_tasks.twocol>div.badge_task");
            if (eleTasks.Any())
            {
                StringBuilder sb = new();
                sb.AppendLine(Langs.MultipleLineResult);
                foreach (var ele in eleTasks)
                {
                    var eleDesc = ele.QuerySelector("div.badge_task_name");
                    string desc = eleDesc.TextContent.Trim();

                    var eleImg = ele.QuerySelector("img");
                    string? imgSrc = eleImg?.GetAttribute("src");
                    bool status = imgSrc?.EndsWith("_on.png") ?? false;

                    sb.AppendLine($"{desc}: {(status ? "√" : "×")}");
                }
                return sb.ToString();
            }
            else
            {
                return Langs.NetworkError;
            }
        }
    }
}
