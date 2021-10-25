#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Storage;
using Chrxw.ASFEnhance.Localization;
using SteamKit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chrxw.ASFEnhance.Store.Response;
using static Chrxw.ASFEnhance.Utils;

namespace Chrxw.ASFEnhance.Store
{
    internal static class Command
    {
        //读取游戏Sub
        internal static async Task<string?> ResponseGetGameSubes(Bot bot, ulong steamID, string query)
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

            string walletCurrency = bot.WalletCurrency != ECurrencyCode.Invalid ? bot.WalletCurrency.ToString() : string.Format(CurrentCulture, Langs.WalletAreaUnknown);

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
                        response.AppendLine(FormatBotResponse(bot, string.Format(CurrentCulture, Strings.ErrorIsInvalid, entry)));
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
                    response.AppendLine(FormatBotResponse(bot, string.Format(CurrentCulture, Strings.ErrorIsInvalid, entry)));
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
                            response.AppendLine(FormatBotResponse(bot, string.Format(CurrentCulture, Langs.StoreItemHeader, type.ToLowerInvariant(), gameID, storeResponse.gameName)));
                        }
                        else
                        {
                            response.AppendLine(FormatBotResponse(bot, string.Format(CurrentCulture, Langs.StoreItemHeader, type.ToLowerInvariant(), gameID, storeResponse.gameName)));

                            foreach (SubData sub in storeResponse.subData)
                            {
                                response.AppendLine(string.Format(CurrentCulture, Langs.StoreItem, sub.bundle ? "bundle" : "sub", sub.subID, sub.name, sub.price / 100.0, walletCurrency));
                            }
                        }
                        break;
                    default:
                        response.AppendLine(FormatBotResponse(bot, string.Format(CurrentCulture, Langs.GameInvalidType, entry)));
                        break;
                }
            }
            return response.Length > 0 ? response.ToString() : null;
        }
        //读取游戏Sub(多个Bot)
        internal static async Task<string?> ResponseGetGameSubes(ulong steamID, string botNames, string query)
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
                return ASF.IsOwner(steamID) ? FormatStaticResponse(string.Format(CurrentCulture, Strings.BotNotFound, botNames)) : null;
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseGetGameSubes(bot, steamID, query))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }
    }
}
