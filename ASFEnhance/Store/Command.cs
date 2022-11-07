#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Account;
using ASFEnhance.Data;
using ASFEnhance.Localization;
using SteamKit2;
using System.Text;
using static ASFEnhance.Utils;

namespace ASFEnhance.Store
{
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

            var gameIDs = FetchGameIDs(query, SteamGameIDType.All, SteamGameIDType.App);

            StringBuilder response = new();
            response.AppendLine(bot.FormatBotResponse(Langs.MultipleLineResult));

            foreach (var gameID in gameIDs)
            {
                if (gameID.Type != SteamGameIDType.Error)
                {
                    GameStorePageResponse? storeResponse = await WebRequest.GetStoreSubs(bot, gameID).ConfigureAwait(false);

                    if (storeResponse.SubDatas.Count == 0)
                    {
                        response.AppendLine(string.Format(Langs.StoreItemHeader, gameID, storeResponse.GameName));
                    }
                    else
                    {
                        response.AppendLine(string.Format(Langs.StoreItemHeader, gameID, storeResponse.GameName));

                        foreach (var sub in storeResponse.SubDatas)
                        {
                            response.AppendLine(string.Format(Langs.StoreItem, sub.IsBundle ? "Bundle" : "Sub", sub.SubID, sub.Name, sub.Price / 100.0, walletCurrency));
                        }
                    }
                }
                else
                {
                    response.AppendLine(bot.FormatBotResponse(string.Format(Strings.ErrorIsInvalid, gameID.Input)));
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
        /// <param name="appID"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        internal static async Task<string?> ResponsePublishReview(Bot bot, string appID, string comment)
        {
            if (string.IsNullOrEmpty(appID))
            {
                throw new ArgumentNullException(nameof(appID));
            }

            if (string.IsNullOrEmpty(comment))
            {
                throw new ArgumentNullException(nameof(comment));
            }

            if (!int.TryParse(appID, out int gameID) || (gameID == 0))
            {
                throw new ArgumentException(null, nameof(appID));
            }

            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            bool rateUp = gameID > 0;

            RecommendGameResponse? response = await WebRequest.PublishReview(bot, (uint)gameID, comment, rateUp, true, false).ConfigureAwait(false);

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
        /// <param name="appID"></param>
        /// <param name="review"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponsePublishReview(string botNames, string appID, string review)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponsePublishReview(bot, appID, review))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 删除游戏评测
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="targetGameIDs"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseDeleteReview(Bot bot, string targetGameIDs)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            StringBuilder response = new();

            string[] games = targetGameIDs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string game in games)
            {
                if (!uint.TryParse(game, out uint gameID) || (gameID == 0))
                {
                    response.AppendLine(bot.FormatBotResponse(string.Format(Strings.ErrorIsInvalid, nameof(gameID))));
                    continue;
                }

                bool result = await WebRequest.DeleteRecommend(bot, gameID).ConfigureAwait(false);

                response.AppendLine(bot.FormatBotResponse(string.Format(Strings.BotAddLicense, gameID, result ? Langs.Success : Langs.Failure)));
            }

            return response.Length > 0 ? response.ToString() : null;
        }

        /// <summary>
        /// 删除游戏评测 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="appID"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseDeleteReview(string botNames, string appID)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseDeleteReview(bot, appID))).ConfigureAwait(false);

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

            var gameIDs = FetchGameIDs(query, SteamGameIDType.App, SteamGameIDType.App);

            StringBuilder response = new();
            response.AppendLine(bot.FormatBotResponse(Langs.MultipleLineResult));

            foreach (var gameID in gameIDs)
            {
                if (response.Length != 0) { response.AppendLine(); }

                switch (gameID.Type)
                {
                    case SteamGameIDType.App:

                        AppDetailResponse? appDetail = await WebRequest.GetAppDetails(bot, gameID.GameID).ConfigureAwait(false);

                        if (appDetail == null || !appDetail.Success)
                        {
                            response.AppendLine(string.Format(Langs.AppDetailResult, gameID.Input, Langs.FetchAppDetailFailed));
                        }
                        else
                        {
                            response.AppendLine(string.Format(Langs.AppDetailResult, gameID.Input, Langs.Success));

                            AppDetailData data = appDetail.Data;
                            response.AppendLine(string.Format(Langs.AppDetailName, data.Name));

                            string type = data.Type switch {
                                "game" => Langs.AppTypeGame,
                                "music" => Langs.AppTypeMusic,
                                "dlc" => Langs.AppTypeDLC,
                                _ => data.Type,
                            };

                            response.AppendLine(string.Format(Langs.AppType, type));

                            if (data.FullGame != null)
                            {
                                response.AppendLine(string.Format(Langs.AppFullGame, data.FullGame.AppID, data.FullGame.Name));
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

                                response.AppendLine(string.Format(Langs.AppReleasedDate, data.ReleaseDate.Date + string.Format(Langs.AppReleasedDateEx, retired ? Langs.AppDelisted : Langs.AppComingSoon)));
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

                                foreach (var sub in packageGrooup.Subs)
                                {
                                    uint subID = sub.SubID;
                                    string subName = sub.OptionText;
                                    response.AppendLine(string.Format(Langs.AppSubInfo, subID, subName));
                                }
                            }

                            if (data.Dlc?.Count > 0)
                            {
                                response.AppendLine(string.Format(Langs.AppDlcInfo, string.Join(", ", data.Dlc)));
                            }

                        }
                        break;

                    default:
                        response.AppendLine(bot.FormatBotResponse(string.Format(Strings.ErrorIsInvalid, gameID.Input)));
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
                if (!ulong.TryParse(entry, out ulong appID) || (appID == 0))
                {
                    response.AppendLine(bot.FormatBotResponse(string.Format(Strings.ErrorIsInvalid, nameof(appID))));
                    continue;
                }

                var result = await WebRequest.RequestAccess(bot, appID).ConfigureAwait(false);

                if (result == null)
                {
                    response.AppendLine(bot.FormatBotResponse(string.Format(Strings.BotAddLicense, appID, Langs.NetworkError)));
                }
                else
                {
                    response.AppendLine(bot.FormatBotResponse(string.Format(Strings.BotAddLicense, appID, result.Result == EResult.OK ? Langs.Success : Langs.Failure)));
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
    }
}
