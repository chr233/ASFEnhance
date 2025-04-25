namespace ASFEnhance.IPC.Requests;

/// <summary>
/// 下单请求
/// </summary>
public sealed record PurchaseRequest
{
    /// <summary>
    /// SubID列表
    /// </summary>
    public HashSet<uint>? SubIds { get; set; }

    /// <summary>
    /// BundleID列表
    /// </summary>
    public HashSet<uint>? BundleIds { get; set; }

    /// <summary>
    /// 跳过已拥有
    /// </summary>
    public bool SkipOwned { get; set; } = true;

    /// <summary>
    /// 卡单
    /// </summary>
    public bool FakePurchase { get; set; }
}
