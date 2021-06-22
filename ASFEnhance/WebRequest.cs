using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SteamKit2;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Web.Responses;
using ArchiSteamFarm.Steam.Integration;
using ArchiSteamFarm.Steam.Data;
using AngleSharp.Dom;
using System.Text.RegularExpressions;
using static Chrxw.ASFEnhance.Data;

namespace Chrxw.ASFEnhance
{
    internal static class WebRequest
    {
        //读取购物车
        internal static async Task<  List<CartData>?> GetCartGames(Bot bot)
        {
            Uri request = new(SteamStoreURL, "/cart/");

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

            if (response == null)
            {
                return null;
            }

            IEnumerable<IElement?> gameNodes = response.Content.SelectNodes("//div[@class='cart_item_list']/div");

            List<CartData> cartGames = new();

            const string regPattern = @"\w+\/\d+";

            foreach (IElement gameNode in gameNodes)
            {
                IElement? eleName = gameNode.SelectSingleElementNode("//div[@class='cart_item_desc']/a");
                IElement? elePrice = gameNode.SelectSingleElementNode("//div[@class='price']");

                string gameName = eleName.TextContent;
                string gameLink = eleName.GetAttribute("href");
                string gamePrice = elePrice.TextContent;

                Match match = Regex.Match(gameLink, regPattern, RegexOptions.IgnoreCase);

                if (!match.Success)
                {
                    continue;
                }
                string gameID = match.Groups[0].Value;

                CartData cartItem = new();

                cartItem.gameID = gameID;
                cartItem.gameName = gameName;
                cartItem.gamePrice = gamePrice;

                cartGames.Add(cartItem);
            }

            IElement? totalPrice = response.Content.SelectSingleNode("//div[@id='cart_estimated_total']");

            CartData total = new();
            total.gameName = "预计总额";
            total.gameID = "";
            total.gamePrice = totalPrice.TextContent;

            return cartGames;
        }
        //添加愿望单
        internal static async Task<bool> AddWishlist(Bot bot, uint gameID)
        {
            if (gameID == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(gameID));
            }

            Uri request = new(SteamStoreURL, "/api/addtowishlist");
            Uri referer = new(SteamStoreURL, "/app/" + gameID);

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

            ObjectResponse<ResultResponse>? response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<ResultResponse>(request, data: data, referer: referer).ConfigureAwait(false);

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
        //删除愿望单
        internal static async Task<bool> RemoveWishlist(Bot bot, uint gameID)
        {
            if (gameID == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(gameID));
            }

            Uri request = new(SteamStoreURL, "/api/removefromwishlist");
            Uri referer = new(SteamStoreURL, "/app/" + gameID);

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

            ObjectResponse<ResultResponse>? response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<ResultResponse>(request, data: data, referer: referer).ConfigureAwait(false);

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

        internal static Uri SteamStoreURL => ArchiWebHandler.SteamStoreURL;
    }
}
