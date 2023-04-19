using AngleSharp.Text;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using System.Text;
using System.Text.RegularExpressions;


namespace ASFEnhance.Profile;

internal static partial class Command
{
    /// <summary>
    /// 获取个人资料摘要
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetProfileSummary(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        string result = await WebRequest.GetSteamProfile(bot).ConfigureAwait(false) ?? Langs.GetProfileFailed;

        return bot.FormatBotResponse(result);
    }

    /// <summary>
    /// 获取个人资料摘要 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetProfileSummary(string botNames)
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

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseGetProfileSummary(bot))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 获取Steam64Id
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static string? ResponseGetSteamId(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        return bot.FormatBotResponse(bot.SteamID.ToString());
    }

    /// <summary>
    /// 获取Steam64Id (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetSteamId(string botNames)
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

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => Task.Run(() => ResponseGetSteamId(bot)))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }


    /// <summary>
    /// 获取个人资料链接
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static string? ResponseGetProfileLink(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        Uri profileLink = new(SteamCommunityURL + $"profiles/{bot.SteamID}");

        return bot.FormatBotResponse(profileLink.ToString());
    }

    /// <summary>
    /// 获取个人资料链接
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetProfileLink(string botNames)
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

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => Task.Run(() => ResponseGetProfileLink(bot)))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 获取好友代码
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static string? ResponseGetFriendCode(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        ulong friendCode = SteamId2Steam32(bot.SteamID);

        return bot.FormatBotResponse(friendCode.ToString());
    }

    /// <summary>
    /// 获取好友代码 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetFriendCode(string botNames)
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

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => Task.Run(() => ResponseGetFriendCode(bot)))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 获取交易链接
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetTradeLink(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        string tradeLink = await WebRequest.GetTradeofferPrivacyPage(bot).ConfigureAwait(false) ?? Langs.NetworkError;

        return bot.FormatBotResponse(tradeLink);
    }

    /// <summary>
    /// 获取交易链接 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetTradeLink(string botNames)
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

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseGetTradeLink(bot))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 清除昵称历史
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseClearAliasHistory(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        await WebRequest.ClearAliasHisrory(bot).ConfigureAwait(false);

        return bot.FormatBotResponse(Langs.Done);
    }

    /// <summary>
    /// 清除昵称历史 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseClearAliasHistory(string botNames)
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

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseClearAliasHistory(bot))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 获取年度总结
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetReplay(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        string? token = await WebRequest.GetReplayToken(bot).ConfigureAwait(false);

        if (string.IsNullOrEmpty(token))
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        string? result = await WebRequest.GetReplayPic(bot, 2022, token).ConfigureAwait(false);

        if (string.IsNullOrEmpty(result))
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        return bot.FormatBotResponse(result);
    }

    /// <summary>
    /// 获取年度总结 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetReplay(string botNames)
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

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseGetReplay(bot))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }


    /// <summary>
    /// 设置年度总结可见性
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseSetReplayPrivacy(Bot bot, string query)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        if (string.IsNullOrEmpty(query))
        {
            throw new ArgumentNullException(nameof(query));
        }

        if (!int.TryParse(query, out int privacy))
        {
            return bot.FormatBotResponse(Langs.ReplayPrivacyError);
        }

        string? token = await WebRequest.GetReplayToken(bot).ConfigureAwait(false);

        if (string.IsNullOrEmpty(token))
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        string? result = await WebRequest.SetReplayPermission(bot, 2022, token, privacy).ConfigureAwait(false);

        if (string.IsNullOrEmpty(result))
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        return bot.FormatBotResponse(result);
    }

    /// <summary>
    /// 设置年度总结可见性 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseSetReplayPrivacy(string botNames, string query)
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

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseSetReplayPrivacy(bot, query))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 设置个人资料游戏头像
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="strGameId"></param>
    /// <param name="strAvatarId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseSetProfileGameAvatar(Bot bot, string? strGameId, string? strAvatarId)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        Random rand = new();

        int gameId, avatarId;

        if (string.IsNullOrEmpty(strGameId))
        {
            //使用随机GameId
            var gameIds = await WebRequest.GetGamdIdsOfAvatarList(bot).ConfigureAwait(false);
            if (gameIds?.Count > 0)
            {
                gameId = gameIds[rand.Next(gameIds.Count)];
            }
            else
            {
                return Langs.GameAvatarGameNotFound;
            }
        }
        else
        {
            if (!int.TryParse(strGameId, out gameId))
            {
                return bot.FormatBotResponse(Langs.GameAvatarInvalidGameId);
            }
        }

        var avatarIds = await WebRequest.GetAvilableAvatarsOfGame(bot, gameId).ConfigureAwait(false);
        if (avatarIds?.Count > 0)
        {
            if (string.IsNullOrEmpty(strAvatarId))
            {
                //使用随机头像
                avatarId = avatarIds[rand.Next(avatarIds.Count)];
            }
            else
            {
                if (!int.TryParse(strAvatarId, out avatarId))
                {
                    return bot.FormatBotResponse(Langs.GameAvatarInvalidAvatarId);
                }
            }

            bool result = await WebRequest.ApplyGameAvatar(bot, gameId, avatarId).ConfigureAwait(false);
            return bot.FormatBotResponse(result ? Langs.Success : Langs.Failure);
        }
        else
        {
            return Langs.GameAvatarGameAvatarIsEmpty;
        }
    }

    /// <summary>
    /// 设置个人资料游戏头像 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="gameId"></param>
    /// <param name="avatarId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseSetProfileGameAvatar(string botNames, string? gameId, string? avatarId)
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

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseSetProfileGameAvatar(bot, gameId, avatarId))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    [GeneratedRegex("%(?:(l|u|d|bot)(\\d*))%")]
    private static partial Regex MatchVariables();

    /// <summary>
    /// 高级重命名命令
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseAdvNickName(Bot bot, string query)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        Regex matchVariable = MatchVariables();
        var matches = matchVariable.Matches(query.ToLowerInvariant());
        if (matches?.Count > 0)
        {
            Random rand = new();
            Queue<string> replaceMent = new();

            foreach (var match in matches.ToList())
            {
                StringBuilder sb = new();

                string flag = match.Groups[1].Value;
                string strCount = match.Groups[2].Value;

                if (!int.TryParse(strCount, out int count) || count <= 0)
                {
                    count = 1;
                }

                while (count-- > 0)
                {
                    if (flag == "bot")
                    {
                        sb.Append(bot.BotName);
                    }
                    else
                    {
                        char? str = flag switch
                        {
                            "l" => (char)rand.Next(97, 123),
                            "u" => (char)rand.Next(65, 91),
                            "d" => (char)rand.Next(48, 58),
                            _ => null,
                        };
                        sb.Append(str);
                    }
                }
                replaceMent.Enqueue(sb.ToString());
            }

            foreach (var match in matches.ToList())
            {
                if (replaceMent.Count == 0)
                {
                    break;
                }
                string replace = replaceMent.Dequeue();
                query = query.ReplaceFirst(match.Value, replace);
            }
        }

        string? result = await bot.Commands.Response(EAccess.Master, $"NICKNAME {bot.BotName} {query}").ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// 高级重命名命令 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseAdvNickName(string botNames, string query)
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

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseAdvNickName(bot, query))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 设置个人资料头像
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="imgUrl"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseSetProfileAvatar(Bot bot, string imgUrl)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        if (string.IsNullOrEmpty(imgUrl))
        {
            throw new ArgumentNullException(nameof(imgUrl));
        }

        var result = await WebRequest.ApplyCustomAvatar(bot, imgUrl).ConfigureAwait(false);
        return bot.FormatBotResponse(result);
    }

    /// <summary>
    /// 设置个人资料头像 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="imgUrl"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseSetProfileAvatar(string botNames, string imgUrl)
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

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseSetProfileAvatar(bot, imgUrl))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 删除个人资料头像
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="imgUrl"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseDelProfileAvatar(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var result = await WebRequest.ApplyCustomAvatar(bot, Static.DefaultSteamAvatar).ConfigureAwait(false);
        return bot.FormatBotResponse(result);
    }

    /// <summary>
    /// 删除个人资料头像 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="imgUrl"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseDelProfileAvatar(string botNames)
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

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseDelProfileAvatar(bot))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 合成徽章
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseCraftBadge(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var craftableAppids = await WebRequest.FetchCraftableBadgeDict(bot).ConfigureAwait(false);
        if (craftableAppids == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }
        int count = craftableAppids.Count;
        if (count == 0)
        {
            return bot.FormatBotResponse(Langs.NoCraftableBadge);
        }

        var result = await Utilities.InParallel(craftableAppids.Select((item, _) => WebRequest.CraftBadge(bot, item.Key, item.Value + 1, 0, 1))).ConfigureAwait(false);
        int success = result.Count(x => x);

        return bot.FormatBotResponse(string.Format(Langs.CraftBadgeResult, success, count));
    }

    /// <summary>
    /// 合成徽章 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseCraftBadge(string botNames)
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

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseCraftBadge(bot))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 获取交易链接
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
    /// 获取交易链接 (多个Bot)
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
