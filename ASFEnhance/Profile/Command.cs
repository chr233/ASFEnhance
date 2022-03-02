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

using static Chrxw.ASFEnhance.Utils;


namespace Chrxw.ASFEnhance.Profile
{
    internal static class Command
    {
        /// <summary>
        /// 获取个人资料摘要
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="access"></param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        internal static async Task<string?> ResponseGetProfileSummary(Bot bot, EAccess access)
        {
            if (!Enum.IsDefined(access))
            {
                throw new InvalidEnumArgumentException(nameof(access), (int)access, typeof(EAccess));
            }

            if (access < EAccess.FamilySharing)
            {
                return null;
            }

            if (!bot.IsConnectedAndLoggedOn)
            {
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            string result = await WebRequest.GetSteamProfile(bot).ConfigureAwait(false) ?? string.Format(CurrentCulture, Langs.GetProfileFailed);

            return FormatBotResponse(bot, result);
        }

        /// <summary>
        /// 获取个人资料摘要 (多个Bot)
        /// </summary>
        /// <param name="access"></param>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseGetProfileSummary(EAccess access, string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseGetProfileSummary(bot, access))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 获取Steam64ID
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="access"></param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        internal static string? ResponseGetSteamID(Bot bot, EAccess access)
        {
            if (!Enum.IsDefined(access))
            {
                throw new InvalidEnumArgumentException(nameof(access), (int)access, typeof(EAccess));
            }

            if (access < EAccess.FamilySharing)
            {
                return null;
            }

            if (!bot.IsConnectedAndLoggedOn)
            {
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            return FormatBotResponse(bot, bot.SteamID.ToString());
        }

        /// <summary>
        /// 获取Steam64ID (多个Bot)
        /// </summary>
        /// <param name="access"></param>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseGetSteamID(EAccess access, string botNames)
        {
            if (!Enum.IsDefined(access))
            {
                throw new InvalidEnumArgumentException(nameof(access), (int)access, typeof(EAccess));
            }

            if (access < EAccess.FamilySharing)
            {
                return null;
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return access >= EAccess.Owner ? FormatStaticResponse(string.Format(CurrentCulture, Strings.BotNotFound, botNames)) : null;
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => Task.Run(() => ResponseGetSteamID(bot, access)))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 获取好友代码
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="access"></param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        internal static string? ResponseGetFriendCode(Bot bot, EAccess access)
        {
            if (!Enum.IsDefined(access))
            {
                throw new InvalidEnumArgumentException(nameof(access), (int)access, typeof(EAccess));
            }

            if (access < EAccess.FamilySharing)
            {
                return null;
            }

            if (!bot.IsConnectedAndLoggedOn)
            {
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            ulong friendCode = bot.SteamID - 0x110000100000000;

            return FormatBotResponse(bot, friendCode.ToString());
        }

        /// <summary>
        /// 获取好友代码 (多个Bot)
        /// </summary>
        /// <param name="access"></param>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseGetFriendCode(EAccess access, string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => Task.Run(() => ResponseGetFriendCode(bot, access)))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }
    }
}
