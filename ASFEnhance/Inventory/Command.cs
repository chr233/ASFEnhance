using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Data;

namespace ASFEnhance.Inventory;

internal static class Command
{
    /// <summary>
    /// 堆叠物品
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="strAppId"></param>
    /// <param name="strContextId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseStackInventory(Bot bot, string strAppId, string strContextId)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        if (!uint.TryParse(strAppId, out var appId))
        {
            return bot.FormatBotResponse(Langs.ArgumentNotInteger, nameof(appId));
        }

        if (!uint.TryParse(strContextId, out var contextId))
        {
            return bot.FormatBotResponse(Langs.ArgumentNotInteger, nameof(contextId));
        }

        var inventory = await bot.ArchiWebHandler.GetInventoryAsync(bot.SteamID, appId, contextId).ToListAsync().ConfigureAwait(false);
        var itemGroup = new Dictionary<ulong, List<Asset>>();

        foreach (var item in inventory)
        {
            if (!itemGroup.TryGetValue(item.ClassID, out var list))
            {
                list = [];
                itemGroup.Add(item.ClassID, list);
            }

            list.Add(item);
        }

        uint stackedCount = 0;
        foreach (var list in itemGroup.Values)
        {
            if (list.Count <= 1)
            {
                continue;
            }
            else
            {
                for (int i = 1; i < list.Count; i++)
                {
                    await WebRequest.CombineItemStacks(bot, appId, list[i].AssetID, list[0].AssetID, list[i].Amount).ConfigureAwait(false);
                    await Task.Delay(500).ConfigureAwait(false);
                    stackedCount++;
                }
            }
        }

        return bot.FormatBotResponse(Langs.StackedSuccess, stackedCount);
    }

    /// <summary>
    /// 堆叠物品 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="strAppId"></param>
    /// <param name="strContextId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseStackInventory(string botNames, string strAppId, string strContextId)
    {
        if (string.IsNullOrEmpty(botNames))
            throw new ArgumentNullException(nameof(botNames));

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
            return FormatStaticResponse(Strings.BotNotFound, botNames);

        var results = await Utilities.InParallel(bots.Select(bot => ResponseStackInventory(bot, strAppId, strContextId))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 取消堆叠物品
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="strAppId"></param>
    /// <param name="strContextId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseUnStackInventory(Bot bot, string strAppId, string strContextId)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        if (!uint.TryParse(strAppId, out var appId))
        {
            return bot.FormatBotResponse(Langs.ArgumentNotInteger, nameof(appId));
        }

        if (!uint.TryParse(strContextId, out var contextId))
        {
            return bot.FormatBotResponse(Langs.ArgumentNotInteger, nameof(contextId));
        }

        var inventory = await bot.ArchiWebHandler.GetInventoryAsync(bot.SteamID, appId, contextId).ToListAsync().ConfigureAwait(false);
        var itemGroup = new List<Asset>();

        foreach (var item in inventory)
        {
            if (item.Amount > 1)
            {
                itemGroup.Add(item);
            }
        }

        uint unStackedCount = 0;
        foreach (var item in itemGroup)
        {
            if (item.Amount <= 1)
            {
                continue;
            }
            else
            {
                for (int i = 1; i < item.Amount; i++)
                {
                    await WebRequest.SplitItemStack(bot, appId, item.AssetID, 1).ConfigureAwait(false);
                    await Task.Delay(500).ConfigureAwait(false);
                    unStackedCount++;
                }
            }
        }

        return bot.FormatBotResponse(Langs.UnStackedSuccess, unStackedCount);
    }

    /// <summary>
    /// 取消堆叠物品 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="strAppId"></param>
    /// <param name="strContextId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseUnStackInventory(string botNames, string strAppId, string strContextId)
    {
        if (string.IsNullOrEmpty(botNames))
            throw new ArgumentNullException(nameof(botNames));

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
            return FormatStaticResponse(Strings.BotNotFound, botNames);

        var results = await Utilities.InParallel(bots.Select(bot => ResponseUnStackInventory(bot, strAppId, strContextId))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }
}
