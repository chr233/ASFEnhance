#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Steam;
using System.Text.RegularExpressions;
using static ASFEnhance.Utils;

namespace ASFEnhance.Event
{
    internal static class WebRequest
    {
        /// <summary>
        /// 模拟探索队列
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
        internal static async Task<string?> FetchEventToken(Bot bot,string salePage)
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
    }
}
