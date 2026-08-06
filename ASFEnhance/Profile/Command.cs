using AngleSharp.Text;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Data;
using ASFEnhance.Data.Plugin;
using SteamKit2;
using System.Text;


namespace ASFEnhance.Profile;

internal static class Command
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

        var result = await WebRequest.GetSteamProfile(bot).ConfigureAwait(false);

        if (result != null)
        {
            var sb = new StringBuilder();
            sb.AppendLine(Langs.MultipleLineResult);
            sb.AppendLine(Langs.ProfileHeader);
            sb.AppendLineFormat(Langs.ProfileNickname, result.NickName);
            sb.AppendLineFormat(Langs.ProfileState, result.IsOnline ? Langs.Online : Langs.Offline);

            sb.AppendLineFormat(Langs.ProfileLevel, result.Level);

            if (result.BadgeCount > 0)
            {
                sb.AppendLineFormat(Langs.ProfileBadges, result.BadgeCount);
            }

            if (result.GameCount > 0)
            {
                sb.AppendLineFormat(Langs.ProfileGames, result.GameCount);
            }

            if (result.ScreenshotCount > 0)
            {
                sb.AppendLineFormat(Langs.ProfileScreenshots, result.ScreenshotCount);
            }

            if (result.VideoCount > 0)
            {
                sb.AppendLineFormat(Langs.ProfileVideos, result.VideoCount);
            }

            if (result.WorkshopCount > 0)
            {
                sb.AppendLineFormat(Langs.ProfileWorkshop, result.WorkshopCount);
            }

            if (result.ReviewCount > 0)
            {
                sb.AppendLineFormat(Langs.ProfileRecommended, result.ReviewCount);
            }

            if (result.GuideCount > 0)
            {
                sb.AppendLineFormat(Langs.ProfileGuide, result.GuideCount);
            }

            if (result.ArtworkCount > 0)
            {
                sb.AppendLineFormat(Langs.ProfileImages, result.ArtworkCount);
            }

            if (result.GroupCount > 0)
            {
                sb.AppendLineFormat(Langs.ProfileGroups, result.GroupCount);
            }

            if (result.FriendCount > 0)
            {
                int maxFriend = (5 * result.Level) + 250;
                if (maxFriend > 2000)
                {
                    maxFriend = 2000;
                }
                sb.AppendLineFormat(Langs.ProfileFriends, result.FriendCount, maxFriend > 0 ? maxFriend : "-");
            }

            return bot.FormatBotResponse(sb.ToString());
        }
        else
        {
            return bot.FormatBotResponse(Langs.GetProfileFailed);
        }
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

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseGetProfileSummary(bot))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

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

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => Task.Run(() => ResponseGetSteamId(bot)))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }


    /// <summary>
    /// 获取个人资料链接
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetProfileLink(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var absPath = $"/profiles/{bot.SteamID}";
        var customPath = await bot.GetProfileLink().ConfigureAwait(false);

        var sb = new StringBuilder();

        if (string.Compare(absPath, customPath, StringComparison.OrdinalIgnoreCase) != 0)
        {
            sb.AppendLine(bot.FormatBotResponse(new Uri(SteamCommunityURL, absPath).ToString()));
            sb.AppendLine(bot.FormatBotResponse(new Uri(SteamCommunityURL, customPath).ToString()));
        }
        else
        {
            sb.AppendLine(bot.FormatBotResponse(new Uri(SteamCommunityURL, absPath).ToString()));
        }

        return sb.ToString();
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

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => Task.Run(() => ResponseGetProfileLink(bot)))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

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

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => Task.Run(() => ResponseGetFriendCode(bot)))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

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

        string tradeLink = await WebRequest.GetTradeOfferPrivacyPage(bot).ConfigureAwait(false) ?? Langs.NetworkError;

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

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseGetTradeLink(bot))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

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

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseClearAliasHistory(bot))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 获取年度总结
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="years"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetReplay(Bot bot, string years)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var entries = years.Split(SeparatorDot, StringSplitOptions.RemoveEmptyEntries);
        var sb = new StringBuilder();

        foreach (var entry in entries)
        {
            if (!int.TryParse(entry, out var intYear) || intYear < 2022 || intYear > 9999)
            {
                sb.AppendLine(bot.FormatBotResponse(Langs.CookieItem, intYear, Langs.ArgumentErrorYear));
                continue;
            }

            var token = bot.AccessToken;
            if (string.IsNullOrEmpty(token))
            {
                sb.AppendLine(bot.FormatBotResponse(Langs.CookieItem, intYear, Langs.NetworkError));
                continue;
            }

            await WebRequest.GetYearInReview(bot).ConfigureAwait(false);

            var result = await WebRequest.GetReplayPic(bot, intYear, token).ConfigureAwait(false);
            sb.AppendLine(bot.FormatBotResponse(Langs.CookieItem, intYear, result ?? Langs.NetworkError));
        }

        return sb.ToString();
    }

    /// <summary>
    /// 获取年度总结 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="years"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetReplay(string botNames, string years)
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

        var results = await Utilities.InParallel(bots.Select(bot => ResponseGetReplay(bot, years))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }


    /// <summary>
    /// 设置年度总结可见性
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="query"></param>
    /// <param name="year"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseSetReplayPrivacy(Bot bot, string query, string year)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        if (!int.TryParse(year, out var intYear) || intYear < 2022 || intYear > 9999)
        {
            return bot.FormatBotResponse(Langs.ArgumentErrorYear);
        }

        if (string.IsNullOrEmpty(query))
        {
            throw new ArgumentNullException(nameof(query));
        }

        if (!int.TryParse(query, out int privacy))
        {
            return bot.FormatBotResponse(Langs.ReplayPrivacyError);
        }

        var token = bot.AccessToken;
        if (string.IsNullOrEmpty(token))
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        var result = await WebRequest.SetReplayPermission(bot, intYear, privacy).ConfigureAwait(false);
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
    /// <param name="year"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseSetReplayPrivacy(string botNames, string query, string year)
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

        var results = await Utilities.InParallel(bots.Select(bot => ResponseSetReplayPrivacy(bot, query, year))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

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

        var rand = new Random();
        int gameId;
        if (string.IsNullOrEmpty(strGameId))
        {
            //使用随机GameId
            var gameIds = await WebRequest.GetGameIdsOfAvatarList(bot).ConfigureAwait(false);
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

        var avatarIds = await WebRequest.GetAvailableAvatarsOfGame(bot, gameId).ConfigureAwait(false);
        if (avatarIds?.Count > 0)
        {
            int avatarId;
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

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseSetProfileGameAvatar(bot, gameId, avatarId))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }


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

        var matchVariable = RegexUtils.MatchVariables();
        var matches = matchVariable.Matches(query.ToLowerInvariant());
        if (matches?.Count > 0)
        {
            var rand = new Random();
            Queue<string> replaceMent = new();

            foreach (var match in matches.ToList())
            {
                var sb = new StringBuilder();

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

        var result = await bot.Commands.Response(EAccess.Master, $"NICKNAME {bot.BotName} {query}").ConfigureAwait(false);
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

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseAdvNickName(bot, query))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

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

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseSetProfileAvatar(bot, imgUrl))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

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

        const string defaultAvatar = "https://avatars.akamai.steamstatic.com/fef49e7fa7e1997310d705b2a6158ff8dc1cdfeb_full.jpg";

        var result = await WebRequest.ApplyCustomAvatar(bot, defaultAvatar).ConfigureAwait(false);
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

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseDelProfileAvatar(bot))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

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

        // Key: AppId, Value: 徽章等级, <0 代表闪亮
        var craftableAppids = await WebRequest.FetchCraftableBadgeDict(bot).ConfigureAwait(false);
        if (craftableAppids == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }
        var count = craftableAppids.Count;
        if (count == 0)
        {
            return bot.FormatBotResponse(Langs.NoCraftableBadge);
        }

        var semaphore = new SemaphoreSlim(1);
        var result = await Utilities.InParallel(
            craftableAppids.Select((kv) => WebRequest.CraftBadge(bot, semaphore, kv.Key, kv.Value > 0, 1)
        )).ConfigureAwait(false);

        var success = result.Count(x => x);

        return bot.FormatBotResponse(Langs.CraftBadgeResult, success, count);
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

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseCraftBadge(bot))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 合成指定游戏的徽章
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseCraftSpecifyBadge(Bot bot, string query)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var appIds = FetchGameIds(query, ESteamGameIdType.App, ESteamGameIdType.App);
        if (appIds.Count == 0)
        {
            return bot.FormatBotResponse(Langs.ArgumentNotInteger, "AppId");
        }

        var strAppIds = query.Split(SeparatorDot, StringSplitOptions.RemoveEmptyEntries);

        var sb = new StringBuilder();
        foreach (string strAppId in strAppIds)
        {
            if (!int.TryParse(strAppId, out var appId) || (appId == 0))
            {
                sb.AppendLine(bot.FormatBotResponse(Strings.ErrorIsInvalid, nameof(appId)));
            }
            else
            {
                bool foil = appId < 0;
                if (appId < 0)
                {
                    appId = -appId;
                }
                var result = await WebRequest.CraftBadge(bot, appId, foil, 1).ConfigureAwait(false);
                sb.AppendLine(bot.FormatBotResponse(Strings.BotAddLicense, appId, result ? Langs.Success : Langs.Failure));
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// 合成指定游戏的徽章 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseCraftSpecifyBadge(string botNames, string query)
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

        var results = await Utilities.InParallel(bots.Select(bot => ResponseCraftSpecifyBadge(bot, query))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 修改真实名称
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="realName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseEditRealName(Bot bot, string? realName)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var payload = await WebRequest.GetProfilePayload(bot).ConfigureAwait(false);
        if (payload == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        payload.RealName = realName;

        var result = await WebRequest.SaveProfilePayload(bot, payload).ConfigureAwait(false);

        if (result?.Success == EResult.OK && !string.IsNullOrEmpty(result.Redirect))
        {
            return bot.FormatBotResponse(Langs.EditCustomUrlSuccess, result.Redirect.Replace("/edit/info", ""));
        }
        else
        {
            return bot.FormatBotResponse(Langs.EditCustomUrlFailed, result?.ErrMsg ?? Langs.NetworkError);
        }
    }

    /// <summary>
    /// 修改真实名称 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="realName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseEditRealName(string botNames, string? realName)
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

        var results = await Utilities.InParallel(bots.Select(bot => ResponseEditRealName(bot, realName))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 修改自定义Url
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="customUrl"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseEditCustomUrl(Bot bot, string? customUrl)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var payload = await WebRequest.GetProfilePayload(bot).ConfigureAwait(false);
        if (payload == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        payload.CustomURL = customUrl;

        var result = await WebRequest.SaveProfilePayload(bot, payload).ConfigureAwait(false);

        if (result?.Success == EResult.OK && !string.IsNullOrEmpty(result.Redirect))
        {
            return bot.FormatBotResponse(Langs.EditCustomUrlSuccess, result.Redirect.Replace("/edit/info", ""));
        }
        else
        {
            return bot.FormatBotResponse(Langs.EditCustomUrlFailed, result?.ErrMsg ?? Langs.NetworkError);
        }
    }

    /// <summary>
    /// 修改自定义Url (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="customUrl"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseEditCustomUrl(string botNames, string? customUrl)
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

        var results = await Utilities.InParallel(bots.Select(bot => ResponseEditCustomUrl(bot, customUrl))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 获取钱包
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseBalanceInfo(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        return await WebRequest.GetAccountBalanceInfo(bot).ConfigureAwait(false);
    }

    /// <summary>
    /// 获取钱包 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseBalanceInfo(string botNames)
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

        var results = await Utilities.InParallel(bots.Select(bot => ResponseBalanceInfo(bot))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 设置个人资料装饰器
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="strItemId"></param>
    /// <param name="enable"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseSetProfileModifier(Bot bot, string strItemId)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        if (!ulong.TryParse(strItemId, out var _))
        {
            return bot.FormatBotResponse(Langs.ArgumentNotInteger, nameof(strItemId));
        }

        var ownedItems = await WebRequest.GetProfileItemsOwned(bot).ConfigureAwait(false);
        if (ownedItems?.ProfileModifiers == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        uint appId = 0;
        foreach (var item in ownedItems.ProfileModifiers)
        {
            if (item.CommunityItemId == strItemId)
            {
                appId = item.AppId;
                break;
            }
        }

        if (appId == 0)
        {
            return bot.FormatBotResponse(Langs.ProfileModifierNotFound);
        }

        var result = await WebRequest.SetProfileModifier(bot, appId, strItemId, true).ConfigureAwait(false);

        return bot.FormatBotResponse(result ? Langs.Success : Langs.Failure);
    }

    /// <summary>
    /// 设置个人资料装饰器 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="strItemId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseSetProfileModifier(string botNames, string strItemId)
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

        var results = await Utilities.InParallel(bots.Select(bot => ResponseSetProfileModifier(bot, strItemId))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 清除个人资料装饰器
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseClearProfileModifier(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var equippedItems = await WebRequest.GetProfileItemsEquipped(bot, bot.SteamID).ConfigureAwait(false);

        if (equippedItems?.ProfileModifiers?.CommunityItemId != null)
        {
            var item = equippedItems.ProfileModifiers;
            var result = await WebRequest.SetProfileModifier(bot, item.AppId, item.CommunityItemId, false).ConfigureAwait(false);

            return bot.FormatBotResponse(result ? Langs.Success : Langs.Failure);
        }

        return bot.FormatBotResponse(Langs.ProfileModifierNotEquipped);
    }

    /// <summary>
    /// 清除个人资料装饰器 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseClearProfileModifier(string botNames)
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

        var results = await Utilities.InParallel(bots.Select(ResponseClearProfileModifier)).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 设置个人资料主题
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="themeName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseSetProfileTheme(Bot bot, string? themeName)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        if (!string.IsNullOrEmpty(themeName))
        {
            themeName = themeName.ToLowerInvariant();

            List<string> choices = ["summer", "midnight", "steel", "cosmic", "darkmode"];

            if (!choices.Contains(themeName))
            {
                if (themeName != "*")
                {
                    return bot.FormatBotResponse(Langs.InvalidThemeName);
                }

                themeName = choices[Random.Shared.Next(choices.Count)];
            }
        }
        else
        {
            themeName = null;
        }

        var result = await WebRequest.SetProfileTheme(bot, themeName).ConfigureAwait(false);

        if (string.IsNullOrEmpty(themeName))
        {
            return bot.FormatBotResponse(Langs.ClearThemeResult, result ? Langs.Success : Langs.Failure);
        }
        else
        {
            return bot.FormatBotResponse(Langs.SetThemeResult, themeName, result ? Langs.Success : Langs.Failure);
        }
    }

    /// <summary>
    /// 设置个人资料主题 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="themeName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseSetProfileTheme(string botNames, string? themeName)
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

        var results = await Utilities.InParallel(bots.Select(bot => ResponseSetProfileTheme(bot, themeName))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 获取个人资料装饰器
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetProfileItems(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var result = await WebRequest.GetProfileItemsOwned(bot).ConfigureAwait(false);

        if (result == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        if (result.ProfileModifiers == null || result.ProfileModifiers.Count == 0)
        {
            return bot.FormatBotResponse(Langs.ProfileModifierNotOwned);
        }

        var sb = new StringBuilder();

        sb.AppendLine(Langs.MultipleLineResult);
        sb.AppendLine(Langs.OwnedProfileModifiers);
        foreach (var item in result.ProfileModifiers)
        {
            sb.AppendLineFormat(" - {0} ItemId: {1}", item.ItemTitle, item.CommunityItemId);
        }

        return bot.FormatBotResponse(sb.ToString());
    }

    /// <summary>
    /// 获取个人资料装饰器 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetProfileItems(string botNames)
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

        var results = await Utilities.InParallel(bots.Select(ResponseGetProfileItems)).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    private static CountrySelectionData? FindCountrySelection(List<CountrySelectionData> data, string? country, string? state, string? city)
    {
        if (data.Count == 0)
        {
            return null;
        }

        if (!string.IsNullOrEmpty(country))
        {
            if (country.StartsWith('?') || country.StartsWith('？'))
            {
                var i = Random.Shared.Next(0, data.Count);
                return data[i];
            }

            foreach (var c in data)
            {
                if ((!string.IsNullOrEmpty(c.CountryName) && c.CountryName.Equals(country, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(c.CountryCode) && c.CountryCode.Equals(country, StringComparison.OrdinalIgnoreCase)))
                {
                    return c;
                }
            }
        }

        if (!string.IsNullOrEmpty(state))
        {
            if (state.StartsWith('?') || state.StartsWith('？'))
            {
                var i = Random.Shared.Next(0, data.Count);
                return data[i];
            }

            foreach (var s in data)
            {
                if ((!string.IsNullOrEmpty(s.StateName) && s.StateName.Equals(state, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(s.StateCode) && s.StateCode.Equals(state, StringComparison.OrdinalIgnoreCase)))
                {
                    return s;
                }
            }
        }

        if (!string.IsNullOrEmpty(city))
        {
            if (city.StartsWith('?') || city.StartsWith('？'))
            {
                var i = Random.Shared.Next(0, data.Count);
                return data[i];
            }

            foreach (var c in data)
            {
                if ((!string.IsNullOrEmpty(c.CityName) && c.CityName.Equals(city, StringComparison.OrdinalIgnoreCase)) ||
                    (c.CityId != null && c.CityId.Value.ToString().Equals(city, StringComparison.OrdinalIgnoreCase)))
                {
                    return c;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// 获取可用区域选项
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="setting"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetRegionSettingOptions(Bot bot, string? setting)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var (country, state, city) = Utils.ParseRegionSetting(setting);

        var countries = await WebRequest.GetProfileRegionSelection(bot, null, null).ConfigureAwait(false);

        if (countries == null || countries.Count == 0)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        var sb = new StringBuilder();
        sb.AppendLine(Langs.MultipleLineResult);

        if (!string.IsNullOrEmpty(country))
        {
            var fc = FindCountrySelection(countries, country, null, null);

            if (fc != null)
            {
                if (fc.HasStates == 0)
                {
                    sb.AppendLineFormat("当前选择的国家/地区: {0} ({1})", fc.CountryName, fc.CountryCode);
                    sb.AppendLine("当前国家/地区没有州/省选项");
                    return bot.FormatBotResponse(sb.ToString());
                }

                var states = await WebRequest.GetProfileRegionSelection(bot, fc.CountryCode, null).ConfigureAwait(false);

                if (states == null || states.Count == 0)
                {
                    return bot.FormatBotResponse("找不到国家/地区 {0} ({1}) 的区域选项", fc.CountryName, fc.CountryCode);
                }

                if (!string.IsNullOrEmpty(state))
                {
                    var fs = FindCountrySelection(states, null, state, null);

                    if (fs != null)
                    {
                        var cities = await WebRequest.GetProfileRegionSelection(bot, fc.CountryCode, fs.StateCode).ConfigureAwait(false);

                        if (cities == null || cities.Count == 0)
                        {
                            return bot.FormatBotResponse("找不到州/省 {0} ({1}) 的区域选项", fs.StateName, fs.StateCode);
                        }

                        if (!string.IsNullOrEmpty(city))
                        {
                            var fci = FindCountrySelection(cities, null, null, city);
                            if (fci != null)
                            {
                                sb.AppendLineFormat("当前选择的国家/地区: {0} ({1})", fc.CountryName, fc.CountryCode);
                                sb.AppendLineFormat("当前选择的州/省: {0} ({1})", fs.StateName, fs.StateCode);
                                sb.AppendLineFormat("当前选择的城市: {0} ({1})", fci.CityName, fci.CityId);
                            }
                            else
                            {
                                return bot.FormatBotResponse("找不到城市 {0} 的区域选项", city);
                            }
                        }
                        else
                        {
                            sb.AppendLineFormat("当前选择的国家/地区: {0} ({1})", fc.CountryName, fc.CountryCode);
                            sb.AppendLineFormat("当前选择的州/省: {0} ({1})", fs.StateName, fs.StateCode);
                            sb.AppendLine("可用城市选项 (括号内为城市代码):");
                            foreach (var item in cities)
                            {
                                sb.AppendLineFormat(" - {0} ({1})", item.CityName, item.CityId);
                            }
                        }
                    }
                    else
                    {
                        return bot.FormatBotResponse("找不到州/省 {0} 的区域选项", state);
                    }
                }
                else
                {
                    sb.AppendLineFormat("当前选择的国家/地区: {0} ({1})", fc.CountryName, fc.CountryCode);
                    sb.AppendLine("可用州/省选项 (括号内为州/省代码):");
                    foreach (var item in states)
                    {
                        sb.AppendLineFormat(" - {0} ({1})", item.StateName, item.StateCode);
                    }
                    sb.AppendLine();
                    sb.AppendLineFormat("如果需要查询下一级, 可以使用命令 GETPROFILEREGIONOPTION {0} {1}|{2}", bot.BotName, fc.CountryCode, states[0].StateCode);
                }
            }
            else
            {
                return bot.FormatBotResponse("找不到国家/地区 {0} 的区域选项", country);
            }
        }
        else
        {
            sb.AppendLine("可用国家/地区选项 (括号内为国家/地区代码):");
            foreach (var item in countries)
            {
                sb.AppendLineFormat(" - {0} ({1})", item.CountryName, item.CountryCode);
            }
            sb.AppendLine();
            sb.AppendLineFormat("如果需要查询下一级, 使用命令 GETPROFILEREGIONOPTION {0} {1}", bot.BotName, countries[0].CountryCode);
        }

        return bot.FormatBotResponse(sb.ToString());
    }

    /// <summary>
    /// 获取可用区域选项 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="setting"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetRegionSettingOptions(string botNames, string? setting)
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

        var results = await Utilities.InParallel(bots.Select(bot => ResponseGetRegionSettingOptions(bot, setting))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 获取账号的区域设定
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetProfileRegion(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var profileSetting = await WebRequest.GetProfilePayload(bot).ConfigureAwait(false);

        if (profileSetting == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        if (profileSetting.Location == null)
        {
            return bot.FormatBotResponse("机器人尚未设置区域");
        }
        else
        {
            var location = profileSetting.Location;

            if (string.IsNullOrEmpty(location.Country))
            {
                location.Country = "未设定";
                location.CountryCode = "/";
            }
            if (string.IsNullOrEmpty(location.State))
            {
                location.State = "未设定";
                location.StateCode = "/";
            }
            if (string.IsNullOrEmpty(location.City))
            {
                location.City = "未设定";
                location.CityCode = "/";
            }

            var sb = new StringBuilder();
            sb.AppendLine(Langs.MultipleLineResult);
            sb.AppendLineFormat("国家/地区: {0}, 代码: {1}", location.Country, location.CountryCode);
            sb.AppendLineFormat("州/省: {0}, 代码: {1}", location.State, location.StateCode);
            sb.AppendLineFormat("城市: {0}, 代码: {1}", location.City, location.CityCode);
            sb.AppendLine();

            return bot.FormatBotResponse(sb.ToString());
        }
    }

    /// <summary>
    /// 获取账号的区域设定 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetProfileRegion(string botNames)
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

        var results = await Utilities.InParallel(bots.Select(ResponseGetProfileRegion)).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 设置账号区域
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="setting"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseSetProfileRegion(Bot bot, string? setting)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var (country, state, city) = Utils.ParseRegionSetting(setting);

        var countries = await WebRequest.GetProfileRegionSelection(bot, null, null).ConfigureAwait(false);

        if (countries == null || countries.Count == 0)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        var sb = new StringBuilder();
        sb.AppendLine(Langs.MultipleLineResult);

        var profileBefore = await WebRequest.GetProfilePayload(bot).ConfigureAwait(false);

        if (profileBefore?.Location == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        var location = profileBefore.Location;
        sb.AppendLine("原区域设定:");
        sb.AppendLineFormat(" - 国家/地区: {0}, 代码: {1}", location.Country, location.CountryCode);
        sb.AppendLineFormat(" - 州/省: {0}, 代码: {1}", location.State, location.StateCode);
        sb.AppendLineFormat(" - 城市: {0}, 代码: {1}", location.City, location.CityCode);

        if (!string.IsNullOrEmpty(country))
        {
            var fc = FindCountrySelection(countries, country, null, null);
            if (fc != null)
            {
                //location.Country = fc.CountryName;
                location.CountryCode = fc.CountryCode;

                if (fc.HasStates != 0)
                {
                    var states = await WebRequest.GetProfileRegionSelection(bot, fc.CountryCode, null).ConfigureAwait(false);

                    if (!string.IsNullOrEmpty(state))
                    {
                        if (states == null || states.Count == 0)
                        {
                            return bot.FormatBotResponse("找不到国家/地区 {0} ({1}) 的下一级区域选项", fc.CountryName, fc.CountryCode);
                        }

                        var fs = FindCountrySelection(states, null, state, null);
                        if (fs != null)
                        {
                            //location.State = fs.StateName;
                            location.StateCode = fs.StateCode;

                            var cities = await WebRequest.GetProfileRegionSelection(bot, fc.CountryCode, fs.StateCode).ConfigureAwait(false);

                            if (!string.IsNullOrEmpty(city))
                            {
                                if (cities == null || cities.Count == 0)
                                {
                                    return bot.FormatBotResponse("找不到州/省 {0} ({1}) 的下一级区域选项", fs.StateName, fs.StateCode);
                                }

                                var fci = FindCountrySelection(cities, null, null, city);
                                if (fci != null)
                                {
                                    //location.City = fci.CityName;
                                    location.CityCode = fci.CityId.ToString();
                                }
                                else
                                {
                                    return bot.FormatBotResponse("找不到指定城市 {0} 的区域选项", city);
                                }
                            }
                            else
                            {
                                location.CityCode = "";
                            }
                        }
                        else
                        {
                            return bot.FormatBotResponse("找不到指定州/省 {0} 的区域选项", state);
                        }
                    }
                    else
                    {
                        location.StateCode = "";
                    }
                }
            }
            else
            {
                return bot.FormatBotResponse("找不到指定国家/地区 {0} 的区域选项", country);
            }
        }
        else
        {
            location.CountryCode = "";
        }

        await WebRequest.SaveProfilePayload(bot, profileBefore).ConfigureAwait(false);

        var profileAfter = await WebRequest.GetProfilePayload(bot).ConfigureAwait(false);

        if (profileAfter?.Location == null)
        {
            sb.AppendLine("更新后的区域设定获取失败");
        }
        else
        {
            var l = profileAfter.Location;
            sb.AppendLine("更新后的区域设定:");
            sb.AppendLineFormat(" - 国家/地区: {0}, 代码: {1}", l.Country, l.CountryCode);
            sb.AppendLineFormat(" - 州/省: {0}, 代码: {1}", l.State, l.StateCode);
            sb.AppendLineFormat(" - 城市: {0}, 代码: {1}", l.City, l.CityCode);
        }

        return bot.FormatBotResponse(sb.ToString());
    }

    /// <summary>
    /// 设置账号区域 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="setting"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseSetProfileRegion(string botNames, string? setting)
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

        var results = await Utilities.InParallel(bots.Select(bot => ResponseSetProfileRegion(bot, setting))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }
}
