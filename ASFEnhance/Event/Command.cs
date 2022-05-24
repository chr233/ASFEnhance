#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Localization;
using System.Text;
using static ASFEnhance.Utils;

namespace ASFEnhance.Event
{
    internal static class Command
    {
        /// <summary>
        /// 活动命令
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="targetGameIDs"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseEvent(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            var token = await WebRequest.FetchSecrets(bot).ConfigureAwait(false);

            if (string.IsNullOrEmpty(token))
            {
                return Langs.CartNetworkError;
            }

            await WebRequest.RacingFestivalBadge(bot, token).ConfigureAwait(false);

            return Langs.EventTaskComplete;
        }

        /// <summary>
        /// 活动命令 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="targetGameIDs"></param>
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
    }
}
