#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Localization;
using static ASFEnhance.Utils;

namespace ASFEnhance.Event
{
    internal static class Command
    {
        /// <summary>
        /// 获取夏促活动徽章
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="gruopID"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseEvent(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            var userInfo = await WebRequest.FetUserInfo(bot).ConfigureAwait(false);
            if (userInfo == null)
            {
                return bot.FormatBotResponse(Langs.NetworkError);
            }

            var a1 = await WebRequest.StartTask(bot, userInfo).ConfigureAwait(false);


            for (int index = 0; index < 10; index++)
            {
                var capsuleinsert = await WebRequest.FetCapsuleinsert(bot, index).ConfigureAwait(false);
                if (capsuleinsert == null)
                {
                    continue;
                }
                var a2 = await WebRequest.FinishTask(bot, userInfo, capsuleinsert, index).ConfigureAwait(false);
            }

            return bot.FormatBotResponse("Done!");
        }

        /// <summary>
        /// 获取夏促活动徽章 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="gruopID"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseEvent(string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseEvent(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 获取夏促活动主题
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="gruopID"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseEventTheme(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            var userInfo = await WebRequest.FetUserInfo(bot).ConfigureAwait(false);
            if (userInfo == null)
            {
                return bot.FormatBotResponse(Langs.NetworkError);
            }

            var a3 = await WebRequest.UnlockTheme(bot, userInfo).ConfigureAwait(false);

            return bot.FormatBotResponse("Done!");
        }

        /// <summary>
        /// 获取夏促活动主题 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="gruopID"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseEventTheme(string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseEventTheme(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }
    }
}
