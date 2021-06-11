using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchiSteamFarm;
using AngleSharp.Dom;
using System.IO;
using static ArchiSteamFarm.Json.Steam;
using static ArchiSteamFarm.WebBrowser;
using SteamKit2;
using ArchiSteamFarm.Localization;

namespace ASFEnhance
{
    internal static class WebRequest
    {
        internal static async Task<bool> AddWishlist(Bot bot, uint gameID)
        {
            if (gameID == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(gameID));
            }

            const string request = "/api/addtowishlist";
            string referer = string.Format("{0}/app/{1}", SteamStoreURL, gameID);

            string? sessionID = bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookieValue(SteamStoreURL, "sessionid");

            if (string.IsNullOrEmpty(sessionID))
            {
                bot.ArchiLogger.LogNullError(nameof(sessionID));
                return false;
            }

            Dictionary<string, string> data = new(2, StringComparer.Ordinal)
            {
                { "appid", gameID.ToString() },
                { "sessionid", sessionID! }
            };

            ObjectResponse<EResultResponse>? response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<EResultResponse>(SteamStoreURL, request, data: data, referer: referer).ConfigureAwait(false);

            if (response == null)
            {
                return false;
            }

            if (response.Content.Result != EResult.OK)
            {
                bot.ArchiLogger.LogGenericWarning(Strings.WarningFailed);

                return false;
            }

            return true;
        }
        internal static async Task<bool> RemoveWishlist(Bot bot, uint gameID)
        {
            if (gameID == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(gameID));
            }

            const string request = "/api/removefromwishlist";
            string referer = string.Format("{0}/app/{1}", SteamStoreURL, gameID);

            string? sessionID = bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookieValue(SteamStoreURL, "sessionid");

            if (string.IsNullOrEmpty(sessionID))
            {
                bot.ArchiLogger.LogNullError(nameof(sessionID));
                return false;
            }

            Dictionary<string, string> data = new(2, StringComparer.Ordinal)
            {
                { "appid", gameID.ToString() },
                { "sessionid", sessionID! }
            };

            ObjectResponse<EResultResponse>? response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<EResultResponse>(SteamStoreURL, request, data: data, referer: referer).ConfigureAwait(false);

            if (response == null)
            {
                return false;
            }

            if (response.Content.Result != EResult.OK)
            {
                bot.ArchiLogger.LogGenericWarning(Strings.WarningFailed);

                return false;
            }

            return true;
        }

        internal static string SteamStoreURL => ArchiWebHandler.SteamStoreURL;
    }
}
