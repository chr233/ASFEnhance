using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ASFEnhance.IPC.Requests;

/// <summary>
/// 鉴赏家Id列表请求
/// </summary>
public sealed record ClanIdListRequest
{
    /// <summary>
    /// 鉴赏家ID列表
    /// </summary>
    [Required]
    public HashSet<uint>? ClanIds { get; set; }
}
