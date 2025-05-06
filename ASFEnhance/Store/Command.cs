using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Data.Common;
using ASFEnhance.Data.Plugin;
using SteamKit2;
using System.Data;
using System.Text;

namespace ASFEnhance.Store;

internal static class Command
{
    /// <summary>
    /// 获取游戏评测
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="strAppId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    internal static async Task<string?> ResponseGetReview(Bot bot, string strAppId)
    {
        if (string.IsNullOrEmpty(strAppId))
        {
            throw new ArgumentNullException(nameof(strAppId));
        }

        if (!uint.TryParse(strAppId, out var appId) || (appId == 0))
        {
            throw new ArgumentException(null, nameof(strAppId));
        }

        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var result = await WebRequest.GetReviewContent(bot, appId).ConfigureAwait(false);
        return bot.FormatBotResponse(result ?? Langs.NetworkError);
    }

    /// <summary>
    /// 获取游戏评测 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="appId"></param>
    /// <param name="review"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetReview(string botNames, string appId)
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

        var results = await Utilities.InParallel(bots.Select(bot => ResponseGetReview(bot, appId))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 发布游戏评测
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="strAppId"></param>
    /// <param name="comment"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    internal static async Task<string?> ResponsePublishReview(Bot bot, string strAppId, string comment)
    {
        if (string.IsNullOrEmpty(strAppId))
        {
            throw new ArgumentNullException(nameof(strAppId));
        }

        if (string.IsNullOrEmpty(comment))
        {
            throw new ArgumentNullException(nameof(comment));
        }

        if (!int.TryParse(strAppId, out int appId) || (appId == 0))
        {
            throw new ArgumentException(null, nameof(strAppId));
        }

        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        bool rateUp = appId > 0;
        if (!rateUp)
        {
            appId = -appId;
        }

        var response = await WebRequest.PublishReview(bot, (uint)appId, comment, rateUp, true, false).ConfigureAwait(false);

        if (response == null || !response.Result)
        {
            return bot.FormatBotResponse(Langs.RecommendPublishFailed, response?.ErrorMsg);
        }

        var reviewUri = new Uri(SteamCommunityURL, $"/profiles/{bot.SteamID}/recommended/{appId}/");

        return bot.FormatBotResponse(string.Format(Langs.AccountSubItem, Langs.RecommendPublishSuccess, reviewUri));
    }

    /// <summary>
    /// 发布游戏评测 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="appId"></param>
    /// <param name="review"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponsePublishReview(string botNames, string appId, string review)
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

        var results = await Utilities.InParallel(bots.Select(bot => ResponsePublishReview(bot, appId, review))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 删除游戏评测
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="targetGameIds"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseDeleteReview(Bot bot, string targetGameIds)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var sb = new StringBuilder();

        var games = targetGameIds.Split(',', StringSplitOptions.RemoveEmptyEntries);

        foreach (string game in games)
        {
            if (!uint.TryParse(game, out uint gameId) || (gameId == 0))
            {
                sb.AppendLine(bot.FormatBotResponse(Strings.ErrorIsInvalid, nameof(gameId)));
                continue;
            }

            bool result = await WebRequest.DeleteRecommend(bot, gameId).ConfigureAwait(false);

            sb.AppendLine(bot.FormatBotResponse(Strings.BotAddLicense, gameId, result ? Langs.Success : Langs.Failure));
        }

        return sb.ToString();
    }

    /// <summary>
    /// 删除游戏评测 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="appId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseDeleteReview(string botNames, string appId)
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

        var results = await Utilities.InParallel(bots.Select(bot => ResponseDeleteReview(bot, appId))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 读取游戏商店详情
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetAppsDetail(Bot bot, string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            throw new ArgumentNullException(nameof(query));
        }

        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var gameIds = FetchGameIds(query, ESteamGameIdType.All, ESteamGameIdType.App);

        var sb = new StringBuilder();
        sb.AppendLine(bot.FormatBotResponse(Langs.MultipleLineResult));

        var hasWarn = false;
        var items = new List<IdData>();
        foreach (var gameId in gameIds)
        {
            switch (gameId.Type)
            {
                case ESteamGameIdType.App:
                case ESteamGameIdType.Sub:
                case ESteamGameIdType.Bundle:
                    items.Add(new IdData(gameId));
                    break;
                default:
                    hasWarn = true;
                    sb.AppendLine(bot.FormatBotResponse(Langs.CartInvalidType, gameId.Input));
                    break;
            }
        }

        if (items.Count == 0)
        {
            return sb.ToString();
        }

        if (hasWarn)
        {
            sb.AppendLine();
        }

        var gameInfoResponse = await bot.GetStoreItems(items).ConfigureAwait(false);
        var storeItems = gameInfoResponse?.StoreItems;
        if (storeItems != null && storeItems.Count > 0)
        {
            foreach (var item in storeItems)
            {
                var itemPrefix = item.ItemType switch
                {
                    0 => "app",
                    1 => "sub",
                    2 => "bundle",
                    _ => item.ItemType.ToString(),
                };

                sb.AppendLineFormat(Langs.AppDetailKey, itemPrefix, item.Id);
                sb.AppendLineFormat(Langs.AppDetailName, item.Name);
                sb.AppendLineFormat(Langs.AppType, item.Type);
                sb.AppendLineFormat(Langs.AppShortDescription, item.FullDescription?[..20]);
                sb.AppendLineFormat(Langs.AppDetailFreeGame, Bool2Str(item.IsFree));

                if (item.PurchaseOptions?.Count > 0)
                {
                    sb.AppendLine(Langs.AppDetailPurchaseOption);
                    foreach (var option in item.PurchaseOptions)
                    {
                        if (option.PackageId > 0)
                        {
                            sb.AppendLineFormat(Langs.PurchaseOptionSub, option.PackageId);
                        }
                        else if (option.BundleId > 0)
                        {
                            sb.AppendLineFormat(Langs.PurchaseOptionBundle, option.BundleId);
                        }
                        else
                        {
                            continue;
                        }

                        sb.AppendLineFormat(Langs.PurchaseOptionName, option.PurchaseOptionName);
                        sb.AppendLineFormat(Langs.PurchaseOptionPrice, option.FormattedFinalPrice);
                        sb.AppendLineFormat(Langs.PurchaseOptionGameCount, option.IncludedGameCount);
                        sb.AppendLineFormat(Langs.PurchaseOptionGiftAble, option.UserCanPurchaseAsGift);
                    }
                }
                sb.AppendLine();
            }
        }
        else
        {
            sb.AppendLine(Langs.CanNotParseAnyGameInfo);
        }

        return sb.ToString();
    }

    /// <summary>
    /// 读取游戏商店详情 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetAppsDetail(string botNames, string query)
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

        var results = await Utilities.InParallel(bots.Select(bot => ResponseGetAppsDetail(bot, query))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 搜索游戏
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseSearchGame(Bot bot, string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            throw new ArgumentNullException(nameof(query));
        }

        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        string? result = await WebRequest.SearchGame(bot, query).ConfigureAwait(false);

        return result != null ? bot.FormatBotResponse(result) : null;
    }

    /// <summary>
    /// 搜索游戏 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseSearchGame(string botNames, string query)
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

        var results = await Utilities.InParallel(bots.Select(bot => ResponseSearchGame(bot, query))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 请求游戏访问权限
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseRequestAccess(Bot bot, string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            throw new ArgumentNullException(nameof(query));
        }

        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var response = new StringBuilder();

        var entries = query.Split(SeparatorDot, StringSplitOptions.RemoveEmptyEntries);

        foreach (string entry in entries)
        {
            if (!ulong.TryParse(entry, out ulong appId) || (appId == 0))
            {
                response.AppendLine(bot.FormatBotResponse(Strings.ErrorIsInvalid, nameof(appId)));
                continue;
            }

            var result = await WebRequest.RequestAccess(bot, appId).ConfigureAwait(false);

            if (result == null)
            {
                response.AppendLine(bot.FormatBotResponse(Strings.BotAddLicense, appId, Langs.NetworkError));
            }
            else
            {
                response.AppendLine(bot.FormatBotResponse(Strings.BotAddLicense, appId, result.Result == EResult.OK ? Langs.Success : Langs.Failure));
            }
        }

        return response.Length > 0 ? response.ToString() : null;
    }

    /// <summary>
    /// 请求游戏访问权限 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseRequestAccess(string botNames, string query)
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

        var results = await Utilities.InParallel(bots.Select(bot => ResponseRequestAccess(bot, query))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 浏览链接
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseViewPage(Bot bot, string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            throw new ArgumentNullException(nameof(query));
        }

        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        try
        {
            Uri request = new(query);
            string? result = await WebRequest.FetchPage(bot, request).ConfigureAwait(false);
            return bot.FormatBotResponse(result ?? Langs.NetworkError);
        }
        catch (Exception)
        {
            return bot.FormatBotResponse(Langs.ViewPageUrlNotValid);
        }
    }

    /// <summary>
    /// 浏览链接 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseViewPage(string botNames, string query)
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

        var results = await Utilities.InParallel(bots.Select(bot => ResponseViewPage(bot, query))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 购买点数徽章
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="defId"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseUnlockPointBadge(Bot bot, string defId, string level)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        if (!uint.TryParse(defId, out uint intDefId))
        {
            return bot.FormatBotResponse(string.Format(Langs.ArgumentNotInteger, nameof(defId)));
        }

        if (!uint.TryParse(level, out uint intLevel))
        {
            return bot.FormatBotResponse(string.Format(Langs.ArgumentNotInteger, nameof(level)));
        }

        var result = await WebRequest.RedeemPointsForBadgeLevel(bot, intDefId, intLevel).ConfigureAwait(false);

        return bot.FormatBotResponse(result);
    }

    /// <summary>
    /// 购买点数徽章 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="defId"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseUnlockPointBadge(string botNames, string defId, string level)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseUnlockPointBadge(bot, defId, level))).ConfigureAwait(false);

        var responses = new List<string>(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, results) : null;
    }

    /// <summary>
    /// 购买点数物品
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="defIds"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseUnlockPointItem(Bot bot, string defIds)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var sb = new StringBuilder();
        var entries = defIds.Split(SeparatorDot, StringSplitOptions.RemoveEmptyEntries);

        foreach (string entry in entries)
        {
            if (!uint.TryParse(entry, out var intDefId) || (intDefId == 0))
            {
                sb.AppendLine(bot.FormatBotResponse(Strings.ErrorIsInvalid, nameof(defIds)));
                continue;
            }

            var result = await WebRequest.RedeemPoints(bot, intDefId).ConfigureAwait(false);
            sb.AppendLine(bot.FormatBotResponse(Strings.BotAddLicense, intDefId, result ?? Langs.NetworkError));
        }

        return sb.ToString();
    }

    /// <summary>
    /// 购买点数物品 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="defIds"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseUnlockPointItem(string botNames, string defIds)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseUnlockPointItem(bot, defIds))).ConfigureAwait(false);

        var responses = new List<string>(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, results) : null;
    }
}
