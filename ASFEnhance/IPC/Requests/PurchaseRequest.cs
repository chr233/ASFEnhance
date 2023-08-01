using Newtonsoft.Json;

namespace ASFEnhance.IPC.Requests;

/// <summary>
/// 下单请求
/// </summary>
public sealed record PurchaseRequest
{
    /// <summary>
    /// SubID列表
    /// </summary>
    [JsonProperty(Required = Required.Default)]
    public HashSet<uint>? SubIds { get; set; }

    /// <summary>
    /// BundleID列表
    /// </summary>
    [JsonProperty(Required = Required.Default)]
    public HashSet<uint>? BundleIds { get; set; }

    /// <summary>
    /// 跳过已拥有
    /// </summary>
    [JsonProperty(Required = Required.DisallowNull)]
    public bool SkipOwned { get; set; } = true;

    /// <summary>
    /// 卡单
    /// </summary>
    [JsonProperty(Required = Required.DisallowNull)]
    public bool FakePurchase { get; set; }
}
