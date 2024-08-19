using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Data;
using System.Text;

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
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseUnStackInventory(bot, strAppId, strContextId))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 获取待处理的礼物
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetPendingGifts(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var result = await WebRequest.GetPendingGifts(bot).ConfigureAwait(false);
        if (result == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        if (result.Count == 0)
        {
            return bot.FormatBotResponse(Langs.NoPendingGift);
        }

        var sb = new StringBuilder();
        sb.AppendLine(Langs.MultipleLineResult);

        foreach (var gift in result)
        {
            sb.AppendLineFormat(Langs.PendingGiftListDetail, gift.GiftId, gift.GameName, gift.SenderName, gift.SenderSteamId);
        }

        return bot.FormatBotResponse(sb.ToString());
    }

    /// <summary>
    /// 获取待处理的礼物 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetPendingGifts(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
            throw new ArgumentNullException(nameof(botNames));

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseGetPendingGifts(bot))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 接受指定礼物
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="strGiftIds"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseAcceptGift(Bot bot, string strGiftIds)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var giftResponse = await WebRequest.GetPendingGifts(bot).ConfigureAwait(false);
        if (giftResponse == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        if (giftResponse.Count == 0)
        {
            return bot.FormatBotResponse(Langs.NoPendingGift);
        }

        List<Task<string>> tasks = [];

        if (strGiftIds == "*")
        {
            foreach (var gift in giftResponse)
            {
                tasks.Add(WebRequest.AcceptGift(bot, gift.GiftId));
            }
        }
        else
        {
            var querys = strGiftIds.Split(SeparatorDotSpace);
            foreach (var gift in giftResponse)
            {
                foreach (var query in querys)
                {
                    if (gift.GiftId.ToString() == query)
                    {
                        tasks.Add(WebRequest.AcceptGift(bot, gift.GiftId));
                    }
                }
            }
        }

        if (tasks.Count == 0)
        {
            return bot.FormatBotResponse(Langs.SpecifyPendingGiftNotFound);
        }

        var results = await Utilities.InParallel(tasks).ConfigureAwait(false);

        var sb = new StringBuilder();
        sb.AppendLine(Langs.MultipleLineResult);

        foreach (var resulr in results)
        {
            sb.AppendLine(resulr);
        }

        return bot.FormatBotResponse(sb.ToString());
    }

    /// <summary>
    /// 接受指定礼物 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="strGiftIds"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseAcceptGift(string botNames, string strGiftIds)
    {
        if (string.IsNullOrEmpty(botNames))
            throw new ArgumentNullException(nameof(botNames));

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
            return FormatStaticResponse(Strings.BotNotFound, botNames);

        var results = await Utilities.InParallel(bots.Select(bot => ResponseAcceptGift(bot, strGiftIds))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 接受指定礼物
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="strGiftIds"></param>
    /// <param name="reason"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseDeclinetGift(Bot bot, string strGiftIds, string? reason)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var giftResponse = await WebRequest.GetPendingGifts(bot).ConfigureAwait(false);
        if (giftResponse == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        if (giftResponse.Count == 0)
        {
            return bot.FormatBotResponse(Langs.NoPendingGift);
        }

        List<Task<string>> tasks = [];

        if (strGiftIds == "*")
        {
            foreach (var gift in giftResponse)
            {
                tasks.Add(WebRequest.DeclineGift(bot, gift.GiftId, gift.SenderSteamId, reason));
            }
        }
        else
        {
            var querys = strGiftIds.Split(SeparatorDotSpace);
            foreach (var gift in giftResponse)
            {
                foreach (var query in querys)
                {
                    if (gift.GiftId.ToString() == query)
                    {
                        tasks.Add(WebRequest.DeclineGift(bot, gift.GiftId, gift.SenderSteamId, reason));
                    }
                }
            }
        }

        if (tasks.Count == 0)
        {
            return bot.FormatBotResponse(Langs.SpecifyPendingGiftNotFound);
        }

        var results = await Utilities.InParallel(tasks).ConfigureAwait(false);

        var sb = new StringBuilder();
        sb.AppendLine(Langs.MultipleLineResult);

        foreach (var resulr in results)
        {
            sb.AppendLine(resulr);
        }

        return bot.FormatBotResponse(sb.ToString());
    }

    /// <summary>
    /// 接受指定礼物 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="strGiftIds"></param>
    /// <param name="reason"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseDeclinetGift(string botNames, string strGiftIds, string? reason)
    {
        if (string.IsNullOrEmpty(botNames))
            throw new ArgumentNullException(nameof(botNames));

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
            return FormatStaticResponse(Strings.BotNotFound, botNames);

        var results = await Utilities.InParallel(bots.Select(bot => ResponseDeclinetGift(bot, strGiftIds, reason))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }
}
