using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Account;
using ASFEnhance.Data;
using SteamKit2;
using System.Text;

namespace ASFEnhance.Store;

internal static class Command
{
    /// <summary>
    /// 读取游戏的商店可用Sub
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetGameSubes(Bot bot, string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            throw new ArgumentNullException(nameof(query));
        }

        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        string walletCurrency = bot.WalletCurrency != ECurrencyCode.Invalid ? bot.WalletCurrency.ToString() : Langs.WalletAreaUnknown;

        if (CurrencyHelper.Currency2Symbol.TryGetValue(walletCurrency, out var currency) && !string.IsNullOrEmpty(currency))
        {
            walletCurrency = currency;
        }

        var gameIds = FetchGameIds(query, SteamGameIdType.All, SteamGameIdType.App);

        var sb = new StringBuilder();
        sb.AppendLine(bot.FormatBotResponse(Langs.MultipleLineResult));

        foreach (var gameId in gameIds)
        {
            if (gameId.Type != SteamGameIdType.Error)
            {
                var storeResponse = await WebRequest.GetStoreSubs(bot, gameId).ConfigureAwait(false);

                if (storeResponse == null)
                {
                    sb.AppendLineFormat(Langs.StoreItemHeader, gameId, Langs.NetworkError);
                }
                else
                {
                    if (storeResponse.SubDatas.Count == 0)
                    {
                        sb.AppendLineFormat(Langs.StoreItemHeader, gameId, storeResponse.GameName);
                    }
                    else
                    {
                        sb.AppendLineFormat(Langs.StoreItemHeader, gameId, storeResponse.GameName);

                        foreach (var sub in storeResponse.SubDatas)
                        {
                            sb.AppendLineFormat(Langs.StoreItem, sub.IsBundle ? "Bundle" : "Sub", sub.SubId, sub.Name, sub.Price / 100.0, walletCurrency);
                        }
                    }
                }
            }
            else
            {
                sb.AppendLine(bot.FormatBotResponse(Strings.ErrorIsInvalid, gameId.Input));
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// 读取游戏的商店可用Sub (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetGameSubes(string botNames, string query)
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

        var results = await Utilities.InParallel(bots.Select(bot => ResponseGetGameSubes(bot, query))).ConfigureAwait(false);

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

        return bot.FormatBotResponse(Langs.RecommendPublishSuccess);
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

        var gameIds = FetchGameIds(query, SteamGameIdType.App, SteamGameIdType.App);

        StringBuilder response = new();
        response.AppendLine(bot.FormatBotResponse(Langs.MultipleLineResult));

        foreach (var gameId in gameIds)
        {
            if (response.Length != 0) { response.AppendLine(); }

            switch (gameId.Type)
            {
                case SteamGameIdType.App:

                    AppDetailResponse? appDetail = await WebRequest.GetAppDetails(bot, gameId.GameId).ConfigureAwait(false);

                    if (appDetail == null || !appDetail.Success)
                    {
                        response.AppendLineFormat(Langs.AppDetailResult, gameId.Input, Langs.FetchAppDetailFailed);
                    }
                    else
                    {
                        response.AppendLineFormat(Langs.AppDetailResult, gameId.Input, Langs.Success);

                        AppDetailData data = appDetail.Data;
                        response.AppendLineFormat(Langs.AppDetailName, data.Name);

                        string type = data.Type switch
                        {
                            "game" => Langs.AppTypeGame,
                            "music" => Langs.AppTypeMusic,
                            "dlc" => Langs.AppTypeDLC,
                            _ => data.Type,
                        };

                        response.AppendLineFormat(Langs.AppType, type);

                        if (data.FullGame != null)
                        {
                            response.AppendLineFormat(Langs.AppFullGame, data.FullGame.AppId, data.FullGame.Name);
                        }

                        response.AppendLineFormat(Langs.AppDevelopers, string.Join(", ", data.Developers));
                        response.AppendLineFormat(Langs.AppPublishers, string.Join(", ", data.Publishers));

                        response.AppendLineFormat(Langs.AppCategories, string.Join(", ", data.Categories));
                        response.AppendLineFormat(Langs.AppGenres, string.Join(", ", data.Genres));

                        response.AppendLineFormat(Langs.AppShortDescription, data.ShortDescription);

                        response.AppendLineFormat(Langs.AppSupportedPlatforms, data.Platforms.Windows.ToStr(), data.Platforms.Mac.ToStr(), data.Platforms.Linux.ToStr());

                        if (data.Recommendations != null)
                        {
                            response.AppendLineFormat(Langs.AppSteamRecommended, data.Recommendations.Total);
                        }

                        if (data.Metacritic != null)
                        {
                            response.AppendLineFormat(Langs.AppMetacriticScore, data.Metacritic.Score);
                        }

                        if (data.PackageGroups.Count == 0)
                        {
                            bool retired = data.ReleaseDate != null && !data.ReleaseDate.ComingSoon;
                            string releaseData = data.ReleaseDate?.Date ?? Langs.AccountSubUnknown;
                            response.AppendLineFormat(Langs.AppReleasedDate, releaseData + string.Format(Langs.AppReleasedDateEx, retired ? Langs.AppDelisted : Langs.AppComingSoon));
                        }
                        else
                        {
                            response.AppendLineFormat(Langs.AppReleasedDate, data.ReleaseDate.Date);

                            if (data.PriceOverview != null)
                            {
                                if (data.PriceOverview.DiscountPercent != 0)
                                {
                                    response.AppendLineFormat(Langs.AppDiscount, data.PriceOverview.DiscountPercent, data.PriceOverview.FinalFormatted);
                                }
                                else
                                {
                                    response.AppendLine(Langs.AppNoDiscount);
                                }

                            }

                            var packageGrooup = data.PackageGroups.First();

                            if (packageGrooup?.Subs?.Count > 0)
                            {
                                foreach (var sub in packageGrooup.Subs)
                                {
                                    uint subId = sub.SubId;
                                    string subName = sub.OptionText;
                                    response.AppendLineFormat(Langs.AppSubInfo, subId, subName);
                                }
                            }
                        }

                        if (data.Dlc?.Count > 0)
                        {
                            response.AppendLineFormat(Langs.AppDlcInfo, string.Join(", ", data.Dlc));
                        }

                    }
                    break;

                default:
                    response.AppendLine(bot.FormatBotResponse(Strings.ErrorIsInvalid, gameId.Input));
                    break;
            }
        }
        return response.Length > 0 ? response.ToString() : null;
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
