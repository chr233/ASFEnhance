using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;

namespace ASFEnhance.Event;

internal static class Command
{
    /// <summary>
    ///     获取DL2贴纸 6.28 - ?
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseDL2(Bot bot, string? query)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var token = bot.AccessToken ?? throw new AccessTokenNullException(bot);
        if (string.IsNullOrEmpty(token))
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        if (!string.IsNullOrEmpty(query))
        {
            if (int.TryParse(query, out var id))
            {
                await WebRequest.DoEventTask(bot, token, id).ConfigureAwait(false);
            }
            else
            {
                return bot.FormatBotResponse(Langs.AccountSubInvalidArg);
            }
        }
        else
        {
            var door_indexs = new[] { 1 };
            var tasks = door_indexs.Select(id => WebRequest.DoEventTask(bot, token, id));
            await Utilities.InParallel(tasks).ConfigureAwait(false);
        }

        return bot.FormatBotResponse("Done!");
    }

    /// <summary>
    ///     获取DL2贴纸 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseDL2(string botNames, string? query)
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

        var results = await Utilities.InParallel(bots.Select(bot => ResponseDL2(bot, query))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    ///     领取活动道具
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseClaimItem(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var token = bot.AccessToken;
        if (string.IsNullOrEmpty(token))
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        var result = await WebRequest.ClaimDailySticker(bot, token).ConfigureAwait(false);

        if (result?.RewardItem == null)
        {
            return bot.FormatBotResponse(Langs.NoItemToClaim);
        }

        var name = result.RewardItem.CommunityItem?.ItemName ??
                   result.RewardItem.CommunityItem?.ItemTitle ?? "UNKNOWN";
        var localTime = DateTimeOffset.FromUnixTimeSeconds(result.NextClaimTime).LocalDateTime;

        return bot.FormatBotResponse(Langs.ClaimItemSuccessful, name, localTime.ToString("yyyy-MM-dd HH:mm:ss"));
    }

    /// <summary>
    ///     领取活动道具 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseClaimItem(string botNames)
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

        var results = await Utilities.InParallel(bots.Select(ResponseClaimItem)).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    ///     领取20周年贴纸
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseClaim20Th(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var token = bot.AccessToken;
        if (string.IsNullOrEmpty(token))
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        var defIds = new List<int>
        {
            241812,
            241811,
            241810,
            241809,
            241807,
            241808
        };
        var results = await Utilities.InParallel(defIds.Select(id => WebRequest.RedeenPointShopItem(bot, token, id)))
            .ConfigureAwait(false);

        var count = 0;
        foreach (var result in results)
        {
            if (result)
            {
                count++;
            }
        }

        return bot.FormatBotResponse(Langs.SendRequestSuccess, count);
    }

    /// <summary>
    ///     领取20周年贴纸 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseClaim20Th(string botNames)
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

        var results = await Utilities.InParallel(bots.Select(ResponseClaim20Th)).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    ///     秋促投票
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="gameIDs"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseAutumnSteamAwardVote(Bot bot, string gameIDs)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var entries = gameIDs.Split(SeparatorDot, StringSplitOptions.RemoveEmptyEntries);

        List<int> intGamsIDs = [];

        const int categories = 11;

        foreach (var entry in entries)
        {
            if (!int.TryParse(entry, out var choice) || choice <= 0)
            {
                continue;
            }

            intGamsIDs.Add(choice);
            if (intGamsIDs.Count >= categories)
            {
                break;
            }
        }

        if (intGamsIDs.Count < categories) //不足11个游戏自动补齐
        {
            List<int> defaultGames = [
                3167020,
                2399420,
                548430,
                2407270,
                2543180,
                2947440,
                2988300,
                1030300,
                1903340,
                2089600,
                3444020
            ];
            while (intGamsIDs.Count < categories)
            {
                intGamsIDs.Add(defaultGames[intGamsIDs.Count]);
            }
        }

        var token = bot.AccessToken;
        if (string.IsNullOrEmpty(token))
        {
            bot.ArchiLogger.LogNullError(nameof(token));
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        var semaphore = new SemaphoreSlim(1);

        int categoryBegin = 130;

        List<Task> tasks = [];

        for (var i = 0; i < categories; i++)
        {
            tasks.Add(WebRequest.MakeVoteForAutumnSale(bot, intGamsIDs[i], categoryBegin++, token, semaphore, 6));
        }

        await Utilities.InParallel(tasks).ConfigureAwait(false);

        var summerBadgeStatus = await WebRequest.CheckAutumnSaleBadge(bot).ConfigureAwait(false);
        return bot.FormatBotResponse(summerBadgeStatus);
    }

    /// <summary>
    ///     秋促投票 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="choose"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseAutumnSteamAwardVote(string botNames, string choose)
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

        var results = await Utilities.InParallel(bots.Select(bot => ResponseAutumnSteamAwardVote(bot, choose)))
            .ConfigureAwait(false);
        var responses = new List<string>(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    ///     检查秋促徽章
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="steamID"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseCheckAutumnSteamAwardVote(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var summerBadgeStatus = await WebRequest.CheckAutumnSaleBadge(bot).ConfigureAwait(false);

        return bot.FormatBotResponse(summerBadgeStatus);
    }

    /// <summary>
    ///     检查秋促徽章 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseCheckAutumnSteamAwardVote(string botNames)
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

        var results = await Utilities.InParallel(bots.Select(ResponseCheckAutumnSteamAwardVote))
            .ConfigureAwait(false);
        var responses = new List<string>(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    ///     冬促投票
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="gameIDs"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseWinterSteamAwardVote(Bot bot, string gameIDs)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var entries = gameIDs.Split(SeparatorDot, StringSplitOptions.RemoveEmptyEntries);

        List<int> intGamsIDs = [];

        const int categories = 11;

        foreach (var entry in entries)
        {
            if (!int.TryParse(entry, out var choice) || choice <= 0)
            {
                continue;
            }

            intGamsIDs.Add(choice);
            if (intGamsIDs.Count >= categories)
            {
                break;
            }
        }

        if (intGamsIDs.Count < categories) //不足11个游戏自动补齐
        {
            List<int> defaultGames =
            [
                1030300,
                555160,
                553850,
                1145350,
                2001120,
                2947440,
                3167020,
                1030300,
                1903340,
                3180070,
                2968420
            ];
            while (intGamsIDs.Count < categories)
            {
                intGamsIDs.Add(defaultGames[intGamsIDs.Count]);
            }
        }

        var token = bot.AccessToken;
        if (string.IsNullOrEmpty(token))
        {
            bot.ArchiLogger.LogNullError(nameof(token));
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        var semaphore = new SemaphoreSlim(1);

        int categoryBegin = 130;

        List<Task> tasks = [];

        for (var i = 0; i < categories; i++)
        {
            tasks.Add(WebRequest.MakeWinterSteamAwardVote(bot, intGamsIDs[i], categoryBegin++, token, semaphore, 4147080));
        }

        await Utilities.InParallel(tasks).ConfigureAwait(false);

        var summerBadgeStatus = await WebRequest.CheckWinterSteamAwardVote(bot).ConfigureAwait(false);
        return bot.FormatBotResponse(summerBadgeStatus ?? Langs.NetworkError);
    }

    /// <summary>
    ///     冬促投票 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="choose"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseWinterSteamAwardVote(string botNames, string choose)
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

        var results = await Utilities.InParallel(bots.Select(bot => ResponseWinterSteamAwardVote(bot, choose)))
            .ConfigureAwait(false);
        var responses = new List<string>(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    ///     检查冬促投票
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseCheckWinterSteamAwardVote(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var token = bot.AccessToken;
        if (string.IsNullOrEmpty(token))
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        var summerBadgeStatus = await WebRequest.CheckWinterSteamAwardVote(bot).ConfigureAwait(false);

        return bot.FormatBotResponse(summerBadgeStatus ?? Langs.NetworkError);
    }

    /// <summary>
    ///     检查冬促投票 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseCheckWinterSteamAwardVote(string botNames)
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

        var results = await Utilities.InParallel(bots.Select(ResponseCheckWinterSteamAwardVote))
            .ConfigureAwait(false);
        var responses = new List<string>(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    ///     领取点数商店物品
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="steamID"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseClaimPointStoreItem(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var items = await WebRequest.QueryRewardItems(bot).ConfigureAwait(false);

        if (items?.Definitions == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        List<Task> tasks = [];

        foreach (var item in items.Definitions)
        {
            if (item.PointCost == "0")
            {
                var task = Store.WebRequest.RedeemPoints(bot, item.DefId);
                tasks.Add(task);
            }
        }

        if (tasks.Count == 0)
        {
            return bot.FormatBotResponse(Langs.NoItemToClaim);
        }

        await Utilities.InParallel(tasks).ConfigureAwait(false);

        return bot.FormatBotResponse(Langs.ClaimedPointShopItems, tasks.Count);
    }

    /// <summary>
    ///     领取点数商店物品 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseClaimPointStoreItem(string botNames)
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

        var results = await Utilities.InParallel(bots.Select(ResponseClaimPointStoreItem))
            .ConfigureAwait(false);
        var responses = new List<string>(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }
}