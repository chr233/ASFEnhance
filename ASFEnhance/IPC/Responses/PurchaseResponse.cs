namespace ASFEnhance.IPC.Responses;

public sealed record PurchaseResultResponse
{
    public AddCartResult AddCartResult { get; set; } = new();
    public PurchaseResult PurchaseResult { get; set; } = new();
}

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

public sealed record PurchaseResult
{
    public HashSet<CartItem> CartItems { get; set; } = new();
    public bool Success { get; set; }
    public long Cost { get; set; }
    public string Currency { get; set; } = "";
    public long BalancePrev { get; set; }
    public long BalanceNow { get; set; }
    public bool PurchaseForSelf { get; set; }
    public bool PurchaseAsGift { get; set; }
}

public sealed record CartItem
{
    public string Type { get; set; } = "";
    public uint Id { get; set; }
    public string Name { get; set; } = "";
}
