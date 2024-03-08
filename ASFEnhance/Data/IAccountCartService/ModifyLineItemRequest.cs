using ASFEnhance.Data.Common;
using Newtonsoft.Json;

namespace ASFEnhance.Data.IAccountCartService;

/// <summary>
/// 
/// </summary>
public sealed record ModifyLineItemRequest
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("line_item_id")]
    public ulong LineItemId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("user_country")]
    public string UserCountry { get; set; } = Langs.CountryCode;
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("gift_info")]
    public GiftInfoData? GIftInfo { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("flags")]
    public FlagsData? Flags { get; set; }
}
