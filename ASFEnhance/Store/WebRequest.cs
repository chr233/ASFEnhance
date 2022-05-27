#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;
using ASFEnhance.Localization;
using System.Text;
using static ASFEnhance.Store.CurrencyHelper;
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
            return await GetStoreSubs(bot, gameID.GameType.ToString(), gameID.GameID).ConfigureAwait(false);
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
        private static async Task<ExchangeAPIResponse?> GetExchangeRatio(string currency)
        {
            Uri request = new($"https://api.exchangerate-api.com/v4/latest/{currency}");
            ObjectResponse<ExchangeAPIResponse> response = await ASF.WebBrowser.UrlGetToJsonObject<ExchangeAPIResponse>(request).ConfigureAwait(false);
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
        /// 获取账号消费历史记录
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> GetAccountHistoryDetail(Bot bot)
        {
            // 读取在线汇率
            string myCurrency = bot.WalletCurrency.ToString();
            ExchangeAPIResponse? exchangeRate = await GetExchangeRatio(myCurrency).ConfigureAwait(false);
            if (exchangeRate == null)
            {
                return Langs.GetExchangeRateFailed;
            }

            // 获取货币符号
            string symbol = myCurrency;
            if (Currency2Symbol.ContainsKey(myCurrency))
            {
                symbol = Currency2Symbol[myCurrency];
            }

            StringBuilder result = new();
            result.AppendLine(Langs.MultipleLineResult);

            int giftedSpend = 0;
            int totalSpend = 0;
            int totalExternalSpend = 0;

            // 读取账户消费历史
            result.AppendLine(Langs.PurchaseHistorySummary);
            HtmlDocumentResponse? accountHistory = await GetAccountHistoryAjax(bot).ConfigureAwait(false);
            if (accountHistory == null)
            {
                result.AppendLine(Langs.CartNetworkError);
            }
            else
            {
                // 解析表格元素
                IElement? tbodyElement = accountHistory.Content.QuerySelector("table>tbody");
                if (tbodyElement == null)
                {
                    return Langs.ParseHtmlFailed;
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

                    giftedSpend = historyData.GiftPurchase;
                    totalSpend = historyData.StorePurchase + historyData.InGamePurchase;
                    totalExternalSpend = historyData.StorePurchase - historyData.StorePurchaseWallet + historyData.GiftPurchase - historyData.GiftPurchaseWallet;

                    result.AppendLine(Langs.PruchaseHistoryGroupType);
                    result.AppendLine(string.Format(Langs.PruchaseHistoryTypeStorePurchase, historyData.StorePurchase / 100.0, symbol));
                    result.AppendLine(string.Format(Langs.PruchaseHistoryTypeExternal, (historyData.StorePurchase - historyData.StorePurchaseWallet) / 100.0, symbol));
                    result.AppendLine(string.Format(Langs.PruchaseHistoryTypeWallet, historyData.StorePurchaseWallet / 100.0, symbol));
                    result.AppendLine(string.Format(Langs.PruchaseHistoryTypeGiftPurchase, historyData.GiftPurchase / 100.0, symbol));
                    result.AppendLine(string.Format(Langs.PruchaseHistoryTypeExternal, (historyData.GiftPurchase - historyData.GiftPurchaseWallet) / 100.0, symbol));
                    result.AppendLine(string.Format(Langs.PruchaseHistoryTypeWallet, historyData.GiftPurchaseWallet / 100.0, symbol));
                    result.AppendLine(string.Format(Langs.PruchaseHistoryTypeInGamePurchase, historyData.InGamePurchase / 100.0, symbol));
                    result.AppendLine(string.Format(Langs.PruchaseHistoryTypeMarketPurchase, historyData.MarketPurchase / 100.0, symbol));
                    result.AppendLine(string.Format(Langs.PruchaseHistoryTypeMarketSelling, historyData.MarketSelling / 100.0, symbol));

                    result.AppendLine(Langs.PruchaseHistoryGroupOther);
                    result.AppendLine(string.Format(Langs.PruchaseHistoryTypeWalletPurchase, historyData.WalletPurchase / 100.0, symbol));
                    result.AppendLine(string.Format(Langs.PruchaseHistoryTypeOther, historyData.Other / 100.0, symbol));
                    result.AppendLine(string.Format(Langs.PruchaseHistoryTypeRefunded, historyData.RefundPurchase / 100.0, symbol));
                    result.AppendLine(string.Format(Langs.PruchaseHistoryTypeExternal, (historyData.RefundPurchase - historyData.RefundPurchaseWallet) / 100.0, symbol));
                    result.AppendLine(string.Format(Langs.PruchaseHistoryTypeWallet, historyData.RefundPurchaseWallet / 100.0, symbol));
                }
            }

            result.AppendLine(Langs.PruchaseHistoryGroupStatus);
            result.AppendLine(string.Format(Langs.PruchaseHistoryStatusTotalPurchase, totalSpend / 100.0, symbol));
            result.AppendLine(string.Format(Langs.PruchaseHistoryStatusTotalExternalPurchase, totalExternalSpend / 100.0, symbol));
            result.AppendLine(string.Format(Langs.PruchaseHistoryStatusTotalGift, giftedSpend / 100.0, symbol));
            result.AppendLine(string.Format(Langs.PruchaseHistoryGroupGiftCredit));
            result.AppendLine(string.Format(Langs.PruchaseHistoryCreditMin, (totalSpend - giftedSpend) / 100, symbol));
            result.AppendLine(string.Format(Langs.PruchaseHistoryCreditMax, (totalSpend * 1.8 - giftedSpend) / 100, symbol));
            result.AppendLine(string.Format(Langs.PruchaseHistoryExternalMin, (totalExternalSpend - giftedSpend) / 100, symbol));
            result.AppendLine(string.Format(Langs.PruchaseHistoryExternalMax, (totalExternalSpend * 1.8 - giftedSpend) / 100, symbol));

            DateTime updateTime = DateTimeOffset.FromUnixTimeSeconds(exchangeRate.UpdateTime).UtcDateTime;

            result.AppendLine(Langs.PruchaseHistoryGroupAbout);
            result.AppendLine(string.Format(Langs.PruchaseHistoryAboutBaseRate, exchangeRate.Base));
            result.AppendLine(string.Format(Langs.PruchaseHistoryAboutPlugin, nameof(ASFEnhance)));
            result.AppendLine(string.Format(Langs.PruchaseHistoryAboutUpdateTime, updateTime));
            result.AppendLine(string.Format(Langs.PruchaseHistoryAboutRateSource));

            return result.ToString();
        }
    }
}
