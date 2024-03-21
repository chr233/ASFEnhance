using ASFEnhance.Data.Common;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data.IAccountCartService;

/// <summary>
/// 
/// </summary>
public sealed record AddItemsToCartRequest
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("user_country")]
    public string UserCountry { get; set; } = Langs.CountryCode;
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("items")]
    public List<ItemData>? Items { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("navdata")]
    public NavdataData? Navdata { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public sealed record ItemData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("packageid")]
        public uint? PackageId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("bundleid")]
        public uint? BundleId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("gift_info")]
        public GiftInfoData? GIftInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("flags")]
        public FlagsData? Flags { get; set; }
    }
}
