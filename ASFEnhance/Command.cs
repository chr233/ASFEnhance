using System;
using System.Collections.Generic;
using System.Composition;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Plugins.Interfaces;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Interaction;
using ArchiSteamFarm.Steam.Storage;
using SteamKit2;
using static Chrxw.ASFEnhance.Data;

namespace Chrxw.ASFEnhance
{
    [Export(typeof(IPlugin))]
    internal sealed class ASFEnhance : IBotCommand
    {
        public string Name => nameof(ASFEnhance);
        public Version Version => typeof(ASFEnhance).Assembly.GetName().Version ?? throw new ArgumentNullException(nameof(Version));


        public async Task<string?> OnBotCommand(Bot bot, ulong steamID, string message, string[] args)
        {
            switch (args[0].ToUpperInvariant())
            {
                case "K":
                case "KEY":
                    return ResponseGrubKeys(message);

                case "CART" when args.Length > 1:
                case "C" when args.Length > 1:
                    return await ResponseGetCartGames(steamID, args[1]);

                case "CART":
                case "C":
                    return await ResponseGetCartGames(bot, steamID);

                case "PA":
                    return await bot.Commands.Response(steamID, "POINTS ASF").ConfigureAwait(false);

                case "LA":
                    return await bot.Commands.Response(steamID, "LEVEL ASF").ConfigureAwait(false);

                case "BA":
                    return await bot.Commands.Response(steamID, "BALANCE ASF").ConfigureAwait(false);

                case "AL" when args.Length > 1:
                    return await bot.Commands.Response(steamID, "ADDLICENSE " + Utilities.GetArgsAsText(message, 1)).ConfigureAwait(false);

                case "ADDWISHLIST" when args.Length > 2:
                case "AW" when args.Length > 2:
                    return await ResponseAddWishlist(steamID, args[1], Utilities.GetArgsAsText(message, 2)).ConfigureAwait(false);
                case "ADDWISHLIST" when args.Length > 1:
                case "AW" when args.Length > 1:
                    return await ResponseAddWishlist(bot, steamID, args[1]).ConfigureAwait(false);

                case "REMOVEWISHLIST" when args.Length > 2:
                case "RW" when args.Length > 2:
                    return await ResponseRemoveWishlist(steamID, args[1], Utilities.GetArgsAsText(message, 2)).ConfigureAwait(false);
                case "REMOVEWISHLIST" when args.Length > 1:
                case "RW" when args.Length > 1:
                    return await ResponseRemoveWishlist(bot, steamID, args[1]).ConfigureAwait(false);

                default:
                    return null;
            }
        }
        //读取购物车
        private static async Task<string?> ResponseGetCartGames(Bot bot, ulong steamID)
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

            List<CartData>? result = await WebRequest.GetCartGames(bot).ConfigureAwait(false);

            StringBuilder response = new();

            foreach (CartData cartItem in result)
            {
                response.AppendLine(FormatBotResponse(bot, string.Format("{0} {1} {2}", cartItem.gameID, cartItem.gameName, cartItem.gamePrice)));
            }

            return response.Length > 0 ? response.ToString() : null;
        }
        //读取购物车(多个Bot)
        private static async Task<string?> ResponseGetCartGames(ulong steamID, string botNames)
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

        // 添加愿望单
        private static async Task<string?> ResponseAddWishlist(Bot bot, ulong steamID, string targetGameIDs)
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
        private static async Task<string?> ResponseAddWishlist(ulong steamID, string botNames, string targetGameIDs)
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
        private static async Task<string?> ResponseRemoveWishlist(Bot bot, ulong steamID, string targetGameIDs)
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
        private static async Task<string?> ResponseRemoveWishlist(ulong steamID, string botNames, string targetGameIDs)
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


        // 提取KEY
        private static string? ResponseGrubKeys(string message)
        {
            string rs = GrubKeys.String2keysString(message);
            return !string.IsNullOrEmpty(rs) ? rs : "未找到结果";
        }

        public void OnLoaded()
        {
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            ASF.ArchiLogger.LogGenericInfo(string.Format("欢迎使用 ASFEnhance {0}.{1}.{2}", version.Major, version.Minor, version.Build));
            ASF.ArchiLogger.LogGenericInfo("作者 Chr_, 联系方式 chr@chrxw.com");
        }


        internal static string FormatStaticResponse(string response) => Commands.FormatStaticResponse(response);
        internal static string FormatBotResponse(Bot bot, string response) => bot.Commands.FormatBotResponse(response);



    }

}
