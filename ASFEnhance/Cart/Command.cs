#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Web.Responses;
using Chrxw.ASFEnhance.Data;
using Chrxw.ASFEnhance.Localization;
using SteamKit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chrxw.ASFEnhance.Cart.Response;
using static Chrxw.ASFEnhance.Utils;


namespace Chrxw.ASFEnhance.Cart
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
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            CartResponse cartResponse = await WebRequest.GetCartGames(bot).ConfigureAwait(false);

            StringBuilder response = new();

            string walletCurrency = bot.WalletCurrency != ECurrencyCode.Invalid ? bot.WalletCurrency.ToString() : string.Format(CurrentCulture, Langs.WalletAreaUnknown);

            if (cartResponse.cartData.Count > 0)
            {
                response.AppendLine(FormatBotResponse(bot, string.Format(CurrentCulture, Langs.CartTotalPrice, cartResponse.totalPrice / 100.0, walletCurrency)));

                foreach (CartData cartItem in cartResponse.cartData)
                {
                    response.AppendLine(string.Format(CurrentCulture, Langs.CartItemInfo, cartItem.path, cartItem.name, cartItem.price / 100.0));
                }

                response.AppendLine(FormatBotResponse(bot, string.Format(CurrentCulture, Langs.CartPurchaseSelf, cartResponse.purchaseSelf ? "√" : "×")));
                response.AppendLine(FormatBotResponse(bot, string.Format(CurrentCulture, Langs.CartPurchaseGift, cartResponse.purchaseGift ? "√" : "×")));
            }
            else
            {
                response.AppendLine(FormatBotResponse(bot, string.Format(CurrentCulture, Langs.CartIsEmpty)));
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
                return FormatStaticResponse(string.Format(CurrentCulture, Strings.BotNotFound, botNames));
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
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            Dictionary<string, SteamGameID> gameIDs = FetchGameIDs(query, SteamGameIDType.App);

            StringBuilder response = new();

            foreach (KeyValuePair<string, SteamGameID> item in gameIDs)
            {
                if (response.Length != 0) { response.AppendLine(); }

                string input = item.Key;
                SteamGameID gameID = item.Value;

                switch (gameID.Type)
                {
                    case SteamGameIDType.Sub:
                    case SteamGameIDType.Bundle:
                        bool? success = await WebRequest.AddCert(bot, gameID).ConfigureAwait(false);
                        response.AppendLine(FormatBotResponse(bot, string.Format(CurrentCulture, Strings.BotAddLicense, input, success == null ? Langs.CartNetworkError : (bool)success ? EResult.OK : EResult.Fail)));
                        break;
                    default:
                        response.AppendLine(FormatBotResponse(bot, string.Format(CurrentCulture, Langs.CartInvalidType, input)));
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
                return FormatStaticResponse(string.Format(CurrentCulture, Strings.BotNotFound, botNames));
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
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            bool? result = await WebRequest.ClearCert(bot).ConfigureAwait(false);

            if (result == null)
            {
                return FormatBotResponse(bot, string.Format(CurrentCulture, Langs.CartEmptyResponse));
            }

            return FormatBotResponse(bot, string.Format(CurrentCulture, Langs.CartResetResult, (bool)result ? Langs.Success : Langs.Failure));
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
                return FormatStaticResponse(string.Format(CurrentCulture, Strings.BotNotFound, botNames));
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
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            string result = await WebRequest.CartGetCountries(bot).ConfigureAwait(false);

            return FormatBotResponse(bot, result);
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
                return FormatStaticResponse(string.Format(CurrentCulture, Strings.BotNotFound, botNames));
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
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            bool result = await WebRequest.CartSetCountry(bot, countryCode).ConfigureAwait(false);

            return FormatBotResponse(bot, string.Format(CurrentCulture, Langs.SetCurrentCountry, result ? Langs.Success : Langs.Failure));
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
                return FormatStaticResponse(string.Format(CurrentCulture, Strings.BotNotFound, botNames));
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
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            HtmlDocumentResponse? response1 = await WebRequest.CheckOut(bot, false).ConfigureAwait(false);

            if (response1 == null)
            {
                return string.Format(CurrentCulture, Langs.PurchaseCartFailureEmpty);
            }

            ObjectResponse<PurchaseResponse?> response2 = await WebRequest.InitTransaction(bot).ConfigureAwait(false);

            if (response2 == null)
            {
                return string.Format(CurrentCulture, Langs.PurchaseCartFailureFinalizeTransactionIsNull);
            }

            string? transID = response2.Content.TransID ?? response2.Content.TransActionID;

            if (string.IsNullOrEmpty(transID))
            {
                return string.Format(CurrentCulture, Langs.PurchaseCartTransIDIsNull);
            }

            ObjectResponse<FinalPriceResponse?> response3 = await WebRequest.GetFinalPrice(bot, transID, false).ConfigureAwait(false);

            if (response3 == null || response2.Content.TransID == null)
            {
                return string.Format(CurrentCulture, Langs.PurchaseCartGetFinalPriceIsNull);
            }

            float OldBalance = bot.WalletBalance;

            ObjectResponse<TransactionStatusResponse?> response4 = await WebRequest.FinalizeTransaction(bot, transID).ConfigureAwait(false);

            if (response4 == null)
            {
                return string.Format(CurrentCulture, Langs.PurchaseCartFailureFinalizeTransactionIsNull);
            }

            await Task.Delay(2000).ConfigureAwait(false);

            float nowBalance = bot.WalletBalance;

            if (nowBalance < OldBalance)
            {
                //成功购买之后自动清空购物车
                await WebRequest.ClearCert(bot).ConfigureAwait(false);

                return FormatBotResponse(bot, string.Format(CurrentCulture, Langs.PurchaseDone, response4.Content.PurchaseReceipt.FormattedTotal));
            }
            else
            {
                return FormatBotResponse(bot, string.Format(CurrentCulture, Langs.PurchaseFailed));
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
                return FormatStaticResponse(string.Format(CurrentCulture, Strings.BotNotFound, botNames));
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
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            Bot? targetBot = Bot.GetBot(botBName);

            if (targetBot == null)
            {
                return FormatStaticResponse(string.Format(CurrentCulture, Strings.BotNotFound, botBName));
            }

            ulong steamID32 = SteamID2Steam32(targetBot.SteamID);

            HtmlDocumentResponse? response1 = await WebRequest.CheckOut(bot, false).ConfigureAwait(false);

            if (response1 == null)
            {
                return string.Format(CurrentCulture, Langs.PurchaseCartFailureEmpty);
            }

            ObjectResponse<PurchaseResponse?> response2 = await WebRequest.InitTransaction(bot, steamID32).ConfigureAwait(false);

            if (response2 == null)
            {
                return string.Format(CurrentCulture, Langs.PurchaseCartFailureFinalizeTransactionIsNull);
            }

            string? transID = response2.Content.TransID ?? response2.Content.TransActionID;

            if (string.IsNullOrEmpty(transID))
            {
                return string.Format(CurrentCulture, Langs.PurchaseCartTransIDIsNull);
            }

            ObjectResponse<FinalPriceResponse?> response3 = await WebRequest.GetFinalPrice(bot, transID, true).ConfigureAwait(false);

            if (response3 == null || response2.Content.TransID == null)
            {
                return string.Format(CurrentCulture, Langs.PurchaseCartGetFinalPriceIsNull);
            }

            float OldBalance = bot.WalletBalance;

            ObjectResponse<TransactionStatusResponse?> response4 = await WebRequest.FinalizeTransaction(bot, transID).ConfigureAwait(false);

            if (response4 == null)
            {
                return string.Format(CurrentCulture, Langs.PurchaseCartFailureFinalizeTransactionIsNull);
            }

            await Task.Delay(2000).ConfigureAwait(false);

            float nowBalance = bot.WalletBalance;

            if (nowBalance < OldBalance)
            {
                //成功购买之后自动清空购物车
                await WebRequest.ClearCert(bot).ConfigureAwait(false);

                return FormatBotResponse(bot, string.Format(CurrentCulture, Langs.PurchaseDone, response4.Content.PurchaseReceipt.FormattedTotal));
            }
            else
            {
                return FormatBotResponse(bot, string.Format(CurrentCulture, Langs.PurchaseFailed));
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
                return FormatStaticResponse(string.Format(CurrentCulture, Strings.BotNotFound, botAName));
            }

            return await ResponsePurchaseGift(botA, botBName).ConfigureAwait(false);
        }
    }
}
