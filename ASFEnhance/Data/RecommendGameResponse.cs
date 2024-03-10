using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed record RecommendGameResponse
{
    [JsonPropertyName("success")]
    public bool Result { get; private set; }

    [JsonPropertyName("strError")]
    public string? ErrorMsg { get; private set; }
}
