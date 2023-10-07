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

        if (CurrencyHelper.Currency2Symbol.ContainsKey(walletCurrency))
        {
            walletCurrency = CurrencyHelper.Currency2Symbol[walletCurrency];
        }

        var gameIds = FetchGameIds(query, SteamGameIdType.All, SteamGameIdType.App);

        StringBuilder response = new();
        response.AppendLine(bot.FormatBotResponse(Langs.MultipleLineResult));

        foreach (var gameId in gameIds)
        {
            if (gameId.Type != SteamGameIdType.Error)
            {
                var storeResponse = await WebRequest.GetStoreSubs(bot, gameId).ConfigureAwait(false);

                if (storeResponse == null)
                {
                    response.AppendLineFormat(Langs.StoreItemHeader, gameId, Langs.NetworkError);
                }
                else
                {
                    if (storeResponse.SubDatas.Count == 0)
                    {
                        response.AppendLineFormat(Langs.StoreItemHeader, gameId, storeResponse.GameName);
                    }
                    else
                    {
                        response.AppendLineFormat(Langs.StoreItemHeader, gameId, storeResponse.GameName);

                        foreach (var sub in storeResponse.SubDatas)
                        {
                            response.AppendLineFormat(Langs.StoreItem, sub.IsBundle ? "Bundle" : "Sub", sub.SubId, sub.Name, sub.Price / 100.0, walletCurrency));
                        }
                    }
                }
            }
            else
            {
                response.AppendLine(bot.FormatBotResponse(string.Format(Strings.ErrorIsInvalid, gameId.Input)));
            }
        }

        return response.ToString();
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

        HashSet<Bot>? bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
        }

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseGetGameSubes(bot, query))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

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

        RecommendGameResponse? response = await WebRequest.PublishReview(bot, (uint)appId, comment, rateUp, true, false).ConfigureAwait(false);

        if (response == null || !response.Result)
        {
            return bot.FormatBotResponse(string.Format(Langs.RecommendPublishFailed, response?.ErrorMsg));
        }

        return bot.FormatBotResponse(string.Format(Langs.RecommendPublishSuccess));
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

        HashSet<Bot>? bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
        }

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponsePublishReview(bot, appId, review))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

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

        StringBuilder response = new();

        string[] games = targetGameIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string game in games)
        {
            if (!uint.TryParse(game, out uint gameId) || (gameId == 0))
            {
                response.AppendLine(bot.FormatBotResponse(string.Format(Strings.ErrorIsInvalid, nameof(gameId))));
                continue;
            }

            bool result = await WebRequest.DeleteRecommend(bot, gameId).ConfigureAwait(false);

            response.AppendLine(bot.FormatBotResponse(string.Format(Strings.BotAddLicense, gameId, result ? Langs.Success : Langs.Failure)));
        }

        return response.Length > 0 ? response.ToString() : null;
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

        HashSet<Bot>? bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
        }

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseDeleteReview(bot, appId))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

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
                        response.AppendLine(string.Format(Langs.AppDetailResult, gameId.Input, Langs.FetchAppDetailFailed));
                    }
                    else
                    {
                        response.AppendLine(string.Format(Langs.AppDetailResult, gameId.Input, Langs.Success));

                        AppDetailData data = appDetail.Data;
                        response.AppendLine(string.Format(Langs.AppDetailName, data.Name));

                        string type = data.Type switch
                        {
                            "game" => Langs.AppTypeGame,
                            "music" => Langs.AppTypeMusic,
                            "dlc" => Langs.AppTypeDLC,
                            _ => data.Type,
                        };

                        response.AppendLine(string.Format(Langs.AppType, type));

                        if (data.FullGame != null)
                        {
                            response.AppendLine(string.Format(Langs.AppFullGame, data.FullGame.AppId, data.FullGame.Name));
                        }

                        response.AppendLine(string.Format(Langs.AppDevelopers, string.Join(", ", data.Developers)));
                        response.AppendLine(string.Format(Langs.AppPublishers, string.Join(", ", data.Publishers)));

                        response.AppendLine(string.Format(Langs.AppCategories, string.Join(", ", data.Categories)));
                        response.AppendLine(string.Format(Langs.AppGenres, string.Join(", ", data.Genres)));

                        response.AppendLine(string.Format(Langs.AppShortDescription, data.ShortDescription));

                        response.AppendLine(string.Format(Langs.AppSupportedPlatforms, data.Platforms.Windows ? "√" : "×", data.Platforms.Mac ? "√" : "×", data.Platforms.Linux ? "√" : "×"));

                        if (data.Recommendations != null)
                        {
                            response.AppendLine(string.Format(Langs.AppSteamRecommended, data.Recommendations.Total));
                        }

                        if (data.Metacritic != null)
                        {
                            response.AppendLine(string.Format(Langs.AppMetacriticScore, data.Metacritic.Score));
                        }

                        if (data.PackageGroups.Count == 0)
                        {
                            bool retired = data.ReleaseDate != null && !data.ReleaseDate.ComingSoon;
                            string releaseData = data.ReleaseDate?.Date ?? Langs.AccountSubUnknown;
                            response.AppendLine(string.Format(Langs.AppReleasedDate, releaseData + string.Format(Langs.AppReleasedDateEx, retired ? Langs.AppDelisted : Langs.AppComingSoon)));
                        }
                        else
                        {
                            response.AppendLine(string.Format(Langs.AppReleasedDate, data.ReleaseDate.Date));

                            if (data.PriceOverview != null)
                            {
                                if (data.PriceOverview.DiscountPercent != 0)
                                {
                                    response.AppendLine(string.Format(Langs.AppDiscount, data.PriceOverview.DiscountPercent, data.PriceOverview.FinalFormatted));
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
                                    response.AppendLine(string.Format(Langs.AppSubInfo, subId, subName));
                                }
                            }
                        }

                        if (data.Dlc?.Count > 0)
                        {
                            response.AppendLine(string.Format(Langs.AppDlcInfo, string.Join(", ", data.Dlc)));
                        }

                    }
                    break;

                default:
                    response.AppendLine(bot.FormatBotResponse(string.Format(Strings.ErrorIsInvalid, gameId.Input)));
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

        HashSet<Bot>? bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
        }

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseGetAppsDetail(bot, query))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

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

        HashSet<Bot>? bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
        }

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseSearchGame(bot, query))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

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

        StringBuilder response = new();

        string[] entries = query.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string entry in entries)
        {
            if (!ulong.TryParse(entry, out ulong appId) || (appId == 0))
            {
                response.AppendLine(bot.FormatBotResponse(string.Format(Strings.ErrorIsInvalid, nameof(appId))));
                continue;
            }

            var result = await WebRequest.RequestAccess(bot, appId).ConfigureAwait(false);

            if (result == null)
            {
                response.AppendLine(bot.FormatBotResponse(string.Format(Strings.BotAddLicense, appId, Langs.NetworkError)));
            }
            else
            {
                response.AppendLine(bot.FormatBotResponse(string.Format(Strings.BotAddLicense, appId, result.Result == EResult.OK ? Langs.Success : Langs.Failure)));
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

        HashSet<Bot>? bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
        }

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseRequestAccess(bot, query))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

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

        HashSet<Bot>? bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
        }

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseViewPage(bot, query))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }
}
