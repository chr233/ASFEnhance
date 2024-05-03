using ASFEnhance.Data.Common;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data.IAccountCartService;

/// <summary>
/// 
/// </summary>
public sealed record ModifyLineItemRequest
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
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("gift_info")]
    public GiftInfoData? GiftInfo { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("flags")]
    public FlagsData? Flags { get; set; }
}
