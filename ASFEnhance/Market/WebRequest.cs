using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;
using ASFEnhance.Data;
using ASFEnhance.Data.WebApi;
using SteamKit2;

namespace ASFEnhance.Market;

internal static class WebRequest
{
    /// <summary>
    /// 获取市场物品信息
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="appId"></param>
    /// <param name="marketHash"></param>
    /// <returns></returns>
    public static async Task<MarketInfoResponse?> GetMarketInfo(Bot bot, string appId, string marketHash)
    {
        var request = new Uri(SteamCommunityURL, $"/market/listings/{appId}/{marketHash}?l={Langs.Language}");
        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

        if (response?.Content == null)
        {
            return null;
        }

        var eleJs = response.Content.QuerySelector("#responsive_page_template_content > script:nth-of-type(2)");

        if (string.IsNullOrEmpty(eleJs?.TextContent))
        {
            return null;
        }

        var match = RegexUtils.MatchMarketItemId().Match(eleJs.TextContent);
        if (!match.Success)
        {
            return null;
        }
        var itemId = match.Groups[1].Value;

        var eleNavs = response.Content.QuerySelectorAll("div.market_listing_nav > a");

        var name = string.Join(" ", eleNavs.Select(x => x.TextContent));

        var hash = Uri.UnescapeDataString(marketHash);

        return new MarketInfoResponse(name, appId, hash, itemId);
    }

    /// <summary>
    /// 获取市场价格信息
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="itemId"></param>
    /// <param name="country"></param>
    /// <param name="currencyCode"></param>
    /// <returns></returns>
    public static async Task<GetItemOrderListResponse?> GetMarketPriceInfo(Bot bot, string itemId, string country, ECurrencyCode currencyCode)
    {
        var request = new Uri(SteamCommunityURL, $"/market/itemordershistogram?country={country}&language={Langs.Language}&currency={(int)currencyCode}&item_nameid={itemId}");
        var response = await bot.ArchiWebHandler.UrlGetToJsonObjectWithSession<GetItemOrderListResponse>(request).ConfigureAwait(false);

        var result = response?.Content;

        if (result != null)
        {
            if (result.BuyOrderGraph != null)
            {
                result.BuyInfoList = [];
                foreach (var item in result.BuyOrderGraph)
                {
                    if (item != null && item.Count == 3)
                    {
                        var price = item[0].GetDecimal();
                        var amount = item[1].GetInt32();
                        var summary = item[2].GetString();

                        var info = new SellInfoData(price, amount, summary);
                        result.BuyInfoList.Add(info);
                    }
                }
            }

            if (result.SellOrderGraph != null)
            {
                result.SellInfoList = [];
                foreach (var item in result.SellOrderGraph)
                {
                    if (item != null && item.Count == 3)
                    {
                        var price = item[0].GetDecimal();
                        var amount = item[1].GetInt32();
                        var summary = item[2].GetString();

                        var info = new SellInfoData(price, amount, summary);
                        result.SellInfoList.Add(info);
                    }
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 获取求购订单列表
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    public static async Task<List<OrderInfoData>?> GetOrderList(Bot bot)
    {
        var request = new Uri(SteamCommunityURL, "/market/");

        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

        if (response?.Content == null)
        {
            return null;
        }

        List<OrderInfoData> result = [];

        var regex = RegexUtils.MatchMarketOrderIt();
        var eleItems = response.Content.QuerySelectorAll("div[id^='mybuyorder']");
        foreach (var ele in eleItems)
        {
            var match = regex.Match(ele.Id ?? "");
            if (!match.Success)
            {
                continue;
            }

            var orderId = match.Groups[1].Value;
            var name = ele.QuerySelector("a.market_listing_item_name_link")?.TextContent?.Trim();
            var price = ele.QuerySelector("div:nth-of-type(2) span.market_listing_price")?.TextContent?.Trim();
            var amount = ele.QuerySelector("div:nth-of-type(3) span.market_listing_price")?.TextContent?.Trim();

            var item = new OrderInfoData(name, price, amount, orderId);
            result.Add(item);
        }

        return result;
    }

    /// <summary>
    /// 创建求购订单
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="appId"></param>
    /// <param name="marketHash"></param>
    /// <param name="price"></param>
    /// <param name="amount"></param>
    /// <param name="currencyCode"></param>
    /// <param name="confirmation"></param>
    /// <returns></returns>
    public static async Task<CreateBuyOrderResponse?> CreateBuyOrder(Bot bot, string appId, string marketHash, decimal price, int amount, ECurrencyCode currencyCode, string? confirmation)
    {
        var request = new Uri(SteamCommunityURL, "/market/createbuyorder/");

        var priceTotal = (price * amount * 100);

        Dictionary<string, string> data = new(10) {
            { "currency", ((int)currencyCode).ToString() },
            { "appid", appId.ToString() },
            { "market_hash_name", marketHash },
            { "price_total", priceTotal.ToString("0") },
            { "tradefee_tax", "0" },
            { "quantity", amount.ToString() },
            { "billing_state", "" },
            { "save_my_address", "0" },
            { "confirmation", confirmation ?? "" },
        };

        var response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<CreateBuyOrderResponse>(request, data: data, session: ArchiWebHandler.ESession.Lowercase).ConfigureAwait(false);

        return response?.Content;
    }

    /// <summary>
    /// 取消求购订单
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="buyOrder"></param>
    /// <returns></returns>
    public static async Task<CancelBuyOrderResponse?> CancelBuyOrder(Bot bot, long buyOrder)
    {
        var request = new Uri(SteamCommunityURL, "/market/cancelbuyorder/");

        Dictionary<string, string> data = new(2) {
            { "buy_orderid", buyOrder.ToString() },
        };

        var response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<CancelBuyOrderResponse>(request, data: data, session: ArchiWebHandler.ESession.Lowercase).ConfigureAwait(false);

        return response?.Content;
    }
}