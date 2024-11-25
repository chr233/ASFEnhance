using ArchiSteamFarm.Core;
using ArchiSteamFarm.Helpers.Json;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;
using ASFEnhance.Data.IAccountPrivateAppsService;
using ASFEnhance.Data.Plugin;
using ASFEnhance.Data.WebApi;
using System.Net;
using System.Text;
using static ASFEnhance.Account.CurrencyHelper;

namespace ASFEnhance.Account;

internal static class WebRequest
{
    /// <summary>
    ///     加载账户历史记录
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="cursorData"></param>
    /// <returns></returns>
    private static async Task<AccountHistoryResponse?> AjaxLoadMoreHistory(Bot bot,
        AccountHistoryResponse.CursorData cursorData)
    {
        var request = new Uri(SteamStoreURL, "/account/AjaxLoadMoreHistory/?l=schinese");

        var data = new Dictionary<string, string>(5, StringComparer.Ordinal)
        {
            { "cursor[wallet_txnid]", cursorData.WalletTxnid },
            { "cursor[timestamp_newest]", cursorData.TimestampNewest.ToString() },
            { "cursor[balance]", cursorData.Balance },
            { "cursor[currency]", cursorData.Currency.ToString() }
        };

        var response = await bot.ArchiWebHandler!
            .UrlPostToJsonObjectWithSession<AccountHistoryResponse>(request, referer: SteamStoreURL, data: data)
            .ConfigureAwait(false);

        return response?.Content;
    }

    /// <summary>
    ///     获取在线汇率
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="currency"></param>
    /// <returns></returns>
    private static async Task<ExchangeAPIResponse?> GetExchangeRatio(string currency)
    {
        var request = new Uri($"https://api.exchangerate-api.com/v4/latest/{currency}");
        var response = await ASF.WebBrowser!.UrlGetToJsonObject<ExchangeAPIResponse>(request).ConfigureAwait(false);
        return response?.Content;
    }

    /// <summary>
    ///     获取更多历史记录
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    private static async Task<HtmlDocumentResponse?> GetAccountHistoryAjax(Bot bot)
    {
        var request = new Uri(SteamStoreURL, "/account/history?l=schinese");
        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL)
            .ConfigureAwait(false);
        return response;
    }

    /// <summary>
    ///     获取账号消费历史记录
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string> GetAccountHistoryDetail(Bot bot)
    {
        // 读取在线汇率
        var myCurrency = bot.WalletCurrency.ToString();
        var exchangeRate = await GetExchangeRatio(myCurrency).ConfigureAwait(false);
        if (exchangeRate == null)
        {
            return bot.FormatBotResponse(Langs.GetExchangeRateFailed);
        }

        // 获取货币符号
        if (!Currency2Symbol.TryGetValue(myCurrency, out var symbol))
        {
            symbol = myCurrency;
        }

        var result = new StringBuilder();
        result.AppendLine(bot.FormatBotResponse(Langs.MultipleLineResult));

        var giftedSpend = 0;
        var totalSpend = 0;
        var totalExternalSpend = 0;

        // 读取账户消费历史
        result.AppendLine(Langs.PurchaseHistorySummary);
        var accountHistory = await GetAccountHistoryAjax(bot).ConfigureAwait(false);
        if (accountHistory == null)
        {
            return Langs.NetworkError;
        }

        // 解析表格元素
        var tbodyElement = accountHistory?.Content?.QuerySelector("table>tbody");
        if (tbodyElement == null)
        {
            return Langs.ParseHtmlFailed;
        }

        // 获取下一页指针(为null代表没有下一页)
        var cursor = HtmlParser.ParseCursorData(accountHistory);

        var historyData = HtmlParser.ParseHistory(tbodyElement, exchangeRate.Rates, myCurrency);

        while (cursor != null)
        {
            var ajaxHistoryResponse = await AjaxLoadMoreHistory(bot, cursor).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(ajaxHistoryResponse?.HtmlContent))
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
        totalExternalSpend = historyData.StorePurchase - historyData.StorePurchaseWallet + historyData.GiftPurchase -
                             historyData.GiftPurchaseWallet;

        result.AppendLine(Langs.PurchaseHistoryGroupType);
        result.AppendLineFormat(Langs.PurchaseHistoryTypeStorePurchase, historyData.StorePurchase / 100.0, symbol);
        result.AppendLineFormat(Langs.PurchaseHistoryTypeExternal,
            (historyData.StorePurchase - historyData.StorePurchaseWallet) / 100.0, symbol);
        result.AppendLineFormat(Langs.PurchaseHistoryTypeWallet, historyData.StorePurchaseWallet / 100.0, symbol);
        result.AppendLineFormat(Langs.PurchaseHistoryTypeGiftPurchase, historyData.GiftPurchase / 100.0, symbol);
        result.AppendLineFormat(Langs.PurchaseHistoryTypeExternal,
            (historyData.GiftPurchase - historyData.GiftPurchaseWallet) / 100.0, symbol);
        result.AppendLineFormat(Langs.PurchaseHistoryTypeWallet, historyData.GiftPurchaseWallet / 100.0, symbol);
        result.AppendLineFormat(Langs.PurchaseHistoryTypeInGamePurchase, historyData.InGamePurchase / 100.0, symbol);
        result.AppendLineFormat(Langs.PurchaseHistoryTypeMarketPurchase, historyData.MarketPurchase / 100.0, symbol);
        result.AppendLineFormat(Langs.PurchaseHistoryTypeMarketSelling, historyData.MarketSelling / 100.0, symbol);

        result.AppendLine(Langs.PurchaseHistoryGroupOther);
        result.AppendLineFormat(Langs.PurchaseHistoryTypeWalletPurchase, historyData.WalletPurchase / 100.0, symbol);
        result.AppendLineFormat(Langs.PurchaseHistoryTypeOther, historyData.Other / 100.0, symbol);
        result.AppendLineFormat(Langs.PurchaseHistoryTypeRefunded, historyData.RefundPurchase / 100.0, symbol);
        result.AppendLineFormat(Langs.PurchaseHistoryTypeExternal,
            (historyData.RefundPurchase - historyData.RefundPurchaseWallet) / 100.0, symbol);
        result.AppendLineFormat(Langs.PurchaseHistoryTypeWallet, historyData.RefundPurchaseWallet / 100.0, symbol);

        result.AppendLine(Langs.PurchaseHistoryGroupStatus);
        result.AppendLineFormat(Langs.PurchaseHistoryStatusTotalPurchase, totalSpend / 100.0, symbol);
        result.AppendLineFormat(Langs.PurchaseHistoryStatusTotalExternalPurchase, totalExternalSpend / 100.0, symbol);
        result.AppendLineFormat(Langs.PurchaseHistoryStatusTotalGift, giftedSpend / 100.0, symbol);
        result.AppendLine(Langs.PurchaseHistoryGroupGiftCredit);
        result.AppendLineFormat(Langs.PurchaseHistoryCreditMin, (totalSpend - giftedSpend) / 100, symbol);
        result.AppendLineFormat(Langs.PurchaseHistoryCreditMax, ((totalSpend * 1.8) - giftedSpend) / 100, symbol);
        result.AppendLineFormat(Langs.PurchaseHistoryExternalMin, (totalExternalSpend - giftedSpend) / 100, symbol);
        result.AppendLineFormat(Langs.PurchaseHistoryExternalMax, ((totalExternalSpend * 1.8) - giftedSpend) / 100,
            symbol);

        var updateTime = DateTimeOffset.FromUnixTimeSeconds(exchangeRate.UpdateTime).UtcDateTime;

        result.AppendLine(Langs.PurchaseHistoryGroupAbout);
        result.AppendLineFormat(Langs.PurchaseHistoryAboutBaseRate, exchangeRate.Base);
        result.AppendLineFormat(Langs.PurchaseHistoryAboutPlugin, nameof(ASFEnhance));
        result.AppendLineFormat(Langs.PurchaseHistoryAboutUpdateTime, updateTime);
        result.AppendLine(Langs.PurchaseHistoryAboutRateSource);

        return result.ToString();
    }

    /// <summary>
    ///     获取许可证信息
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<List<LicensesData>?> GetOwnedLicenses(Bot bot)
    {
        var request = new Uri(SteamStoreURL, "/account/licenses/?l=schinese");
        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL)
            .ConfigureAwait(false);
        return HtmlParser.ParseLincensesPage(response);
    }

    /// <summary>
    ///     移除许可证
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="subId"></param>
    /// <returns></returns>
    internal static async Task<bool> RemoveLicense(Bot bot, uint subId)
    {
        var request = new Uri(SteamStoreURL, "/account/removelicense");
        var referer = new Uri(SteamStoreURL, "/account/licenses/");

        var data = new Dictionary<string, string>(2) { { "packageid", subId.ToString() } };

        var response = await bot.ArchiWebHandler.UrlPostToHtmlDocumentWithSession(request, data: data, referer: referer)
            .ConfigureAwait(false);
        return response?.StatusCode == HttpStatusCode.OK;
    }

    /// <summary>
    ///     获取邮箱通知偏好
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<EmailOptions?> GetAccountEmailOptions(Bot bot)
    {
        var request = new Uri(SteamStoreURL, "/account/emailoptout");
        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL)
            .ConfigureAwait(false);
        return HtmlParser.ParseEmailOptionPage(response);
    }

    /// <summary>
    ///     设置邮箱通知偏好
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="option"></param>
    /// <returns></returns>
    internal static async Task<EmailOptions?> SetAccountEmailOptions(Bot bot, EmailOptions option)
    {
        var request = new Uri(SteamStoreURL, "/account/emailoptout");

        var data = new Dictionary<string, string>(11)
        {
            { "action", "save" }, { "opt_out_all", option.EnableEmailNotification ? "0" : "1" }
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

        var response = await bot.ArchiWebHandler
            .UrlPostToHtmlDocumentWithSession(request, referer: SteamStoreURL, data: data).ConfigureAwait(false);
        return HtmlParser.ParseEmailOptionPage(response);
    }


    /// <summary>
    ///     获取通知偏好
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<NotificationOptions?> GetAccountNotificationOptions(Bot bot)
    {
        var request = new Uri(SteamStoreURL, "/account/notificationsettings");
        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL)
            .ConfigureAwait(false);
        return HtmlParser.ParseNotificationOptionPage(response);
    }

    /// <summary>
    ///     设置通知偏好
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="option"></param>
    /// <returns></returns>
    internal static async Task<BaseResultResponse?> SetAccountNotificationOptions(Bot bot, NotificationOptions option)
    {
        var request = new Uri(SteamStoreURL, "/account/ajaxsetnotificationsettings");

        var optionList = new List<NotificationPayload>
        {
            new(ENotificationType.ReceivedGift, option.ReceivedGift),
            new(ENotificationType.SubscribedDissionReplyed, option.SubscribedDissionReplyed),
            new(ENotificationType.ReceivedNewItem, option.ReceivedNewItem),
            new(ENotificationType.MajorSaleStart, option.MajorSaleStart),
            new(ENotificationType.ItemInWishlistOnSale, option.ItemInWishlistOnSale),
            new(ENotificationType.ReceivedTradeOffer, option.ReceivedTradeOffer),
            new(ENotificationType.ReceivedSteamSupportReply, option.ReceivedSteamSupportReply),
            new(ENotificationType.SteamTurnNotification, option.SteamTurnNotification)
        };

        var json = optionList.ToJsonText();

        var data = new Dictionary<string, string>(11) { { "notificationpreferences", json } };

        var response = await bot.ArchiWebHandler
            .UrlPostToJsonObjectWithSession<BaseResultResponse>(request, referer: SteamStoreURL, data: data)
            .ConfigureAwait(false);
        return response?.Content;
    }

    /// <summary>
    ///     获取用户封禁状态
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="token"></param>
    /// <param name="steamids"></param>
    /// <returns></returns>
    internal static async Task<GetPlayerBansResponse?> GetPlayerBans(Bot bot, string token, ulong steamids)
    {
        var request = new Uri(SteamApiURL, $"/ISteamUser/GetPlayerBans/v1/?key={token}&steamids={steamids}");
        var response = await bot.ArchiWebHandler
            .UrlGetToJsonObjectWithSession<GetPlayerBansResponse>(request, referer: SteamStoreURL)
            .ConfigureAwait(false);
        return response?.Content;
    }

    internal static async Task<string?> GetAccountBans(Bot bot)
    {
        var request = new Uri(SteamCommunityURL, $"/profiles/{bot.SteamID}/currentbans");
        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL)
            .ConfigureAwait(false);

        return HtmlParser.ParseAccountBans(response?.Content);
    }

    /// <summary>
    ///     获取礼物Id
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<HashSet<ulong>?> GetReceivedGift(Bot bot)
    {
        var request = new Uri(SteamStoreURL, "/gifts");

        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

        return HtmlParser.ParseGiftPage(response);
    }

    /// <summary>
    ///     接收礼物
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="giftId"></param>
    /// <returns></returns>
    internal static async Task<UnpackGiftResponse?> AcceptReceivedGift(Bot bot, ulong giftId)
    {
        var request = new Uri(SteamStoreURL, $"/gifts/{giftId}/unpack");

        var response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<UnpackGiftResponse>(request, null, null)
            .ConfigureAwait(false);

        return response?.Content;
    }

    /// <summary>
    ///     获取游戏游玩时间
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    internal static async Task<Dictionary<uint, GetOwnedGamesResponse.GameData>?> GetGamePlayTime(Bot bot, string token)
    {
        var request = new Uri(SteamApiURL,
            $"/IPlayerService/GetOwnedGames/v1/?access_token={token}&steamid={bot.SteamID}&include_appinfo=true&include_played_free_games=true&include_free_sub=true&skip_unvetted_apps=true&language={DefaultOrCurrentLanguage}&include_extended_appinfo=true");
        var response = await bot.ArchiWebHandler
            .UrlGetToJsonObjectWithSession<GetOwnedGamesResponse>(request, referer: SteamStoreURL)
            .ConfigureAwait(false);

        if (response?.Content?.Response?.Games != null)
        {
            var result = new Dictionary<uint, GetOwnedGamesResponse.GameData>();

            foreach (var game in response.Content.Response.Games)
            {
                result.TryAdd(game.AppId, game);
            }

            return result;
        }

        return null;
    }

    /// <summary>
    ///     获取账号邮箱
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> GetAccountEmail(Bot bot)
    {
        var request = new Uri(SteamStoreURL, "/account");
        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL)
            .ConfigureAwait(false);
        return HtmlParser.ParseAccountEmail(response?.Content);
    }

    /// <summary>
    ///     检查是否存在ApiKey
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<bool?> CheckApiKey(Bot bot)
    {
        var request = new Uri(SteamCommunityURL, "/dev/apikey");
        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL)
            .ConfigureAwait(false);

        if (response?.Content == null)
        {
            return null;
        }

        return response.Content.QuerySelector("#BG_bottom form") != null;
    }

    /// <summary>
    ///     注销APIKey
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task RevokeApiKey(Bot bot)
    {
        var request = new Uri(SteamCommunityURL, "/dev/revokekey");

        var data = new Dictionary<string, string>(2) { { "Revoke", "Revoke+My+Steam+Web+API+Key" } };

        await bot.ArchiWebHandler.UrlPostWithSession(request, data: data).ConfigureAwait(false);
    }

    /// <summary>
    ///     获取私密应用列表
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    /// <exception cref="AccessTokenNullException"></exception>
    internal static async Task<GetPrivateAppListResponse?> GetPrivateAppList(Bot bot)
    {
        var token = bot.AccessToken ?? throw new AccessTokenNullException();
        var request = new Uri(SteamApiURL, $"/IAccountPrivateAppsService/GetPrivateAppList/v1/?access_token={token}");

        var response = await bot.ArchiWebHandler
            .UrlGetToJsonObjectWithSession<AbstractResponse<GetPrivateAppListResponse>>(request).ConfigureAwait(false);

        return response?.Content?.Response;
    }

    /// <summary>
    ///     设置私密应用
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="appIds"></param>
    /// <param name="isPrivate"></param>
    /// <returns></returns>
    /// <exception cref="AccessTokenNullException"></exception>
    internal static async Task<bool> ToggleAppPrivacy(Bot bot, List<uint> appIds, bool isPrivate)
    {
        var request = new Uri(SteamApiURL, "/IAccountPrivateAppsService/ToggleAppPrivacy/v1/");

        var data = new Dictionary<string, string>(2)
        {
            { "access_token", bot.AccessToken ?? throw new AccessTokenNullException() },
            { "private", isPrivate ? "true" : "false" }
        };

        var i = 0;
        foreach (var appId in appIds)
        {
            data.Add($"appids[{i++}]", appId.ToString());
        }

        var response = await bot.ArchiWebHandler.UrlPost(request, data).ConfigureAwait(false);
        return response;
    }

    /// <summary>
    ///     获取市场是否受限
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<bool?> GetIfMarketLimited(Bot bot)
    {
        var request = new Uri(SteamCommunityURL, "/market/");
        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

        if (response?.Content == null)
        {
            return null;
        }

        var helpLink = response.Content.QuerySelector("#headertooltip a.tooltip")?.GetAttribute("href");
        if (string.IsNullOrEmpty(helpLink) || !helpLink.Contains("451E-96B3-D194-50FC"))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    ///     获取电话号码后缀
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> GetPhoneSuffix(Bot bot)
    {
        var request = new Uri(SteamStoreURL, $"/phone/manage?l={Langs.Language}");
        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

        if (response?.Content == null)
        {
            return null;
        }

        return response.Content.QuerySelector("div.phone_header_description>span")?.TextContent?.Trim();
    }

    /// <summary>
    ///     获取注册时间
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> GetRegisteDate(Bot bot)
    {
        var request = new Uri(SteamCommunityURL, $"/profiles/{bot.SteamID}/badges/1?l={Langs.Language}");
        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

        if (response?.Content == null)
        {
            return null;
        }

        return response.Content.QuerySelector("div.badge_description")?.TextContent?.Trim();
    }

    internal static async Task<string?> GetMyBans(Bot bot)
    {
        var request = new Uri(SteamHelpURL, "/wizard/VacBans");
        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

        if (response?.Content == null)
        {
            return null;
        }


        var bans = response.Content.QuerySelectorAll("div.refund_info_box>div>span");

        if (bans.Length == 0)
        {
            return "未收到封禁";
        }

        var sb = new StringBuilder();
        foreach (var ban in bans)
        {
            sb.AppendLine(ban.TextContent.Trim());
        }

        return sb.ToString();
    }
}