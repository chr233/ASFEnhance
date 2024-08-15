using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

/// <summary>
/// 鉴赏家信息
/// </summary>
public sealed record CuratorItem
{
    /// <summary>
    /// 名称
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [JsonPropertyName("curator_description")]
    public string? Description { get; set; }

    /// <summary>
    /// ID
    /// </summary>
    [JsonPropertyName("clanID")]
    public string? ClanId { get; set; }

    /// <summary>
    /// 关注人数
    /// </summary>
    [JsonPropertyName("total_followers")]
    public uint TotalFollowers { get; set; }

    /// <summary>
    /// 评测数量
    /// </summary>
    [JsonPropertyName("total_reviews")]
    public uint TotalReviews { get; set; }

    /// <summary>
    /// 推荐评测数量
    /// </summary>
    [JsonPropertyName("total_recommended")]
    public uint TotalRecommanded { get; set; }

    /// <summary>
    /// 不推荐评测数量
    /// </summary>
    [JsonPropertyName("total_not_recommended")]
    public uint TotalNotRecommanded { get; set; }

    /// <summary>
    /// 情报评测数量
    /// </summary>
    [JsonPropertyName("total_informative")]
    public uint TotalInformative { get; set; }
}
