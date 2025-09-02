using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;

namespace ASFEnhance.Explorer;

internal static class Command
{
    /// <summary>
    /// 浏览探索队列
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseExploreDiscoveryQueue(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var appIds = await WebRequest.GetDiscoveryQueue(bot).ConfigureAwait(false);
        if (appIds == null || appIds.Count == 0)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        foreach (var appid in appIds)
        {
            await WebRequest.SkipDiscoveryQueueItem(bot, appid).ConfigureAwait(false);
            await Task.Delay(200).ConfigureAwait(false);
        }

        return bot.FormatBotResponse(Langs.DiscoveryQueueExploreredSuccess);
    }

    /// <summary>
    /// 浏览探索队列 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseExploreDiscoveryQueue(string botNames)
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

        var results = await Utilities.InParallel(bots.Select(ResponseExploreDiscoveryQueue)).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }
}