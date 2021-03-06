﻿using AngleSharp.Dom;
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
        internal static Uri SteamStoreURL => ArchiWebHandler.SteamStoreURL;
        internal static Uri SteamCommunityURL => ArchiWebHandler.SteamCommunityURL;
    }
}
