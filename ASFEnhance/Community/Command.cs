#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using Chrxw.ASFEnhance.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chrxw.ASFEnhance.Community.Response;
using static Chrxw.ASFEnhance.Utils;

namespace Chrxw.ASFEnhance.Community
{
    internal static class Command
    {
        /// <summary>
        /// 加入指定群组
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="gruopID"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseJoinGroup(Bot bot, string gruopID)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            (JoinGroupStatus status, string? message) = await WebRequest.JoinGroup(bot, gruopID).ConfigureAwait(false);

            string statusString = status switch {
                JoinGroupStatus.Failed => Langs.Failure,
                JoinGroupStatus.Joined => Langs.Joined,
                JoinGroupStatus.Unjoined => Langs.Unjoined,
                JoinGroupStatus.Applied => Langs.Applied,
                _ => throw new NotImplementedException(),
            };

            switch (status)
            {
                case JoinGroupStatus.Joined:
                case JoinGroupStatus.Unjoined:
                case JoinGroupStatus.Applied:
                    return FormatBotResponse(bot, string.Format(CurrentCulture, Langs.JoinGroup, message, statusString));
                case JoinGroupStatus.Failed:
                default:
                    return FormatBotResponse(bot, string.Format(CurrentCulture, Langs.JoinGroup, statusString, message ?? Langs.CartNetworkError));
            }
        }

        /// <summary>
        /// 加入指定群组 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="gruopID"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseJoinGroup(string botNames, string gruopID)
        {
            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return FormatStaticResponse(string.Format(CurrentCulture, Strings.BotNotFound, botNames));
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseJoinGroup(bot, gruopID))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 退出指定群组
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseLeaveGroup(Bot bot, string groupID)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            if (!ulong.TryParse(groupID, out ulong intGroupID))
            {
                return FormatBotResponse(bot, string.Format(Langs.ArgumentNotInteger, nameof(groupID)));
            }

            bool result = await WebRequest.LeaveGroup(bot, intGroupID).ConfigureAwait(false);

            return FormatBotResponse(bot, string.Format(Langs.LeaveGroup, result ? Langs.Success : Langs.Failure));
        }

        /// <summary>
        /// 退出指定群组 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseLeaveGroup(string botNames, string groupID)
        {
            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return FormatStaticResponse(string.Format(CurrentCulture, Strings.BotNotFound, botNames));
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseLeaveGroup(bot, groupID))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 获取群组列表
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseGroupList(Bot bot)
        {
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
                StringBuilder result = new(string.Format(CurrentCulture, Langs.GroupListTitle));

                result.AppendLine();

                for (int i = 0; i < groupList.Count; i++)
                {
                    GroupData group = groupList[i];
                    result.AppendLine(string.Format(CurrentCulture, Langs.GroupListItem, i + 1, group.Name, group.GroupID));
                }
                return FormatBotResponse(bot, result.ToString());
            }
        }

        /// <summary>
        /// 获取群组列表 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseGroupList(string botNames)
        {
            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return FormatStaticResponse(string.Format(CurrentCulture, Strings.BotNotFound, botNames));
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseGroupList(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }
    }
}
