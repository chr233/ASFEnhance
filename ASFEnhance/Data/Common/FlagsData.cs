using System.Text.Json.Serialization;

namespace ASFEnhance.Data.Common;
/// <summary>
/// 
/// </summary>
public class FlagsData
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("is_gift")]
    public bool IsGift { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("is_private")]
    public bool IsPrivate { get; set; }
}
