#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;
using ArchiSteamFarm.Steam.Interaction;
using ArchiSteamFarm.Steam.Storage;
using ArchiSteamFarm.Web.Responses;
using Chrxw.ASFEnhance.Data;
using SteamKit2;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chrxw.ASFEnhance.Cart.Response;

namespace Chrxw.ASFEnhance.Cart
{
    internal static class Command
    {
        //读取购物车
        internal static async Task<string?> ResponseGetCartGames(Bot bot, ulong steamID)
        {
            if ((steamID == 0) || !new SteamID(steamID).IsIndividualAccount)
            {
                throw new ArgumentOutOfRangeException(nameof(steamID));
            }

            if (!bot.HasAccess(steamID, BotConfig.EAccess.Operator))
            {
                return null;
            }

            if (!bot.IsConnectedAndLoggedOn)
            {
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            CartResponse cartResponse = await WebRequest.GetCartGames(bot).ConfigureAwait(false);

            StringBuilder response = new();

            string walletCurrency = bot.WalletCurrency != ECurrencyCode.Invalid ? bot.WalletCurrency.ToString() : "钱包区域未知";

            if (cartResponse.cartData.Count > 0)
            {
                response.AppendLine(FormatBotResponse(bot, string.Format("购物车总额: {0:F2} {1}", cartResponse.totalPrice / 100.0, walletCurrency)));

                foreach (CartData cartItem in cartResponse.cartData)
                {
                    response.AppendLine(string.Format("{0} {1} {2:F2}", cartItem.path, cartItem.name, cartItem.price / 100.0));
                }

                response.AppendLine(FormatBotResponse(bot, string.Format("为自己购买: {0}", cartResponse.purchaseSelf ? "√" : "×")));
                response.AppendLine(FormatBotResponse(bot, string.Format("作为礼物购买: {0}", cartResponse.purchaseGift ? "√" : "×")));
            }
            else
            {
                response.AppendLine(FormatBotResponse(bot, "购物车是空的"));
            }

            return response.Length > 0 ? response.ToString() : null;
        }
        //读取购物车(多个Bot)
        internal static async Task<string?> ResponseGetCartGames(ulong steamID, string botNames)
        {
            if ((steamID == 0) || !new SteamID(steamID).IsIndividualAccount)
            {
                throw new ArgumentOutOfRangeException(nameof(steamID));
            }

            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return ASF.IsOwner(steamID) ? FormatStaticResponse(string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botNames)) : null;
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseGetCartGames(bot, steamID))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        //添加购物车
        internal static async Task<string?> ResponseAddCartGames(Bot bot, ulong steamID, string query)
        {
            if ((steamID == 0) || !new SteamID(steamID).IsIndividualAccount)
            {
                throw new ArgumentOutOfRangeException(nameof(steamID));
            }

            if (!bot.HasAccess(steamID, BotConfig.EAccess.Operator))
            {
                return null;
            }

            if (!bot.IsConnectedAndLoggedOn)
            {
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            string[] entries = query.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            StringBuilder response = new();

            foreach (string entry in entries)
            {
                uint gameID;
                string type;

                int index = entry.IndexOf('/', StringComparison.Ordinal);

                if ((index > 0) && (entry.Length > index + 1))
                {
                    if (!uint.TryParse(entry[(index + 1)..], out gameID) || (gameID == 0))
                    {
                        response.AppendLine(FormatBotResponse(bot, string.Format(CultureInfo.CurrentCulture, Strings.ErrorIsInvalid, nameof(gameID))));
                        continue;
                    }

                    type = entry[..index];
                }
                else if (uint.TryParse(entry, out gameID) && (gameID > 0))
                {
                    type = "SUB";
                }
                else
                {
                    response.AppendLine(FormatBotResponse(bot, string.Format(CultureInfo.CurrentCulture, Strings.ErrorIsInvalid, nameof(gameID))));
                    continue;
                }

                bool? result;

                switch (type.ToUpperInvariant())
                {
                    case "S":
                    case "SUB":
                        result = await WebRequest.AddCert(bot, gameID, false).ConfigureAwait(false);
                        break;
                    case "B":
                    case "BUNDLE":
                        result = await WebRequest.AddCert(bot, gameID, true).ConfigureAwait(false);
                        break;
                    default:
                        response.AppendLine(FormatBotResponse(bot, string.Format("{0}: 类型无效,只能为 SUB 或 BUNDLE", entry)));
                        continue;
                }

                if (result != null)
                {
                    response.AppendLine(FormatBotResponse(bot, string.Format(CultureInfo.CurrentCulture, Strings.BotAddLicense, entry, (bool)result ? EResult.OK : EResult.Fail)));
                }
                else
                {
                    response.AppendLine(FormatBotResponse(bot, string.Format(CultureInfo.CurrentCulture, Strings.BotAddLicense, entry, "网络错误")));
                }
            }
            return response.Length > 0 ? response.ToString() : null;
        }
        //添加购物车(多个Bot)
       internal static async Task<string?> ResponseAddCartGames(ulong steamID, string botNames, string query)
        {
            if ((steamID == 0) || !new SteamID(steamID).IsIndividualAccount)
            {
                throw new ArgumentOutOfRangeException(nameof(steamID));
            }

            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return ASF.IsOwner(steamID) ? FormatStaticResponse(string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botNames)) : null;
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseAddCartGames(bot, steamID, query))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        //清空购物车
        internal static async Task<string?> ResponseClearCartGames(Bot bot, ulong steamID)
        {
            if ((steamID == 0) || !new SteamID(steamID).IsIndividualAccount)
            {
                throw new ArgumentOutOfRangeException(nameof(steamID));
            }

            if (!bot.HasAccess(steamID, BotConfig.EAccess.Operator))
            {
                return null;
            }

            if (!bot.IsConnectedAndLoggedOn)
            {
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            bool? result = await WebRequest.ClearCert(bot).ConfigureAwait(false);

            if (result == null)
            {
                return FormatBotResponse(bot, "响应为空");
            }

            return FormatBotResponse(bot, (bool)result ? "清空购物车成功" : "清空购物车失败");
        }
        //清空购物车(多个Bot)
       internal static async Task<string?> ResponseClearCartGames(ulong steamID, string botNames)
        {
            if ((steamID == 0) || !new SteamID(steamID).IsIndividualAccount)
            {
                throw new ArgumentOutOfRangeException(nameof(steamID));
            }

            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return ASF.IsOwner(steamID) ? FormatStaticResponse(string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botNames)) : null;
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseClearCartGames(bot, steamID))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }
        // 获取购物车可用区域
        internal static async Task<string?> ResponseGetCartCountries(Bot bot, ulong steamID)
        {
            if ((steamID == 0) || !new SteamID(steamID).IsIndividualAccount)
            {
                throw new ArgumentOutOfRangeException(nameof(steamID));
            }

            if (!bot.HasAccess(steamID, BotConfig.EAccess.Operator))
            {
                return null;
            }

            if (!bot.IsConnectedAndLoggedOn)
            {
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            List<CartCountryData> result = await WebRequest.CartGetCountries(bot).ConfigureAwait(false);

            if (result.Count == 0)
            {
                return FormatBotResponse(bot, "无可用区域选项,结算货币为钱包货币");
            }

            StringBuilder response = new();

            response.AppendLine("代码 | 区域名称 | 当前区域");

            foreach (CartCountryData cc in result)
            {
                if (cc.current)
                {
                    response.AppendLine($" {cc.code} | {cc.name} | *");
                }
                else
                {
                    response.AppendLine($" {cc.code} | {cc.name}");
                }
            }

            return FormatBotResponse(bot, response.ToString());
        }
        // 获取购物车可用区域(多个Bot)
        internal static async Task<string?> ResponseGetCartCountries(ulong steamID, string botNames)
        {
            if ((steamID == 0) || !new SteamID(steamID).IsIndividualAccount)
            {
                throw new ArgumentOutOfRangeException(nameof(steamID));
            }

            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return ASF.IsOwner(steamID) ? FormatStaticResponse(string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botNames)) : null;
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseGetCartCountries(bot, steamID))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }
        // 购物车改区
        internal static async Task<string?> ResponseSetCountry(Bot bot, ulong steamID, string countryCode)
        {
            if ((steamID == 0) || !new SteamID(steamID).IsIndividualAccount)
            {
                throw new ArgumentOutOfRangeException(nameof(steamID));
            }

            if (!bot.HasAccess(steamID, BotConfig.EAccess.Operator))
            {
                return null;
            }

            if (!bot.IsConnectedAndLoggedOn)
            {
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            bool result = await WebRequest.CartSetCountry(bot, countryCode).ConfigureAwait(false);

            return FormatBotResponse(bot, result ? "结算货币设置完成" : "结算货币设置失败");
        }
        // 购物车改区(多个Bot)
        internal static async Task<string?> ResponseSetCountry(ulong steamID, string botNames, string countryCode)
        {
            if ((steamID == 0) || !new SteamID(steamID).IsIndividualAccount)
            {
                throw new ArgumentOutOfRangeException(nameof(steamID));
            }

            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return ASF.IsOwner(steamID) ? FormatStaticResponse(string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botNames)) : null;
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseSetCountry(bot, steamID, countryCode))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        // 下单
        internal static async Task<string?> ResponsePurchase(Bot bot, ulong steamID)
        {
            if ((steamID == 0) || !new SteamID(steamID).IsIndividualAccount)
            {
                throw new ArgumentOutOfRangeException(nameof(steamID));
            }

            if (!bot.HasAccess(steamID, BotConfig.EAccess.Master))
            {
                return null;
            }

            if (!bot.IsConnectedAndLoggedOn)
            {
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            //Random random = new();

            //string snr = string.Format("{0}_{1}_{2}__{3}", 1, random.Next(1, 10), random.Next(1, 10), random.Next(100, 999));

            HtmlDocumentResponse? response1 = await WebRequest.CheckOut(bot, false).ConfigureAwait(false);

            if (response1 == null)
            {
                return "购物车是空的,无需结账";
            }

            ObjectResponse<PurchaseResponse?> response2 = await WebRequest.InitTransaction(bot).ConfigureAwait(false);

            if (response2 == null)
            {
                return "购买失败, FinalizeTransaction 返回值为空";
            }

            string transID = response2.Content.TransID ?? response2.Content.TransActionID ?? "";

            if (string.IsNullOrEmpty(transID))
            {
                return "购买失败, transID 为Null";
            }

            ObjectResponse<FinalPriceResponse?> response3 = await WebRequest.GetFinalPrice(bot, transID, false).ConfigureAwait(false);

            //if (response3 == null || response2.Content.TransID == null)
            //{
            //    return "购买失败, GetFinalPrice 返回值为空";
            //}

            ObjectResponse<TransactionStatusResponse?> response4 = await WebRequest.FinalizeTransaction(bot, transID).ConfigureAwait(false);

            if (response4 == null)
            {
                return "购买失败, FinalizeTransaction 返回值为空";
            }

            return FormatBotResponse(bot, string.Format("购买完成, 消费金额: {0}", response4.Content.PurchaseReceipt.FormattedTotal));
        }
        // 下单(多个Bot)
        internal static async Task<string?> ResponsePurchase(ulong steamID, string botNames)
        {
            if ((steamID == 0) || !new SteamID(steamID).IsIndividualAccount)
            {
                throw new ArgumentOutOfRangeException(nameof(steamID));
            }

            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return ASF.IsOwner(steamID) ? FormatStaticResponse(string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botNames)) : null;
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponsePurchase(bot, steamID))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }



        internal static string FormatStaticResponse(string response)
        {
            return Commands.FormatStaticResponse(response);
        }

        internal static string FormatBotResponse(Bot bot, string response)
        {
            return bot.Commands.FormatBotResponse(response);
        }
    }
}
