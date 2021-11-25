#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Web.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Chrxw.ASFEnhance.Event.Response;
using static Chrxw.ASFEnhance.Utils;

namespace Chrxw.ASFEnhance.Event
{
    internal static class WebRequest
    {
        // 秋促投票
        internal static async Task<bool?> MakeVote(Bot bot, uint gameID, int categoryID)
        {
            Uri request = new(SteamStoreURL, "/steamawards/nominategame");
            Uri referer = new(SteamStoreURL, "/steamawards/category/63");

            string? sessionID = bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookieValue(SteamStoreURL, "sessionid");

            if (string.IsNullOrEmpty(sessionID))
            {
                bot.ArchiLogger.LogNullError(nameof(sessionID));
                return false;
            }

            Dictionary<string, string> data = new(5, StringComparer.Ordinal)
            {
                { "sessionid", sessionID! },
                { "nominatedid", gameID.ToString() },
                { "categoryid", categoryID.ToString() },
                { "source", "3" },
            };

            await bot.ArchiWebHandler.UrlPostWithSession(request, data: data, referer: referer).ConfigureAwait(false);

            return true;
        }

        // 检查秋促徽章
        internal static async Task<SummerBadgeResponse?> CheckSummerBadge(Bot bot)
        {
            Uri request = new(SteamCommunityURL, "/profiles/" + bot.SteamID.ToString() + "/badges/56");

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamCommunityURL).ConfigureAwait(false);

            if (response == null)
            {
                ASF.ArchiLogger.LogGenericInfo("null");
                return null;
            }

            bool[] taskStatus = new bool[] { false, false, false, false };

            for (int i = 0; i < 4; i++)
            {
                string xpath = string.Format("//div[@class='badge_task'][{0}]/img", i + 1);
                IElement? eleTask = response.Content.SelectSingleNode(xpath);
                string taskSrc = eleTask?.GetAttribute("src") ?? "";

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
}
