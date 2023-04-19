using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using System.Text;


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

            StringBuilder sb = new();
            foreach (var targetBot in targetBots)
            {
                bot.SteamFriends.AddFriend(targetBot.SteamID);
                sb.Append(string.Format(Langs.SendBotFriendRequest, targetBot.BotName, Langs.Success));
                await Task.Delay(200).ConfigureAwait(false);
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
                    bot.SteamFriends.AddFriend(steamId);
                    sb.AppendLine(string.Format(Langs.CookieItem, entry, Langs.SendFriendRequestSuccess));
                }
                else
                {
                    sb.AppendLine(string.Format(Langs.CookieItem, entry, Langs.ProfileNotFound));
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

        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseDeleteFriend(Bot bot, string query)
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
                    bot.SteamFriends.RemoveFriend(steamId);
                    sb.AppendLine(string.Format(Langs.CookieItem, entry, Langs.SendFriendRequestSuccess));
                    await Task.Delay(200).ConfigureAwait(false);
                }
                else
                {
                    sb.AppendLine(string.Format(Langs.CookieItem, entry, Langs.ProfileNotFound));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 删除好友 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseDeleteFriend(string botNames, string query)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseDeleteFriend(bot, query))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 删除所有好友
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseDeleteAllFriend(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            int friendCount = bot.SteamFriends.GetFriendCount();
            if (friendCount > 0)
            {
                for (int i = 0; i < friendCount; i++)
                {
                    var steamId = bot.SteamFriends.GetFriendByIndex(i);
                    bot.SteamFriends.RemoveFriend(steamId);
                    await Task.Delay(500).ConfigureAwait(false);
                }
                return bot.FormatBotResponse(string.Format(Langs.DeleteFriendSuccess, friendCount));
            }
            else
            {
                return bot.FormatBotResponse(Langs.NoFriend);
            }
        }

        /// <summary>
        /// 删除所有好友 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseDeleteAllFriend(string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseDeleteAllFriend(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }
    }
}
