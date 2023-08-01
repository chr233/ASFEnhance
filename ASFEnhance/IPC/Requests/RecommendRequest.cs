using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ASFEnhance.IPC.Requests;

/// <summary>
/// 发布评测请求
/// </summary>
public sealed record RecommendRequest
{
    /// <summary>
    /// 评测列表
    /// </summary>
    [JsonProperty(Required = Required.Always)]
    [Required]
    public HashSet<RecommendOption>? Recommends { get; set; }
}
/// <summary>
/// 评测选项
/// </summary>
public sealed record RecommendOption
{
    /// <summary>
    /// AppId
    /// </summary>
    [JsonProperty(Required = Required.Always)]
    [Required]
    public uint AppId { get; set; }

    /// <summary>
    /// 是否推荐
    /// </summary>
    [JsonProperty(Required = Required.DisallowNull)]
    public bool RateUp { get; set; } = true;

    /// <summary>
    /// 允许回复
    /// </summary>
    [JsonProperty(Required = Required.DisallowNull)]
    public bool AllowReply { get; set; } = true;

    /// <summary>
    /// 免费获取的游戏
    /// </summary>
    [JsonProperty(Required = Required.DisallowNull)]
    public bool ForFree { get; set; }

    /// <summary>
    /// 是否公开
    /// </summary>
    [JsonProperty(Required = Required.DisallowNull)]
    public bool Public { get; set; } = true;

    /// <summary>
    /// 评测内容
    /// </summary>
    [JsonProperty(Required = Required.Always)]
    [Required]
    public string Comment { get; set; } = "";
}
