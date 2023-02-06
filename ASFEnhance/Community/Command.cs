using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;


namespace ASFEnhance.Community
{
    internal static class Command
    {
        /// <summary>
        /// 清除通知小绿信
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseClearNotification(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            await WebRequest.PureCommentNotifications(bot).ConfigureAwait(false);
            await WebRequest.PureInventoryNotifications(bot).ConfigureAwait(false);

            return bot.FormatBotResponse(Langs.Done);
        }

        /// <summary>
        /// 清除通知小绿信 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseClearNotification(string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseClearNotification(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }
    }
}
