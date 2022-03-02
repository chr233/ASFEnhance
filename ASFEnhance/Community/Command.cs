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
using System.Text;
using System.Threading.Tasks;
using static Chrxw.ASFEnhance.Utils;
using static Chrxw.ASFEnhance.Community.Response;
using System.ComponentModel;

namespace Chrxw.ASFEnhance.Community
{
    internal static class Command
    {
        /// <summary>
        /// 加入指定群组
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="access"></param>
        /// <param name="gruopID"></param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        internal static async Task<string?> ResponseJoinGroup(Bot bot, EAccess access, string gruopID)
        {
            if (!Enum.IsDefined(access))
            {
                throw new InvalidEnumArgumentException(nameof(access), (int)access, typeof(EAccess));
            }

            if (access < EAccess.Master)
            {
                return null;
            }

            if (!bot.IsConnectedAndLoggedOn)
            {
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            bool? result = await WebRequest.JoinGroup(bot, gruopID).ConfigureAwait(false);

            if (result == null)
            {
                return FormatBotResponse(bot, string.Format(CurrentCulture, Langs.CartNetworkError));
            }

            return FormatBotResponse(bot, string.Format(CurrentCulture, Langs.JoinGroup, (bool)result ? Langs.Success : Langs.Failure));
        }

        /// <summary>
        /// 加入指定群组 (多个Bot)
        /// </summary>
        /// <param name="access"></param>
        /// <param name="botNames"></param>
        /// <param name="gruopID"></param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseJoinGroup(EAccess access, string botNames, string gruopID)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseJoinGroup(bot, access, gruopID))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        //TODO
        /// <summary>
        /// 获取群组列表
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="access"></param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        internal static async Task<string?> ResponseGroupList(Bot bot, EAccess access)
        {
            if (!Enum.IsDefined(access))
            {
                throw new InvalidEnumArgumentException(nameof(access), (int)access, typeof(EAccess));
            }

            if (access < EAccess.Operator)
            {
                return null;
            }

            if (!bot.IsConnectedAndLoggedOn)
            {
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            List<GroupData> groupList = await WebRequest.GetGroupList(bot).ConfigureAwait(false);

            if (groupList.Count == 0)
            {
                return FormatBotResponse(bot, string.Format(CurrentCulture, Langs.GroupListEmpty));
            }
            else
            {
                StringBuilder result = new(string.Format(CurrentCulture, "No | 群组名称       | 群组ID"));
                for (int i = 0; i < groupList.Count; i++)
                {
                    GroupData group = groupList[i];
                    result.AppendLine(string.Format(CurrentCulture, "{0} | {1,10} | {2,18}", i, group.Name, group.GroupID));
                }
                return FormatBotResponse(bot, result.ToString());
            }
        }

        /// <summary>
        /// 获取群组列表 (多个Bot)
        /// </summary>
        /// <param name="access"></param>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseGroupList(EAccess access, string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseGroupList(bot, access))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }
    }
}
