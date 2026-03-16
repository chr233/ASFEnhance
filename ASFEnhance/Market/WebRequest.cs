using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;
using ASFEnhance.Data;
using ASFEnhance.Data.WebApi;
using SteamKit2;

namespace ASFEnhance.Market;

internal static class WebRequest
{
    /// <summary>
    /// 获取账号家庭组信息
    /// </summary>
    /// <param name="bot"></param>
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

    public static async Task<GetItemOrderListResponse?> GetMarketDetail(Bot bot, string itemId, string country, ECurrencyCode currencyCode)
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

    public static async Task<CancelBuyOrderResponse?> CancelBuyOrder(Bot bot, string buyOrder)
    {
        var request = new Uri(SteamCommunityURL, "/market/cancelbuyorder/");

        Dictionary<string, string> data = new(2) {
            { "buy_orderid", buyOrder },
        };

        var response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<CancelBuyOrderResponse>(request, data: data, session: ArchiWebHandler.ESession.Lowercase).ConfigureAwait(false);

        return response?.Content;
    }
}