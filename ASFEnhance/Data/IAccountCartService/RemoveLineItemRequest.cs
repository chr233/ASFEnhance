using System.Text.Json.Serialization;

namespace ASFEnhance.Data.IAccountCartService;

/// <summary>
/// 
/// </summary>
public sealed record RemoveLineItemRequest
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("line_item_id")]
    public ulong LineItemId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("user_country")]
    public string UserCountry { get; set; } = Langs.CountryCode;
}
