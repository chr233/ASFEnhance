using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Account;
using ASFEnhance.Data;
using ASFEnhance.Localization;
using SteamKit2;
using System.Text;
using static ASFEnhance.Utils;


namespace ASFEnhance.Cart
{
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

            var cartResponse = await WebRequest.GetCartGames(bot).ConfigureAwait(false);

            if (cartResponse == null)
            {
                return bot.FormatBotResponse(Langs.NetworkError);
            }

            string walletCurrency = bot.WalletCurrency != ECurrencyCode.Invalid ? bot.WalletCurrency.ToString() : "";

            if (!CurrencyHelper.Currency2Symbol.TryGetValue(walletCurrency, out var currencySymbol))
            {
                currencySymbol = walletCurrency;
            }

            StringBuilder response = new();

            if (cartResponse.CartItems.Count > 0)
            {
                response.AppendLine(bot.FormatBotResponse(Langs.MultipleLineResult));
                response.AppendLine(string.Format(Langs.CartTotalPrice, cartResponse.TotalPrice / 100.0, currencySymbol));

                foreach (var cartItem in cartResponse.CartItems)
                {
                    response.AppendLine(string.Format(Langs.CartItemInfo, cartItem.GameId, cartItem.Name, cartItem.Price / 100.0));
                }

                response.AppendLine(string.Format(Langs.CartPurchaseSelf, cartResponse.PurchaseForSelf ? "√" : "×"));
                response.AppendLine(string.Format(Langs.CartPurchaseGift, cartResponse.PurchaseAsGift ? "√" : "×"));
            }
            else
            {
                response.AppendLine(bot.FormatBotResponse(Langs.CartIsEmpty));
            }

            return response.Length > 0 ? response.ToString() : null;
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

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseGetCartGames(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 添加商品到购物车
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseAddCartGames(Bot bot, string query)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            var gameIds = FetchGameIds(query, SteamGameIdType.Sub | SteamGameIdType.Bundle, SteamGameIdType.Sub);

            StringBuilder response = new();

            foreach (var gameId in gameIds)
            {
                if (response.Length != 0) { response.AppendLine(); }

                switch (gameId.Type)
                {
                    case SteamGameIdType.Sub:
                    case SteamGameIdType.Bundle:
                        bool? success = await WebRequest.AddCart(bot, gameId).ConfigureAwait(false);
                        response.AppendLine(bot.FormatBotResponse(string.Format(Strings.BotAddLicense, gameId.Input, success == null ? Langs.NetworkError : (bool)success ? EResult.OK : EResult.Fail)));
                        break;
                    default:
                        response.AppendLine(bot.FormatBotResponse(string.Format(Langs.CartInvalidType, gameId.Input)));
                        break;
                }
            }

            return response.Length > 0 ? response.ToString() : null;
        }

        /// <summary>
        /// 添加商品到购物车 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseAddCartGames(string botNames, string query)
        {
            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseAddCartGames(bot, query))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

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

            bool? result = await WebRequest.ClearCart(bot).ConfigureAwait(false);

            if (result == null)
            {
                return bot.FormatBotResponse(Langs.CartEmptyResponse);
            }

            return bot.FormatBotResponse(string.Format(Langs.CartResetResult, (bool)result ? Langs.Success : Langs.Failure));
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

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseClearCartGames(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

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

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseGetCartCountries(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        // TODO
        /// <summary>
        /// 购物车改区
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseSetCountry(Bot bot, string countryCode)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            bool result = await WebRequest.CartSetCountry(bot, countryCode).ConfigureAwait(false);

            return bot.FormatBotResponse(string.Format(Langs.SetCurrentCountry, result ? Langs.Success : Langs.Failure));
        }

        // TODO
        /// <summary>
        /// 购物车改区 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseSetCountry(string botNames, string countryCode)
        {
            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseSetCountry(bot, countryCode))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

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

            var response1 = await WebRequest.CheckOut(bot, false).ConfigureAwait(false);

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

            var response3 = await WebRequest.GetFinalPrice(bot, transId, false).ConfigureAwait(false);

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
                await WebRequest.ClearCart(bot).ConfigureAwait(false);

                return bot.FormatBotResponse(string.Format(Langs.PurchaseDone, response4?.PurchaseReceipt?.FormattedTotal));
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

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponsePurchaseSelf(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }


        /// <summary>
        /// 购物车下单 (送礼)
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponsePurchaseGift(Bot bot, string botBName)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            Bot? targetBot = Bot.GetBot(botBName);

            if (targetBot == null)
            {
                return FormatStaticResponse(string.Format(Strings.BotNotFound, botBName));
            }

            ulong steamId32 = SteamId2Steam32(targetBot.SteamID);

            var response1 = await WebRequest.CheckOut(bot, false).ConfigureAwait(false);

            if (response1 == null)
            {
                return bot.FormatBotResponse(Langs.PurchaseCartFailureEmpty);
            }

            var response2 = await WebRequest.InitTransaction(bot, steamId32).ConfigureAwait(false);

            if (response2 == null)
            {
                return bot.FormatBotResponse(Langs.PurchaseCartFailureFinalizeTransactionIsNull);
            }

            string? transId = response2.TransId ?? response2.TransActionId;

            if (string.IsNullOrEmpty(transId))
            {
                return bot.FormatBotResponse(Langs.PurchaseCartTransIDIsNull);
            }

            var response3 = await WebRequest.GetFinalPrice(bot, transId, true).ConfigureAwait(false);

            if (response3 == null || response2.TransId == null)
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
                await WebRequest.ClearCart(bot).ConfigureAwait(false);

                return bot.FormatBotResponse(string.Format(Langs.PurchaseDone, response4?.PurchaseReceipt?.FormattedTotal));
            }
            else
            {
                return bot.FormatBotResponse(Langs.PurchaseFailed);
            }
        }

        /// <summary>
        /// 购物车下单 (送礼) (指定BotA)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponsePurchaseGift(string botAName, string botBName)
        {
            if (string.IsNullOrEmpty(botAName))
            {
                throw new ArgumentNullException(nameof(botAName));
            }

            Bot? botA = Bot.GetBot(botAName);

            if (botA == null)
            {
                return FormatStaticResponse(string.Format(Strings.BotNotFound, botAName));
            }

            return await ResponsePurchaseGift(botA, botBName).ConfigureAwait(false);
        }
    }
}
