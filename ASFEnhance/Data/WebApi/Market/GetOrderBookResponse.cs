using SteamKit2;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data.WebApi.Market;

public sealed record GetOrderBookResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("data")]
    public BookData? Data { get; set; }

    public sealed record BookData
    {
        [JsonPropertyName("amtMaxBuyOrder")]
        public decimal? AmtMaxBuyOrder { get; set; }

        [JsonPropertyName("amtMinSellOrder")]
        public decimal AmtMinSellOrder { get; set; }

        [JsonPropertyName("eCurrency")]
        public ECurrencyCode Currency { get; set; }

        [JsonPropertyName("cBuyOrders")]
        public int CBuyOrdersCount { get; set; }

        [JsonPropertyName("cSellOrders")]
        public int SellOrdersCount { get; set; }

        [JsonPropertyName("rgCompactBuyOrders")]
        public List<int>? BuyOrders { get; set; }

        [JsonPropertyName("rgCompactSellOrders")]
        public List<int>? SellOrders { get; set; }

        [JsonIgnore]
        public List<SellInfoData>? SellInfoList { get; set; }

        [JsonIgnore]
        public List<SellInfoData>? BuyInfoList { get; set; }
    }
}
