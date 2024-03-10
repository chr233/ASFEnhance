using System.Text.Json.Serialization;

namespace ASFEnhance.Data.Common;
/// <summary>
/// 
/// </summary>
public class NavdataData
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("domain")]
    public string? Domain { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("controller")]
    public string? Controller { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("method")]
    public string? Method { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("submethod")]
    public string? SubMethod { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("feature")]
    public string? Feature { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("depth")]
    public int Depth { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("countrycode")]
    public string CountryCode { get; set; } = Langs.CountryCode;
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("webkey")]
    public int WebKey { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("is_client")]
    public bool IsClient { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("curator_data")]
    public CuratorData? CuratorData { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("is_likely_bot")]
    public bool IsLikelyBot { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("is_utm")]
    public bool IsUtm { get; set; }
}