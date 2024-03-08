using Newtonsoft.Json;

namespace ASFEnhance.Data.Common;
/// <summary>
/// 
/// </summary>
public class FlagsData
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("is_gift")]
    public bool IsGift { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("is_private")]
    public bool IsPrivate { get; set; }
}
