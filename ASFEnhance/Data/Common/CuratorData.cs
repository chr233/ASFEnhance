using Newtonsoft.Json;

namespace ASFEnhance.Data.Common;
/// <summary>
/// 
/// </summary>
public class CuratorData
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("clanid")]
    public string? ClanId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("listid")]
    public string? ListId { get; set; }
}