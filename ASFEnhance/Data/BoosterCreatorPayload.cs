using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed record BoosterCreatorPayload
{
    [JsonPropertyName("appid")]
    public uint AppId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("series")]
    public int Series { get; set; }

    [JsonPropertyName("price")]
    public string? Price { get; set; }

    [JsonPropertyName("unavailable")]
    public bool Unavailable { get; set; }

    [JsonPropertyName("available_at_time")]
    public string? AvailableAtTime { get; set; }
}
