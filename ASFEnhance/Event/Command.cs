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
using System.Threading.Tasks;
using static Chrxw.ASFEnhance.Event.Response;
using static Chrxw.ASFEnhance.Utils;

namespace Chrxw.ASFEnhance.Event
{
    internal static class Command
    {
        internal static string Boolen2String(bool value)
        {
            return value ? "√" : "×";
        }
        // 秋促投票
        internal static async Task<string?> ResponseSteamEvents(Bot bot, ulong steamID, string gameIDs)
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

            string[] entries = gameIDs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            List<uint> intGamsIDs = new();

            foreach (string entry in entries)
            {
                if (uint.TryParse(entry, out uint choice))
                {
                    intGamsIDs.Add(choice);
                    if (intGamsIDs.Count >= 10)
                    {
                        break;
                    }
                }
            }

            if (intGamsIDs.Count < 10) //不足10个游戏自动补齐
            {
                Random rd = new();
                uint[] defaultGames = new uint[] { 1639930, 1506980, 1374480, 585020, 1639230, 1584090, 1111460, 977880, 1366540, 1398740, 1369630, 1195290 };
                while (intGamsIDs.Count < 10)
                {
                    intGamsIDs.Add(defaultGames[rd.Next(defaultGames.Length)]);
                }
            }

            for (int i = 0; i < 10; i++)
            {
                int categoryID = 61 + i;
                await WebRequest.MakeVote(bot, intGamsIDs[i], categoryID).ConfigureAwait(false);
            }

            SummerBadgeResponse? summerBadgeStatus = await WebRequest.CheckSummerBadge(bot).ConfigureAwait(false);

            if (summerBadgeStatus == null)
            {
                return FormatBotResponse(bot, Langs.EventReadBadgeStatusFailed);
            }

            return FormatBotResponse(bot, string.Format(CurrentCulture, Langs.EventVoteResponse, Boolen2String(summerBadgeStatus.VoteOne), Boolen2String(summerBadgeStatus.VoteAll)));
        }

        // 秋促投票(多个Bot)
        internal static async Task<string?> ResponseSteamEvents(ulong steamID, string botNames, string choose)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseSteamEvents(bot, steamID, choose))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        // 检查秋促徽章
        internal static async Task<string?> ResponseCheckSummerBadge(Bot bot, ulong steamID)
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

            SummerBadgeResponse? summerBadgeStatus = await WebRequest.CheckSummerBadge(bot).ConfigureAwait(false);

            if (summerBadgeStatus == null)
            {
                return FormatBotResponse(bot, Langs.EventReadBadgeStatusFailed);
            }

            return FormatBotResponse(bot, string.Format(CurrentCulture, Langs.EventCheckResponse, Boolen2String(summerBadgeStatus.VoteOne), Boolen2String(summerBadgeStatus.VoteAll), Boolen2String(summerBadgeStatus.PlayOne), Boolen2String(summerBadgeStatus.ReviewOne)));
        }

        // 秋促投票(多个Bot)
        internal static async Task<string?> ResponseCheckSummerBadge(ulong steamID, string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseCheckSummerBadge(bot, steamID))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

    }
}
