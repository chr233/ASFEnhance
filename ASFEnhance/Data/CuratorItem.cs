using Newtonsoft.Json;

namespace ASFEnhance.Data;

/// <summary>
/// 鉴赏家信息
/// </summary>
public sealed record CuratorItem
{
    /// <summary>
    /// 名称
    /// </summary>
    [JsonProperty("name", Required = Required.Always)]
    public string Name { get; set; } = "";

    /// <summary>
    /// 描述
    /// </summary>
    [JsonProperty("curator_description", Required = Required.Always)]
    public string Description { get; set; } = "";

    /// <summary>
    /// ID
    /// </summary>
    [JsonProperty("clanId", Required = Required.Always)]
    public string ClanId { get; set; } = "";

    /// <summary>
    /// 关注人数
    /// </summary>
    [JsonProperty("total_followers", Required = Required.Always)]
    public uint TotalFollowers { get; set; }

    /// <summary>
    /// 评测数量
    /// </summary>
    [JsonProperty("total_reviews", Required = Required.Always)]
    public uint TotalReviews { get; set; }

    /// <summary>
    /// 推荐评测数量
    /// </summary>
    [JsonProperty("total_recommended", Required = Required.Always)]
    public uint TotalRecommanded { get; set; }

    /// <summary>
    /// 不推荐评测数量
    /// </summary>
    [JsonProperty("total_not_recommended", Required = Required.Always)]
    public uint TotalNotRecommanded { get; set; }

    /// <summary>
    /// 情报评测数量
    /// </summary>
    [JsonProperty("total_informative", Required = Required.Always)]
    public uint TotalInformative { get; set; }
}
