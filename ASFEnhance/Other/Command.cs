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
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Chrxw.ASFEnhance.Utils;

namespace Chrxw.ASFEnhance.Other
{
    internal static class Command
    {
        // 查看插件版本
        internal static string ResponseASFEnhanceVersion()
        {
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            return string.Format(CurrentCulture, Langs.PluginVer, version.Major, version.Minor, version.Build, version.Revision);
        }
        // 提取KEY
        internal static string? ResponseExtractKeys(string message)
        {
            List<string> keys = new();

            MatchCollection matches;
            matches = Regex.Matches(message, @"[\S\d]{5}-[\S\d]{5}-[\S\d]{5}", RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                keys.Add(match.Value.ToUpperInvariant());
            }

            //matches = Regex.Matches(message, @"([A-Za-z0-9]{15})", RegexOptions.IgnoreCase);
            //foreach (Match match in matches)
            //{
            //    GroupCollection groups = match.Groups;
            //    keys.Add(groups[1].Value.ToUpperInvariant().Insert(10, "-").Insert(5, "-"));
            //}

            return keys.Count > 0 ? string.Join('\n', keys) : string.Format(CurrentCulture, Langs.KeyNotFound);
        }
        // 查看客户端Cookies
        internal static string? ResponseGetCookies(Bot bot, ulong steamID)
        {
            if ((steamID == 0) || !new SteamID(steamID).IsIndividualAccount)
            {
                throw new ArgumentOutOfRangeException(nameof(steamID));
            }

            if (!bot.HasAccess(steamID, BotConfig.EAccess.Master))
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
        // 查看客户端Cookies(多个bot)
        internal static async Task<string?> ResponseGetCookies(ulong steamID, string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => Task.Run(() => ResponseGetCookies(bot, steamID)))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }
    }
}
