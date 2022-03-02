#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Storage;
using Chrxw.ASFEnhance.Localization;
using SteamKit2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using static Chrxw.ASFEnhance.Event.Response;
using static Chrxw.ASFEnhance.Utils;

namespace Chrxw.ASFEnhance.Event
{
    internal static class Command
    {
        // 冬促投票
        internal static async Task<string?> ResponseSteamAwardVote(Bot bot, ulong steamID, string gameIDs)
        {
            if ((steamID == 0) || !new SteamID(steamID).IsIndividualAccount)
            {
                throw new InvalidEnumArgumentException(nameof(steamID));
            }

            // if (!bot.HasAccess(steamID, BotConfig.EAccess.Operator))
            // {
            //     return null;
            // }

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
                uint[,] aviliableChoose = new uint[,] {
                    { 892970   ,1063730  ,1091500  ,1196590  ,1551360 } ,
                    { 752480   ,1358140  ,1402320  ,1499120  ,1576350 },
                    { 570      ,105600   ,252490   ,275850   ,1172470 },
                    { 892970   ,924970   ,1240440  ,1426210  ,1782210 },
                    { 607080   ,848450   ,860510   ,1178830  ,1551360 },
                    { 1092790  ,1097200  ,1195290  ,1252330  ,1282730 },
                    { 699130   ,1203220  ,1325200  ,1466860  ,1517290 },
                    { 1088850  ,1113560  ,1382330  ,1384160  ,1490890 },
                    { 936790   ,1091500  ,1196590  ,1259420  ,1328670 },
                    { 1135690  ,1210320  ,1248130  ,1291340  ,1455840 }
                };
                for (int i = intGamsIDs.Count; i < 10; i++)
                {
                    intGamsIDs.Add(aviliableChoose[i, rd.Next(0, 5)]);
                }
            }

            for (int i = 0; i < 10; i++)
            {
                int categoryID = 61 + i;
                await WebRequest.MakeVote(bot, intGamsIDs[i], categoryID).ConfigureAwait(false);
            }

            uint? voteCount = await WebRequest.CheckSummerBadge(bot).ConfigureAwait(false);

            if (voteCount == null)
            {
                return FormatBotResponse(bot, Langs.EventReadBadgeStatusFailed);
            }

            return FormatBotResponse(bot, string.Format(CurrentCulture, Langs.EventVoteResponse, voteCount, 10));
        }

        // 冬促投票(多个Bot)
        internal static async Task<string?> ResponseSteamAwardVote(ulong steamID, string botNames, string choose)
        {
            if ((steamID == 0) || !new SteamID(steamID).IsIndividualAccount)
            {
                throw new InvalidEnumArgumentException(nameof(steamID));
            }

            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                // return access >= EAccess.Owner ? FormatStaticResponse(string.Format(CurrentCulture, Strings.BotNotFound, botNames)) : null;
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseSteamAwardVote(bot, steamID, choose))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        // 检查冬促徽章
        internal static async Task<string?> ResponseCheckSteamAwardVote(Bot bot, ulong steamID)
        {
            if ((steamID == 0) || !new SteamID(steamID).IsIndividualAccount)
            {
                throw new InvalidEnumArgumentException(nameof(steamID));
            }

            // if (!bot.HasAccess(steamID, BotConfig.EAccess.Operator))
            // {
            //     return null;
            // }

            if (!bot.IsConnectedAndLoggedOn)
            {
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            uint? voteCount = await WebRequest.CheckSummerBadge(bot).ConfigureAwait(false);

            if (voteCount == null)
            {
                return FormatBotResponse(bot, Langs.EventReadBadgeStatusFailed);
            }

            return FormatBotResponse(bot, string.Format(CurrentCulture, Langs.EventVoteResponse, voteCount, 10));
        }

        // 冬促投票(多个Bot)
        internal static async Task<string?> ResponseCheckSteamAwardVote(ulong steamID, string botNames)
        {
            if ((steamID == 0) || !new SteamID(steamID).IsIndividualAccount)
            {
                throw new InvalidEnumArgumentException(nameof(steamID));
            }

            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                // return access >= EAccess.Owner ? FormatStaticResponse(string.Format(CurrentCulture, Strings.BotNotFound, botNames)) : null;
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseCheckSteamAwardVote(bot, steamID))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }
        // 领取每日贴纸
        internal static async Task<string?> ResponseClaimDailySticker(Bot bot, ulong steamID)
        {
            if ((steamID == 0) || !new SteamID(steamID).IsIndividualAccount)
            {
                throw new InvalidEnumArgumentException(nameof(steamID));
            }

            // if (!bot.HasAccess(steamID, BotConfig.EAccess.Operator))
            // {
            //     return null;
            // }

            if (!bot.IsConnectedAndLoggedOn)
            {
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            bool? result = await WebRequest.ClaimDailySticker(bot).ConfigureAwait(false);

            return FormatBotResponse(bot, string.Format(CurrentCulture, Langs.EventClaimItem, result));
        }

        // 领取每日贴纸(多个Bot)
        internal static async Task<string?> ResponseClaimDailySticker(ulong steamID, string botNames)
        {
            if ((steamID == 0) || !new SteamID(steamID).IsIndividualAccount)
            {
                throw new InvalidEnumArgumentException(nameof(steamID));
            }

            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                // return access >= EAccess.Owner ? FormatStaticResponse(string.Format(CurrentCulture, Strings.BotNotFound, botNames)) : null;
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseClaimDailySticker(bot, steamID))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }
    }
}
