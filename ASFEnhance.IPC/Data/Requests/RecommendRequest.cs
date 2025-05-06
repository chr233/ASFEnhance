using System.ComponentModel.DataAnnotations;

namespace ASFEnhance.IPC.Data.Requests;

/// <summary>
/// 发布评测请求
/// </summary>
public sealed record RecommendRequest
{
    /// <summary>
    /// 评测列表
    /// </summary>
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
    [Required]
    public uint AppId { get; set; }

    /// <summary>
    /// 是否推荐
    /// </summary>

    public bool RateUp { get; set; } = true;

    /// <summary>
    /// 允许回复
    /// </summary>

    public bool AllowReply { get; set; } = true;

    /// <summary>
    /// 免费获取的游戏
    /// </summary>

    public bool ForFree { get; set; }

    /// <summary>
    /// 是否公开
    /// </summary>

    public bool Public { get; set; } = true;

    /// <summary>
    /// 评测内容
    /// </summary>
    [Required]
    public string Comment { get; set; } = "";
}
