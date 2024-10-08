using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Data.Common;
using ASFEnhance.Data.Plugin;
using SteamKit2;
using System.Text;

namespace ASFEnhance.Cart;

static class Command
{
    /// <summary>
    ///     读取购物车
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

        if (cartResponse?.Cart == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        return await cartResponse.Cart.ToSummary(bot).ConfigureAwait(false);
    }

    /// <summary>
    ///     读取购物车 (多个Bot)
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

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseGetCartGames(bot))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    ///     添加商品到购物车
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

            var cartResponse =
                await WebRequest.AddItemsToAccountsCart(bot, items, isPrivate, null).ConfigureAwait(false);

            if (cartResponse?.Cart == null)
            {
                sb.AppendLine(Langs.NetworkError);
            }
            else
            {
                var result = await cartResponse.Cart.ToSummary(bot).ConfigureAwait(false);
                sb.AppendLine(result);
            }
        }
        else
        {
            sb.AppendLine(Langs.CanNotParseAnyGameInfo);
        }

        return sb.ToString();
    }

    /// <summary>
    ///     添加商品到购物车 (多个Bot)
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

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseAddCartGames(bot, query, isPrivate)))
            .ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    ///     添加商品到购物车(送礼)
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="query"></param>
    /// <param name="giftee"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseAddGiftCartGames(Bot bot, string query, string giftee)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var steamId32 = ulong.MaxValue;

        var targetBot = Bot.GetBot(giftee);
        if (targetBot != null)
        {
            steamId32 = SteamId2Steam32(targetBot.SteamID);
        }
        else if (ulong.TryParse(giftee, out var steamId))
        {
            steamId32 = IsSteam32ID(steamId) ? steamId : SteamId2Steam32(steamId);
        }

        if (steamId32 == ulong.MaxValue)
        {
            return FormatStaticResponse("ADDCARTGIFT [Bots] <SubIds|BundleIds> SteamId|FriendCode|BotName", giftee);
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
                Message = Config.CustomGifteeMessage ?? "Send by ASFEnhance",
                Sentiment = bot.Nickname,
                Signature = bot.Nickname
            },
            TimeScheduledSend = 0
        };

        if (items.Count > 0)
        {
            if (hasWarn)
            {
                sb.AppendLine();
            }

            var cartResponse =
                await WebRequest.AddItemsToAccountsCart(bot, items, false, giftInfo).ConfigureAwait(false);

            if (cartResponse?.Cart == null)
            {
                sb.AppendLine(Langs.NetworkError);
            }
            else
            {
                var result = await cartResponse.Cart.ToSummary(bot).ConfigureAwait(false);
                sb.AppendLine(result);
            }
        }
        else
        {
            sb.AppendLine(Langs.CanNotParseAnyGameInfo);
        }

        return sb.ToString();
    }

    /// <summary>
    ///     添加商品到购物车(送礼) (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="query"></param>
    /// <param name="giftee"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseAddGiftCartGames(string botNames, string query, string giftee)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseAddGiftCartGames(bot, query, giftee)))
            .ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    ///     编辑购物车物品
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
            var steamId32 = ulong.MaxValue;

            var targetBot = Bot.GetBot(giftee);
            if (targetBot != null)
            {
                if (!targetBot.IsConnectedAndLoggedOn)
                {
                    return bot.FormatBotResponse(Strings.BotNotConnected, targetBot.BotName);
                }

                steamId32 = SteamId2Steam32(targetBot.SteamID);
            }
            else if (ulong.TryParse(giftee, out var steamId))
            {
                steamId32 = IsSteam32ID(steamId) ? steamId : SteamId2Steam32(steamId);
                ;
            }

            if (steamId32 == ulong.MaxValue)
            {
                return FormatStaticResponse("EDITCARTGIFT [Bots] <lineItemIds> SteamId|FriendCode|BotName", giftee);
            }

            giftInfo = new GiftInfoData
            {
                AccountIdGiftee = steamId32,
                GiftMessage = new GiftMessageData
                {
                    GifteeName = steamId32.ToString(),
                    Message = Config.CustomGifteeMessage ?? "Send by ASFEnhance",
                    Sentiment = bot.Nickname,
                    Signature = bot.Nickname
                },
                TimeScheduledSend = 0
            };
        }

        var lineItemIds = new List<ulong>();
        var entries = query.Split(SeparatorDot, StringSplitOptions.RemoveEmptyEntries);
        foreach (var entry in entries)
        {
            if (ulong.TryParse(entry, out var itemId) && itemId > 0)
            {
                lineItemIds.Add(itemId);
            }
        }

        await Utilities
            .InParallel(lineItemIds.Select(x => WebRequest.ModifyLineItemsOfAccountCart(bot, x, isPrivate, giftInfo)))
            .ConfigureAwait(false);

        return await ResponseGetCartGames(bot).ConfigureAwait(false);
    }

    /// <summary>
    ///     修改商品到购物车 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="giftee"></param>
    /// <param name="isPrivate"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseEditCartGame(string botNames, string query, bool isPrivate,
        string? giftee)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities
            .InParallel(bots.Select(bot => ResponseEditCartGame(bot, query, isPrivate, giftee))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    ///     移除购物车物品
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseRemoveCartGame(Bot bot, string query)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var lineItemIds = new List<ulong>();
        var entries = query.Split(SeparatorDot, StringSplitOptions.RemoveEmptyEntries);
        foreach (var entry in entries)
        {
            if (ulong.TryParse(entry, out var itemId) && itemId > 0)
            {
                lineItemIds.Add(itemId);
            }
        }

        await Utilities.InParallel(lineItemIds.Select(x => WebRequest.RemoveItemFromAccountCart(bot, x)))
            .ConfigureAwait(false);

        return await ResponseGetCartGames(bot).ConfigureAwait(false);
    }

    /// <summary>
    ///     移除购物车物品 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseRemoveCartGame(string botNames, string query)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseRemoveCartGame(bot, query)))
            .ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    ///     清空购物车
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
    ///     清空购物车 (多个Bot)
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

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseClearCartGames(bot))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    ///     获取购物车可用区域
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetCartCountries(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var result = await WebRequest.CartGetCountries(bot).ConfigureAwait(false);

        return bot.FormatBotResponse(result ?? Langs.NetworkError);
    }

    /// <summary>
    ///     获取购物车可用区域 (多个Bot)
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

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseGetCartCountries(bot)))
            .ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    ///     购物车下单
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

        AddressConfig? address = null;
        if (Config.Addresses?.Count > 0)
        {
            address = Config.Addresses[Random.Shared.Next(0, Config.Addresses.Count)];
        }

        var response2 = await WebRequest.InitTransaction(bot, address).ConfigureAwait(false);

        if (response2 == null)
        {
            return bot.FormatBotResponse(Langs.PurchaseCartFailureFinalizeTransactionIsNull);
        }

        var transId = response2?.TransId ?? response2?.TransActionId;

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

        await Task.Delay(4000).ConfigureAwait(false);

        float nowBalance = bot.WalletBalance;

        if (nowBalance < OldBalance)
        {
            //成功购买之后自动清空购物车
            await WebRequest.ClearAccountCart(bot).ConfigureAwait(false);

            return bot.FormatBotResponse(Langs.PurchaseDone, response4?.PurchaseReceipt?.FormattedTotal);
        }

        return bot.FormatBotResponse(Langs.PurchaseFailed);
    }

    /// <summary>
    ///     购物车下单 (多个Bot)
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

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponsePurchaseSelf(bot))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    ///     购物车卡单
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

        var transId = response2?.TransId ?? response2?.TransActionId;

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
    ///     购物车卡单 (多个Bot)
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

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseFakePurchaseSelf(bot)))
            .ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    ///     购买钱包余额
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseAddFunds(Bot bot, string amount)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        if (!long.TryParse(amount, out var funds) || funds < 0)
        {
            return bot.FormatBotResponse(Langs.PurchaseFailedArgumentError);
        }

        var response1 = await WebRequest.SubmitAddFunds(bot, funds * 100).ConfigureAwait(false);
        if (response1 == null)
        {
            return bot.FormatBotResponse(Langs.PurchaseAddFundsError);
        }

        var match = RegexUtils.MatchCartIdFromUri().Match(response1.FinalUri.Query);
        if (!match.Success)
        {
            return bot.FormatBotResponse(Langs.PurchaseAddFundsErrorGetCartError);
        }

        var cartId = match.Groups[1].Value;
        var response2 = await WebRequest.InitTransactionAddFunds(bot, cartId).ConfigureAwait(false);
        if (response2 == null)
        {
            return bot.FormatBotResponse(Langs.PurchaseCartFailureFinalizeTransactionIsNull);
        }

        var transId = response2?.TransId;
        if (string.IsNullOrEmpty(transId))
        {
            return bot.FormatBotResponse(Langs.PurchaseCartTransIDIsNull);
        }

        var response3 = await WebRequest.GetFinalPrice(bot, transId).ConfigureAwait(false);
        if (response3 == null)
        {
            return bot.FormatBotResponse(Langs.PurchaseCartGetFinalPriceIsNull);
        }

        var response4 = await WebRequest.TransactionStatusAddFunds(bot, cartId, transId).ConfigureAwait(false);
        if (response4?.Result != EResult.Pending)
        {
            return bot.FormatBotResponse(Langs.PurchaseAddFundsErrorTransStatusError);
        }

        var response5 = await WebRequest.CheckoutExternalLinkAddFunds(bot, cartId, transId).ConfigureAwait(false);
        if (response5 == null)
        {
            return bot.FormatBotResponse(Langs.PurchaseAddFundsErrorParseExternalPaymentError);
        }

        var finalUrl = await WebRequest.GetRealExternalPaymentLink(bot, response5).ConfigureAwait(false);
        if (finalUrl == null)
        {
            return bot.FormatBotResponse(Langs.PurchaseAddFundsErrorGetPaymentUrlError);
        }

        return bot.FormatBotResponse(Langs.PurchaseAddFundsSuccess, response3.FormattedTotal, finalUrl);
    }

    /// <summary>
    ///     购买钱包余额 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseAddFunds(string botNames, string amount)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseAddFunds(bot, amount)))
            .ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }
}