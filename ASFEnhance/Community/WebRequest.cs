#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Core;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Web.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Chrxw.ASFEnhance.Utils;


namespace Chrxw.ASFEnhance.Community

{
    internal static class WebRequest
    {
        // 加入群组
        internal static async Task<bool?> JoinGroup(Bot bot, string groupID)
        {
            Uri request = new(SteamCommunityURL, "/groups/" + groupID);

            string? sessionID = bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookieValue(SteamStoreURL, "sessionid");

            if (string.IsNullOrEmpty(sessionID))
            {
                bot.ArchiLogger.LogNullError(nameof(sessionID));
                return null;
            }

            Dictionary<string, string> data = new(3, StringComparer.Ordinal)
            {
                { "action", "join" },
                { "sessionID", sessionID }
            };

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlPostToHtmlDocumentWithSession(request, data: data, referer: SteamCommunityURL).ConfigureAwait(false);

            return HtmlParser.CheckJoinGroup(response);
        }
    }
}
