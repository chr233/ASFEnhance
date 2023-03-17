using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;

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
        /// 领取冬促贴纸
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseClaimSticker(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            string? token = await WebRequest.FetchToken(bot).ConfigureAwait(false);

            if (string.IsNullOrEmpty(token))
            {
                return bot.FormatBotResponse(Langs.NetworkError);
            }

            ASFLogger.LogGenericInfo(token);

            await WebRequest.ClaimDailySticker(bot, token).ConfigureAwait(false);

            return bot.FormatBotResponse(Langs.Done);
        }

        /// <summary>
        /// 领取冬促贴纸 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseClaimSticker(string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseClaimSticker(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }
    }
}
