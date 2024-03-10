using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Account;
using ASFEnhance.Data.Common;
using ASFEnhance.Data.Plugin;
using ASFEnhance.Store;
using SteamKit2;
using System.Text;


namespace ASFEnhance.Cart;

internal static class Command
{
    /// <summary>
    /// 读取购物车
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetCartGames(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var cartResponse = await WebRequest.GetAccountCart(bot).ConfigureAwait(false);
        var cartItems = cartResponse?.Cart?.LineItems;

        if (cartItems == null || cartItems.Count == 0)
        {
            return bot.FormatBotResponse(Langs.CartIsEmpty);
        }

        var gameIds = new HashSet<SteamGameId>();
        foreach (var item in cartItems)
        {
            if (item.PackageId > 0)
            {
                gameIds.Add(new SteamGameId(ESteamGameIdType.Sub, item.PackageId.Value));
            }
            else if (item.BundleId > 0)
            {
                gameIds.Add(new SteamGameId(ESteamGameIdType.Bundle, item.BundleId.Value));
            }
        }

        var getItemResponse = await bot.GetStoreItems(gameIds).ConfigureAwait(false);
        var gameInfos = getItemResponse?.StoreItems;

        var gameNameDict = new Dictionary<uint, string>();

        if (gameInfos != null)
        {
            foreach (var info in gameInfos)
            {
                gameNameDict.TryAdd(info.Id, info.Name ?? "Unknown");
            }
        }

        string walletCurrency = bot.WalletCurrency != ECurrencyCode.Invalid ? bot.WalletCurrency.ToString() : "";

        if (!CurrencyHelper.Currency2Symbol.TryGetValue(walletCurrency, out var currencySymbol))
        {
            currencySymbol = walletCurrency;
        }

        var sb = new StringBuilder();

        sb.AppendLine(bot.FormatBotResponse(Langs.MultipleLineResult));
        sb.AppendLineFormat(Langs.CartIsValid, Bool2Str(cartResponse?.Cart?.IsValid == true));

        decimal cartValue = 0;
        foreach (var item in cartItems)
        {
            if (string.IsNullOrEmpty(item.LineItemId) || (item.BundleId == 0 && item.PackageId == 0))
            {
                sb.AppendLineFormat(Langs.CartItem, item.LineItemId, Langs.CanNotParse);
                continue;
            }

            if (decimal.TryParse(item.PriceWhenAdded?.AmountInCents, out var coast))
            {
                cartValue += coast;
            }

            var price = item.PriceWhenAdded?.FormattedAmount ?? "??";
            if (!gameNameDict.TryGetValue(item.PackageId ?? item.BundleId ?? 0, out var gameName))
            {
                gameName = Langs.KeyNotFound;
            }

            if (item.PackageId > 0)
            {
                sb.AppendLineFormat(Langs.CartItemSub, item.LineItemId, item.PackageId);
            }
            else if (item.BundleId > 0)
            {
                sb.AppendLineFormat(Langs.CartItemBundle, item.LineItemId, item.PackageId);
            }

            sb.AppendLineFormat(Langs.AppDetailName, gameName);
            sb.AppendLineFormat(Langs.AppDetailPrice, price);

            if (item.Flags?.IsPrivate == true)
            {
                sb.AppendLine(Langs.CartPrivate);
            }

            if (item.Flags?.IsGift == true)
            {
                if (item.GiftInfo?.AccountIdGiftee > 0)
                {
                    sb.AppendLineFormat(Langs.CartGift, item.GiftInfo.AccountIdGiftee);
                    if (item.GiftInfo != null)
                    {
                        sb.AppendLineFormat(Langs.CartGiftGifteeName, item.GiftInfo.GiftMessage?.GifteeName);
                        sb.AppendLineFormat(Langs.CartGiftMessage, item.GiftInfo.GiftMessage?.Message);
                        sb.AppendLineFormat(Langs.CartGiftSignature, item.GiftInfo.GiftMessage?.Signature);
                        sb.AppendLineFormat(Langs.CartGiftSentiment, item.GiftInfo.GiftMessage?.Sentiment);
                    }
                }
                else
                {
                    sb.AppendLine(Langs.CartGifteeNotSet);
                }
            }
        }

        sb.AppendLineFormat(Langs.CartTotalPrice, cartValue / 100, currencySymbol);

        return sb.Length > 0 ? sb.ToString() : null;
    }

    /// <summary>
    /// 读取购物车 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetCartGames(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseGetCartGames(bot))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 添加商品到购物车
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="query"></param>
    /// <param name="isPrivate"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseAddCartGames(Bot bot, string query, bool isPrivate)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var gameIds = FetchGameIds(query, ESteamGameIdType.Sub | ESteamGameIdType.Bundle, ESteamGameIdType.Sub);

        var sb = new StringBuilder();
        sb.AppendLine(Langs.MultipleLineResult);

        var hasWarn = false;
        var items = new List<SteamGameId>();
        foreach (var gameId in gameIds)
        {
            switch (gameId.Type)
            {
                case ESteamGameIdType.Sub:
                case ESteamGameIdType.Bundle:
                    items.Add(gameId);
                    break;
                default:
                    hasWarn = true;
                    sb.AppendLine(bot.FormatBotResponse(Langs.CartInvalidType, gameId.Input));
                    break;
            }
        }

        if (items.Count > 0)
        {
            if (hasWarn)
            {
                sb.AppendLine();
            }
            var cartResponse = await bot.AddItemToAccountsCart(items, isPrivate, null).ConfigureAwait(false);
            var cartItems = cartResponse?.Cart?.LineItems;
            if (cartItems?.Count > 0 && cartResponse?.Cart?.SubTotal != null)
            {
                var ids = new HashSet<SteamGameId>();
                foreach (var item in cartItems)
                {
                    if (item.PackageId > 0)
                    {
                        ids.Add(new SteamGameId(ESteamGameIdType.Sub, item.PackageId.Value));
                    }
                    else if (item.BundleId > 0)
                    {
                        ids.Add(new SteamGameId(ESteamGameIdType.Bundle, item.BundleId.Value));
                    }
                }

                var getItemResponse = await bot.GetStoreItems(ids).ConfigureAwait(false);
                var gameInfos = getItemResponse?.StoreItems;

                var gameNameDict = new Dictionary<uint, string>();

                if (gameInfos != null)
                {
                    foreach (var info in gameInfos)
                    {
                        gameNameDict.TryAdd(info.Id, info.Name ?? "Unknown");
                    }
                }

                sb.AppendLine(Langs.CurrentCartItems);
                foreach (var item in cartItems)
                {
                    if (string.IsNullOrEmpty(item.LineItemId) || (item.BundleId == 0 && item.PackageId == 0))
                    {
                        sb.AppendLineFormat(Langs.CartItem, item.LineItemId, Langs.CanNotParse);
                        continue;
                    }

                    var price = item.PriceWhenAdded?.FormattedAmount ?? "??";
                    if (!gameNameDict.TryGetValue(item.PackageId ?? item.BundleId ?? 0, out var gameName))
                    {
                        gameName = Langs.KeyNotFound;
                    }

                    if (item.PackageId > 0)
                    {
                        sb.AppendLineFormat(Langs.CartItemSub, item.LineItemId, item.PackageId);
                    }
                    else if (item.BundleId > 0)
                    {
                        sb.AppendLineFormat(Langs.CartItemBundle, item.LineItemId, item.PackageId);
                    }

                    sb.AppendLineFormat(Langs.AppDetailName, gameName);
                    sb.AppendLineFormat(Langs.AppDetailPrice, price);

                    if (item.Flags?.IsPrivate == true)
                    {
                        sb.AppendLine(Langs.CartPrivate);
                    }

                    if (item.Flags?.IsGift == true)
                    {
                        if (item.GiftInfo?.AccountIdGiftee > 0)
                        {
                            sb.AppendLineFormat(Langs.CartGift, item.GiftInfo.AccountIdGiftee);
                            if (item.GiftInfo != null)
                            {
                                sb.AppendLineFormat(Langs.CartGiftGifteeName, item.GiftInfo.GiftMessage?.GifteeName);
                                sb.AppendLineFormat(Langs.CartGiftMessage, item.GiftInfo.GiftMessage?.Message);
                                sb.AppendLineFormat(Langs.CartGiftSignature, item.GiftInfo.GiftMessage?.Signature);
                                sb.AppendLineFormat(Langs.CartGiftSentiment, item.GiftInfo.GiftMessage?.Sentiment);
                            }
                        }
                        else
                        {
                            sb.AppendLine(Langs.CartGifteeNotSet);
                        }
                    }
                }

                sb.AppendLine();
                sb.AppendLineFormat(Langs.CartTotalValue, cartResponse.Cart.SubTotal.FormattedAmount);
                return sb.ToString();
            }
            else
            {
                sb.AppendLine(Langs.NetworkError);
            }
        }
        else
        {
            sb.AppendLine(Langs.CanNotParseAnyGameInfo);
        }

        return sb.ToString();
    }

    /// <summary>
    /// 添加商品到购物车 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="query"></param>
    /// <param name="isPrivate"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseAddCartGames(string botNames, string query, bool isPrivate)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseAddCartGames(bot, query, isPrivate))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 添加商品到购物车(送礼)
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="giftee"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseAddGiftCartGames(Bot bot, string giftee, string query)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        ulong steamId32 = ulong.MaxValue;

        var targetBot = Bot.GetBot(giftee);
        if (targetBot != null)
        {
            steamId32 = SteamId2Steam32(targetBot.SteamID);
        }
        else if (ulong.TryParse(giftee, out var steamId))
        {
            steamId32 = (IsSteam32ID(steamId)) ? steamId : SteamId2Steam32(steamId); ;
        }

        if (steamId32 == ulong.MaxValue)
        {
            return FormatStaticResponse("请使用正确的参数, BotBName 可以为机器人名称或者Steam好友代码", giftee);
        }

        var gameIds = FetchGameIds(query, ESteamGameIdType.Sub | ESteamGameIdType.Bundle, ESteamGameIdType.Sub);

        var sb = new StringBuilder();
        sb.AppendLine(Langs.MultipleLineResult);

        var hasWarn = false;
        var items = new List<SteamGameId>();
        foreach (var gameId in gameIds)
        {
            switch (gameId.Type)
            {
                case ESteamGameIdType.Sub:
                case ESteamGameIdType.Bundle:
                    items.Add(gameId);
                    break;
                default:
                    hasWarn = true;
                    sb.AppendLine(bot.FormatBotResponse(Langs.CartInvalidType, gameId.Input));
                    break;
            }
        }

        var giftInfo = new GiftInfoData
        {
            AccountIdGiftee = steamId32,
            GiftMessage = new GiftMessageData
            {
                GifteeName = steamId32.ToString(),
                Message = "Send by ASFEnhance",
                Sentiment = bot.Nickname,
                Signature = bot.Nickname,
            },
            TimeScheduledSend = 0,
        };

        if (items.Count > 0)
        {
            if (hasWarn)
            {
                sb.AppendLine();
            }
            var cartResponse = await bot.AddItemToAccountsCart(items, false, giftInfo).ConfigureAwait(false);
            var cartItems = cartResponse?.Cart?.LineItems;
            if (cartItems?.Count > 0 && cartResponse?.Cart?.SubTotal != null)
            {

                var ids = new HashSet<SteamGameId>();
                foreach (var item in cartItems)
                {
                    if (item.PackageId > 0)
                    {
                        ids.Add(new SteamGameId(ESteamGameIdType.Sub, item.PackageId.Value));
                    }
                    else if (item.BundleId > 0)
                    {
                        ids.Add(new SteamGameId(ESteamGameIdType.Bundle, item.BundleId.Value));
                    }
                }

                var getItemResponse = await bot.GetStoreItems(ids).ConfigureAwait(false);
                var gameInfos = getItemResponse?.StoreItems;

                var gameNameDict = new Dictionary<uint, string>();

                if (gameInfos != null)
                {
                    foreach (var info in gameInfos)
                    {
                        gameNameDict.TryAdd(info.Id, info.Name ?? "Unknown");
                    }
                }

                sb.AppendLine(Langs.CurrentCartItems);
                foreach (var item in cartItems)
                {
                    if (string.IsNullOrEmpty(item.LineItemId) || (item.BundleId == 0 && item.PackageId == 0))
                    {
                        sb.AppendLineFormat(Langs.CartItem, item.LineItemId, Langs.CanNotParse);
                        continue;
                    }

                    var price = item.PriceWhenAdded?.FormattedAmount ?? "??";
                    if (!gameNameDict.TryGetValue(item.PackageId ?? item.BundleId ?? 0, out var gameName))
                    {
                        gameName = Langs.KeyNotFound;
                    }

                    if (item.PackageId > 0)
                    {
                        sb.AppendLineFormat(Langs.CartItemSub, item.LineItemId, item.PackageId);
                    }
                    else if (item.BundleId > 0)
                    {
                        sb.AppendLineFormat(Langs.CartItemBundle, item.LineItemId, item.PackageId);
                    }

                    sb.AppendLineFormat(Langs.AppDetailName, gameName);
                    sb.AppendLineFormat(Langs.AppDetailPrice, price);

                    if (item.Flags?.IsPrivate == true)
                    {
                        sb.AppendLine(Langs.CartPrivate);
                    }

                    if (item.Flags?.IsGift == true)
                    {
                        if (item.GiftInfo?.AccountIdGiftee > 0)
                        {
                            sb.AppendLineFormat(Langs.CartGift, item.GiftInfo.AccountIdGiftee);
                            if (item.GiftInfo != null)
                            {
                                sb.AppendLineFormat(Langs.CartGiftGifteeName, item.GiftInfo.GiftMessage?.GifteeName);
                                sb.AppendLineFormat(Langs.CartGiftMessage, item.GiftInfo.GiftMessage?.Message);
                                sb.AppendLineFormat(Langs.CartGiftSignature, item.GiftInfo.GiftMessage?.Signature);
                                sb.AppendLineFormat(Langs.CartGiftSentiment, item.GiftInfo.GiftMessage?.Sentiment);
                            }
                        }
                        else
                        {
                            sb.AppendLine(Langs.CartGifteeNotSet);
                        }
                    }
                }

                sb.AppendLine();
                sb.AppendLineFormat(Langs.CartTotalValue, cartResponse.Cart.SubTotal.FormattedAmount);
            }
            else
            {
                sb.AppendLine(Langs.NetworkError);
            }
        }
        else
        {
            sb.AppendLine(Langs.CanNotParseAnyGameInfo);
        }

        return sb.ToString();
    }

    /// <summary>
    /// 添加商品到购物车(送礼) (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="giftee"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseAddGiftCartGames(string botNames, string giftee, string query)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseAddGiftCartGames(bot, giftee, query))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 编辑购物车物品
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="giftee"></param>
    /// <param name="isPrivate"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseEditCartGame(Bot bot, string query, bool isPrivate, string? giftee)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        GiftInfoData? giftInfo = null;


        if (!string.IsNullOrEmpty(giftee))
        {
            ulong steamId32 = ulong.MaxValue;

            var targetBot = Bot.GetBot(giftee);
            if (targetBot != null)
            {
                steamId32 = SteamId2Steam32(targetBot.SteamID);
            }
            else if (ulong.TryParse(giftee, out var steamId))
            {
                steamId32 = (IsSteam32ID(steamId)) ? steamId : SteamId2Steam32(steamId); ;
            }

            if (steamId32 == ulong.MaxValue)
            {
                return FormatStaticResponse("请使用正确的参数, BotBName 可以为机器人名称或者Steam好友代码", giftee);
            }

            giftInfo = new GiftInfoData
            {
                AccountIdGiftee = steamId32,
                GiftMessage = new GiftMessageData
                {
                    GifteeName = steamId32.ToString(),
                    Message = "Send by ASFEnhance",
                    Sentiment = bot.Nickname,
                    Signature = bot.Nickname,
                },
                TimeScheduledSend = 0,
            };
        }

        var lineItemIds = new List<ulong>();
        var entries = query.Split(SeparatorDot, StringSplitOptions.RemoveEmptyEntries);
        foreach (string entry in entries)
        {
            if (ulong.TryParse(entry, out ulong itemId) && itemId > 0)
            {
                lineItemIds.Add(itemId);
            }
        }

        await Utilities.InParallel(lineItemIds.Select(x => bot.ModifyLineItemsOfAccountCart(x, isPrivate, giftInfo))).ConfigureAwait(false);

        return await ResponseGetCartGames(bot).ConfigureAwait(false);
    }

    /// <summary>
    /// 添加商品到购物车 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="giftee"></param>
    /// <param name="isPrivate"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseEditCartGame(string botNames, string query, bool isPrivate, string? giftee)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseEditCartGame(bot, query, isPrivate, giftee))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 清空购物车
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseClearCartGames(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var result = await WebRequest.ClearAccountCart(bot).ConfigureAwait(false);

        if (result == null)
        {
            return bot.FormatBotResponse(Langs.CartEmptyResponse);
        }

        return bot.FormatBotResponse(Langs.CartResetResult, (bool)result ? Langs.Success : Langs.Failure);
    }

    /// <summary>
    /// 清空购物车 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseClearCartGames(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseClearCartGames(bot))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 获取购物车可用区域
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetCartCountries(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        string? result = await WebRequest.CartGetCountries(bot).ConfigureAwait(false);

        return bot.FormatBotResponse(result ?? Langs.NetworkError);
    }

    /// <summary>
    /// 获取购物车可用区域 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetCartCountries(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseGetCartCountries(bot))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 购物车下单
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponsePurchaseSelf(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var response1 = await WebRequest.CheckOut(bot).ConfigureAwait(false);

        if (response1 == null)
        {
            return bot.FormatBotResponse(Langs.PurchaseCartFailureEmpty);
        }

        var response2 = await WebRequest.InitTransaction(bot).ConfigureAwait(false);

        if (response2 == null)
        {
            return bot.FormatBotResponse(Langs.PurchaseCartFailureFinalizeTransactionIsNull);
        }

        string? transId = response2?.TransId ?? response2?.TransActionId;

        if (string.IsNullOrEmpty(transId))
        {
            return bot.FormatBotResponse(Langs.PurchaseCartTransIDIsNull);
        }

        var response3 = await WebRequest.GetFinalPrice(bot, transId).ConfigureAwait(false);

        if (response3 == null || response2?.TransId == null)
        {
            return bot.FormatBotResponse(Langs.PurchaseCartGetFinalPriceIsNull);
        }

        float OldBalance = bot.WalletBalance;

        var response4 = await WebRequest.FinalizeTransaction(bot, transId).ConfigureAwait(false);

        if (response4 == null)
        {
            return bot.FormatBotResponse(Langs.PurchaseCartFailureFinalizeTransactionIsNull);
        }

        await Task.Delay(2000).ConfigureAwait(false);

        float nowBalance = bot.WalletBalance;

        if (nowBalance < OldBalance)
        {
            //成功购买之后自动清空购物车
            await WebRequest.ClearAccountCart(bot).ConfigureAwait(false);

            return bot.FormatBotResponse(Langs.PurchaseDone, response4?.PurchaseReceipt?.FormattedTotal);
        }
        else
        {
            return bot.FormatBotResponse(Langs.PurchaseFailed);
        }
    }

    /// <summary>
    /// 购物车下单 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponsePurchaseSelf(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponsePurchaseSelf(bot))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 购物车卡单
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseFakePurchaseSelf(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var response1 = await WebRequest.CheckOut(bot).ConfigureAwait(false);

        if (response1 == null)
        {
            return bot.FormatBotResponse(Langs.PurchaseCartFailureEmpty);
        }

        var response2 = await WebRequest.InitTransaction(bot).ConfigureAwait(false);

        if (response2 == null)
        {
            return bot.FormatBotResponse(Langs.PurchaseCartFailureFinalizeTransactionIsNull);
        }

        string? transId = response2?.TransId ?? response2?.TransActionId;

        if (string.IsNullOrEmpty(transId))
        {
            return bot.FormatBotResponse(Langs.PurchaseCartTransIDIsNull);
        }

        var response3 = await WebRequest.GetFinalPrice(bot, transId).ConfigureAwait(false);

        if (response3 == null || response2?.TransId == null)
        {
            return bot.FormatBotResponse(Langs.PurchaseCartGetFinalPriceIsNull);
        }

        var response4 = await WebRequest.CancelTransaction(bot, transId).ConfigureAwait(false);

        if (response4 == null)
        {
            return bot.FormatBotResponse(Langs.PurchaseCartFailureFinalizeTransactionIsNull);
        }

        return bot.FormatBotResponse(Langs.FakePurchaseDone);
    }

    /// <summary>
    /// 购物车卡单 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseFakePurchaseSelf(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseFakePurchaseSelf(bot))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 获取礼品卡可选面额
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetDigitalGiftCcardOptions(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var response = await WebRequest.GetDigitalGiftCardOptions(bot).ConfigureAwait(false);

        if (response == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        var sb = new StringBuilder();
        sb.AppendLine(bot.FormatBotResponse(Langs.MultipleLineResult));
        sb.AppendLine("礼品卡名称 | 礼品卡面值");
        foreach (var item in response)
        {
            sb.AppendLineFormat(Langs.AccountSubItem, item.Name, item.Balance);
        }
        return sb.ToString();
    }

    /// <summary>
    /// 获取礼品卡可选面额 (指定BotA)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetDigitalGiftCcardOptions(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseGetDigitalGiftCcardOptions(bot))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 购买礼品卡 (送礼)
    /// </summary>
    /// <param name="botA"></param>
    /// <param name="botBName"></param>
    /// <param name="strBalance"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseSendDigitalGiftCardBot(Bot botA, string botBName, string strBalance)
    {
        if (!botA.IsConnectedAndLoggedOn)
        {
            return botA.FormatBotResponse(Strings.BotNotConnected);
        }

        if (!uint.TryParse(strBalance, out uint balance))
        {
            return botA.FormatBotResponse("balance 必须是整数");
        }

        var targetBot = Bot.GetBot(botBName);

        if (targetBot == null)
        {
            return botA.FormatBotResponse(Strings.BotNotFound, botBName);
        }

        if (!targetBot.IsConnectedAndLoggedOn)
        {
            return botA.FormatBotResponse(Strings.BotNotConnected);
        }

        var _ = await WebRequest.GetDigitalGiftCardOptions(botA).ConfigureAwait(false);

        ulong steamId32 = SteamId2Steam32(targetBot.SteamID);

        var response0 = await WebRequest.SubmitGiftCard(botA, balance).ConfigureAwait(false);

        if (response0 == null)
        {
            return botA.FormatBotResponse(Langs.PurchaseCartFailureEmpty);
        }

        //var response1 = await WebRequest.CheckOut(botA, true).ConfigureAwait(false);

        //if (response1 == null)
        //{
        //    return botA.FormatBotResponse(Langs.PurchaseCartFailureEmpty);
        //}

        var response2 = await WebRequest.InitTransactionDigicalCard(botA, steamId32).ConfigureAwait(false);

        if (response2 == null)
        {
            return botA.FormatBotResponse(Langs.PurchaseCartFailureFinalizeTransactionIsNull);
        }

        string? transId = response2.TransId ?? response2.TransActionId;

        if (response2.Result != EResult.OK || string.IsNullOrEmpty(transId))
        {
            return botA.FormatBotResponse(Langs.PurchaseCartTransIDIsNull);
        }

        var __ = await WebRequest.GetFinalPrice(botA, transId).ConfigureAwait(false);

        var response4 = await WebRequest.GetExternalPaymentUrl(botA, transId).ConfigureAwait(false);

        if (response4 == null)
        {
            return botA.FormatBotResponse(Langs.NetworkError);
        }

        return botA.FormatBotResponse(response4.ToString());
    }

    /// <summary>
    /// 购买礼品卡 (多个Bot)
    /// </summary>
    /// <param name="botAName"></param>
    /// <param name="botBNames"></param>
    /// <param name="strBalance"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseSendDigitalGiftCardBot(string botAName, string botBNames, string strBalance)
    {
        if (string.IsNullOrEmpty(botAName))
        {
            throw new ArgumentNullException(nameof(botAName));
        }

        if (string.IsNullOrEmpty(botBNames))
        {
            throw new ArgumentNullException(nameof(botBNames));
        }

        var botA = Bot.GetBot(botAName);
        if (botA == null)
        {
            return FormatStaticResponse(Strings.BotNotFound, botAName);
        }

        string[] botBNamesList = botBNames.Split(SeparatorDot, StringSplitOptions.RemoveEmptyEntries);

        if (botBNamesList.Length == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botBNames);
        }

        var results = await Utilities.InParallel(botBNamesList.Select(botB => ResponseSendDigitalGiftCardBot(botA, botB, strBalance))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }
}
