#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Data;
using System.Text;

namespace ASFEnhance.Curator
{
    internal static class Command
    {
        /// <summary>
        /// 关注或者取关鉴赏家
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="targetClanIds"></param>
        /// <param name="isFollow"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseFollowCurator(Bot bot, string targetClanIds, bool isFollow)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            if (string.IsNullOrEmpty(targetClanIds))
            {
                throw new ArgumentNullException(nameof(targetClanIds));
            }

            StringBuilder response = new();

            string[] curators = targetClanIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string curator in curators)
            {
                if (!ulong.TryParse(curator, out ulong clanId) || (clanId == 0))
                {
                    response.AppendLine(bot.FormatBotResponse(string.Format(Strings.ErrorIsInvalid, nameof(clanId))));
                    continue;
                }

                bool result = await WebRequest.FollowCurator(bot, clanId, isFollow).ConfigureAwait(false);

                response.AppendLine(bot.FormatBotResponse(string.Format(Strings.BotAddLicense, clanId, result ? Langs.Success : Langs.Failure)));
            }

            return response.Length > 0 ? response.ToString() : null;
        }

        /// <summary>
        /// 关注或者取关鉴赏家 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="targetClanIds"></param>
        /// <param name="isFollow"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseFollowCurator(string botNames, string targetClanIds, bool isFollow)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseFollowCurator(bot, targetClanIds, isFollow))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        private const int ASFenhanceCuratorClanId = 39487086;

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

            if (curators == null)
            {
                return bot.FormatBotResponse(Langs.NetworkError);
            }

            string strClanId = ASFenhanceCuratorClanId.ToString();

            if (!curators.Any(x => x.ClanId == strClanId))
            {
                _ = Task.Run(async () => {
                    await Task.Delay(5000).ConfigureAwait(false);
                    await WebRequest.FollowCurator(bot, ASFenhanceCuratorClanId, true).ConfigureAwait(false);
                });
            }

            if (curators.Count == 0)
            {
                return bot.FormatBotResponse(Langs.NotFollowAnyCurator);
            }

            StringBuilder sb = new();
            sb.AppendLine(bot.FormatBotResponse(Langs.MultipleLineResult));
            sb.AppendLine(Langs.CuratorListTitle);

            foreach (var curator in curators)
            {
                if (curator.ClanId == strClanId)
                {
                    curator.Name = Langs.ASFEnhanceCurator;
                }
                sb.AppendLine(string.Format(Langs.GroupListItem, curator.ClanId, curator.Name, curator.TotalFollowers));
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

        /// <summary>
        /// 取关所有鉴赏家
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseUnFollowAllCurators(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            var curators = await WebRequest.GetFollowingCurators(bot, 0, 100).ConfigureAwait(false);

            if (curators == null)
            {
                return bot.FormatBotResponse(Langs.NetworkError);
            }

            string strClanId = ASFenhanceCuratorClanId.ToString();

            if (!curators.Any(x => x.ClanId == strClanId))
            {
                _ = Task.Run(async () => {
                    await Task.Delay(5000).ConfigureAwait(false);
                    await WebRequest.FollowCurator(bot, ASFenhanceCuratorClanId, true).ConfigureAwait(false);
                });
            }

            if (curators.Count == 0)
            {
                return bot.FormatBotResponse(Langs.NotFollowAnyCurator);
            }

            SemaphoreSlim semaphore = new(3);

            var tasks = curators.Where(x => x.ClanId != strClanId).Select(async curator => {
                await semaphore.WaitAsync().ConfigureAwait(false);
                try
                {
                    return await WebRequest.FollowCurator(bot, ulong.Parse(curator.ClanId), false).ConfigureAwait(false);
                }
                finally
                {
                    semaphore.Release();
                }
            });

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);

            int success = results.Count(x => x);
            int total = results.Length;

            return bot.FormatBotResponse(string.Format(Langs.UnFollowAllCuratorResult, success, total));
        }

        /// <summary>
        /// 取关所有鉴赏家 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseUnFollowAllCurators(string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseUnFollowAllCurators(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }
    }
}
