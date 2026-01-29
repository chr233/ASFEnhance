using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed record UserInfoResponse
{
    [JsonPropertyName("steamid")]
    public string? SteamId { get; set; }
    [JsonPropertyName("country_code")]
    public string? CountryCode { get; set; }
    [JsonPropertyName("accountid")]
    public int AccountId { get; set; }
    [JsonPropertyName("account_name")]
    public string? AccountName { get; set; }
}
