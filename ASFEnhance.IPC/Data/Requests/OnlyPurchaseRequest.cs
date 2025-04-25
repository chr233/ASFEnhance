namespace ASFEnhance.IPC.Data.Requests;

/// <summary>
/// 下单请求
/// </summary>
public sealed record OnlyPurchaseRequest
{
    /// <summary>
    /// 卡单
    /// </summary>
    public bool FakePurchase { get; set; }
}
