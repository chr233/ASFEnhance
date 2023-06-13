using AngleSharp.Common;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using SteamKit2;
using System.Text;


namespace ASFEnhance.Friend;

internal static class Command
{
    /// <summary>
    /// 添加Bot好友
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="botsToAdd"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseAddBotFriend(Bot bot, IEnumerable<Bot> botsToAdd)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        StringBuilder sb = new();
        foreach (Bot botToAdd in botsToAdd)
        {
            var steamId = botToAdd.SteamID;
            if (steamId < 0x110000000000000)
            {
                sb.AppendLine(bot.FormatBotResponse(string.Format(Langs.CookieItem, botToAdd.BotName, Strings.TargetBotNotConnected)));
                continue;
            }

            var relation = bot.SteamFriends.GetClanRelationship(steamId);
            if (relation == EClanRelationship.Member)
            {
                sb.AppendLine(bot.FormatBotResponse(string.Format(Langs.CookieItem, botToAdd.BotName, "已经是好友了")));
            }
            else
            {
                bot.SteamFriends.AddFriend(steamId);
                sb.AppendLine(bot.FormatBotResponse(string.Format(Langs.CookieItem, botToAdd.BotName, Langs.Success)));
                await Task.Delay(200).ConfigureAwait(false);
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// 添加Bot好友 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseAddBotFriend(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        if (Bot.BotsReadOnly == null)
        {
            return FormatStaticResponse(Strings.ErrorNoBotsDefined);
        }

        var botListA = new HashSet<Bot>();
        var botListB = new HashSet<Bot>();

        var sb = new StringBuilder();
        sb.AppendLine(FormatStaticResponse(Langs.MultipleLineResult));

        if (botNames.Contains('+'))
        {
            var splits = botNames.Split('+', 2, StringSplitOptions.RemoveEmptyEntries);

            botNames = splits[0];

            foreach (var query in splits[1].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (query.ToUpperInvariant() == "ASF")
                {
                    foreach (var b in Bot.BotsReadOnly)
                    {
                        botListB.Add(b.Value);
                    }
                    continue;
                }

                var bot = Bot.GetBot(query);
                if (bot == null)
                {
                    sb.AppendLine(string.Format(Langs.CookieItem, query, "机器人未找到"));
                }
                else
                {
                    botListB.Add(bot);
                }
            }
        }

        foreach (var query in botNames.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            if (query.ToUpperInvariant() == "ASF")
            {
                foreach (var b in Bot.BotsReadOnly)
                {
                    botListA.Add(b.Value);
                }
                continue;
            }

            var bot = Bot.GetBot(query);
            if (bot == null)
            {
                sb.AppendLine(string.Format(Langs.CookieItem, query, "机器人未找到"));
            }
            else
            {
                botListA.Add(bot);
            }
        }

        var botDict = new Dictionary<Bot, HashSet<Bot>>();

        foreach (var bot in botListA)
        {
            var tmp = new HashSet<Bot>();
            foreach (var b in botListA)
            {
                if (b != bot)
                {
                    tmp.Add(b);
                }
            }

            foreach (var b in botListB)
            {
                if (b != bot)
                {
                    tmp.Add(b);
                }
            }

            if (tmp.Any())
            {
                botDict[bot] = tmp;
            }
        }

        if (!botDict.Any())
        {
            sb.AppendLine(FormatStaticResponse("未提供足够的参数, 无法添加好友"));
        }
        else
        {
            var results = await Utilities.InParallel(botDict.Select(kv => ResponseAddBotFriend(kv.Key, kv.Value))).ConfigureAwait(false);

            foreach (var result in results)
            {
                sb.AppendLine();
                sb.AppendLine(result);
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// 添加好友
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="query"></param>
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
                sb.AppendLine(bot.FormatBotResponse(string.Format(Langs.CookieItem, entry, Langs.SendFriendRequestSuccess)));
            }
            else
            {
                sb.AppendLine(bot.FormatBotResponse(string.Format(Langs.CookieItem, entry, Langs.ProfileNotFound)));
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// 添加好友 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="query"></param>
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
    /// <param name="query"></param>
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
    /// <param name="query"></param>
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

    /// <summary>
    /// 获取好友邀请链接
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetInviteLink(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var response = await WebRequest.GetAddFriendPage(bot).ConfigureAwait(false);
        if (response == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        var prefix = response.Prefix;
        var token = response.Token;

        return bot.FormatBotResponse(prefix + '/' + token);
    }

    /// <summary>
    /// 获取好友邀请链接 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetInviteLink(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        HashSet<Bot>? bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
        }

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseGetInviteLink(bot))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }
}
