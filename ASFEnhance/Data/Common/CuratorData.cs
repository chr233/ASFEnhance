using System.Text.Json.Serialization;

namespace ASFEnhance.Data.Common;
/// <summary>
/// 
/// </summary>
public class CuratorData
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("clanid")]
    public string? ClanId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("listid")]
    public string? ListId { get; set; }
}