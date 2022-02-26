#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Core;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Web.Responses;
using Chrxw.ASFEnhance.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Chrxw.ASFEnhance.Cart.Response;
using static Chrxw.ASFEnhance.Utils;

namespace Chrxw.ASFEnhance.Cart
{
    internal static class WebRequest
    {

        /// <summary>
        /// 读取当前购物车
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<CartResponse?> GetCartGames(Bot bot)
        {
            Uri request = new(SteamStoreURL, "/cart/");

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

            return HtmlParser.ParseCertPage(response);
        }

        /// <summary>
        /// 添加到购物车
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="subID"></param>
        /// <param name="bundle"></param>
        /// <returns></returns>
        internal static async Task<bool?> AddCert(Bot bot, uint subID, bool bundle = false)
        {
            string type = bundle ? "bundle" : "sub";

            Uri request = new(SteamStoreURL, "/cart/");
            Uri referer = new(SteamStoreURL, "/" + type + "/" + subID);

            Random random = new();

            Dictionary<string, string> data = new(5, StringComparer.Ordinal)
            {
                { "action", "add_to_cart" },
                { type + "id", subID.ToString() },
                { "originating_snr", "1_direct-navigation__" },
                { "snr", string.Format("{0}_{1}_{2}__{3}", 1, random.Next(1, 10), random.Next(1, 10), random.Next(100, 999)) }
            };

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlPostToHtmlDocumentWithSession(request, data: data, referer: referer).ConfigureAwait(false);

            return response != null;
        }

        /// <summary>
        /// 清空当前购物车
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 读取购物车可用区域信息
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<List<CartCountryData>> CartGetCountries(Bot bot)
        {
            Uri request = new(SteamStoreURL, "/cart/");

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

            return HtmlParser.ParseCertCountries(response);
        }

        //TODO
        /// <summary>
        /// 购物车改区
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        internal static async Task<bool> CartSetCountry(Bot bot, string countryCode)
        {
            Uri request = new(SteamStoreURL, "/account/setcountry");
            Uri referer = new(SteamStoreURL, "/cart/");

            Dictionary<string, string> data = new(2, StringComparer.Ordinal)
            {
                { "cc", countryCode }
            };

            HtmlDocumentResponse? result = await bot.ArchiWebHandler.UrlPostToHtmlDocumentWithSession(request, data: data, referer: referer).ConfigureAwait(false);

            if (result == null)
            {
                return false;
            }

            ASF.ArchiLogger.LogGenericInfo(result.StatusCode.ToString());

            return true;
        }

        /// <summary>
        /// 结算购物车
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="asGift"></param>
        /// <returns></returns>
        internal static async Task<HtmlDocumentResponse?> CheckOut(Bot bot, bool asGift = false)
        {
            string queries = string.Format("/checkout/?purchasetype={0}", asGift ? "gift" : "self");

            Uri request = new(SteamStoreURL, queries);
            Uri referer = new(SteamStoreURL, "/cart/");

            string? shoppingCartID = bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookieValue(SteamStoreURL, "shoppingCartGID");

            if (string.IsNullOrEmpty(shoppingCartID) || shoppingCartID == "-1")
            {
                bot.ArchiLogger.LogNullError(nameof(shoppingCartID));
                return null;
            }

            //ASF.ArchiLogger.LogGenericInfo(shoppingCartID);

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: referer).ConfigureAwait(false);

            if (response == null)
            {
                bot.ArchiLogger.LogNullError(nameof(response));
                return null;
            }

            //ASF.ArchiLogger.LogGenericInfo(shoppingCartID ?? "Null");
            //ASF.ArchiLogger.LogGenericInfo(response.Content.Title ?? "Null");
            //ASF.ArchiLogger.LogGenericInfo(response.Content.ToString().Length.ToString() ?? "Null");
            //ASF.ArchiLogger.LogGenericWarning(response.StatusCode.ToString());
            //LogCookieCollection(bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookies(SteamStoreURL));

            return response;
        }

        /// <summary>
        /// 初始化付款
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<ObjectResponse<PurchaseResponse?>> InitTransaction(Bot bot)
        {
            Uri request = new(SteamStoreURL, "/checkout/inittransaction/");
            Uri referer = new(SteamStoreURL, "/checkout/");

            string? shoppingCartID = bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookieValue(SteamStoreURL, "shoppingCartGID");

            if (string.IsNullOrEmpty(shoppingCartID))
            {
                if (string.IsNullOrEmpty(shoppingCartID))
                {
                    bot.ArchiLogger.LogNullError(nameof(shoppingCartID));
                    return null;
                }
            }

            //ASF.ArchiLogger.LogGenericWarning(shoppingCartID);
            //LogCookieCollection(bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookies(SteamStoreURL));

            Dictionary<string, string> data = new(4, StringComparer.Ordinal)
            {
                { "gidShoppingCart", shoppingCartID },
                { "gidReplayOfTransID", "-1" },
                { "PaymentMethod", "steamaccount" },
            };

            ObjectResponse<PurchaseResponse?> response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<PurchaseResponse>(request, data: data, referer: referer).ConfigureAwait(false);

            if (response == null)
            {
                bot.ArchiLogger.LogNullError(nameof(response));
                return null;
            }

            //ASF.ArchiLogger.LogGenericInfo(response.Content.Result.ToString());
            //ASF.ArchiLogger.LogGenericInfo(response.Content.TransID);
            //ASF.ArchiLogger.LogGenericWarning(response.StatusCode.ToString());
            //LogCookieCollection(bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookies(SteamStoreURL));

            return response;
        }

        /// <summary>
        /// 获取购物车总价格
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="TransID"></param>
        /// <param name="asGift"></param>
        /// <returns></returns>
        internal static async Task<ObjectResponse<FinalPriceResponse?>> GetFinalPrice(Bot bot, string TransID, bool asGift = false)
        {
            string? shoppingCartID = bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookieValue(SteamStoreURL, "shoppingCartGID");

            if (string.IsNullOrEmpty(shoppingCartID) || shoppingCartID == "-1")
            {
                if (string.IsNullOrEmpty(shoppingCartID))
                {
                    bot.ArchiLogger.LogNullError(nameof(shoppingCartID));
                    return null;
                }
                else
                {
                    bot.ArchiLogger.LogGenericWarning("购物车是空的");
                    return null;
                }
            }

            string queries = string.Format("/checkout/getfinalprice/?count=1&transid={0}&purchasetype={1}&microtxnid=-1&cart={2}&gidReplayOfTransID=-1", TransID, asGift ? "gift" : "self", shoppingCartID);

            Uri request = new(SteamStoreURL, queries);
            Uri referer = new(SteamStoreURL, "/checkout/");

            //ASF.ArchiLogger.LogGenericWarning(TransID);
            //ASF.ArchiLogger.LogGenericWarning(shoppingCartID);
            //ASF.ArchiLogger.LogGenericWarning(asGift ? "gift" : "self");

            ObjectResponse<FinalPriceResponse?> response = await bot.ArchiWebHandler.UrlGetToJsonObjectWithSession<FinalPriceResponse>(request, referer: referer).ConfigureAwait(false);

            if (response == null)
            {
                bot.ArchiLogger.LogNullError(nameof(response));
                return null;
            }

            //ASF.ArchiLogger.LogGenericInfo(response.Content.BasePrice.ToString());
            //ASF.ArchiLogger.LogGenericInfo(response.Content.Discount.ToString());
            //ASF.ArchiLogger.LogGenericInfo(response.Content.Tax.ToString());
            //ASF.ArchiLogger.LogGenericInfo(response.Content.Result.ToString());
            //ASF.ArchiLogger.LogGenericWarning(response.StatusCode.ToString());

            //LogCookieCollection(bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookies(SteamStoreURL));

            return response;
        }

        /// <summary>
        /// 完成付款
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="TransID"></param>
        /// <returns></returns>
        internal static async Task<ObjectResponse<TransactionStatusResponse?>> FinalizeTransaction(Bot bot, string TransID)
        {

            Uri request = new(SteamStoreURL, "/checkout/finalizetransaction/");
            Uri referer = new(SteamStoreURL, "/checkout/");

            Dictionary<string, string> data = new(3, StringComparer.Ordinal)
            {
                { "transid", TransID },
                { "CardCVV2", "" },
                { "browserInfo", @"{""language"":""zh-CN"",""javaEnabled"":""false"",""colorDepth"":24,""screenHeight"":1080,""screenWidth"":1920}" }
            };

            ObjectResponse<FinalizeTransactionResponse?> response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<FinalizeTransactionResponse>(request, data: data, referer: referer).ConfigureAwait(false);

            string queries = string.Format("/checkout/transactionstatus/?count=1&transid={0}", TransID);

            request = new(SteamStoreURL, queries);

            ObjectResponse<TransactionStatusResponse?> response2 = await bot.ArchiWebHandler.UrlGetToJsonObjectWithSession<TransactionStatusResponse>(request, referer: referer).ConfigureAwait(false);

            if (response2 == null)
            {
                bot.ArchiLogger.LogNullError(nameof(response));
                return null;
            }

            //var receipt = response2.Content.PurchaseReceipt;

            //ASF.ArchiLogger.LogGenericInfo("返回值2");
            //ASF.ArchiLogger.LogGenericInfo(response2.Content.Result.ToString());
            //ASF.ArchiLogger.LogGenericWarning(response2.StatusCode.ToString());

            //if (receipt != null)
            //{
            //    ASF.ArchiLogger.LogGenericInfo(receipt.BasePrice.ToString());
            //    ASF.ArchiLogger.LogGenericInfo(receipt.FormattedTotal.ToString());
            //}

            //LogCookieCollection(bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookies(SteamStoreURL));

            return response2;
        }
    }
}
