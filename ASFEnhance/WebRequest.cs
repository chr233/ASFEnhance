using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Data;
using ArchiSteamFarm.Steam.Integration;
using ArchiSteamFarm.Web.Responses;
using SteamKit2;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Chrxw.ASFEnhance.Response;

namespace Chrxw.ASFEnhance
{
    internal static class WebRequest
    {
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

        //读取购物车
        internal static async Task<CartResponse?> GetCartGames(Bot bot)
        {
            Uri request = new(SteamStoreURL, "/cart/");

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

            bot.ArchiLogger.LogGenericWarning(response.Content.Title);

            return HtmlParser.ParseCertPage(response);
        }

        //添加购物车
        internal static async Task<bool?> AddCert(Bot bot, uint subID, bool bundle = false)
        {
            string type = bundle ? "bundle" : "sub";

            Uri request = new(SteamStoreURL, "/cart/");
            Uri referer = new(SteamStoreURL, "/" + type + "/" + subID);

            string? sessionID = bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookieValue(SteamStoreURL, "sessionid");

            if (string.IsNullOrEmpty(sessionID))
            {
                bot.ArchiLogger.LogNullError(nameof(sessionID));
                return null;
            }

            Dictionary<string, string> data = new(3, StringComparer.Ordinal)
            {
                { "action", "add_to_cart" },
                { "sessionid", sessionID! },
                { type, subID.ToString() }
            };

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlPostToHtmlDocumentWithSession(request, data: data, referer: referer).ConfigureAwait(false);

            CartResponse? cartResponse = HtmlParser.ParseCertPage(response);

            if (cartResponse != null && cartResponse.cartData != null)
            {
                string gamePath = type + "/" + subID.ToString();
                foreach (CartData cartData in cartResponse.cartData)
                {
                    if (cartData.path.IndexOf(gamePath) != -1)
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return null;
            }
        }

        //清空购物车
        internal static async Task<bool?> ClearCert(Bot bot)
        {
            Uri request = new(SteamStoreURL, "/cart/");

            bot.ArchiWebHandler.WebBrowser.CookieContainer.SetCookies(SteamStoreURL, "shoppingCartGID=-1");

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

            CartResponse? cartResponse = HtmlParser.ParseCertPage(response);

            if (cartResponse == null)
            {
                return null;
            }

            return cartResponse.cartData.Count == 0;
        }

        //读取商店页Sub
        internal static async Task<StoreResponse?> GetStoreSubs(Bot bot, string type, uint gameID)
        {
            Uri request = new(SteamStoreURL, "/" + type.ToLowerInvariant() + "/" + gameID.ToString());

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL).ConfigureAwait(false);

            return HtmlParser.ParseStorePage(response);
        }

        //夏促任务
        internal static async Task<string?> SummerEvent(Bot bot, uint[] choose)
        {

            string? sb = await GetSummerBadge(bot).ConfigureAwait(false);

            if (sb != null)
            {
                return sb;
            }

            uint genre = 1;
            foreach (uint choice in choose)
            {
                await SelectSummerChoice(bot, genre++, choice).ConfigureAwait(false);
            }

            return await GetSummerBadge(bot).ConfigureAwait(false);
        }

        //获取夏促徽章
        private static async Task<string?> GetSummerBadge(Bot bot)
        {
            Uri request = new(SteamCommunityURL, string.Format("/profiles/{0}/?l=schinese", bot.SteamID.ToString()));

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

            if (response == null)
            {
                bot.ArchiLogger.LogNullError(nameof(response));
                return null;
            }

            IElement? eleBadge = response.Content.SelectSingleNode("//div[@class='profile_count_link_preview']/div[1]");

            if (eleBadge == null)
            {
                bot.ArchiLogger.LogNullError(nameof(eleBadge));
                return null;
            }

            string tooltip = eleBadge.GetAttribute("data-tooltip-html").Substring(0, 5) ?? "读取徽章出错";

            switch (tooltip)
            {
                case "蒙面复仇者":
                case "先锋探路者":
                case "猩猩科学家":
                case "灵异学教授":
                case "幽灵大侦探":
                    return tooltip;
                default:
                    bot.ArchiLogger.LogGenericError(string.Format("tooltip = {0}", tooltip));
                    return null;
            }
        }

        //夏促选择选项
        private static async Task<bool> SelectSummerChoice(Bot bot, uint genre, uint choice)
        {
            Uri request = new(SteamStoreURL, "/promotion/ajaxclaimstickerforgenre");

            string? sessionID = bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookieValue(SteamStoreURL, "sessionid");

            if (string.IsNullOrEmpty(sessionID))
            {
                bot.ArchiLogger.LogNullError(nameof(sessionID));
                return false;
            }

            Dictionary<string, string> data = new(3, StringComparer.Ordinal)
            {
                { "genre", genre.ToString() },
                { "choice", choice.ToString() },
                { "sessionid", sessionID! }
            };

            await bot.ArchiWebHandler.UrlPostWithSession(request, data: data).ConfigureAwait(false);

            return true;
        }

        internal static Uri SteamStoreURL => ArchiWebHandler.SteamStoreURL;
        internal static Uri SteamCommunityURL => ArchiWebHandler.SteamCommunityURL;
    }
}
