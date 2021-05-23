using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchiSteamFarm;
using AngleSharp.Dom;
using System.IO;

namespace ASFEnhance
{
    internal static class WebRequest
    {
        internal static async Task<string?> GetPointSummary(Bot bot)
        {
            const string path = "/pointssummary?l=english";
            WebBrowser.HtmlDocumentResponse? pointssummaryPageResponse = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(SteamStoreURL, path).ConfigureAwait(false);

            if (pointssummaryPageResponse != null)
            {
                IList<IElement> points = pointssummaryPageResponse.Content.SelectNodes("//*[contains(@class,'total_current')][last()]");

                return points.Count > 0 ? string.Format(Properties.Resources.PointSummary, points[0].TextContent) : Properties.Resources.BotPointFailed;
            }
            else
            {
                return Properties.Resources.RequestFailed;
            }
        }

        internal static string SteamStoreURL => ArchiWebHandler.SteamStoreURL;
    }
}
