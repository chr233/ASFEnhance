using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed record CartConfigResponse
{
    [JsonPropertyName("requestcartgid")]
    public int RequestCartGid { get; set; }

    [JsonPropertyName("rgUserCountryOptions")]
    public Dictionary<string, string>? UserCountryOptions { get; set; }

    [JsonIgnore]
    public string? CountryCode { get; set; }
}
