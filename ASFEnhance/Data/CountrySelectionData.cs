using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

/// <summary>
/// 国家地区实体类
/// </summary>
public sealed record CountrySelectionData
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("countrycode")]
    public string? CountryCode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("countryname")]
    public string? CountryName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("hasstates")]
    public int? CountryRegion { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("statecode")]
    public string? StateCode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("statename")]
    public string? StateName { get; set; }

    /// <summary>
    /// 
    /// </summary>

    [JsonPropertyName("cityid")]
    public int? CityId { get; set; }
    /// <summary>
    /// 
    /// </summary>

    [JsonPropertyName("cityname")]
    public string? CityNme { get; set; }
}
