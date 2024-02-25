using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ASFEnhance.IPC.Requests;

/// <summary>
/// AppIds列表请求
/// </summary>
public sealed record AppIdListRequest
{
    /// <summary>
    /// AppId列表
    /// </summary>
    [Required]
    public HashSet<uint>? AppIds { get; set; }
}
