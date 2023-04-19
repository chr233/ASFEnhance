using Newtonsoft.Json;

namespace ASFEnhance.IPC.Requests;

/// <summary>
/// 下单请求
/// </summary>
public sealed record PurchaseRequest
{
    [JsonProperty(Required = Required.Default)]
    public HashSet<uint>? SubIds { get; set; }

    [JsonProperty(Required = Required.Default)]
    public HashSet<uint>? BundleIds { get; set; }

    [JsonProperty(Required = Required.DisallowNull)]
    public bool SkipOwned { get; set; } = true;

    [JsonProperty(Required = Required.DisallowNull)]
    public bool FakePurchase { get; set; } = false;
}
