using ArchiSteamFarm.Steam.Data;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed class FinalPriceResponse : ResultResponse
{
    [JsonPropertyName("base")]
    public int BasePrice { get; set; }

    [JsonPropertyName("tax")]
    public int Tax { get; set; }

    [JsonPropertyName("discount")]
    public int Discount { get; set; }
}
