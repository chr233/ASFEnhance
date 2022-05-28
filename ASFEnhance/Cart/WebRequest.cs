#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Core;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Data;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;
using ASFEnhance.Localization;
using static ASFEnhance.Cart.Response;
using static ASFEnhance.Data.SteamGameID;
using static ASFEnhance.Utils;

namespace ASFEnhance.Cart
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

            return HtmlParser.ParseCartPage(response);
        }

        /// <summary>
        /// 添加到购物车
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="gameID"></param>
        /// <returns></returns>
        internal static async Task<bool?> AddCart(Bot bot, SteamGameID gameID)
        {
            if (gameID.GameType == SteamGameIDType.Sub || gameID.GameType == SteamGameIDType.Bundle)
            {
                return await AddCart(bot, gameID.GameID, gameID.GameType == SteamGameIDType.Bundle).ConfigureAwait(false);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 添加到购物车
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="subID"></param>
        /// <param name="bundle"></param>
        /// <returns></returns>
        internal static async Task<bool?> AddCart(Bot bot, uint subID, bool bundle = false)
        {
            string type = bundle ? "bundle" : "sub";

            Uri request = new(SteamStoreURL, "/cart/");
            Uri referer = new(SteamStoreURL, $"/{type}/{subID}");

            Dictionary<string, string> data = new(5, StringComparer.Ordinal)
            {
                { "action", "add_to_cart" },
                { type + "id", subID.ToString() },
                { "originating_snr", "1_direct-navigation__" },
            };

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlPostToHtmlDocumentWithSession(request, data: data, referer: referer).ConfigureAwait(false);

            return response != null;
        }

        /// <summary>
        /// 添加购物车(游戏物品)
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="appID"></param>
        /// <param name="classID"></param>
        /// <returns></returns>
        internal static async Task<SteamKit2.EResult?> AddCart(Bot bot, uint appID, uint classID)
        {
            Uri request = new(SteamStoreURL, "/cart/addtocart");
            Uri referer = new(SteamStoreURL, $"/itemstore/{appID}/detail/{classID}/");

            Dictionary<string, string> data = new(5, StringComparer.Ordinal)
            {
                { "action", "add_to_cart" },
                { "microtxnappid", appID.ToString() },
                { "microtxnassetclassid", classID.ToString() },
            };

            ObjectResponse<ResultResponse>? response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<ResultResponse>(request, data: data, referer: referer).ConfigureAwait(false);

            if (response == null)
            {
                return null;
            }

            return response.Content.Result;
        }

        /// <summary>
        /// 清空当前购物车
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<bool?> ClearCart(Bot bot)
        {
            Uri request = new(SteamStoreURL, "/cart/");

            bot.ArchiWebHandler.WebBrowser.CookieContainer.SetCookies(SteamStoreURL, "shoppingCartGID=-1");

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

            CartResponse? cartResponse = HtmlParser.ParseCartPage(response);

            if (cartResponse == null)
            {
                return null;
            }

            return cartResponse.CardDatas.Count == 0;
        }

        /// <summary>
        /// 读取购物车可用区域信息
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string> CartGetCountries(Bot bot)
        {
            Uri request = new(SteamStoreURL, "/cart/");

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

            return HtmlParser.ParseCartCountries(response);
        }

        /// <summary>
        /// 购物车改区
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        internal static async Task<bool> CartSetCountry(Bot bot, string countryCode)
        {
            Uri request = new(SteamStoreURL, "/account/setcountry");

            Dictionary<string, string> data = new(2, StringComparer.Ordinal)
            {
                { "cc", countryCode },
            };

            HtmlDocumentResponse? result = await bot.ArchiWebHandler.UrlPostToHtmlDocumentWithSession(request, data: data, referer: SteamStoreURL).ConfigureAwait(false);

            if (result == null) { return false; }

            return result.Content.TextContent == "true";
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

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: referer).ConfigureAwait(false);

            if (response == null)
            {
                bot.ArchiLogger.LogNullError(nameof(response));
                return null;
            }

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

            return response;
        }



        /// <summary>
        /// 初始化付款 (赠送礼物)
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="steamID32"></param>
        /// <returns></returns>
        internal static async Task<ObjectResponse<PurchaseResponse?>> InitTransaction(Bot bot, ulong steamID32)
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

            Dictionary<string, string> data = new(11, StringComparer.Ordinal)
            {
                { "gidShoppingCart", shoppingCartID },
                { "gidReplayOfTransID", "-1" },
                { "PaymentMethod", "steamaccount" },
                { "bIsGift", "1" },
                { "GifteeAccountID", steamID32.ToString() },
                { "GifteeEmail", "" },
                { "GifteeName", string.Format( Langs.GifteeName, nameof(ASFEnhance)) },
                { "GiftMessage", string.Format( Langs.GiftMessage, nameof(ASFEnhance), MyVersion.ToString()) },
                { "Sentiment", "祝你好运" },
                { "Signature", string.Format( Langs.GiftSignature, nameof(ASFEnhance)) },
                { "ScheduledSendOnDate", "0" },
            };

            ObjectResponse<PurchaseResponse?> response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<PurchaseResponse>(request, data: data, referer: referer).ConfigureAwait(false);

            if (response == null)
            {
                bot.ArchiLogger.LogNullError(nameof(response));
                return null;
            }

            return response;
        }

        /// <summary>
        /// 初始化付款 (赠送礼物)
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="steamID32"></param>
        /// <returns></returns>
        internal static async Task<ObjectResponse<PurchaseResponse?>> InitTransaction(Bot bot, string email)
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

            Version version = MyVersion;

            Dictionary<string, string> data = new(11, StringComparer.Ordinal)
            {
                { "gidShoppingCart", shoppingCartID },
                { "gidReplayOfTransID", "-1" },
                { "PaymentMethod", "steamaccount" },
                { "bIsGift", "1" },
                { "GifteeAccountID", "" },
                { "GifteeEmail", email },
                { "GifteeName", string.Format( Langs.GifteeName, nameof(ASFEnhance)) },
                { "GiftMessage", string.Format( Langs.GiftMessage, nameof(ASFEnhance), version.Major, version.Minor, version.Build, version.Revision) },
                { "Sentiment", "祝你好运" },
                { "Signature", string.Format( Langs.GiftSignature, nameof(ASFEnhance)) },
                { "ScheduledSendOnDate", "0" },
            };

            ObjectResponse<PurchaseResponse?> response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<PurchaseResponse>(request, data: data, referer: referer).ConfigureAwait(false);

            if (response == null)
            {
                bot.ArchiLogger.LogNullError(nameof(response));
                return null;
            }

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

            ObjectResponse<FinalPriceResponse?> response = await bot.ArchiWebHandler.UrlGetToJsonObjectWithSession<FinalPriceResponse>(request, referer: referer).ConfigureAwait(false);

            if (response == null)
            {
                bot.ArchiLogger.LogNullError(nameof(response));
                return null;
            }

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

            if (response == null)
            {
                bot.ArchiLogger.LogNullError(nameof(response));
                return null;
            }

            if (response2 == null)
            {
                bot.ArchiLogger.LogNullError(nameof(response2));
                return null;
            }

            return response2;
        }
    }
}
