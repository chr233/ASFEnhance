using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Storage;
using SteamKit2;

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

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseSim4(bot))).ConfigureAwait(false);

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

        string? token = await WebRequest.FetchEventToken(bot, "dyinglight").ConfigureAwait(false);
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

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseDL2(bot))).ConfigureAwait(false);

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

        string? token = await WebRequest.FetchEventToken(bot, "Techland", "techlandsummer2023").ConfigureAwait(false);
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
            uint[] door_indexs = { 1, 3, 4, 5, 8, 7, 2, 6 };
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

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseDL22(bot, query))).ConfigureAwait(false);

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

        string? token = await WebRequest.FetchEventToken(bot, "redfall_launch").ConfigureAwait(false);
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
            uint[] door_indexs = { 1, 2, 3, 4 };
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

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseRle(bot, query))).ConfigureAwait(false);

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

        string? token = await WebRequest.FetchToken(bot).ConfigureAwait(false);

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
    /// <param name="steamID"></param>
    /// <param name="gameIDs"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseSteamEvents(Bot bot, ulong steamID, string gameIDs)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        string[] entries = gameIDs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        List<uint> intGamsIDs = new();

        foreach (string entry in entries)
        {
            if (uint.TryParse(entry, out uint choice))
            {
                intGamsIDs.Add(choice);
                if (intGamsIDs.Count >= 10)
                {
                    break;
                }
            }
        }

        if (intGamsIDs.Count < 10) //不足10个游戏自动补齐
        {
            Random rd = new();
            uint[] defaultGames = new uint[] { 1639930, 1506980, 1374480, 585020, 1639230, 1584090, 1111460, 977880, 1366540, 1398740, 1369630, 1195290 };
            while (intGamsIDs.Count < 10)
            {
                intGamsIDs.Add(defaultGames[rd.Next(defaultGames.Length)]);
            }
        }

        for (int i = 0; i < 10; i++)
        {
            int categoryID = 61 + i;
            await WebRequest.MakeVote(bot, intGamsIDs[i], categoryID).ConfigureAwait(false);
        }

        SummerBadgeResponse? summerBadgeStatus = await WebRequest.CheckSummerBadge(bot).ConfigureAwait(false);

        if (summerBadgeStatus == null)
        {
            return bot.FormatBotResponse(Langs.EventReadBadgeStatusFailed);
        }

        return bot.FormatBotResponse(Langs.EventVoteResponse, Boolen2String(summerBadgeStatus.VoteOne), Boolen2String(summerBadgeStatus.VoteAll));
    }

    /// <summary>
    /// 秋促投票 (多个Bot)
    /// </summary>
    /// <param name="steamID"></param>
    /// <param name="botNames"></param>
    /// <param name="choose"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseSteamEvents(ulong steamID, string botNames, string choose)
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

        var results = await Utilities.InParallel(bots.Select(bot => ResponseSteamEvents(bot, steamID, choose))).ConfigureAwait(false);
        var responses = new List<string>(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 检查秋促徽章
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="steamID"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseCheckSummerBadge(Bot bot, ulong steamID)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var summerBadgeStatus = await WebRequest.CheckSummerBadge(bot).ConfigureAwait(false);
        if (summerBadgeStatus == null)
        {
            return bot.FormatBotResponse(Langs.EventReadBadgeStatusFailed);
        }

        return bot.FormatBotResponse(Langs.EventCheckResponse, Boolen2String(summerBadgeStatus.VoteOne), Boolen2String(summerBadgeStatus.VoteAll), Boolen2String(summerBadgeStatus.PlayOne), Boolen2String(summerBadgeStatus.ReviewOne));
    }

    /// <summary>
    /// 秋促投票 (多个Bot)
    /// </summary>
    /// <param name="steamID"></param>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseCheckSummerBadge(ulong steamID, string botNames)
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

        var results = await Utilities.InParallel(bots.Select(bot => ResponseCheckSummerBadge(bot, steamID))).ConfigureAwait(false);
        var responses = new List<string>(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }
}
