using Newtonsoft.Json;

namespace ASFEnhance.Data.Common;
/// <summary>
/// 
/// </summary>
public sealed record GiftMessageData
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("gifteename")]
    public string? GifteeName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("message")]
    public string? Message { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("sentiment")]
    public string? Sentiment { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("signature")]
    public string? Signature { get; set; }
}
