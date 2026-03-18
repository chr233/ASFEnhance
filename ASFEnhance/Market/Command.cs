using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using SteamKit2;
using System.Text;

namespace ASFEnhance.Market;

internal static class Command
{
    /// <summary>
    /// 获取市场商品信息
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="marketUrl"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetMarketInfo(Bot bot, string marketUrl)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var match = RegexUtils.MatchMarketUrl().Match(marketUrl);
        if (!match.Success)
        {
            return bot.FormatBotResponse(Langs.MarketUrlInvalid);
        }

        var appId = match.Groups[1].Value;
        var marketHash = match.Groups[2].Value;

        var baseInfo = await WebRequest.GetMarketInfo(bot, appId, marketHash).ConfigureAwait(false);

        if (baseInfo == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        var detail = await WebRequest.GetMarketPriceInfo(bot, baseInfo.ItemId, bot.GetUserCountryCode(), bot.WalletCurrency).ConfigureAwait(false);

        var sb = new StringBuilder();

        sb.AppendLine(Langs.MultipleLineResult);
        sb.AppendLineFormat(Langs.MarketItemName, baseInfo.Name);
        sb.AppendLineFormat(Langs.MarketItemId, baseInfo.AppId, baseInfo.ItemId);

        if (detail?.SellInfoList != null)
        {
            var price = detail.SellInfoList.FirstOrDefault();
            sb.AppendLineFormat(Langs.LowestSellPrice, price?.Price, detail.PricePrefix, detail.PriceSuffix);
        }
        if (detail?.BuyInfoList != null)
        {
            var price = detail.BuyInfoList.FirstOrDefault();
            sb.AppendLineFormat(Langs.HighestBuyPrice, price?.Price, detail.PricePrefix, detail.PriceSuffix);
        }

        return bot.FormatBotResponse(sb.ToString());
    }

    /// <summary>
    /// 获取市场商品信息 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="marketUrl"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetMarketInfo(string botNames, string marketUrl)
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

        var results = await Utilities.InParallel(bots.Select(x => ResponseGetMarketInfo(x, marketUrl))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 创建市场求购订单
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="marketUrl"></param>
    /// <param name="strPrice"></param>
    /// <param name="strAmount"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseBuyMarketItem(Bot bot, string marketUrl, string strPrice, string strAmount)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var match = RegexUtils.MatchMarketUrl().Match(marketUrl);
        if (!match.Success)
        {
            return bot.FormatBotResponse(Langs.MarketUrlInvalid);
        }

        if (!decimal.TryParse(strPrice, out var price) || price <= 0)
        {
            return bot.FormatBotResponse(Langs.BuyPriceInvalid);
        }

        if (!int.TryParse(strAmount, out var amount) || amount <= 0)
        {
            return bot.FormatBotResponse(Langs.BuyAmountInvalid);
        }

        var appId = match.Groups[1].Value;
        var marketHash = match.Groups[2].Value;

        var baseInfo = await WebRequest.GetMarketInfo(bot, appId, marketHash).ConfigureAwait(false);

        if (string.IsNullOrEmpty(baseInfo?.HashName))
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        var buyResponse = await WebRequest.CreateBuyOrder(bot, appId, baseInfo.HashName, price, amount, bot.WalletCurrency, null).ConfigureAwait(false);

        if (buyResponse == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        if (buyResponse.Success == EResult.OK)
        {
            var sb = new StringBuilder();

            sb.AppendLine(Langs.MultipleLineResult);
            sb.AppendLineFormat(Langs.MarketItemName, baseInfo.Name);
            sb.AppendLineFormat(Langs.MarketBuyOrderId, buyResponse.BuyOrderId);

            return bot.FormatBotResponse(sb.ToString());
        }
        else if (buyResponse.Success == EResult.Pending && ulong.TryParse(buyResponse.Confirmation?.ConfirmationId, out var confirmId))
        {
            if (bot.HasMobileAuthenticator)
            {
                var (twoFactorSuccess, _, _) = await bot.Actions.HandleTwoFactorAuthenticationConfirmations(true, EMobileConfirmationType.MarketPurchase,
        [confirmId], true).ConfigureAwait(false);

                for (int i = 0; i < 5; i++)
                {
                    buyResponse = await WebRequest.CreateBuyOrder(bot, appId, baseInfo.HashName, price, amount, bot.WalletCurrency, confirmId).ConfigureAwait(false);

                    if (buyResponse != null && buyResponse.Success == EResult.OK)
                    {
                        break;
                    }
                    else
                    {
                        await Task.Delay(2000).ConfigureAwait(false);
                    }
                }

                if (buyResponse?.Success == EResult.OK)
                {
                    return bot.FormatBotResponse(Langs.MarketBuyResult, Langs.Success, $"Id {buyResponse.BuyOrderId}");
                }
                else
                {
                    return bot.FormatBotResponse(Langs.MarketBuyResult, Langs.Failure, Langs.MarketConfirmFailed);
                }
            }
            else
            {
                _ = Task.Run(async () =>
                {
                    for (int i = 0; i < 24; i++)
                    {
                        buyResponse = await WebRequest.CreateBuyOrder(bot, appId, baseInfo.HashName, price, amount, bot.WalletCurrency, confirmId).ConfigureAwait(false);

                        if (buyResponse?.Success == EResult.OK)
                        {
                            bot.ArchiLogger.LogGenericWarning(string.Format(Langs.MarketBuyResult, Langs.Success, $"Id {buyResponse.BuyOrderId}"));
                            break;
                        }
                        else
                        {
                            await Task.Delay(5000).ConfigureAwait(false);
                        }
                    }
                });

                return bot.FormatBotResponse(Langs.MarketBuyRequireManualConfirm);
            }
        }
        else
        {
            var message = buyResponse?.Message ?? Langs.NetworkError;
            return bot.FormatBotResponse(Langs.MarketBuyResult, Langs.Failure, message);
        }
    }

    /// <summary>
    /// 创建市场求购订单 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="marketUrl"></param>
    /// <param name="strPrice"></param>
    /// <param name="strAmount"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseBuyMarketItem(string botNames, string marketUrl, string strPrice, string strAmount)
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

        var results = await Utilities.InParallel(bots.Select(x => ResponseBuyMarketItem(x, marketUrl, strPrice, strAmount))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 获取求购订单列表
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetMarketOrders(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var results = await WebRequest.GetOrderList(bot).ConfigureAwait(false);

        if (results == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        if (results.Count == 0)
        {
            return bot.FormatBotResponse(Langs.MarketOrderListEmpty);
        }

        var sb = new StringBuilder();
        sb.AppendLine(Langs.MultipleLineResult);
        sb.AppendLine(Langs.MarketOrderListTitle);
        foreach (var item in results)
        {
            sb.AppendLineFormat(Langs.MarketOrderItem, item.OrderId, item.Name, item.Price, item.Amount);
        }

        return bot.FormatBotResponse(sb.ToString());
    }

    /// <summary>
    /// 获取求购订单列表 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetMarketOrders(string botNames)
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

        var results = await Utilities.InParallel(bots.Select(ResponseGetMarketOrders)).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 取消市场求购订单
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseCancelOrder(Bot bot, string query)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var entities = query.Split(SeparatorDot, StringSplitOptions.RemoveEmptyEntries);

        var sb = new StringBuilder();

        if (entities.Length > 1)
        {
            sb.AppendLine(Langs.MultipleLineResult);
        }

        foreach (var entity in entities)
        {
            if (long.TryParse(entity, out var orderId) || (orderId == 0))
            {
                var response = await WebRequest.CancelBuyOrder(bot, orderId).ConfigureAwait(false);

                if (response?.Success == EResult.OK)
                {
                    sb.AppendLineFormat(Langs.CookieItem, entity, Langs.MarketOrderCancelSuccess);
                }
                else
                {
                    sb.AppendLineFormat(Langs.CookieItem, entity, Langs.MarketOrderCancelFailed);
                }

                continue;
            }
            else
            {
                sb.AppendLineFormat(Langs.CookieItem, entity, Langs.MarketOrderCancelInvalid);
            }
        }

        return bot.FormatBotResponse(sb.ToString());
    }

    /// <summary>
    /// 取消市场求购订单 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseCancelOrder(string botNames, string query)
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

        var results = await Utilities.InParallel(bots.Select(x => ResponseCancelOrder(x, query))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }
}
