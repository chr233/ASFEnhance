using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

/// <summary>
/// 
/// </summary>
public sealed record RecommendGameResponse
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("success")]
    public bool Result { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("strError")]
    public string? ErrorMsg { get; set; }
}
