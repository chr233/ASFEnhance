namespace ASFEnhance.IPC.Data.Requests;

/// <summary>
/// 下单请求
/// </summary>
public sealed record PurchaseRegionRequest
{
    /// <summary>
    /// SubID列表
    /// </summary>
    public HashSet<uint>? SubIds { get; init; }

    /// <summary>
    /// BundleID列表
    /// </summary>
    public HashSet<uint>? BundleIds { get; init; }

    /// <summary>
    /// 跳过已拥有
    /// </summary>
    public bool SkipOwned { get; init; } = true;

    public string? CountryCode { get; init; }
    public string? Payment { get; init; }
}
