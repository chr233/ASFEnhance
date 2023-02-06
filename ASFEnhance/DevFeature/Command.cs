using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using System.Net;
using System.Text;

namespace ASFEnhance.DevFeature
{
    internal static class Command
    {
        /// <summary>
        /// 获取商店Cookies
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static string? ResponseGetCookies(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            StringBuilder response = new();

            response.AppendLine(Langs.MultipleLineResult);
            response.AppendLine(Langs.ClientCookies);

            CookieCollection cc = bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookies(SteamStoreURL);

            foreach (Cookie c in cc)
            {
                response.AppendLine(string.Format(Langs.CookieItem, c.Name, c.Value));
            }

            return bot.FormatBotResponse(response.ToString());
        }

        /// <summary>
        /// 获取商店Cookies (多个bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseGetCookies(string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => Task.Run(() => ResponseGetCookies(bot)))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 获取Bot APIKey
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseGetAPIKey(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            (_, string? apiKey) = await bot.ArchiWebHandler.CachedApiKey.GetValue().ConfigureAwait(false);

            bool success = !string.IsNullOrEmpty(apiKey);

            return bot.FormatBotResponse(success ? apiKey! : string.Format(Langs.FetchDataFailed, nameof(apiKey)));
        }

        /// <summary>
        /// 获取Bot APIKey (多个bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseGetAPIKey(string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => Task.Run(() => ResponseGetAPIKey(bot)))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 获取Bot AccessToken
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseGetAccessToken(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            (_, string? accessToken) = await bot.ArchiWebHandler.CachedAccessToken.GetValue().ConfigureAwait(false);

            bool success = !string.IsNullOrEmpty(accessToken);

            return bot.FormatBotResponse(success ? accessToken! : string.Format(Langs.FetchDataFailed, nameof(accessToken)));
        }

        /// <summary>
        /// 获取Bot AccessToken (多个bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseGetAccessToken(string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => Task.Run(() => ResponseGetAccessToken(bot)))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }
    }
}
