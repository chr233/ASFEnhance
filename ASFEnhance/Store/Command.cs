#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using Chrxw.ASFEnhance.Data;
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
        /// <summary>
        /// 读取游戏的商店可用Sub
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="access"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseGetGameSubes(Bot bot, string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException(nameof(query));
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

        /// <summary>
        /// 读取游戏的商店可用Sub (多个Bot)
        /// </summary>
        /// <param name="access"></param>
        /// <param name="botNames"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseGetGameSubes(string botNames, string query)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseGetGameSubes(bot, query))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 发布游戏评测
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="access"></param>
        /// <param name="appID"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        internal static async Task<string?> ResponsePublishReview(Bot bot, string appID, string comment)
        {
            if (string.IsNullOrEmpty(appID))
            {
                throw new ArgumentNullException(nameof(appID));
            }

            if (string.IsNullOrEmpty(comment))
            {
                throw new ArgumentNullException(nameof(comment));
            }

            if (!int.TryParse(appID, out int gameID) || (gameID == 0))
            {
                throw new ArgumentException(null, nameof(appID));
            }

            if (!bot.IsConnectedAndLoggedOn)
            {
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            bool rateUp = gameID > 0;

            RecommendGameResponse? response = await WebRequest.PublishReview(bot, (uint)gameID, comment, rateUp, true, false).ConfigureAwait(false);

            if (response == null || !response.Result)
            {
                return string.Format(CurrentCulture, Langs.RecommendPublishFailed, response?.ErrorMsg);
            }

            return string.Format(CurrentCulture, Langs.RecommendPublishSuccess);
        }

        /// <summary>
        /// 发布游戏评测 (多个Bot)
        /// </summary>
        /// <param name="access"></param>
        /// <param name="botNames"></param>
        /// <param name="appID"></param>
        /// <param name="review"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponsePublishReview(string botNames, string appID, string review)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponsePublishReview(bot, appID, review))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        // TODO
        /// <summary>
        /// 删除游戏评测
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="access"></param>
        /// <param name="targetGameIDs"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseDeleteReview(Bot bot, string targetGameIDs)
        {
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
                    response.AppendLine(FormatBotResponse(bot, string.Format(CurrentCulture, Strings.ErrorIsInvalid, nameof(gameID))));
                    continue;
                }

                bool result = await WebRequest.DeleteRecommend(bot, gameID).ConfigureAwait(false);

                response.AppendLine(FormatBotResponse(bot, string.Format(CurrentCulture, Strings.BotAddLicense, gameID, result ? Langs.Success : Langs.Failure)));
            }

            return response.Length > 0 ? response.ToString() : null;
        }

        /// <summary>
        /// 删除游戏评测 (多个Bot)
        /// </summary>
        /// <param name="access"></param>
        /// <param name="botNames"></param>
        /// <param name="appID"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseDeleteReview(string botNames, string appID)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseDeleteReview(bot, appID))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }
    }
}
