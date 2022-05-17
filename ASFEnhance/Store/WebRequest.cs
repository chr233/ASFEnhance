#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;
using ASFEnhance.Localization;
using System.Net;
using System.Text;
using static ASFEnhance.CurrencyHelper;
using static ASFEnhance.Store.Response;
using static ASFEnhance.Utils;

namespace ASFEnhance.Store
{
    internal static class WebRequest
    {
        /// <summary>
        /// 读取商店页面SUB
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="gameID"></param>
        /// <returns></returns>
        internal static async Task<GameStorePageResponse?> GetStoreSubs(Bot bot, SteamGameID gameID)
        {
            return await GetStoreSubs(bot, gameID.Type.ToString(), gameID.GameID).ConfigureAwait(false);
        }

        /// <summary>
        /// 读取商店页面SUB
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="type"></param>
        /// <param name="gameID"></param>
        /// <returns></returns>
        internal static async Task<GameStorePageResponse?> GetStoreSubs(Bot bot, string type, uint gameID)
        {
            Uri request = new(SteamStoreURL, "/" + type.ToLowerInvariant() + "/" + gameID.ToString() + "/?l=schinese");

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL).ConfigureAwait(false);

            return HtmlParser.ParseStorePage(response);
        }


        /// <summary>
        /// 获取App详情
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="appID"></param>
        /// <returns></returns>
        internal static async Task<AppDetailResponse?> GetAppDetails(Bot bot, uint appID)
        {
            string key = appID.ToString();

            Uri request = new(SteamStoreURL, "/api/appdetails?appids=" + key);

            ObjectResponse<Dictionary<string, AppDetailResponse>>? response = await bot.ArchiWebHandler.UrlGetToJsonObjectWithSession<Dictionary<string, AppDetailResponse>>(request, referer: SteamStoreURL).ConfigureAwait(false);

            if (response != null && response.Content.ContainsKey(key))
            {
                return response.Content[key];
            }

            return null;
        }

        /// <summary>
        /// 发布游戏评测
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="gameID"></param>
        /// <param name="comment"></param>
        /// <param name="rateUp"></param>
        /// <param name="isPublic"></param>
        /// <param name="enComment"></param>
        /// <param name="forFree"></param>
        /// <returns></returns>
        internal static async Task<RecommendGameResponse?> PublishReview(Bot bot, uint gameID, string comment, bool rateUp = true, bool isPublic = true, bool enComment = true, bool forFree = false)
        {
            Uri request = new(SteamStoreURL, "/friends/recommendgame");

            string? language = bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookieValue(SteamStoreURL, "Steam_Language") ?? "english";

            Dictionary<string, string> data = new(11, StringComparer.Ordinal)
            {
                { "appid", gameID.ToString() },
                { "steamworksappid", gameID.ToString() },
                { "comment", comment },
                { "rated_up", rateUp ? "true" : "false" },
                { "is_public", isPublic ? "true" : "false" },
                { "language", language },
                { "received_compensation", forFree ? "1" : "0" },
                { "disable_comments", enComment ? "0" : "1" },
                //{ "sessionid", "" },
            };

            ObjectResponse<RecommendGameResponse>? response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<RecommendGameResponse>(request, data: data, referer: SteamStoreURL).ConfigureAwait(false);

            return response?.Content;
        }

        /// <summary>
        /// 删除游戏评测
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="gameID"></param>
        /// <returns></returns>
        internal static async Task<bool> DeleteRecommend(Bot bot, uint gameID)
        {
            Uri request = new(SteamStoreURL, $"/profiles/{bot.SteamID}/recommended/");

            Dictionary<string, string> data = new(3, StringComparer.Ordinal)
            {
                { "action", "delete" },
                //{ "sessionid", "" },
                { "appid", gameID.ToString() },
            };

            await bot.ArchiWebHandler.UrlPostWithSession(request, data: data, referer: SteamCommunityURL).ConfigureAwait(false);

            return true;
        }

        /// <summary>
        /// 搜索游戏
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        internal static async Task<string?> SearchGame(Bot bot, string keyWord)
        {
            Uri request = new(SteamStoreURL, $"/search/?term={keyWord}");

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL).ConfigureAwait(false);

            return HtmlParser.ParseSearchPage(response);
        }


        /// <summary>
        /// 加载账户历史记录
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="cursorData"></param>
        /// <returns></returns>
        private static async Task<Data.AccountHistoryResponse?> AjaxLoadMoreHistory(Bot bot, CursorData cursorData)
        {
            Uri request = new(SteamStoreURL, "/account/AjaxLoadMoreHistory/?l=schinese");

            Dictionary<string, string> data = new(5, StringComparer.Ordinal) {
                { "cursor[wallet_txnid]", cursorData.WalletTxnid },
                { "cursor[timestamp_newest]", cursorData.TimestampNewest.ToString() },
                { "cursor[balance]", cursorData.Balance },
                { "cursor[currency]", cursorData.Currency.ToString() },
                { "sessionid", bot.GetBotSessionID() }
            };

            ObjectResponse<Data.AccountHistoryResponse> response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<Data.AccountHistoryResponse>(request, referer: SteamStoreURL, data: data).ConfigureAwait(false);

            return response?.Content;
        }

        /// <summary>
        /// 获取在线汇率
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        private static async Task<ExchangeAPIResponse?> GetExchangeRatio(Bot bot, string currency)
        {
            Uri request = new($"https://api.exchangerate-api.com/v4/latest/{currency}");

            ObjectResponse<ExchangeAPIResponse> response = await bot.ArchiWebHandler.UrlGetToJsonObjectWithSession<ExchangeAPIResponse>(request).ConfigureAwait(false);

            return response?.Content;
        }

        /// <summary>
        /// 获取更多历史记录
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        private static async Task<HtmlDocumentResponse?> GetAccountHistoryAjax(Bot bot)
        {
            Uri request = new(SteamStoreURL, "/account/history?l=schinese");

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL).ConfigureAwait(false);

            return response;
        }


        /// <summary>
        /// 获取账号外部消费统计
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        private static async Task<TotalSpendResponse?> GetAccountTotalSpend(Bot bot)
        {
            Uri request = new(SteamHelpURL, "/zh-cn/accountdata/AccountSpend/");

            CookieCollection cc = bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookies(SteamStoreURL);

            StringBuilder cookies = new();

            foreach (Cookie c in cc)
            {
                cookies.Append(string.Format(CurrentCulture, "{0}={1};", c.Name, c.Value));
            }

            List<KeyValuePair<string, string>> headers = new() {
                { new("Cookie", cookies.ToString()) }
            };

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL, headers: headers).ConfigureAwait(false);

            return HtmlParser.ParseTotalSpend(response);
        }


        /// <summary>
        /// 获取账号消费历史记录
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> GetAccountHistoryDetail(Bot bot)
        {
            // 读取在线汇率
            string myCurrency = bot.WalletCurrency.ToString();
            ExchangeAPIResponse? exchangeRate = await GetExchangeRatio(bot, myCurrency).ConfigureAwait(false);
            if (exchangeRate == null)
            {
                return string.Format(CurrentCulture, Langs.GetExchangeRateFailed);
            }

            // 获取货币符号
            string symbol = myCurrency;
            if (Currency2Symbol.ContainsKey(myCurrency))
            {
                symbol = Currency2Symbol[myCurrency];
            }

            StringBuilder result = new();
            result.AppendLine(string.Format(CurrentCulture, Langs.MultipleLineResult));

            int totalGifted = 0;
            int totalSpent = 0;

            // 读取账户消费历史
            result.AppendLine(string.Format(CurrentCulture, Langs.PurchaseHistorySummary));
            HtmlDocumentResponse? accountHistory = await GetAccountHistoryAjax(bot).ConfigureAwait(false);
            if (accountHistory == null)
            {
                result.AppendLine(string.Format(CurrentCulture, Langs.CartNetworkError));
            }
            else
            {
                // 解析表格元素
                IElement? tbodyElement = accountHistory.Content.QuerySelector("table>tbody");
                if (tbodyElement == null)
                {
                    return string.Format(Langs.ParseHtmlFailed);
                }
                else
                {
                    // 获取下一页指针(为null代表没有下一页)
                    CursorData? cursor = HtmlParser.ParseCursorData(accountHistory);

                    HistoryParseResponse historyData = HtmlParser.ParseHistory(tbodyElement, exchangeRate.Rates, myCurrency);

                    while (cursor != null)
                    {
                        AccountHistoryResponse? ajaxHistoryResponse = await AjaxLoadMoreHistory(bot, cursor).ConfigureAwait(false);

                        if (ajaxHistoryResponse != null)
                        {
                            tbodyElement.InnerHtml = ajaxHistoryResponse.HtmlContent;
                            cursor = ajaxHistoryResponse.Cursor;
                            historyData += HtmlParser.ParseHistory(tbodyElement, exchangeRate.Rates, myCurrency);
                        }
                        else
                        {
                            cursor = null;
                        }
                    }

                    totalGifted = historyData.GiftPurchase;
                    totalSpent = historyData.StorePurchase;

                    result.AppendLine(string.Format(CurrentCulture, Langs.PruchaseHistoryGroupType));
                    result.AppendLine(string.Format(CurrentCulture, Langs.PruchaseHistoryTypeStorePurchase, historyData.StorePurchase / 100.0, symbol));
                    result.AppendLine(string.Format(CurrentCulture, Langs.PruchaseHistoryTypeGiftPurchase, historyData.GiftPurchase / 100.0, symbol));
                    result.AppendLine(string.Format(CurrentCulture, Langs.PruchaseHistoryTypeInGamePurchase, historyData.InGamePurchase / 100.0, symbol));
                    result.AppendLine(string.Format(CurrentCulture, Langs.PruchaseHistoryTypeMarketPurchase, historyData.MarketPurchase / 100.0, symbol));
                    result.AppendLine(string.Format(CurrentCulture, Langs.PruchaseHistoryTypeMarketSelling, historyData.MarketSelling / 100.0, symbol));
                    result.AppendLine(string.Format(CurrentCulture, Langs.PruchaseHistoryGroupOther));
                    result.AppendLine(string.Format(CurrentCulture, Langs.PruchaseHistoryTypeOther, historyData.StorePurchase / 100.0, symbol));
                    result.AppendLine(string.Format(CurrentCulture, Langs.PruchaseHistoryTypeRefunded, historyData.RefundPurchase / 100.0, symbol));
                }
            }

            // 读取账户外部资金消费统计
            //result.AppendLine(string.Format(CurrentCulture, "外部消费统计:"));
            //TotalSpendResponse? totalSpendData = await GetAccountTotalSpend(bot).ConfigureAwait(false);
            //if (totalSpendData == null)
            //{
            //    result.AppendLine(string.Format(CurrentCulture, Langs.CartNetworkError));
            //}
            //else
            //{
            //    if (!exchangeRate.Rates.TryGetValue("USD", out double fromUSD))
            //    {
            //        result.AppendLine(string.Format(CurrentCulture, "美元汇率获取失败!"));
            //        fromUSD = 1;
            //    };
            //    if (!exchangeRate.Rates.TryGetValue("CNY", out double fromCNY))
            //    {
            //        result.AppendLine(string.Format(CurrentCulture, "人民币汇率获取失败!"));
            //        fromCNY = 1;
            //    };

            //    int totalSpend = (int)(totalSpendData.TotalSpend / fromUSD);
            //    int oldSpend = (int)(totalSpendData.OldSpend / fromUSD);
            //    int pwSpend = (int)(totalSpendData.PWSpend / fromUSD);
            //    int chinaSpend = (int)(totalSpendData.ChinaSpend / fromCNY);

            //    totalSpent = totalSpend + oldSpend + pwSpend + chinaSpend;

            //    result.AppendLine(string.Format(CurrentCulture, " 1.原始金额:"));
            //    result.AppendLine(string.Format(CurrentCulture, " - TotalSpend: {0:0.00} $", totalSpendData.TotalSpend / 100.0));
            //    result.AppendLine(string.Format(CurrentCulture, " - OldSpend:   {0:0.00} $", totalSpendData.OldSpend / 100.0));
            //    result.AppendLine(string.Format(CurrentCulture, " - PWSpend:    {0:0.00} $", totalSpendData.PWSpend / 100.0));
            //    result.AppendLine(string.Format(CurrentCulture, " - ChinaSpend: {0:0.00} ¥", totalSpendData.ChinaSpend / 100.0));
            //    result.AppendLine(string.Format(CurrentCulture, " 2.换算后的金额(使用在线汇率):"));
            //    result.AppendLine(string.Format(CurrentCulture, " - TotalSpend: {0:0.00} {1}", totalSpend / 100.0, symbol));
            //    result.AppendLine(string.Format(CurrentCulture, " - OldSpend:   {0:0.00} {1}", oldSpend / 100.0, symbol));
            //    result.AppendLine(string.Format(CurrentCulture, " - PWSpend:    {0:0.00} {1}", pwSpend / 100.0, symbol));
            //    result.AppendLine(string.Format(CurrentCulture, " - ChinaSpend: {0:0.00} {1}", chinaSpend / 100.0, symbol));
            //}

            result.AppendLine(string.Format(CurrentCulture, Langs.PruchaseHistoryGroupStatus));
            result.AppendLine(string.Format(CurrentCulture, Langs.PruchaseHistoryStatusTotalPurchase, totalSpent / 100.0, symbol));
            result.AppendLine(string.Format(CurrentCulture, Langs.PruchaseHistoryStatusTotalGift, totalGifted / 100.0, symbol));
            result.AppendLine(string.Format(CurrentCulture, Langs.PruchaseHistoryGroupGiftCredit));
            result.AppendLine(string.Format(CurrentCulture, Langs.PruchaseHistoryCreditMin, (totalSpent - totalGifted) / 100, symbol));
            result.AppendLine(string.Format(CurrentCulture, Langs.PruchaseHistoryCreditMax, (totalSpent * 1.8 - totalGifted) / 100, symbol));

            DateTime updateTime = DateTimeOffset.FromUnixTimeSeconds(exchangeRate.UpdateTime).UtcDateTime;

            result.AppendLine(string.Format(CurrentCulture, Langs.PruchaseHistoryGroupAbout));
            result.AppendLine(string.Format(CurrentCulture, Langs.PruchaseHistoryAboutPlugin, nameof(ASFEnhance)));
            result.AppendLine(string.Format(CurrentCulture, Langs.PruchaseHistoryAboutBaseRate, exchangeRate.Base));
            result.AppendLine(string.Format(CurrentCulture, Langs.PruchaseHistoryAboutUpdateTime, updateTime));
            result.AppendLine(string.Format(CurrentCulture, Langs.PruchaseHistoryAboutRateSource));

            return result.ToString();
        }
    }
}
