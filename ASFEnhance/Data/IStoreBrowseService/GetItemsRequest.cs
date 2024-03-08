using Newtonsoft.Json;

namespace ASFEnhance.Data.IStoreBrowseService;
/// <summary>
/// 
/// </summary>
public sealed record GetItemsRequest
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("ids")]
    public List<IdData>? Ids { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("context")]
    public ContextData? Context { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("data_request")]
    public DataRequestData? DataRequest { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public sealed record IdData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("appid")]
        public uint? AppId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("packageid")]
        public uint? PackageId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("bundleid")]
        public uint? BundleId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed record ContextData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; } = Langs.Language;
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; } = Langs.CountryCode;
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("steam_realm")]
        public string SteamRealm { get; set; } = "1";
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed record DataRequestData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("include_all_purchase_options")]
        public bool IncludeAllPurchaseOptions { get; set; }
        [JsonProperty("include_full_description")]
        public bool IncludeFullDescription { get; set; }
    }
}
