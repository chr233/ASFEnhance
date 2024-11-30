using System.Text.Json.Serialization;

namespace ASFEnhance.Data.IStoreService;

/// <summary>
///     获取探索队列响应
/// </summary>
internal sealed record GetDiscoveryQueueResponse
{
    [JsonPropertyName("appids")]
    public List<uint>? AppIds { get; set; }

    [JsonPropertyName("country_code")]
    public string? CountryCode { get; set; }

    [JsonPropertyName("settings")]
    public SettingsData? Settings { get; set; }

    [JsonPropertyName("skipped")]
    public int Skipped { get; set; }

    [JsonPropertyName("exhausted")]
    public bool Exhausted { get; set; }

    [JsonPropertyName("experimental_cohort")]
    public int ExperimentalCohort { get; set; }


    internal sealed record SettingsData
    {
        [JsonPropertyName("include_coming_soon")]
        public bool IncludeComingSoon { get; set; }
    }
}