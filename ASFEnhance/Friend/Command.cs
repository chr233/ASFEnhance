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
    /// <param name="query"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseAddBotFriend(Bot bot, string query)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var targetBots = Bot.GetBots(query)?.Where(x => x.SteamID != bot.SteamID).ToHashSet();

        if (targetBots == null || targetBots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, query);
        }

        var sb = new StringBuilder();
        foreach (var targetBot in targetBots)
        {
            var relation = bot.SteamFriends.GetFriendRelationship(targetBot.SteamID);
            if (relation == EFriendRelationship.Friend || relation == EFriendRelationship.RequestInitiator)
            {
                sb.AppendLine(bot.FormatBotResponse(Langs.SendBotFriendRequest, targetBot.BotName, "已经是好友了/已发送邀请"));
            }
            else
            {
                bot.SteamFriends.AddFriend(targetBot.SteamID);
                sb.AppendLine(bot.FormatBotResponse(Langs.SendBotFriendRequest, targetBot.BotName, Langs.Success));
                await Task.Delay(200).ConfigureAwait(false);
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// 添加Bot好友 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseAddBotFriend(string botNames, string query)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseAddBotFriend(bot, query)))
            .ConfigureAwait(false);

        var responses = results.Where(result => !string.IsNullOrEmpty(result));

        return string.Join(Environment.NewLine, responses);
    }

    /// <summary>
    /// 批量添加Bot好友
    /// </summary>
    /// <param name="bots"></param>
    /// <param name="targetBots"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseAddBotFriendMuli(IEnumerable<Bot> bots, IEnumerable<Bot> targetBots)
    {
        if (!targetBots.Any())
        {
            return FormatStaticResponse(Strings.BotNotFound);
        }

        var sb = new StringBuilder();
        foreach (var bot in bots)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                sb.AppendLine(bot.FormatBotResponse(Strings.BotNotConnected));
                continue;
            }

            foreach (var targetBot in targetBots)
            {
                var relation = bot.SteamFriends.GetFriendRelationship(targetBot.SteamID);
                if (relation == EFriendRelationship.Friend || relation == EFriendRelationship.RequestInitiator)
                {
                    sb.AppendLine(bot.FormatBotResponse(Langs.SendBotFriendRequest, targetBot.BotName, "已经是好友了/已发送邀请"));
                }
                else
                {
                    bot.SteamFriends.AddFriend(targetBot.SteamID);
                    sb.AppendLine(bot.FormatBotResponse(Langs.SendBotFriendRequest, targetBot.BotName, Langs.Success));
                    await Task.Delay(200).ConfigureAwait(false);
                }
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// 批量添加Bot好友 (多个Bot)
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseAddBotFriendMuli(string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            throw new ArgumentNullException(nameof(query));
        }

        var entries = query.Split(SeparatorPlus,
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var botList = new List<List<Bot>?>();

        foreach (var entry in entries)
        {
            var botNames = entry.Split(SeparatorDotSpace,
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (botNames.Length == 0)
            {
                botList.Add(null);
                continue;
            }

            var bots = new List<Bot>();
            foreach (var botName in botNames)
            {
                var bot = Bot.GetBot(botName);
                if (bot != null)
                {
                    bots.Add(bot);
                }
            }

            botList.Add(bots);
        }

        if (botList.Count >= 2)
        {
            var tasks = new List<(List<Bot>, List<Bot>)>();

            for (var i = 0; i < botList.Count - 1; i++)
            {
                var a = botList[i];
                var b = botList[i + 1];

                if (a?.Count > 0 && b?.Count > 0)
                {
                    if (i != 0)
                    {
                        a = [a[^1]];
                    }

                    if (i != botList.Count - 2)
                    {
                        b = [b[0]];
                    }

                    tasks.Add((a, b));
                }
            }

            var results = await Utilities.InParallel(tasks.Select(x => ResponseAddBotFriendMuli(x.Item1, x.Item2)))
                .ConfigureAwait(false);
            return results.Any() ? string.Join(Environment.NewLine, results) : "未识别到有效的机器人名称";
        }

        return FormatStaticResponse("参数错误, botA+BotB ...");
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

        var sb = new StringBuilder();

        var entries = query.Split(SeparatorDot, StringSplitOptions.RemoveEmptyEntries);

        if (entries.Length > 1)
        {
            sb.AppendLine(Langs.MultipleLineResult);
        }

        var matchInviteLink = RegexUtils.MatchFriendInviteLink();

        foreach (var entry in entries)
        {
            var match = matchInviteLink.Match(entry);

            if (match.Success)
            {
                var result = await bot.AddFriendViaInviteLink(match.Groups[1].Value, match.Groups[2].Value)
                    .ConfigureAwait(false);
                sb.AppendLine(bot.FormatBotResponse(result));
            }
            else
            {
                ulong? steamId;

                if (ulong.TryParse(entry, out var value))
                {
                    steamId = await bot.GetSteamIdByProfileLink($"/profiles/{entry}").ConfigureAwait(false) ??
                              await bot.GetSteamIdByProfileLink($"/id/{entry}").ConfigureAwait(false);

                    // 好友代码
                    if (steamId == null && value < 0xFFFFFFFF)
                    {
                        value = SteamId2Steam32(value);
                        steamId = await bot.GetSteamIdByProfileLink($"/profiles/{value}").ConfigureAwait(false);
                    }
                }
                else
                {
                    steamId = await bot.GetSteamIdByProfileLink($"/id/{entry}").ConfigureAwait(false);
                }

                if (steamId != null)
                {
                    bot.SteamFriends.AddFriend(steamId);
                    sb.AppendLine(bot.FormatBotResponse(Langs.CookieItem, entry, Langs.SendFriendRequestSuccess));
                }
                else
                {
                    sb.AppendLine(bot.FormatBotResponse(Langs.CookieItem, entry, Langs.ProfileNotFound));
                }
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

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseAddFriend(bot, query)))
            .ConfigureAwait(false);

        return results.Any() ? string.Join(Environment.NewLine, results) : null;
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

        var sb = new StringBuilder();

        var entries = query.Split(SeparatorDot, StringSplitOptions.RemoveEmptyEntries);

        if (entries.Length > 1)
        {
            sb.AppendLine(Langs.MultipleLineResult);
        }

        foreach (var entry in entries)
        {
            ulong? steamId;

            if (ulong.TryParse(entry, out var value))
            {
                steamId = await bot.GetSteamIdByProfileLink($"/profiles/{entry}").ConfigureAwait(false) ??
                          await bot.GetSteamIdByProfileLink($"/id/{entry}").ConfigureAwait(false);

                // 好友代码
                if (steamId == null && value < 0xFFFFFFFF)
                {
                    value = SteamId2Steam32(value);
                    steamId = await bot.GetSteamIdByProfileLink($"/profiles/{value}").ConfigureAwait(false);
                }
            }
            else
            {
                steamId = await bot.GetSteamIdByProfileLink($"/id/{entry}").ConfigureAwait(false);
            }

            if (steamId != null)
            {
                bot.SteamFriends.RemoveFriend(steamId);
                sb.AppendLineFormat(Langs.CookieItem, entry, Langs.RemoveFriendSuccess);
                await Task.Delay(200).ConfigureAwait(false);
            }
            else
            {
                sb.AppendLineFormat(Langs.CookieItem, entry, Langs.ProfileNotFound);
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

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseDeleteFriend(bot, query)))
            .ConfigureAwait(false);

        return results.Any() ? string.Join(Environment.NewLine, results) : null;
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

        var friendCount = bot.SteamFriends.GetFriendCount();
        if (friendCount > 0)
        {
            for (var i = 0; i < friendCount; i++)
            {
                var steamId = bot.SteamFriends.GetFriendByIndex(i);
                bot.SteamFriends.RemoveFriend(steamId);
                await Task.Delay(500).ConfigureAwait(false);
            }

            return bot.FormatBotResponse(Langs.DeleteFriendSuccess, friendCount);
        }

        return bot.FormatBotResponse(Langs.NoFriend);
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

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseDeleteAllFriend(bot)))
            .ConfigureAwait(false);

        return results.Any() ? string.Join(Environment.NewLine, results) : null;
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

        var response = await bot.GetAddFriendPage().ConfigureAwait(false);
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

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseGetInviteLink(bot))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }
}