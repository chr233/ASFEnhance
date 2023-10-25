using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ASFEnhance.IPC.Requests;

/// <summary>
/// AppIds列表请求
/// </summary>
public sealed record AppIdListRequest
{
    /// <summary>
    /// AppId列表
    /// </summary>
    [JsonProperty(Required = Required.Always)]
    [Required]
    public HashSet<uint>? AppIds { get; set; }
}
