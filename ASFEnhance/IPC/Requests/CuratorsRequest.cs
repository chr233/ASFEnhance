using Newtonsoft.Json;

namespace ASFEnhance.IPC.Requests;

/// <summary>
/// 鉴赏家列表请求
/// </summary>
public sealed record CuratorsRequest
{
    /// <summary>
    /// 起始位置
    /// </summary>
    [JsonProperty(Required = Required.DisallowNull)]
    public uint Start { get; set; }

    /// <summary>
    /// 获取数量
    /// </summary>
    [JsonProperty(Required = Required.DisallowNull)]
    public uint Count { get; set; } = 30;
}
