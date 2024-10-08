using ASFEnhance.Data.WebApi;
using SteamKit2;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed record FinalPriceResponse : BaseResultResponse
{
    [JsonPropertyName("base")]
    public string? BasePrice { get; set; }

    [JsonPropertyName("tax")]
    public string? Tax { get; set; }

    [JsonPropertyName("discount")]
    public string? Discount { get; set; }
    
    [JsonPropertyName("currencycode")]
    public ECurrencyCode CurrencyCode { get; set; }
    
    [JsonPropertyName("formattedTotal")]
    public string? FormattedTotal { get; set; }
}
