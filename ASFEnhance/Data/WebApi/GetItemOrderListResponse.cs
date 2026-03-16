using SteamKit2;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data.WebApi;

internal sealed record GetItemOrderListResponse
{
    [JsonPropertyName("success")]
    public EResult Success { get; set; }

    [JsonPropertyName("highest_buy_order")]
    public string? HighestBuyOrder { get; set; }

    [JsonPropertyName("lowest_sell_order")]
    public string? LowestSellOrder { get; set; }

    [JsonPropertyName("buy_order_graph")]
    public List<List<JsonElement>>? BuyOrderGraph { get; set; }
    [JsonPropertyName("sell_order_graph")]
    public List<List<JsonElement>>? SellOrderGraph { get; set; }

    [JsonIgnore]
    public List<SellInfoData>? SellInfoList { get; set; }

    [JsonIgnore]
    public List<SellInfoData>? BuyInfoList { get; set; }

    [JsonPropertyName("price_prefix")]
    public string? PricePrefix { get; set; }
    [JsonPropertyName("price_suffix")]
    public string? PriceSuffix { get; set; }

}
