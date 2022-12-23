using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Storage;
using ASFEnhance.Localization;
using SteamKit2;
using static ASFEnhance.Utils;

namespace ASFEnhance.Event
{
    internal static class Command
    {
        /// <summary>
        /// 获取SIM4贴纸 10.19 - ?
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseSim4(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            string? token = await WebRequest.FetchEventToken(bot, "simscelebrationsale").ConfigureAwait(false);
            if (string.IsNullOrEmpty(token))
            {
                return bot.FormatBotResponse(Langs.NetworkError);
            }

            uint[] door_indexs = { 1, 3, 4, 5, 2 };

            foreach (uint index in door_indexs)
            {
                await WebRequest.DoEventTask(bot, token, index).ConfigureAwait(false);
            }

            return bot.FormatBotResponse("Done!");
        }

        /// <summary>
        /// 获取SIM4贴纸 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseSim4(string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseSim4(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 获取DL2贴纸 11.11 - ?
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseDL2(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            string? token = await WebRequest.FetchEventToken(bot, "dyinglight").ConfigureAwait(false);
            if (string.IsNullOrEmpty(token))
            {
                return bot.FormatBotResponse(Langs.NetworkError);
            }

            uint[] door_indexs = { 1, 3, 4, 5, 2 };

            foreach (uint index in door_indexs)
            {
                await WebRequest.DoEventTask(bot, token, index).ConfigureAwait(false);
            }

            return bot.FormatBotResponse("Done!");
        }

        /// <summary>
        /// 获取DL2贴纸 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseDL2(string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseDL2(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 获取Steam Deck 贴纸 12.1 - ?
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseSteamDeck(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            string? token = await WebRequest.FetchSteamDeckEventToken(bot).ConfigureAwait(false);
            if (string.IsNullOrEmpty(token))
            {
                return bot.FormatBotResponse(Langs.NetworkError);
            }
            await WebRequest.ClaimSteamDeckStick(bot, token).ConfigureAwait(false);

            return bot.FormatBotResponse("Done!");
        }

        /// <summary>
        /// 获取Steam Deck 贴纸 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseSteamDeck(string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseSteamDeck(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 冬促投票
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseSteamAwardVote(Bot bot, string query)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            string[] entries = query.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            List<uint> intGamsIDs = new();


            foreach (string entry in entries)
            {
                if (uint.TryParse(entry, out uint choice))
                {
                    intGamsIDs.Add(choice);
                    if (intGamsIDs.Count >=WebRequest.AllVotes)
                    {
                        break;
                    }
                }
            }

            if (intGamsIDs.Count < WebRequest.AllVotes) //不足10个游戏自动补齐
            {
                Random rd = new();
                uint[,] aviliableChoose = new uint[,] {
                    { 534380   ,1245620  ,1332010  ,1593500  ,1938090 } ,
                    { 1592190  ,1659040  ,1782330  ,1849900  ,1987080 },
                    { 570      ,108600   ,275850   ,548430   ,1091500 },
                    { 648800   ,1144200  ,1446780  ,1818750  ,1938090 },
                    { 698670   ,1063660  ,1313140  ,1817190  ,1954200 },
                    { 261550   ,1167630  ,1332010  ,1533420  ,1637320 },
                    { 493520   ,529340   ,1142710  ,1245620  ,1811260 },
                    { 1061910  ,1237320  ,1462040  ,1687950  ,1761390 },
                    { 1182900  ,1593500  ,1659420  ,1703340  ,1817070 },
                    { 920210   ,1290000  ,1401590  ,1455840  ,1657630 },
                    { 1449850  ,1794680  ,1850570  ,1942280  ,1997040 },
                };
                for (int i = intGamsIDs.Count; i < WebRequest.AllVotes; i++)
                {
                    intGamsIDs.Add(aviliableChoose[i, rd.Next(0, 5)]);
                }
            }

            for (int i = 0; i <WebRequest.AllVotes; i++)
            {
                int categoryID = 72 + i;
                await WebRequest.MakeVote(bot, intGamsIDs[i], categoryID).ConfigureAwait(false);
                await Task.Delay(100).ConfigureAwait(false);
            }

            int voted = await WebRequest.CheckSummerBadge(bot).ConfigureAwait(false);

            if (voted==-1)
            {
                return bot.FormatBotResponse(Langs.NetworkError);
            }

            return bot.FormatBotResponse(string.Format(Langs.EventVoteResponse, voted, WebRequest.AllVotes));
        }

        /// <summary>
        /// 冬促投票(多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseSteamAwardVote(string botNames, string query)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseSteamAwardVote(bot, query))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 检查冬促投票
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseCheckSteamAwardVote(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            int voted = await WebRequest.CheckSummerBadge(bot).ConfigureAwait(false);

            if (voted==-1)
            {
                return bot.FormatBotResponse(Langs.NetworkError);
            }

            return bot.FormatBotResponse(string.Format(Langs.EventVoteResponse, voted, WebRequest.AllVotes));
        }

        /// <summary>
        /// 检查冬促投票(多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseCheckSteamAwardVote(string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseCheckSteamAwardVote(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }
    }
}
