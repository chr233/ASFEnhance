#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;
using ArchiSteamFarm.Steam.Interaction;
using ArchiSteamFarm.Steam.Storage;
using ASFEnhance.Localization;
using SteamKit2;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Chrxw.ASFEnhance.Response;

namespace Chrxw.ASFEnhance
{
    internal static class Command
    {
        public static async Task<string?> ProcessCommand(Bot bot, ulong steamID, string message, string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    throw new InvalidOperationException(nameof(args.Length));
                case 1: //不带参数
                    switch (args[0].ToUpperInvariant())
                    {
                        case "P":
                            return await bot.Commands.Response(steamID, "POINTS").ConfigureAwait(false);
                        case "PA":
                            return await bot.Commands.Response(steamID, "POINTS ASF").ConfigureAwait(false);
                        case "LA":
                            return await bot.Commands.Response(steamID, "LEVEL ASF").ConfigureAwait(false);
                        case "BA":
                            return await bot.Commands.Response(steamID, "BALANCE ASF").ConfigureAwait(false);
                        case "CA":
                            return await ResponseGetCartGames(steamID, "ASF").ConfigureAwait(false);

                        case "ASFENHANCE":
                        case "ASFE":
                            return ResponseASFEnhanceVersion();

                        case "KEY":
                        case "K":
                            return ResponseExtractKeys(Utilities.GetArgsAsText(message, 1));

                        case "CART":
                        case "C":
                            return await ResponseGetCartGames(bot, steamID).ConfigureAwait(false);

                        case "CARTCOUNTRY":
                        case "CC":
                            return await ResponseGetCartCountries(bot, steamID).ConfigureAwait(false);

                        case "CARTRESET":
                        case "CR":
                            return await ResponseClearCartGames(bot, steamID).ConfigureAwait(false);

                        case "FRIENDCODE":
                        case "FC":
                            return ResponseGetFriendCode(bot, steamID);

                        case "STEAMID":
                        case "SID":
                            return ResponseGetSteamID(bot, steamID);

                        case "PROFILE":
                        case "PF":
                            return await ResponseGetProfileSummary(bot, steamID).ConfigureAwait(false);

                        case "COOKIES":
                            return ResponseGetCookies(bot, steamID);

                        default:
                            return null;
                    }
                default: //带参数
                    switch (args[0].ToUpperInvariant())
                    {
                        case "AL":
                            return await bot.Commands.Response(steamID, "ADDLICENSE " + Utilities.GetArgsAsText(message, 1)).ConfigureAwait(false);
                        case "P":
                            return await bot.Commands.Response(steamID, "POINTS " + Utilities.GetArgsAsText(message, 1)).ConfigureAwait(false);

                        case "K":
                        case "KEY":
                            return ResponseExtractKeys(message);

                        case "ADDWISHLIST" when args.Length > 2:
                        case "AW" when args.Length > 2:
                            return await ResponseAddWishlist(steamID, args[1], Utilities.GetArgsAsText(message, 2)).ConfigureAwait(false);
                        case "ADDWISHLIST":
                        case "AW":
                            return await ResponseAddWishlist(bot, steamID, args[1]).ConfigureAwait(false);

                        case "REMOVEWISHLIST" when args.Length > 2:
                        case "RW" when args.Length > 2:
                            return await ResponseRemoveWishlist(steamID, args[1], Utilities.GetArgsAsText(message, 2)).ConfigureAwait(false);
                        case "REMOVEWISHLIST":
                        case "RW":
                            return await ResponseRemoveWishlist(bot, steamID, args[1]).ConfigureAwait(false);

                        case "CART":
                        case "C":
                            return await ResponseGetCartGames(steamID, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "ADDCART" when args.Length > 2:
                        case "AC" when args.Length > 2:
                            return await ResponseAddCartGames(steamID, args[1], Utilities.GetArgsAsText(args, 2, ",")).ConfigureAwait(false);
                        case "ADDCART":
                        case "AC":
                            return await ResponseAddCartGames(bot, steamID, args[1]).ConfigureAwait(false);

                        case "CC":
                        case "CARTCOUNTRY":
                            return await ResponseGetCartCountries(steamID, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "CARTRESET":
                        case "CR":
                            return await ResponseClearCartGames(steamID, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "SETCOUNTRY" when args.Length > 2:
                        case "SC" when args.Length > 2:
                            return await ResponseSetCountry(steamID, args[1], Utilities.GetArgsAsText(args, 2, ",")).ConfigureAwait(false);
                        case "SETCOUNTRY":
                        case "SC":
                            return await ResponseSetCountry(bot, steamID, args[1]).ConfigureAwait(false);

                        case "SUBS" when args.Length > 2:
                        case "S" when args.Length > 2:
                            return await ResponseGetGameSubes(steamID, args[1], Utilities.GetArgsAsText(args, 2, ",")).ConfigureAwait(false);
                        case "SUBS":
                        case "S":
                            return await ResponseGetGameSubes(bot, steamID, args[1]).ConfigureAwait(false);

                        case "FRIENDCODE":
                        case "FC":
                            return await ResponseGetFriendCode(steamID, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "STEAMID":
                        case "SID":
                            return await ResponseGetSteamID(steamID, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "PROFILE":
                        case "PF":
                            return await ResponseGetProfileSummary(steamID, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "COOKIES":
                            return await ResponseGetCookies(steamID, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        default:
                            return null;
                    }
            }
        }

        // 查看插件版本
        static private string ResponseASFEnhanceVersion()
        {
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            ASF.ArchiLogger.LogGenericInfo(string.Format(Langs.Culture.ToString()));

            ASF.ArchiLogger.LogGenericInfo(string.Format(Langs.test));

            Langs.Culture = new CultureInfo("zh-TW");

            ASF.ArchiLogger.LogGenericInfo(string.Format(Langs.Culture.ToString()));

            ASF.ArchiLogger.LogGenericInfo(string.Format(Langs.test));

            return string.Format(CultureInfo.CurrentCulture, Langs.PluginVer, version.Major, version.Minor, version.Build, version.Revision);
        }
        // 提取KEY
        static private string? ResponseExtractKeys(string message)
        {
            string result = GrubKeys.GrubKeysFromString(message) ?? "未找到结果";
            return result;
        }

        // 添加愿望单
        static async private Task<string?> ResponseAddWishlist(Bot bot, ulong steamID, string targetGameIDs)
        {
            if ((steamID == 0) || !new SteamID(steamID).IsIndividualAccount)
            {
                throw new ArgumentOutOfRangeException(nameof(steamID));
            }

            if (string.IsNullOrEmpty(targetGameIDs))
            {
                throw new ArgumentNullException(nameof(targetGameIDs));
            }

            if (!bot.HasAccess(steamID, BotConfig.EAccess.Master))
            {
                return null;
            }

            if (!bot.IsConnectedAndLoggedOn)
            {
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            StringBuilder response = new();

            string[] games = targetGameIDs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string game in games)
            {
                if (!uint.TryParse(game, out uint gameID) || (gameID == 0))
                {
                    response.AppendLine(FormatBotResponse(bot, string.Format(CultureInfo.CurrentCulture, Strings.ErrorIsInvalid, nameof(gameID))));
                    continue;
                }

                bool result = await WebRequest.AddWishlist(bot, gameID).ConfigureAwait(false);

                response.AppendLine(FormatBotResponse(bot, string.Format(CultureInfo.CurrentCulture, Strings.BotAddLicense, gameID, result ? EResult.OK : EResult.Fail)));
            }

            return response.Length > 0 ? response.ToString() : null;
        }
        // 添加愿望单(多个bot)
        static async private Task<string?> ResponseAddWishlist(ulong steamID, string botNames, string targetGameIDs)
        {
            if ((steamID == 0) || !new SteamID(steamID).IsIndividualAccount)
            {
                throw new ArgumentOutOfRangeException(nameof(steamID));
            }

            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            if (string.IsNullOrEmpty(targetGameIDs))
            {
                throw new ArgumentNullException(nameof(targetGameIDs));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return ASF.IsOwner(steamID) ? FormatStaticResponse(string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botNames)) : null;
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseAddWishlist(bot, steamID, targetGameIDs))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        // 移除愿望单
        static async private Task<string?> ResponseRemoveWishlist(Bot bot, ulong steamID, string targetGameIDs)
        {
            if ((steamID == 0) || !new SteamID(steamID).IsIndividualAccount)
            {
                throw new ArgumentOutOfRangeException(nameof(steamID));
            }

            if (string.IsNullOrEmpty(targetGameIDs))
            {
                throw new ArgumentNullException(nameof(targetGameIDs));
            }

            if (!bot.HasAccess(steamID, BotConfig.EAccess.Master))
            {
                return null;
            }

            if (!bot.IsConnectedAndLoggedOn)
            {
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            StringBuilder response = new();

            string[] games = targetGameIDs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string game in games)
            {
                if (!uint.TryParse(game, out uint gameID) || (gameID == 0))
                {
                    response.AppendLine(FormatBotResponse(bot, string.Format(CultureInfo.CurrentCulture, Strings.ErrorIsInvalid, nameof(gameID))));
                    continue;
                }

                bool result = await WebRequest.RemoveWishlist(bot, gameID).ConfigureAwait(false);

                response.AppendLine(FormatBotResponse(bot, string.Format(CultureInfo.CurrentCulture, Strings.BotAddLicense, gameID, result ? EResult.OK : EResult.Fail)));
            }

            return response.Length > 0 ? response.ToString() : null;
        }
        // 移除愿望单(多个bot)
        static async private Task<string?> ResponseRemoveWishlist(ulong steamID, string botNames, string targetGameIDs)
        {
            if ((steamID == 0) || !new SteamID(steamID).IsIndividualAccount)
            {
                throw new ArgumentOutOfRangeException(nameof(steamID));
            }

            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            if (string.IsNullOrEmpty(targetGameIDs))
            {
                throw new ArgumentNullException(nameof(targetGameIDs));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return ASF.IsOwner(steamID) ? FormatStaticResponse(string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botNames)) : null;
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseRemoveWishlist(bot, steamID, targetGameIDs))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        //读取购物车
        static async private Task<string?> ResponseGetCartGames(Bot bot, ulong steamID)
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
        static async private Task<string?> ResponseGetCartGames(ulong steamID, string botNames)
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
        static async private Task<string?> ResponseAddCartGames(Bot bot, ulong steamID, string query)
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
        static async private Task<string?> ResponseAddCartGames(ulong steamID, string botNames, string query)
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
        static async private Task<string?> ResponseClearCartGames(Bot bot, ulong steamID)
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
        static async private Task<string?> ResponseClearCartGames(ulong steamID, string botNames)
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

        //读取游戏Sub
        static async private Task<string?> ResponseGetGameSubes(Bot bot, ulong steamID, string query)
        {
            if ((steamID == 0) || !new SteamID(steamID).IsIndividualAccount)
            {
                throw new ArgumentOutOfRangeException(nameof(steamID));
            }

            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (!bot.HasAccess(steamID, BotConfig.EAccess.Operator))
            {
                return null;
            }

            if (!bot.IsConnectedAndLoggedOn)
            {
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            string walletCurrency = bot.WalletCurrency != ECurrencyCode.Invalid ? bot.WalletCurrency.ToString() : "钱包区域未知";

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
                        response.AppendLine(FormatBotResponse(bot, string.Format(CultureInfo.CurrentCulture, Strings.ErrorIsInvalid, entry)));
                        continue;
                    }

                    type = entry[..index];
                }
                else if (uint.TryParse(entry, out gameID) && (gameID > 0))
                {
                    type = "APP";
                }
                else
                {
                    response.AppendLine(FormatBotResponse(bot, string.Format(CultureInfo.CurrentCulture, Strings.ErrorIsInvalid, entry)));
                    continue;
                }

                switch (type.ToUpperInvariant())
                {
                    case "A":
                        type = "APP";
                        break;
                    case "S":
                        type = "SUB";
                        break;
                    case "B":
                        type = "BUNDLE";
                        break;
                    default:
                        break;
                }

                switch (type.ToUpperInvariant())
                {
                    case "APP":
                    case "SUB":
                    case "BUNDLE":
                        StoreResponse? storeResponse = await WebRequest.GetStoreSubs(bot, type, gameID).ConfigureAwait(false);

                        if (storeResponse.subData.Count == 0)
                        {
                            response.AppendLine(FormatBotResponse(bot, string.Format("{0}/{1}: {2}", type.ToLowerInvariant(), gameID, storeResponse.gameName)));
                        }
                        else
                        {
                            response.AppendLine(FormatBotResponse(bot, string.Format("{0}/{1}: {2}", type.ToLowerInvariant(), gameID, storeResponse.gameName)));

                            foreach (SubData sub in storeResponse.subData)
                            {
                                response.AppendLine(string.Format("{0}/{1} {2} {3:F2} {4}", sub.bundle ? "bundle" : "sub", sub.subID, sub.name, sub.price / 100.0, walletCurrency));
                            }
                        }
                        break;
                    default:
                        response.AppendLine(FormatBotResponse(bot, string.Format("{0}/{1} 类型无效 [APP|SUB|BUNDLE]", type.ToLowerInvariant(), gameID)));
                        break;
                }
            }
            return response.Length > 0 ? response.ToString() : null;
        }
        //读取游戏Sub(多个Bot)
        static async private Task<string?> ResponseGetGameSubes(ulong steamID, string botNames, string query)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseGetGameSubes(bot, steamID, query))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        // 查看个人资料
        static async private Task<string?> ResponseGetProfileSummary(Bot bot, ulong steamID)
        {
            if ((steamID == 0) || !new SteamID(steamID).IsIndividualAccount)
            {
                throw new ArgumentOutOfRangeException(nameof(steamID));
            }

            if (!bot.HasAccess(steamID, BotConfig.EAccess.FamilySharing))
            {
                return null;
            }

            if (!bot.IsConnectedAndLoggedOn)
            {
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            string result = await WebRequest.GetSteamProfile(bot).ConfigureAwait(false) ?? "读取个人资料失败";

            return FormatBotResponse(bot, result);
        }
        // 查看个人资料(多个Bot)
        static async private Task<string?> ResponseGetProfileSummary(ulong steamID, string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseGetProfileSummary(bot, steamID))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        // 查看STEAMID
        static private string? ResponseGetSteamID(Bot bot, ulong steamID)
        {
            if ((steamID == 0) || !new SteamID(steamID).IsIndividualAccount)
            {
                throw new ArgumentOutOfRangeException(nameof(steamID));
            }

            if (!bot.HasAccess(steamID, BotConfig.EAccess.FamilySharing))
            {
                return null;
            }

            if (!bot.IsConnectedAndLoggedOn)
            {
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            return FormatBotResponse(bot, bot.SteamID.ToString());
        }
        // 查看STEAMID(多个Bot)
        static async private Task<string?> ResponseGetSteamID(ulong steamID, string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => Task.Run(() => ResponseGetSteamID(bot, steamID)))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        // 查看好友代码
        static private string? ResponseGetFriendCode(Bot bot, ulong steamID)
        {
            if ((steamID == 0) || !new SteamID(steamID).IsIndividualAccount)
            {
                throw new ArgumentOutOfRangeException(nameof(steamID));
            }

            if (!bot.HasAccess(steamID, BotConfig.EAccess.FamilySharing))
            {
                return null;
            }

            if (!bot.IsConnectedAndLoggedOn)
            {
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            ulong friendCode = bot.SteamID - 0x110000100000000;

            return FormatBotResponse(bot, friendCode.ToString());
        }
        // 查看好友代码(多个Bot)
        static async private Task<string?> ResponseGetFriendCode(ulong steamID, string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => Task.Run(() => ResponseGetFriendCode(bot, steamID)))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        // 获取购物车可用区域
        static async private Task<string?> ResponseGetCartCountries(Bot bot, ulong steamID)
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
        static async private Task<string?> ResponseGetCartCountries(ulong steamID, string botNames)
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
        static async private Task<string?> ResponseSetCountry(Bot bot, ulong steamID, string countryCode)
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
        static async private Task<string?> ResponseSetCountry(ulong steamID, string botNames, string countryCode)
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


        // 查看客户端Cookies
        static private string? ResponseGetCookies(Bot bot, ulong steamID)
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

            StringBuilder response = new();

            response.AppendLine(FormatBotResponse(bot, "Steam商店的Cookies:"));

            CookieCollection cc = bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookies(SteamStoreURL);

            foreach (Cookie c in cc)
            {
                response.AppendLine($"{c.Name} : {c.Value}");
            }

            return response.ToString();
        }
        // 查看客户端Cookies(多个bot)
        static async private Task<string?> ResponseGetCookies(ulong steamID, string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => Task.Run(() => ResponseGetCookies(bot, steamID)))).ConfigureAwait(false);

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

        internal static Uri SteamStoreURL => ArchiWebHandler.SteamStoreURL;

    }

}
