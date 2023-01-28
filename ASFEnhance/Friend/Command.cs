using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Localization;
using SteamKit2;
using System.Text;
using static ASFEnhance.Utils;


namespace ASFEnhance.Friend
{
    internal static class Command
    {
        /// <summary>
        /// 添加Bot好友
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseAddBotFriend(Bot bot, string query)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            HashSet<Bot>? targetBots = Bot.GetBots(query)?.Where(x => x.SteamID != bot.SteamID).ToHashSet();

            if ((targetBots == null) || (targetBots.Count == 0))
            {
                return FormatStaticResponse(string.Format(Strings.BotNotFound, query));
            }

            var results = await Utilities.InParallel(targetBots.Select(targetBot => WebRequest.SendFriendRequest(bot, targetBot.SteamID))).ConfigureAwait(false);

            StringBuilder sb = new();
            sb.AppendLine(bot.FormatBotResponse(Langs.MultipleLineResult));

            foreach (var (steamId, response) in results)
            {
                sb.Append(string.Format(Langs.SendBotFriendRequest, steamId, response?.Result == EResult.OK ? Langs.Success : Langs.Failure));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 添加Bot好友 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseAddBotFriend(string botNames, string query)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseAddBotFriend(bot, query))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseAddFriend(Bot bot, string query)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            StringBuilder sb = new();

            string[] entries = query.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (entries.Length > 1)
            {
                sb.AppendLine(Langs.MultipleLineResult);
            }

            foreach (string entry in entries)
            {
                ulong? steamId;

                if (ulong.TryParse(entry, out ulong value))
                {
                    steamId = await WebRequest.GetSteamIdByProfileLink(bot, $"/profiles/{entry}").ConfigureAwait(false) ??
                        await WebRequest.GetSteamIdByProfileLink(bot, $"/id/{entry}").ConfigureAwait(false);

                    // 好友代码
                    if (steamId == null && value < 0xFFFFFFFF)
                    {
                        value = SteamId2Steam32(value);
                        steamId = await WebRequest.GetSteamIdByProfileLink(bot, $"/profiles/{value}").ConfigureAwait(false);
                    }
                }
                else
                {
                    steamId = await WebRequest.GetSteamIdByProfileLink(bot, $"/id/{entry}").ConfigureAwait(false);
                }

                if (steamId != null)
                {
                    var (_, response) = await WebRequest.SendFriendRequest(bot, steamId.Value).ConfigureAwait(false);
                    sb.AppendLine(string.Format(Langs.CookieItem, entry, response?.Result == EResult.OK ? "发送好友请求成功" : "发送好友请求失败"));
                }
                else
                {
                    sb.AppendLine(string.Format(Langs.CookieItem, entry, "未找到个人资料"));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 添加好友 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseAddFriend(string botNames, string query)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseAddFriend(bot, query))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }
    }
}
