using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;

namespace ASFEnhance.Event;

internal static class Command
{
    /// <summary>
    /// 获取SIM4贴纸 10.19 - ?
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseSim4(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        string? token = await WebRequest.FetchEventToken(bot, "simscelebrationsale").ConfigureAwait(false);
        if (string.IsNullOrEmpty(token))
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        uint[] door_indexs = { 1, 3, 4, 5, 2 };

        foreach (uint index in door_indexs)
        {
            await WebRequest.DoEventTask(bot, token, index).ConfigureAwait(false);
        }

        return bot.FormatBotResponse("Done!");
    }

    /// <summary>
    /// 获取SIM4贴纸 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseSim4(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseSim4(bot))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 获取DL2贴纸 11.11 - ?
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseDL2(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var token = await WebRequest.FetchEventToken(bot, "dyinglight").ConfigureAwait(false);
        if (string.IsNullOrEmpty(token))
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        var door_indexs = new uint[] { 1, 3, 4, 5, 2 };

        foreach (uint index in door_indexs)
        {
            await WebRequest.DoEventTask(bot, token, index).ConfigureAwait(false);
        }

        return bot.FormatBotResponse("Done!");
    }

    /// <summary>
    /// 获取DL2贴纸 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseDL2(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseDL2(bot))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 获取DL2贴纸 6.30 - ?
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseDL22(Bot bot, string? query)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var token = await WebRequest.FetchEventToken(bot, "Techland", "techlandsummer2023").ConfigureAwait(false);
        if (string.IsNullOrEmpty(token))
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        if (!string.IsNullOrEmpty(query))
        {
            if (uint.TryParse(query, out uint id))
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
            var door_indexs = new uint[] { 1, 3, 4, 5, 8, 7, 2, 6 };
            var tasks = door_indexs.Select(id => WebRequest.DoEventTask(bot, token, id));
            await Utilities.InParallel(tasks).ConfigureAwait(false);
        }

        return bot.FormatBotResponse("Done!");
    }

    /// <summary>
    /// 获取DL2贴纸 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseDL22(string botNames, string? query)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseDL22(bot, query))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }


    /// <summary>
    /// 获取RLE贴纸 5.1 - ?
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseRle(Bot bot, string? query)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var token = await WebRequest.FetchEventToken(bot, "redfall_launch").ConfigureAwait(false);
        if (string.IsNullOrEmpty(token))
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        if (!string.IsNullOrEmpty(query))
        {
            if (uint.TryParse(query, out uint id))
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
            var door_indexs = new uint[] { 1, 2, 3, 4 };
            var tasks = door_indexs.Select(id => WebRequest.DoEventTask(bot, token, id));
            await Utilities.InParallel(tasks).ConfigureAwait(false);
        }

        return bot.FormatBotResponse("Done!");
    }

    /// <summary>
    /// 获取RLE贴纸 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseRle(string botNames, string? query)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseRle(bot, query))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 领取活动道具
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseClaimItem(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var token = await WebRequest.FetchToken(bot).ConfigureAwait(false);
        if (string.IsNullOrEmpty(token))
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        await WebRequest.ClaimDailySticker(bot, token).ConfigureAwait(false);

        return bot.FormatBotResponse(Langs.Done);
    }

    /// <summary>
    /// 领取活动道具 (多个Bot)
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

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseClaimItem(bot))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 领取20周年贴纸
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseClaim20Th(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var (_, token) = await bot.ArchiWebHandler.CachedAccessToken.GetValue().ConfigureAwait(false);

        if (string.IsNullOrEmpty(token))
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        var defIds = new List<int> { 241812, 241811, 241810, 241809, 241807, 241808 };
        var results = await Utilities.InParallel(defIds.Select(id => WebRequest.RedeenPointShopItem(bot, token, id))).ConfigureAwait(false);

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
    /// 领取20周年贴纸 (多个Bot)
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

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseClaim20Th(bot))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 秋促投票
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="gameIDs"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseSteamAwardVote(Bot bot, string gameIDs)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var entries = gameIDs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        var intGamsIDs = new List<int>();

        foreach (string entry in entries)
        {
            if (int.TryParse(entry, out var choice) && choice > 0)
            {
                intGamsIDs.Add(choice);
                if (intGamsIDs.Count >= 11)
                {
                    break;
                }
            }
        }

        if (intGamsIDs.Count < 11) //不足11个游戏自动补齐
        {
            var defaultGames = new int[] { 1086940, 1922010, 1374480, 990080, 2344520, 2254740, 2411910, 1817230, 2242710, 1868140, 2194530 };
            while (intGamsIDs.Count < 11)
            {
                intGamsIDs.Add(defaultGames[intGamsIDs.Count]);
            }
        }

        var token = await WebRequest.FetchWebApiToken(bot).ConfigureAwait(false);
        if (string.IsNullOrEmpty(token))
        {
            bot.ArchiLogger.LogNullError(nameof(token));
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        var semaphore = new SemaphoreSlim(1);

        var tasks = new List<Task>();

        for (int i = 0; i < 11; i++)
        {
            tasks.Add(WebRequest.MakeVote(bot, intGamsIDs[i], 90 + i, token, semaphore));
        }

        await Utilities.InParallel(tasks).ConfigureAwait(false);

        var summerBadgeStatus = await WebRequest.CheckSummerBadge(bot).ConfigureAwait(false);
        return bot.FormatBotResponse(summerBadgeStatus);
    }

    /// <summary>
    /// 秋促投票 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="choose"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseSteamAwardVote(string botNames, string choose)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseSteamAwardVote(bot, choose))).ConfigureAwait(false);
        var responses = new List<string>(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 检查秋促徽章
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="steamID"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseCheckSteamAwardVote(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var summerBadgeStatus = await WebRequest.CheckSummerBadge(bot).ConfigureAwait(false);

        return bot.FormatBotResponse(summerBadgeStatus);
    }

    /// <summary>
    /// 秋促投票 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseCheckSteamAwardVote(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseCheckSteamAwardVote(bot))).ConfigureAwait(false);
        var responses = new List<string>(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }
}
