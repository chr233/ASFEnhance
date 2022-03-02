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
using System.Net;
using System.Text;
using System.Threading.Tasks;

using static Chrxw.ASFEnhance.Utils;

namespace Chrxw.ASFEnhance.DevFeature
{
    internal static class Command
    {
        /// <summary>
        /// 获取商店Cookies
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="access"></param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        internal static string? ResponseGetCookies(Bot bot, EAccess access)
        {
            if (!Enum.IsDefined(access))
            {
                throw new InvalidEnumArgumentException(nameof(access), (int)access, typeof(EAccess));
            }

            if (access < EAccess.Owner)
            {
                return null;
            }

            if (!bot.IsConnectedAndLoggedOn)
            {
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            StringBuilder response = new();

            response.AppendLine(string.Format(CurrentCulture, Langs.ClientCookies));

            CookieCollection cc = bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookies(SteamStoreURL);

            foreach (Cookie c in cc)
            {
                response.AppendLine(string.Format(CurrentCulture, Langs.CookieItem, c.Name, c.Value));
            }

            return FormatBotResponse(bot, response.ToString());
        }
        /// <summary>
        /// 获取商店Cookies (多个bot)
        /// </summary>
        /// <param name="access"></param>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseGetCookies(EAccess access, string botNames)
        {
            if (!Enum.IsDefined(access))
            {
                throw new InvalidEnumArgumentException(nameof(access), (int)access, typeof(EAccess));
            }

            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return access >= EAccess.Owner ? FormatStaticResponse(string.Format(CurrentCulture, Strings.BotNotFound, botNames)) : null;
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => Task.Run(() => ResponseGetCookies(bot, access)))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 获取Bot APIKey
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="access"></param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseGetAPIKey(Bot bot, EAccess access)
        {
            if (!Enum.IsDefined(access))
            {
                throw new InvalidEnumArgumentException(nameof(access), (int)access, typeof(EAccess));
            }

            if (access < EAccess.Owner)
            {
                return null;
            }

            if (!bot.IsConnectedAndLoggedOn)
            {
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            (bool success, string? apiKey) = await bot.ArchiWebHandler.CachedApiKey.GetValue().ConfigureAwait(false);

            return FormatBotResponse(bot, success ? apiKey : string.Format(CurrentCulture, Langs.FetchDataFailed, nameof(apiKey)));
        }

        /// <summary>
        /// 获取Bot APIKey (多个bot)
        /// </summary>
        /// <param name="access"></param>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseGetAPIKey(EAccess access, string botNames)
        {
            if (!Enum.IsDefined(access))
            {
                throw new InvalidEnumArgumentException(nameof(access), (int)access, typeof(EAccess));
            }

            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return access >= EAccess.Owner ? FormatStaticResponse(string.Format(CurrentCulture, Strings.BotNotFound, botNames)) : null;
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => Task.Run(() => ResponseGetAPIKey(bot, access)))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 获取Bot AccessToken
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="access"></param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseGetAccessToken(Bot bot, EAccess access)
        {
            if (!Enum.IsDefined(access))
            {
                throw new InvalidEnumArgumentException(nameof(access), (int)access, typeof(EAccess));
            }

            if (access < EAccess.Owner)
            {
                return null;
            }

            if (!bot.IsConnectedAndLoggedOn)
            {
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            (bool success, string? accessToken) = await bot.ArchiWebHandler.CachedAccessToken.GetValue().ConfigureAwait(false);

            return FormatBotResponse(bot, success ? accessToken : string.Format(CurrentCulture, Langs.FetchDataFailed, nameof(accessToken)));
        }

        /// <summary>
        /// 获取Bot AccessToken (多个bot)
        /// </summary>
        /// <param name="access"></param>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseGetAccessToken(EAccess access, string botNames)
        {
            if (!Enum.IsDefined(access))
            {
                throw new InvalidEnumArgumentException(nameof(access), (int)access, typeof(EAccess));
            }

            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return access >= EAccess.Owner ? FormatStaticResponse(string.Format(CurrentCulture, Strings.BotNotFound, botNames)) : null;
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => Task.Run(() => ResponseGetAccessToken(bot, access)))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }
    }
}
