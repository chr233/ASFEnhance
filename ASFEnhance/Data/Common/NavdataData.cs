using Newtonsoft.Json;

namespace ASFEnhance.Data.Common;
/// <summary>
/// 
/// </summary>
public class NavdataData
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("domain")]
    public string? Domain { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("controller")]
    public string? Controller { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("method")]
    public string? Method { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("submethod")]
    public string? SubMethod { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("feature")]
    public string? Feature { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("depth")]
    public int Depth { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("countrycode")]
    public string CountryCode { get; set; } = Langs.CountryCode;
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("webkey")]
    public int WebKey { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("is_client")]
    public bool IsClient { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("curator_data")]
    public CuratorData? CuratorData { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("is_likely_bot")]
    public bool IsLikelyBot { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("is_utm")]
    public bool IsUtm { get; set; }
}