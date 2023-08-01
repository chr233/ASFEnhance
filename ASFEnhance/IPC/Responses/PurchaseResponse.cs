namespace ASFEnhance.IPC.Responses;

/// <summary>
/// 购物结果响应
/// </summary>
public sealed record PurchaseResultResponse
{
    /// <inheritdoc cref="Responses.AddCartResult"/>
    public AddCartResult AddCartResult { get; set; } = new();
    /// <inheritdoc cref="Responses.PurchaseResult"/>
    public PurchaseResult PurchaseResult { get; set; } = new();
}

/// <summary>
/// 添加购物车结果
/// </summary>
public sealed record AddCartResult
{
    /// <summary>
    /// SubIds
    /// </summary>
    public Dictionary<string, bool> SubIds { get; set; } = new();
    /// <summary>
    /// 捆绑包Ids
    /// </summary>
    public Dictionary<string, bool> BundleIds { get; set; } = new();
    /// <summary>
    /// 是否能为自己购买
    /// </summary>
    public bool PurchaseForSelf { get; set; }
    /// <summary>
    /// 是否能作为礼物购买
    /// </summary>
    public bool PurchaseAsGift { get; set; }
}

/// <summary>
/// 购买结果
/// </summary>
public sealed record PurchaseResult
{
    /// <inheritdoc cref="CartItem"/>
    public HashSet<CartItem> CartItems { get; set; } = new();
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Success { get; set; }
    /// <summary>
    /// 花费
    /// </summary>
    public long Cost { get; set; }
    /// <summary>
    /// 货币
    /// </summary>
    public string? Currency { get; set; }
    /// <summary>
    /// 购物前余额
    /// </summary>
    public long BalancePrev { get; set; }
    /// <summary>
    /// 购物后余额
    /// </summary>
    public long BalanceNow { get; set; }
    /// <summary>
    /// 为自己购买
    /// </summary>
    public bool PurchaseForSelf { get; set; }
    /// <summary>
    /// 作为礼物购买
    /// </summary>
    public bool PurchaseAsGift { get; set; }
}

/// <summary>
/// 购物车内容
/// </summary>
public sealed record CartItem
{
    /// <summary>
    /// 物品类型
    /// </summary>
    public string Type { get; set; } = "";
    /// <summary>
    /// SubID
    /// </summary>
    public uint Id { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = "";
}
