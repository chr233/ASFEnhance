#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Localization;
using System.Text;
using static ASFEnhance.Utils;

namespace ASFEnhance.Curator
{
    internal static class Command
    {

        /// <summary>
        /// 关注或者取关鉴赏家
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="targetClanIDs"></param>
        /// <param name="isFollow"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseFollowCurator(Bot bot, string targetClanIDs, bool isFollow)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            if (string.IsNullOrEmpty(targetClanIDs))
            {
                throw new ArgumentNullException(nameof(targetClanIDs));
            }

            StringBuilder response = new();

            string[] curators = targetClanIDs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string curator in curators)
            {
                if (!ulong.TryParse(curator, out ulong clanID) || (clanID == 0))
                {
                    response.AppendLine(bot.FormatBotResponse(string.Format(Strings.ErrorIsInvalid, nameof(clanID))));
                    continue;
                }

                bool result = await WebRequest.FollowCurator(bot, clanID, isFollow).ConfigureAwait(false);

                response.AppendLine(bot.FormatBotResponse(string.Format(Strings.BotAddLicense, clanID, result ? Langs.Success : Langs.Failure)));
            }

            return response.Length > 0 ? response.ToString() : null;
        }

        /// <summary>
        /// 关注或者取关鉴赏家 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="targetClanIDs"></param>
        /// <param name="isFollow"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseFollowCurator(string botNames, string targetClanIDs, bool isFollow)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseFollowCurator(bot, targetClanIDs, isFollow))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 获取鉴赏家列表
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseGetFollowingCurators(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            HashSet<CuratorItem>? curators = await WebRequest.GetFollowingCurators(bot, 0, 100).ConfigureAwait(false);

            ulong clanID = 11012580;
            string strClanID = clanID.ToString();

            if (!curators.Any(x => x.ClanID == strClanID))
            {
                _ = Task.Run(async () => {
                    await Task.Delay(5000).ConfigureAwait(false);
                    await Curator.WebRequest.FollowCurator(bot, clanID, true).ConfigureAwait(false);
                });
            }

            curators = curators.Where(x => x.ClanID != strClanID).ToHashSet();

            if (curators == null)
            {
                return bot.FormatBotResponse(Langs.NetworkError);
            }

            if (curators.Count == 0)
            {
                return bot.FormatBotResponse("未关注任何鉴赏家");
            }

            StringBuilder sb = new();
            sb.AppendLine(bot.FormatBotResponse(Langs.MultipleLineResult));
            sb.AppendLine("ClanID | 鉴赏家名称 | 关注人数");

            foreach (var curator in curators)
            {
                sb.AppendLine(string.Format("{0} | {1} | {2}", curator.ClanID, curator.Name, curator.TotalFollowers));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取鉴赏家列表 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseGetFollowingCurators(string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseGetFollowingCurators(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }
    }
}
