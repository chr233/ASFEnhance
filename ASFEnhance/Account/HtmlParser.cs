using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using static ASFEnhance.Account.CurrencyHelper;
using static ASFEnhance.Utils;

namespace ASFEnhance.Account
{
    internal static partial class HtmlParser
    {
        [GeneratedRegex("g_historyCursor = ([^;]+)")]
        private static partial Regex MatchHistortyCursor();

        [GeneratedRegex("^\\s*([-+])?([^\\d,.]*)([\\d,.]+)([^\\d,.]*)$")]
        private static partial Regex MatchHistoryItem();

        [GeneratedRegex("\\( (\\d+),")]
        private static partial Regex MatchSubId();
        /// <summary>
        /// 获取Cursor对象
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static AccountHistoryResponse.CursorData ParseCursorData(HtmlDocumentResponse response)
        {
            if (response == null)
            {
                return null;
            }

            string content = response.Content.Body.InnerHtml;
            Match match = MatchHistortyCursor().Match(content);
            if (!match.Success)
            {
                return null;
            }

            content = match.Groups[1].Value;
            try
            {
                AccountHistoryResponse.CursorData cursorData = JsonConvert.DeserializeObject<AccountHistoryResponse.CursorData>(content);
                return cursorData;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 解析历史记录条目
        /// </summary>
        /// <param name="tableElement"></param>
        /// <param name="currencyRates"></param>
        /// <param name="defaultCurrency"></param>
        /// <returns></returns>
        internal static HistoryParseResponse ParseHistory(IElement tableElement, Dictionary<string, double> currencyRates, string defaultCurrency)
        {
            Regex pattern = MatchHistoryItem();

            // 识别货币符号
            string ParseSymbol(string symbol1, string symbol2)
            {
                const char USD = '$';
                const char RMB = '¥';

                string currency = string.Empty;

                if (!string.IsNullOrEmpty(symbol1))
                {
                    if (SymbolCurrency.ContainsKey(symbol1))
                    {
                        currency = SymbolCurrency[symbol1];
                    }
                }

                if (!string.IsNullOrEmpty(symbol2))
                {
                    if (SymbolCurrency.ContainsKey(symbol2))
                    {
                        currency = SymbolCurrency[symbol2];
                    }
                }

                if (string.IsNullOrEmpty(currency))
                {
                    if (symbol1.Contains(USD) || symbol2.Contains(USD))
                    {
                        return "USD";
                    }
                    else if (symbol1.Contains(RMB) || symbol2.Contains(RMB))
                    { //人民币和日元符号相同, 使用钱包默认货币单位
                        return defaultCurrency;
                    }
                    else
                    {
                        ASFLogger.LogGenericWarning(string.Format("检测货币符号失败, 使用默认货币单位 {0}", defaultCurrency));
                        return defaultCurrency;
                    }
                }
                else
                {
                    return currency;
                }
            }

            // 识别货币数值
            int ParseMoneyString(string strMoney)
            {
                Match match = pattern.Match(strMoney);

                if (!match.Success)
                {
                    return 0;
                }
                else
                {
                    bool negative = match.Groups[1].Value == "-";
                    string symbol1 = match.Groups[2].Value.Trim();
                    string strPrice = match.Groups[3].Value;
                    string symbol2 = match.Groups[4].Value.Trim();

                    string currency = ParseSymbol(symbol1, symbol2);

                    bool useDot = DotCurrency.Contains(currency);

                    if (useDot)
                    {
                        strPrice = strPrice.Replace(".", "").Replace(',', '.');
                    }
                    else
                    {
                        strPrice = strPrice.Replace(",", "");
                    }

                    int price;

                    if (double.TryParse(strPrice, out double fPrice))
                    {
                        price = (int)fPrice * 100;
                    }
                    else
                    {
                        strPrice = strPrice.Replace(".", "");
                        if (!int.TryParse(strPrice, out price))
                        {
                            ASFLogger.LogGenericWarning(string.Format("解析价格 {0} 失败", match.Groups[3].Value));
                            return 0;
                        }
                    }

                    if (currencyRates.TryGetValue(currency, out double rate))
                    {
                        return (negative ? -1 : 1) * (int)(price / rate);
                    }
                    else
                    {
                        ASFLogger.LogGenericWarning(string.Format("无 {0} 货币的汇率", currency));
                        return (negative ? -1 : 1) * price;
                    }
                }
            }

            HistoryParseResponse result = new();

            IHtmlCollection<IElement> rows = tableElement.QuerySelectorAll("tr");

            foreach (var row in rows)
            {
                if (!row.HasChildNodes)
                {
                    continue;
                }

                IElement whtItem = row.QuerySelector("td.wht_items");
                IElement whtType = row.QuerySelector("td.wht_type");
                IElement whtTotal = row.QuerySelector("td.wht_total");
                IElement whtChange = row.QuerySelector("td.wht_wallet_change.wallet_column");

                bool isRefund = whtType.ClassName.Contains("wht_refunded");

                string strItem = whtItem?.Text().Trim().Replace("\t", "") ?? "";
                string strType = whtType?.Text().Trim().Replace("\t", "") ?? "";
                string strTotal = whtTotal?.Text().Trim().Replace("\t", "") ?? "";
                string strChange = whtChange?.Text().Trim().Replace("\t", "") ?? "";

                if (!string.IsNullOrEmpty(strType))
                {
                    // 排除退款和转换货币
                    if (!string.IsNullOrEmpty(strType) && !strType.StartsWith("转换") && !strType.StartsWith("退款"))
                    {
                        int total = ParseMoneyString(strTotal);
                        int walletChange;

                        if (string.IsNullOrEmpty(strChange))
                        {
                            walletChange = 0;
                        }
                        else
                        {
                            walletChange = Math.Abs(ParseMoneyString(strChange));
                        }

                        if (total == 0)
                        {
                            continue;
                        }

                        if (strType.StartsWith("购买"))
                        {
                            if (!strItem.Contains("钱包资金"))
                            {
                                if (!isRefund)
                                {
                                    result.StorePurchase += total;
                                    result.StorePurchaseWallet += walletChange;
                                }
                                else
                                {
                                    result.RefundPurchase += total;
                                    result.RefundPurchaseWallet += walletChange;
                                }
                            }
                            else
                            {
                                result.WalletPurchase += total;
                            }
                        }
                        else if (strType.StartsWith("礼物购买"))
                        {
                            if (!isRefund)
                            {
                                result.GiftPurchase += total;
                                result.GiftPurchaseWallet += walletChange;
                            }
                            else
                            {
                                result.RefundPurchase += total;
                                result.RefundPurchaseWallet += walletChange;
                            }
                        }
                        else if (strType.StartsWith("游戏内购买"))
                        {
                            if (!isRefund)
                            {
                                result.InGamePurchase += walletChange;
                            }
                            else
                            {
                                result.RefundPurchase += walletChange;
                                result.RefundPurchaseWallet += walletChange;
                            }
                        }
                        else if (strType.Contains("市场交易"))
                        {
                            if (!isRefund)
                            {
                                if (walletChange >= 0)
                                {
                                    result.MarketSelling += total;
                                }
                                else
                                {
                                    result.MarketPurchase += total;
                                }
                            }
                            else
                            {
                                result.RefundPurchase += walletChange;
                            }
                        }
                        else
                        {
                            if (!isRefund)
                            {
                                result.Other += total;
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 解析Sub页
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static List<LicensesData> ParseLincensesPage(HtmlDocumentResponse response)
        {
            if (response == null)
            {
                return null;
            }

            var trEles = response.Content.SelectNodes<IElement>("//tbody/tr[@data-panel]");

            List<LicensesData> result = new();

            Regex matchSubId = MatchSubId();

            foreach (var ele in trEles)
            {
                var freeLicenseEle = ele.SelectSingleNode<IElement>(".//div[@class='free_license_remove_link']/a");
                string link = freeLicenseEle?.GetAttribute("href");

                var nameEle = ele.SelectSingleNode<IElement>(".//td[2]");
                string name = nameEle?.TextContent ?? "Null";
                string[] args = name.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);

                uint subId = 0;
                if (link != null)
                {
                    Match match = matchSubId.Match(link);
                    if (match.Success)
                    {
                        string strId = match.Groups[1].Value;
                        _ = uint.TryParse(strId, out subId);
                    }

                    if (args.Length >= 2)
                    {
                        name = string.Join(' ', args[1..]);
                    }
                }
                else
                {
                    name = string.Join(' ', args);
                }

                IElement typeEle = ele.SelectSingleNode<IElement>(".//td[3]");
                string typeStr = typeEle?.TextContent.Trim();

                LicenseType licenseType = typeStr switch {
                    "零售" => LicenseType.Retail,
                    "免费赠送" => LicenseType.Complimentary,
                    "Steam 商店" => LicenseType.SteamStore,
                    "礼物/玩家通行证" => LicenseType.GiftOrGuestPass,
                    _ => LicenseType.Unknown,
                };

                result.Add(new() {
                    Type = licenseType,
                    Name = name,
                    PackageId = subId,
                });
            }

            return result;
        }

        /// <summary>
        /// 解析电子邮件偏好
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static EmailOptions? ParseEmailOptionPage(HtmlDocumentResponse response)
        {
            if (response == null)
            {
                return null;
            }

            var inputEles = response.Content.QuerySelectorAll<IElement>("input[name^='opt']");

            EmailOptions result = new();

            foreach (var ele in inputEles)
            {
                bool check = ele.HasAttribute("checked");

                switch (ele.Id)
                {
                    case "opt_out_prefs":
                        result.EnableEmailNotification = check;
                        break;
                    case "opt_out_wishlist":
                        result.WhenWishlistDiscount = check;
                        break;
                    case "opt_out_wishlist_release":
                        result.WhenWishlistRelease = check;
                        break;
                    case "opt_out_greenlight_release":
                        result.WhenGreenLightRelease = check;
                        break;
                    case "opt_out_creator_home_releases":
                        result.WhenFollowPublisherRelease = check;
                        break;
                    case "opt_out_seasonal":
                        result.WhenSaleEvent = check;
                        break;
                    case "opt_out_curator_connect":
                        result.WhenReceiveCuratorReview = check;
                        break;
                    case "opt_out_loyalty_awards_received":
                        result.WhenReceiveCommunityReward = check;
                        break;
                    case "opt_out_in_library_events":
                        result.WhenGameEventNotification = check;
                        break;
                    case "opt_out_all":
                        break;
                    default:
                        break;
                }
            }

            return result;
        }
    }
}
