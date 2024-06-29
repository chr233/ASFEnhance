using ArchiSteamFarm.Core;
using ArchiSteamFarm.Helpers.Json;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Storage;
using System.Text.Json;

namespace ASFEnhance.Explorer;

internal static class Command
{
    /// <summary>
    /// 浏览探索队列
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static string? ResponseExploreDiscoveryQueue(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        if (!bot.BotConfig.FarmingPreferences.HasFlag(BotConfig.EFarmingPreferences.AutoSteamSaleEvent))
        {
            return bot.FormatBotResponse(Langs.SteamSaleEventIsNull2);
        }

        var steamSaleEvent = Type.GetType("ArchiSteamFarm.Steam.Integration.SteamSaleEvent,ArchiSteamFarm");

        if (steamSaleEvent == null)
        {
            return bot.FormatBotResponse(Langs.SteamSaleEventIsNull2);
        }

        var steamSaleEventCls = bot.GetPrivateField("SteamSaleEvent", steamSaleEvent);

        if (steamSaleEventCls == null)
        {
            return bot.FormatBotResponse(Langs.SteamSaleEventIsNull2);
        }

        var saleEventTimer = steamSaleEventCls.GetPrivateField<Timer>("SaleEventTimer");

        saleEventTimer?.Change(TimeSpan.FromSeconds(5), TimeSpan.FromHours(8.1));

        return bot.FormatBotResponse(Langs.ExplorerStart);
    }

    /// <summary>
    /// 浏览探索队列
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="semaphore"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseExploreDiscoveryQueue(Bot bot, SemaphoreSlim semaphore)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        await semaphore.WaitAsync().ConfigureAwait(false);

        try
        {
            var steamSaleEvent = Type.GetType("ArchiSteamFarm.Steam.Integration.SteamSaleEvent,ArchiSteamFarm");
            if (steamSaleEvent == null)
            {
                return bot.FormatBotResponse(Langs.SteamSaleEventIsNull2);
            }
            var steamSaleEventCls = bot.GetPrivateField("SteamSaleEvent", steamSaleEvent);
            var saleEventTimer = steamSaleEventCls?.GetPrivateField<Timer>("SaleEventTimer");
            saleEventTimer?.Change(TimeSpan.FromSeconds(5), TimeSpan.FromHours(8.1));
            return bot.FormatBotResponse(Langs.ExplorerStart);
        }
        finally
        {
            await Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(false);
            semaphore.Release();
        }
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

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var semaphore = new SemaphoreSlim(1);

        var results = await Utilities.InParallel(bots.Select(bot => ResponseExploreDiscoveryQueue(bot, semaphore))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }


    /// <summary>
    /// 开启AutoSteamSaleEvent
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseEnableAutoSteamSaleEvent(Bot bot)
    {
        if (bot.BotConfig.FarmingPreferences.HasFlag(BotConfig.EFarmingPreferences.AutoSteamSaleEvent))
        {
            return bot.FormatBotResponse(Langs.AlreadyEnabledAutoSteamSaleEvent);
        }

        var filePath = Bot.GetFilePath(bot.BotName, Bot.EFileType.Config);
        if (string.IsNullOrEmpty(filePath))
        {
            return bot.FormatBotResponse(Langs.ConfigFilePathNotFound);
        }

        try
        {
            var currentJson = await File.ReadAllTextAsync(filePath).ConfigureAwait(false);
            if (string.IsNullOrEmpty(currentJson))
            {
                return bot.FormatBotResponse(Langs.ReadConfigContentFailed);
            }

            var newValue = bot.BotConfig.FarmingPreferences | BotConfig.EFarmingPreferences.AutoSteamSaleEvent;

            var jsonObject = JsonSerializer.Deserialize<Dictionary<string, object>>(currentJson, JsonUtilities.DefaultJsonSerialierOptions);
            if (jsonObject != null)
            {
                if (jsonObject.ContainsKey("FarmingPreferences"))
                {
                    jsonObject["FarmingPreferences"] = newValue;
                }
                else
                {
                    jsonObject.Add("FarmingPreferences", newValue);
                }

                currentJson = JsonSerializer.Serialize(jsonObject, JsonUtilities.IndentedJsonSerialierOptions);
            }

            await File.WriteAllTextAsync(filePath, currentJson).ConfigureAwait(false);
            return bot.FormatBotResponse(Langs.UpdateConfigContentSuccess);
        }
        catch (Exception ex)
        {
            ASFLogger.LogGenericException(ex);
            return bot.FormatBotResponse(Langs.RwConfigContentError);
        }
    }

    /// <summary>
    /// 开启AutoSteamSaleEvent (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseEnableAutoSteamSaleEvent(string botNames)
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

        var semaphore = new SemaphoreSlim(1);

        var results = await Utilities.InParallel(bots.Select(bot => ResponseEnableAutoSteamSaleEvent(bot))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }
}
