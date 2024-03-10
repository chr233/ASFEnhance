using System.Text.Json.Serialization;

namespace ASFEnhance.Data.Common;
/// <summary>
/// 
/// </summary>
public sealed record GiftMessageData
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("gifteename")]
    public string? GifteeName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("sentiment")]
    public string? Sentiment { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("signature")]
    public string? Signature { get; set; }
}
