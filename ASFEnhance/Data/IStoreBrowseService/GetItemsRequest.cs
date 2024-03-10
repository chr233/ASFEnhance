using ASFEnhance.Data.Common;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data.IStoreBrowseService;
/// <summary>
/// 
/// </summary>
public sealed record GetItemsRequest
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("ids")]
    public List<IdData>? Ids { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("context")]
    public ContextData? Context { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("data_request")]
    public DataRequestData? DataRequest { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public sealed record ContextData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("language")]
        public string Language { get; set; } = Langs.Language;
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; } = Langs.CountryCode;
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("steam_realm")]
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
        [JsonPropertyName("include_all_purchase_options")]
        public bool IncludeAllPurchaseOptions { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("include_full_description")]
        public bool IncludeFullDescription { get; set; }
    }
}
