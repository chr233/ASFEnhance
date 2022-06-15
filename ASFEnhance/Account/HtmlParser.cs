#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using AngleSharp.Dom;
using ArchiSteamFarm.Web.Responses;
using ArchiSteamFarm.Web;
using ArchiSteamFarm.Core;
using ASFEnhance.Data;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using static ASFEnhance.Account.CurrencyHelper;
using static ASFEnhance.Utils;

namespace ASFEnhance.Account
{
    internal static class HtmlParser
    {
        /// <summary>
        /// 获取Cursor对象
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static AccountHistoryResponse.CursorData? ParseCursorData(HtmlDocumentResponse response)
        {
            if (response == null)
            {
                return null;
            }

            string content = response.Content.Body.InnerHtml;
            Match match = Regex.Match(content, @"g_historyCursor = ([^;]+)");
            if (!match.Success)
            {
                return null;
            }

            content = match.Groups[1].Value;
            try
            {
                AccountHistoryResponse.CursorData? cursorData = JsonConvert.DeserializeObject<AccountHistoryResponse.CursorData>(content);
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
            Regex pattern = new(@"^\s*([-+])?([^\d,.]*)([\d,.]+)([^\d,.]*)$");

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

                    if (currencyRates.ContainsKey(currency))
                    {
                        double rate = currencyRates[currency];
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
                        else if (strType.StartsWith("市场交易") || strType.Contains("市场交易"))
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

        internal static List<LicensesData>? ParseLincensesPage(HtmlDocumentResponse response)
        {
            if (response == null)
            {
                return null;
            }

            var trEles = response.Content.SelectNodes("//tbody/tr[@data-panel]");

            List<LicensesData> result = new();

            Regex matchSubID = new(@"\( (\d+),");

            foreach (var ele in trEles)
            {
                IElement? freeLicenseEle = ele.SelectSingleElementNode(".//div[@class='free_license_remove_link']/a");
                string? link = freeLicenseEle?.GetAttribute("href");

                IElement nameEle = ele.SelectSingleElementNode(".//td[2]");
                string name = nameEle?.TextContent ?? "Null";
                string[] args = name.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);

                uint subID = 0;
                if (link != null)
                {
                    Match match = matchSubID.Match(link);
                    if (match.Success)
                    {
                        string strID = match.Groups[1].Value;
                        uint.TryParse(strID, out subID);
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

                IElement typeEle = ele.SelectSingleElementNode(".//td[3]");
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
                    PackageID = subID,
                });
            }

            return result;
        }
    }
}
