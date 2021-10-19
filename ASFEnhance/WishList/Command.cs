#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Interaction;
using ArchiSteamFarm.Steam.Storage;
using SteamKit2;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chrxw.ASFEnhance.Wishlist
{
    internal static class Command
    {
        // 添加愿望单
        internal static async Task<string?> ResponseAddWishlist(Bot bot, ulong steamID, string targetGameIDs)
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
        internal static async Task<string?> ResponseAddWishlist(ulong steamID, string botNames, string targetGameIDs)
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
        internal static async Task<string?> ResponseRemoveWishlist(Bot bot, ulong steamID, string targetGameIDs)
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
        internal static async Task<string?> ResponseRemoveWishlist(ulong steamID, string botNames, string targetGameIDs)
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
