#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using AngleSharp;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Web.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static Chrxw.ASFEnhance.Community.Response;
using static Chrxw.ASFEnhance.Utils;


namespace Chrxw.ASFEnhance.Community

{
    internal static class WebRequest
    {
        /// <summary>
        /// 加入指定群组
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="groupID"></param>
        /// <returns>
        /// <list type="table">
        /// <item>true:  加入成功</item>
        /// <item>false: 加入失败</item>
        /// <item>null:  群组不存在</item>
        /// </list>
        /// </returns>
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


        internal static async Task<List<GroupData>?> GetGroupList(Bot bot)
        {
            Uri request = new(SteamCommunityURL, "/profiles/" + bot.SteamID + "/groups/");

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL).ConfigureAwait(false);

            using (FileStream fs = new FileStream("1.html", FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    response.Content.ToHtml(sw);
                }
            }


            return HtmlParser.ParseGropuList(response);
        }

        internal static async Task<List<GroupData>?> GetTradeLink(Bot bot)
        {
            Uri request = new(SteamCommunityURL, "/profiles/" + bot.SteamID + "/tradeoffers/privacy");

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL).ConfigureAwait(false);

            using (FileStream fs = new FileStream("1.html", FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    response.Content.ToHtml(sw);
                }
            }


            return HtmlParser.ParseGropuList(response);
        }

    }
}
