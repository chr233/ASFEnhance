using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using System.Text;

namespace ASFEnhance.Market;

internal static class Command
{
    /// <summary>
    /// 获取市场商品信息
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetMarketInfo(Bot bot, string query)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var match = RegexUtils.MatchMarketUrl().Match(query);
        if (!match.Success)
        {
            return bot.FormatBotResponse("市场链接似乎无效");
        }

        var appId = match.Groups[1].Value;
        var marketHash = match.Groups[2].Value;

        var baseInfo = await WebRequest.GetMarketInfo(bot, appId, marketHash).ConfigureAwait(false);

        if (baseInfo == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        var detail = await WebRequest.GetMarketDetail(bot, baseInfo.ItemId, bot.GetUserCountryCode(), bot.WalletCurrency).ConfigureAwait(false);

        var sb = new StringBuilder();

        sb.AppendLine(Langs.MultipleLineResult);
        sb.AppendLineFormat("物品名称: {0}", baseInfo.Name);
        sb.AppendLineFormat("物品Id: {0} - {1}", baseInfo.AppId, baseInfo.ItemId);
        sb.AppendLineFormat("Hash: {0}", baseInfo.HashName);

        if (detail?.SellInfoList != null)
        {
            var price = detail.SellInfoList.FirstOrDefault();
            sb.AppendLineFormat("最低出售价: {1}{0}{2}", price?.Price, detail.PricePrefix, detail.PriceSuffix);
        }
        if (detail?.BuyInfoList != null)
        {
            var price = detail.BuyInfoList.FirstOrDefault();
            sb.AppendLineFormat("最高求购价: {1}{0}{2}", price?.Price, detail.PricePrefix, detail.PriceSuffix);
        }

        return bot.FormatBotResponse(sb.ToString());
    }

    /// <summary>
    /// 获取市场商品信息 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetMarketInfo(string botNames, string query)
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

        var results = await Utilities.InParallel(bots.Select(x => ResponseGetMarketInfo(x, query))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 创建市场求购订单
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseBuyMarketItem(Bot bot, string query)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var match = RegexUtils.MatchMarketUrl().Match(query);
        if (!match.Success)
        {
            return bot.FormatBotResponse("市场链接似乎无效");
        }

        var appId = match.Groups[1].Value;
        var marketHash = match.Groups[2].Value;

        var baseInfo = await WebRequest.GetMarketInfo(bot, appId, marketHash).ConfigureAwait(false);

        if (baseInfo == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        var detail = await WebRequest.GetMarketDetail(bot, baseInfo.ItemId, bot.GetUserCountryCode(), bot.WalletCurrency).ConfigureAwait(false);

        var sb = new StringBuilder();

        sb.AppendLine(Langs.MultipleLineResult);
        sb.AppendLineFormat("物品名称: {0}", baseInfo.Name);
        sb.AppendLineFormat("物品Id: {0} - {1}", baseInfo.AppId, baseInfo.ItemId);

        if (detail?.SellInfoList != null)
        {
            var price = detail.SellInfoList.FirstOrDefault();
            sb.AppendLineFormat("最低出售价: {1}{0}{2}", price?.Price, detail.PricePrefix, detail.PriceSuffix);
        }
        if (detail?.BuyInfoList != null)
        {
            var price = detail.BuyInfoList.FirstOrDefault();
            sb.AppendLineFormat("最高求购价: {1}{0}{2}", price?.Price, detail.PricePrefix, detail.PriceSuffix);
        }

        return bot.FormatBotResponse(sb.ToString());
    }

    /// <summary>
    /// 创建市场求购订单 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseBuyMarketItem(string botNames, string query)
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

        var results = await Utilities.InParallel(bots.Select(x => ResponseBuyMarketItem(x, query))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }




}
