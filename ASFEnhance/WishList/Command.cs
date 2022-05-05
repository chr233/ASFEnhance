#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Localization;
using System.Text;

using static ASFEnhance.Utils;


namespace ASFEnhance.Wishlist
{
    internal static class Command
    {
        /// <summary>
        /// 添加愿望单
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="targetGameIDs"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseAddWishlist(Bot bot, string targetGameIDs)
        {
            if (string.IsNullOrEmpty(targetGameIDs))
            {
                throw new ArgumentNullException(nameof(targetGameIDs));
            }

            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            StringBuilder response = new();

            string[] games = targetGameIDs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string game in games)
            {
                if (!uint.TryParse(game, out uint gameID) || (gameID == 0))
                {
                    response.AppendLine(bot.FormatBotResponse(string.Format(CurrentCulture, Strings.ErrorIsInvalid, nameof(gameID))));
                    continue;
                }

                bool result = await WebRequest.AddWishlist(bot, gameID).ConfigureAwait(false);

                response.AppendLine(bot.FormatBotResponse(string.Format(CurrentCulture, Strings.BotAddLicense, gameID, result ? Langs.Success : Langs.Failure)));
            }

            return response.Length > 0 ? response.ToString() : null;
        }

        /// <summary>
        /// 添加愿望单 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="targetGameIDs"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseAddWishlist(string botNames, string targetGameIDs)
        {
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
                return FormatStaticResponse(string.Format(CurrentCulture, Strings.BotNotFound, botNames));
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseAddWishlist(bot, targetGameIDs))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 删除愿望单
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="targetGameIDs"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseRemoveWishlist(Bot bot, string targetGameIDs)
        {
            if (string.IsNullOrEmpty(targetGameIDs))
            {
                throw new ArgumentNullException(nameof(targetGameIDs));
            }

            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            StringBuilder response = new();

            string[] games = targetGameIDs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string game in games)
            {
                if (!uint.TryParse(game, out uint gameID) || (gameID == 0))
                {
                    response.AppendLine(bot.FormatBotResponse(string.Format(CurrentCulture, Strings.ErrorIsInvalid, nameof(gameID))));
                    continue;
                }

                bool result = await WebRequest.RemoveWishlist(bot, gameID).ConfigureAwait(false);

                response.AppendLine(bot.FormatBotResponse(string.Format(CurrentCulture, Strings.BotAddLicense, gameID, result ? Langs.Success : Langs.Failure)));
            }

            return response.Length > 0 ? response.ToString() : null;
        }


        /// <summary>
        /// 删除愿望单 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="targetGameIDs"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseRemoveWishlist(string botNames, string targetGameIDs)
        {
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
                return FormatStaticResponse(string.Format(CurrentCulture, Strings.BotNotFound, botNames));
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseRemoveWishlist(bot, targetGameIDs))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }
    }
}
