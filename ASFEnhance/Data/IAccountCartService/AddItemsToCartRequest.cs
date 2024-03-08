using ASFEnhance.Data.Common;
using Newtonsoft.Json;

namespace ASFEnhance.Data.IAccountCartService;

/// <summary>
/// 
/// </summary>
public sealed record AddItemsToCartRequest
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("user_country")]
    public string UserCountry { get; set; } = Langs.CountryCode;
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("items")]
    public List<ItemData>? Items { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("navdata")]
    public NavdataData? Navdata { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public sealed record ItemData
    {
        /// <summary>
        /// 
        /// </summary>
        public uint? PackageId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public uint? BundleId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public GiftInfoData? GIftInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public FlagsData? Flags { get; set; }
    }
}
