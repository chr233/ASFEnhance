#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using AngleSharp.Dom;

using ArchiSteamFarm.Core;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Web.Responses;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using static Chrxw.ASFEnhance.Utils;

namespace Chrxw.ASFEnhance.Event
{
    internal static class WebRequest
    {
        // 冬促投票
        internal static async Task<bool?> MakeVote(Bot bot, uint gameID, int categoryID)
        {
            Uri request = new(SteamStoreURL, "/salevote");
            Uri referer = new(SteamStoreURL, "/steamawards/2021");

            Dictionary<string, string> data = new(5, StringComparer.Ordinal)
            {
                { "sessionid", "" },
                { "appid", gameID.ToString() },
                { "voteid", categoryID.ToString() },
                { "developerid", "0" },
            };

            await bot.ArchiWebHandler.UrlPostWithSession(request, data: data, referer: referer).ConfigureAwait(false);

            return true;
        }

        // 检查冬促投票
        internal static async Task<uint?> CheckSummerBadge(Bot bot)
        {
            Uri request = new(SteamStoreURL, "/steamawards/2021");

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamCommunityURL).ConfigureAwait(false);

            if (response == null)
            {
                ASFLogger.LogGenericInfo("null");
                return null;
            }

            uint voteCount = 0;


            IEnumerable<IElement> nodes = response.Content.SelectNodes("//div[@class='steamawards_shortcuts_ctn']//svg[@id='Icons']");

            foreach (IElement node in nodes)
            {
                voteCount++;
            }

            return voteCount;
        }

        // 领取冬促贴纸
        internal static async Task<bool> ClaimDailySticker(Bot bot)
        {
            Uri request = new("https://api.steampowered.com/ISaleItemRewardsService/ClaimItem/v1?access_token=" + bot.ArchiWebHandler.CachedAccessToken);
            Uri referer = new(SteamStoreURL, "/points/shop/");

            await bot.ArchiWebHandler.UrlPostWithSession(request, referer: referer).ConfigureAwait(false);

            return true;
        }
    }
}
