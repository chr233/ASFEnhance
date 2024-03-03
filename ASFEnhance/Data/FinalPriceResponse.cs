using ArchiSteamFarm.Steam.Data;
using Newtonsoft.Json;

namespace ASFEnhance.Data;

internal sealed class FinalPriceResponse : ResultResponse
{
    [JsonProperty(PropertyName = "base", Required = Required.DisallowNull)]
    public int BasePrice { get; private set; }

    [JsonProperty(PropertyName = "tax", Required = Required.DisallowNull)]
    public int Tax { get; private set; }

    [JsonProperty(PropertyName = "discount", Required = Required.DisallowNull)]
    public int Discount { get; private set; }
}
