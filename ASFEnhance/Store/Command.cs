#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Data;
using ASFEnhance.Localization;
using SteamKit2;
using System.Text;
using static ASFEnhance.Store.Response;
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

            string walletCurrency = bot.WalletCurrency != ECurrencyCode.Invalid ? bot.WalletCurrency.ToString() : string.Format(CurrentCulture, Langs.WalletAreaUnknown);

            Dictionary<string, SteamGameID> gameIDs = FetchGameIDs(query, SteamGameIDType.App);

            StringBuilder response = new();

            foreach (KeyValuePair<string, SteamGameID> item in gameIDs)
            {
                if (response.Length != 0) { response.AppendLine(); }

                string input = item.Key;
                SteamGameID gameID = item.Value;

                switch (gameID.Type)
                {
                    case SteamGameIDType.App:
                    case SteamGameIDType.Sub:
                    case SteamGameIDType.Bundle:

                        string type = gameID.Type.ToString();

                        GameStorePageResponse? storeResponse = await WebRequest.GetStoreSubs(bot, gameID).ConfigureAwait(false);

                        if (storeResponse.SubDatas.Count == 0)
                        {
                            response.AppendLine(bot.FormatBotResponse(string.Format(CurrentCulture, Langs.StoreItemHeader, type, gameID, storeResponse.GameName)));
                        }
                        else
                        {
                            response.AppendLine(bot.FormatBotResponse(string.Format(CurrentCulture, Langs.StoreItemHeader, type, gameID, storeResponse.GameName)));

                            foreach (SingleSubData sub in storeResponse.SubDatas)
                            {
                                response.AppendLine(string.Format(CurrentCulture, Langs.StoreItem, sub.IsBundle ? "Bundle" : "Sub", sub.SubID, sub.Name, sub.Price / 100.0, walletCurrency));
                            }
                        }
                        break;

                    default:
                        response.AppendLine(bot.FormatBotResponse(string.Format(CurrentCulture, Strings.ErrorIsInvalid, input)));
                        break;
                }
            }
            return response.Length > 0 ? response.ToString() : null;
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
                return FormatStaticResponse(string.Format(CurrentCulture, Strings.BotNotFound, botNames));
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
                return bot.FormatBotResponse(string.Format(CurrentCulture, Langs.RecommendPublishFailed, response?.ErrorMsg));
            }

            return bot.FormatBotResponse(string.Format(CurrentCulture, Langs.RecommendPublishSuccess));
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
                return FormatStaticResponse(string.Format(CurrentCulture, Strings.BotNotFound, botNames));
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponsePublishReview(bot, appID, review))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        // TODO
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
                    response.AppendLine(bot.FormatBotResponse(string.Format(CurrentCulture, Strings.ErrorIsInvalid, nameof(gameID))));
                    continue;
                }

                bool result = await WebRequest.DeleteRecommend(bot, gameID).ConfigureAwait(false);

                response.AppendLine(bot.FormatBotResponse(string.Format(CurrentCulture, Strings.BotAddLicense, gameID, result ? Langs.Success : Langs.Failure)));
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
                return FormatStaticResponse(string.Format(CurrentCulture, Strings.BotNotFound, botNames));
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

            Dictionary<string, SteamGameID> gameIDs = FetchGameIDs(query, SteamGameIDType.App);

            StringBuilder response = new();

            foreach (KeyValuePair<string, SteamGameID> item in gameIDs)
            {
                if (response.Length != 0) { response.AppendLine(); }

                string input = item.Key;
                SteamGameID gameID = item.Value;

                switch (gameID.Type)
                {
                    case SteamGameIDType.App:

                        AppDetailResponse? appDetail = await WebRequest.GetAppDetails(bot, gameID.GameID).ConfigureAwait(false);

                        if (appDetail == null || !appDetail.Success)
                        {
                            response.AppendLine(bot.FormatBotResponse(string.Format(CurrentCulture, Langs.AppDetailResult, input, Langs.FetchAppDetailFailed)));
                        }
                        else
                        {
                            response.AppendLine(bot.FormatBotResponse(string.Format(CurrentCulture, Langs.AppDetailResult, input, Langs.Success)));

                            AppDetailData data = appDetail.Data;
                            response.AppendLine(string.Format(CurrentCulture, "名称: {0}", data.Name));

                            string type = data.Type switch {
                                "game" => Langs.AppTypeGame,
                                "music" => Langs.AppTypeMusic,
                                "dlc" => Langs.AppTypeDLC,
                                _ => data.Type,
                            };

                            response.AppendLine(string.Format(CurrentCulture, Langs.AppType, type));

                            if (data.FullGame != null)
                            {
                                response.AppendLine(string.Format(CurrentCulture, Langs.AppFullGame, data.FullGame.AppID, data.FullGame.Name));
                            }

                            response.AppendLine(string.Format(CurrentCulture, Langs.AppDevelopers, string.Join(", ", data.Developers)));
                            response.AppendLine(string.Format(CurrentCulture, Langs.AppPublishers, string.Join(", ", data.Publishers)));

                            response.AppendLine(string.Format(CurrentCulture, Langs.AppCategories, string.Join(", ", data.Categories)));
                            response.AppendLine(string.Format(CurrentCulture, Langs.AppGenres, string.Join(", ", data.Genres)));

                            response.AppendLine(string.Format(CurrentCulture, Langs.AppShortDescription, data.ShortDescription));

                            response.AppendLine(string.Format(CurrentCulture, Langs.AppSupportedPlatforms, data.Platforms.Windows ? "√" : "×", data.Platforms.Mac ? "√" : "×", data.Platforms.Linux ? "√" : "×"));

                            if (data.Recommendations != null)
                            {
                                response.AppendLine(string.Format(CurrentCulture, Langs.AppSteamRecommended, data.Recommendations.Total));
                            }

                            if (data.Metacritic != null)
                            {
                                response.AppendLine(string.Format(CurrentCulture, Langs.AppMetacriticScore, data.Metacritic.Score));
                            }

                            if (data.PackageGroups.Count == 0)
                            {
                                bool retired = data.ReleaseDate != null && !data.ReleaseDate.ComingSoon;

                                response.AppendLine(string.Format(CurrentCulture, Langs.AppReleasedDate, data.ReleaseDate.Date + string.Format(Langs.AppReleasedDateEx, retired ? Langs.AppDelisted : Langs.AppComingSoon)));
                            }
                            else
                            {
                                response.AppendLine(string.Format(CurrentCulture, Langs.AppReleasedDate, data.ReleaseDate.Date));

                                if (data.PriceOverview != null)
                                {
                                    if (data.PriceOverview.DiscountPercent != 0)
                                    {
                                        response.AppendLine(string.Format(CurrentCulture, Langs.AppDiscount, data.PriceOverview.DiscountPercent, data.PriceOverview.FinalFormatted));
                                    }
                                    else
                                    {
                                        response.AppendLine(string.Format(CurrentCulture, Langs.AppNoDiscount));
                                    }

                                }

                                PackageGroupsData packageGrooup = data.PackageGroups.First();

                                foreach (SubData sub in packageGrooup.Subs)
                                {
                                    uint subID = sub.SubID;
                                    string subName = sub.OptionText;
                                    response.AppendLine(string.Format(CurrentCulture, Langs.AppSubInfo, subID, subName));
                                }
                            }

                            if (data.Dlc?.Count > 0)
                            {
                                response.AppendLine(string.Format(CurrentCulture, Langs.AppDlcInfo, string.Join(", ", data.Dlc)));
                            }

                        }
                        break;

                    default:
                        response.AppendLine(bot.FormatBotResponse(string.Format(CurrentCulture, Strings.ErrorIsInvalid, input)));
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
                return FormatStaticResponse(string.Format(CurrentCulture, Strings.BotNotFound, botNames));
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
                return FormatStaticResponse(string.Format(CurrentCulture, Strings.BotNotFound, botNames));
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseSearchGame(bot, query))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

    }
}
