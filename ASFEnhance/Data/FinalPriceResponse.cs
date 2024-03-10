using ArchiSteamFarm.Steam.Data;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed class FinalPriceResponse : ResultResponse
{
    [JsonPropertyName("base")]
    public int BasePrice { get; private set; }

    [JsonPropertyName("tax")]
    public int Tax { get; private set; }

    [JsonPropertyName("discount")]
    public int Discount { get; private set; }
}
