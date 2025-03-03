using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Helpers.Json;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;
using ASFEnhance.Data.Plugin;
using System.Globalization;
using System.Text;
using static ASFEnhance.Account.CurrencyHelper;

namespace ASFEnhance.Account;

static class HtmlParser
{
    /// <summary>
    ///     获取Cursor对象
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    internal static AccountHistoryResponse.CursorData? ParseCursorData(HtmlDocumentResponse? response)
    {
        if (response?.Content?.Body == null)
        {
            return null;
        }

        var content = response.Content.Body.InnerHtml;
        var match = RegexUtils.MatchHistortyCursor().Match(content);
        if (!match.Success)
        {
            return null;
        }

        content = match.Groups[1].Value;
        try
        {
            var cursorData = content.ToJsonObject<AccountHistoryResponse.CursorData>();
            return cursorData;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    ///     解析历史记录条目
    /// </summary>
    /// <param name="tableElement"></param>
    /// <param name="currencyRates"></param>
    /// <param name="defaultCurrency"></param>
    /// <returns></returns>
    internal static HistoryParseResponse ParseHistory(IElement tableElement, Dictionary<string, decimal> currencyRates,
        string defaultCurrency)
    {
        var pattern = RegexUtils.MatchHistoryItem();

        // 识别货币符号
        string ParseSymbol(string symbol1, string symbol2)
        {
            const char USD = '$';
            const char RMB = '¥';

            var currency = string.Empty;

            if (!string.IsNullOrEmpty(symbol1))
            {
                if (SymbolCurrency.TryGetValue(symbol1, out var c))
                {
                    currency = c;
                }
            }

            if (!string.IsNullOrEmpty(symbol2))
            {
                if (SymbolCurrency.TryGetValue(symbol2, out var c))
                {
                    currency = c;
                }
            }

            if (string.IsNullOrEmpty(currency))
            {
                if (symbol1.Contains(USD) || symbol2.Contains(USD))
                {
                    return "USD";
                }

                if (symbol1.Contains(RMB) || symbol2.Contains(RMB))
                {
                    //人民币和日元符号相同, 使用钱包默认货币单位
                    return defaultCurrency;
                }

                ASFLogger.LogGenericWarning($"检测货币符号失败, 使用默认货币单位 {defaultCurrency}");
                return defaultCurrency;
            }

            return currency;
        }

        // 识别货币数值
        decimal ParseMoneyString(string strMoney)
        {
            var match = pattern.Match(strMoney);

            if (!match.Success)
            {
                return 0;
            }

            var negative = match.Groups[1].Value == "-";
            var symbol1 = match.Groups[2].Value.Trim();
            var strPrice = match.Groups[3].Value;
            var symbol2 = match.Groups[4].Value.Trim();

            var currency = ParseSymbol(symbol1, symbol2);

            // 获取当前文化信息
            var currentCulture = CultureInfo.CurrentCulture;
            var numberFormat = (NumberFormatInfo)currentCulture.NumberFormat.Clone();

            // 根据货币符号设置小数点和千分位分隔符
            if (DotCurrency.Contains(currency))
            {
                numberFormat.CurrencyDecimalSeparator = ".";
                numberFormat.CurrencyGroupSeparator = ",";
            }
            else
            {
                numberFormat.CurrencyDecimalSeparator = ",";
                numberFormat.CurrencyGroupSeparator = ".";
            }

            if (decimal.TryParse(strPrice, NumberStyles.Currency, numberFormat,
                    out var price))
            {
                if (currencyRates.TryGetValue(currency, out var rate))
                {
                    return (negative ? -1 : 1) * (price / rate);
                }

                ASFLogger.LogGenericWarning($"无 {currency} 货币的汇率");
                return (negative ? -1 : 1) * price;
            }

            ASFLogger.LogGenericWarning($"解析价格 {match.Groups[3].Value} 失败");
            return 0;
        }

        HistoryParseResponse result = new();

        var rows = tableElement.QuerySelectorAll("tr");

        foreach (var row in rows)
        {
            if (!row.HasChildNodes)
            {
                continue;
            }

            var whtItem = row?.QuerySelector("td.wht_items");
            var whtType = row?.QuerySelector("td.wht_type");
            var whtTotal = row?.QuerySelector("td.wht_total");
            var whtChange = row?.QuerySelector("td.wht_wallet_change.wallet_column");

            var isRefund = whtType?.ClassName?.Contains("wht_refunded") ?? false;

            var strItem = whtItem?.Text().Trim().Replace("\t", "") ?? "";
            var strType = whtType?.Text().Trim().Replace("\t", "") ?? "";
            var strTotal = whtTotal?.Text().Replace("资金", "").Trim().Replace("\t", "") ?? "";
            var strChange = whtChange?.Text().Trim().Replace("\t", "") ?? "";

            if (!string.IsNullOrEmpty(strType))
            {
                // 排除退款和转换货币
                if (!string.IsNullOrEmpty(strType) && !strType.StartsWith("转换") && !strType.StartsWith("退款"))
                {
                    var total = (int)(ParseMoneyString(strTotal) * 100);

                    int walletChange;
                    int walletChangeAbs;

                    if (string.IsNullOrEmpty(strChange))
                    {
                        walletChange = 0;
                    }
                    else
                    {
                        walletChange = (int)(ParseMoneyString(strChange) * 100);
                    }

                    walletChangeAbs = Math.Abs(walletChange);

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
                                result.StorePurchaseWallet += walletChangeAbs;
                            }
                            else
                            {
                                result.RefundPurchase += total;
                                result.RefundPurchaseWallet += walletChangeAbs;
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
                            result.GiftPurchaseWallet += walletChangeAbs;
                        }
                        else
                        {
                            result.RefundPurchase += total;
                            result.RefundPurchaseWallet += walletChangeAbs;
                        }
                    }
                    else if (strType.StartsWith("游戏内购买"))
                    {
                        if (!isRefund)
                        {
                            result.InGamePurchase += walletChangeAbs;
                        }
                        else
                        {
                            result.RefundPurchase += total;
                            result.RefundPurchaseWallet += walletChangeAbs;
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
                            result.RefundPurchase += total;
                        }
                    }
                    else
                    {
                        if (!isRefund)
                        {
                            result.Other += total;
                        }
                        else
                        {
                            result.RefundPurchase -= total;
                        }
                    }
                }
            }
        }

        return result;
    }

    /// <summary>
    ///     解析Sub页
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    internal static List<LicensesData>? ParseLincensesPage(HtmlDocumentResponse? response)
    {
        if (response?.Content == null)
        {
            return null;
        }

        var trEles = response.Content.SelectNodes<IElement>("//tbody/tr[@data-panel]");

        var result = new List<LicensesData>();

        var matchSubId = RegexUtils.MatchSubId();

        foreach (var ele in trEles)
        {
            var freeLicenseEle = ele.QuerySelector("div.free_license_remove_link>a");
            var link = freeLicenseEle?.GetAttribute("href");

            var nameEle = ele.SelectSingleNode<IElement>(".//td[2]");
            var name = nameEle?.TextContent ?? "Null";
            var args = name.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);

            uint subId = 0;
            if (link != null)
            {
                var match = matchSubId.Match(link);
                if (match.Success)
                {
                    var strId = match.Groups[1].Value;
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

            var typeEle = ele.SelectSingleNode<IElement>(".//td[3]");
            var typeStr = typeEle?.TextContent.Trim();

            var licenseType = typeStr switch
            {
                "零售" => LicenseType.Retail,
                "免费赠送" => LicenseType.Complimentary,
                "Steam 商店" => LicenseType.SteamStore,
                "礼物/玩家通行证" => LicenseType.GiftOrGuestPass,
                _ => LicenseType.Unknown
            };

            result.Add(new LicensesData { Type = licenseType, Name = name, PackageId = subId });
        }

        return result;
    }

    /// <summary>
    ///     解析电子邮件偏好
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    internal static EmailOptions? ParseEmailOptionPage(HtmlDocumentResponse? response)
    {
        if (response?.Content == null)
        {
            return null;
        }

        var inputEles = response.Content.QuerySelectorAll<IElement>("input[name^='opt']");

        var result = new EmailOptions();

        foreach (var ele in inputEles)
        {
            var check = ele.HasAttribute("checked");

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
            }
        }

        return result;
    }

    /// <summary>
    ///     解析通知偏好
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    internal static NotificationOptions? ParseNotificationOptionPage(HtmlDocumentResponse? response)
    {
        if (response?.Content == null)
        {
            return null;
        }

        var element = response.Content.QuerySelector<IElement>("#application_config");
        var payload = element?.GetAttribute("data-notificationpreferences");

        if (payload == null)
        {
            return null;
        }

        try
        {
            var optionsList = payload.ToJsonObject<List<NotificationPayload>>();
            if (optionsList == null)
            {
                return null;
            }

            var result = new NotificationOptions();

            foreach (var option in optionsList)
            {
                switch (option.NotificationType)
                {
                    case ENotificationType.ReceivedGift:
                        result.ReceivedGift = option.NotificationTargets;
                        break;
                    case ENotificationType.SubscribedDissionReplyed:
                        result.SubscribedDissionReplyed = option.NotificationTargets;
                        break;
                    case ENotificationType.ReceivedNewItem:
                        result.ReceivedNewItem = option.NotificationTargets;
                        break;
                    case ENotificationType.ReceivedFriendInvitation:
                        result.ReceivedFriendInvitation = option.NotificationTargets;
                        break;
                    case ENotificationType.MajorSaleStart:
                        result.MajorSaleStart = option.NotificationTargets;
                        break;
                    case ENotificationType.ItemInWishlistOnSale:
                        result.ItemInWishlistOnSale = option.NotificationTargets;
                        break;
                    case ENotificationType.ReceivedTradeOffer:
                        result.ReceivedTradeOffer = option.NotificationTargets;
                        break;
                    case ENotificationType.ReceivedSteamSupportReply:
                        result.ReceivedSteamSupportReply = option.NotificationTargets;
                        break;
                    case ENotificationType.SteamTurnNotification:
                        result.SteamTurnNotification = option.NotificationTargets;
                        break;
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            ASFLogger.LogGenericException(ex);
            return null;
        }
    }

    /// <summary>
    ///     解析礼物页面
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    internal static HashSet<ulong>? ParseGiftPage(HtmlDocumentResponse? response)
    {
        if (response?.Content == null)
        {
            return null;
        }

        var result = new HashSet<ulong>();

        var eleAcceptBtns = response.Content.QuerySelectorAll(".gift_controls_buttons>div[id$='init']");
        if (eleAcceptBtns.Length > 0)
        {
            var regex = RegexUtils.MatchGiftId();
            foreach (var ele in eleAcceptBtns)
            {
                var eleId = ele.Id ?? "";
                var match = regex.Match(eleId);
                if (match.Success && ulong.TryParse(match.Groups[1].Value, out var giftId))
                {
                    result.Add(giftId);
                }
            }
        }

        return result;
    }

    /// <summary>
    ///     解析账号邮箱
    /// </summary>
    /// <param name="document"></param>
    /// <returns></returns>
    internal static string? ParseAccountEmail(IDocument? document)
    {
        if (document == null)
        {
            return null;
        }

        var eleEmail =
            document.QuerySelector(
                "#main_content div.account_setting_sub_block:nth-child(1) > div:nth-child(2) span.account_data_field");
        return eleEmail?.TextContent;
    }

    internal static string? ParseAccountBans(IDocument? document)
    {
        if (document == null)
        {
            return null;
        }

        var sb = new StringBuilder();
        var elements = document.QuerySelectorAll("div.maincontent > h2");
        if (elements.Length > 0)
        {
            sb.AppendLine(Langs.BanRecords);
            foreach (var ele in elements)
            {
                sb.AppendLine(ele.TextContent.Trim());
            }

            return sb.ToString();
        }

        return Langs.NoBanRecords;
    }
}