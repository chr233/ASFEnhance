namespace ASFEnhance.IPC.Data.Responses;

/// <summary>
/// App信息响应
/// </summary>
public sealed class AppDetailDictResponse : Dictionary<string, AppDetail>
{
}

/// <summary>
/// APP信息
/// </summary>
public sealed record AppDetail
{
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Success { get; set; }
    /// <summary>
    /// AppID
    /// </summary>
    public uint AppId { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = "";
    /// <summary>
    /// 类型
    /// </summary>
    public string Type { get; set; } = "";
    /// <summary>
    /// 说明
    /// </summary>
    public string Desc { get; set; } = "";
    /// <summary>
    /// 是否免费
    /// </summary>
    public bool IsFree { get; set; }
    /// <summary>
    /// 是否已发售
    /// </summary>
    public bool Released { get; set; }
    /// <summary>
    /// SubID列表
    /// </summary>
    public List<SubInfo>? Subs { get; set; }
}

/// <summary>
/// Sub信息
/// </summary>
public sealed record SubInfo
{
    /// <summary>
    /// SubID
    /// </summary>
    public uint? PackageId { get; set; }
    /// <summary>
    /// BundleId
    /// </summary>
    public uint? BundleId { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? PriceInCents { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? PriceFormatted { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public bool CanPurchaseAsGift { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int IncludeGameCount { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public bool RequiresShipping { get; set; }
}
