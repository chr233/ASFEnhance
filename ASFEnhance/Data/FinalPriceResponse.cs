using ASFEnhance.Data.WebApi;
using SteamKit2;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

/// <summary>
/// 
/// </summary>
public sealed record FinalPriceResponse : BaseResultResponse
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("base")]
    public string? BasePrice { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("tax")]
    public string? Tax { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("discount")]
    public string? Discount { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("currencycode")]
    public ECurrencyCode CurrencyCode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("formattedTotal")]
    public string? FormattedTotal { get; set; }
}
