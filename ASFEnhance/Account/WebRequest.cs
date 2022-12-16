using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;
using ASFEnhance.Localization;
using System.Net;
using System.Text;
using static ASFEnhance.Account.CurrencyHelper;
using static ASFEnhance.Utils;

namespace ASFEnhance.Account
{
    internal static class WebRequest
    {
        /// <summary>
        /// 加载账户历史记录
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="cursorData"></param>
        /// <returns></returns>
        private static async Task<AccountHistoryResponse?> AjaxLoadMoreHistory(Bot bot, AccountHistoryResponse.CursorData cursorData)
        {
            Uri request = new(SteamStoreURL, "/account/AjaxLoadMoreHistory/?l=schinese");

            Dictionary<string, string> data = new(5, StringComparer.Ordinal) {
                { "cursor[wallet_txnid]", cursorData.WalletTxnid },
                { "cursor[timestamp_newest]", cursorData.TimestampNewest.ToString() },
                { "cursor[balance]", cursorData.Balance },
                { "cursor[currency]", cursorData.Currency.ToString() },
            };

            var response = await bot.ArchiWebHandler!.UrlPostToJsonObjectWithSession<AccountHistoryResponse>(request, referer: SteamStoreURL, data: data).ConfigureAwait(false);

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
            var response = await ASF.WebBrowser!.UrlGetToJsonObject<ExchangeAPIResponse>(request).ConfigureAwait(false);
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
        internal static async Task<string> GetAccountHistoryDetail(Bot bot)
        {
            // 读取在线汇率
            string myCurrency = bot.WalletCurrency.ToString();
            ExchangeAPIResponse? exchangeRate = await GetExchangeRatio(myCurrency).ConfigureAwait(false);
            if (exchangeRate == null)
            {
                return bot.FormatBotResponse(Langs.GetExchangeRateFailed);
            }

            // 获取货币符号
            string symbol = myCurrency;
            if (Currency2Symbol.ContainsKey(myCurrency))
            {
                symbol = Currency2Symbol[myCurrency];
            }

            StringBuilder result = new();
            result.AppendLine(bot.FormatBotResponse(Langs.MultipleLineResult));

            int giftedSpend = 0;
            int totalSpend = 0;
            int totalExternalSpend = 0;

            // 读取账户消费历史
            result.AppendLine(Langs.PurchaseHistorySummary);
            HtmlDocumentResponse? accountHistory = await GetAccountHistoryAjax(bot).ConfigureAwait(false);
            if (accountHistory == null)
            {
                return Langs.NetworkError;
            }

            // 解析表格元素
            IElement? tbodyElement = accountHistory?.Content?.QuerySelector("table>tbody");
            if (tbodyElement == null)
            {
                return Langs.ParseHtmlFailed;
            }

            // 获取下一页指针(为null代表没有下一页)
            AccountHistoryResponse.CursorData? cursor = HtmlParser.ParseCursorData(accountHistory);

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

        /// <summary>
        /// 获取许可证信息
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<List<LicensesData>?> GetOwnedLicenses(Bot bot)
        {
            Uri request = new(SteamStoreURL, "/account/licenses/?l=schinese");
            var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL).ConfigureAwait(false);
            return HtmlParser.ParseLincensesPage(response);
        }

        /// <summary>
        /// 移除许可证
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="subId"></param>
        /// <returns></returns>
        internal static async Task<bool> RemoveLicense(Bot bot, uint subId)
        {
            Uri request = new(SteamStoreURL, "/account/removelicense");
            Uri referer = new Uri(SteamStoreURL, "/account/licenses/");

            Dictionary<string, string> data = new(2) {
                { "packageid", subId.ToString() },
            };

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlPostToHtmlDocumentWithSession(request, data: data, referer: referer).ConfigureAwait(false);
            return response?.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// 获取邮箱通知偏好
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<EmailOptions?> GetAccountEmailOptions(Bot bot)
        {
            Uri request = new(SteamStoreURL, "/account/emailoptout");
            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL).ConfigureAwait(false);
            return HtmlParser.ParseEmailOptionPage(response);
        }

        /// <summary>
        /// 设置邮箱通知偏好
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<EmailOptions?> SetAccountEmailOptions(Bot bot, EmailOptions option)
        {
            Uri request = new(SteamStoreURL, "/account/emailoptout");

            Dictionary<string, string> data = new(11) {
                { "action", "save" },
                { "opt_out_all",option.EnableEmailNotification ? "0" : "1" },
            };

            if (option.EnableEmailNotification)
            {
                if (option.WhenWishlistDiscount)
                {
                    data.Add("opt_out_wishlist_inverse", "on");
                }
                if (option.WhenWishlistRelease)
                {
                    data.Add("opt_out_wishlist_releases_inverse", "on");
                }
                if (option.WhenGreenLightRelease)
                {
                    data.Add("opt_out_greenlight_releases_inverse", "on");
                }
                if (option.WhenFollowPublisherRelease)
                {
                    data.Add("opt_out_creator_home_releases_inverse", "on");
                }
                if (option.WhenSaleEvent)
                {
                    data.Add("opt_out_seasonal_inverse", "on");
                }
                if (option.WhenReceiveCuratorReview)
                {
                    data.Add("opt_out_curator_connect_inverse", "on");
                }
                if (option.WhenReceiveCommunityReward)
                {
                    data.Add("opt_out_loyalty_awards_inverse", "on");
                }
                if (option.WhenGameEventNotification)
                {
                    data.Add("opt_out_in_library_events_inverse", "on");
                }
            }

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlPostToHtmlDocumentWithSession(request, referer: SteamStoreURL, data: data).ConfigureAwait(false);
            return HtmlParser.ParseEmailOptionPage(response);
        }
    }
}
