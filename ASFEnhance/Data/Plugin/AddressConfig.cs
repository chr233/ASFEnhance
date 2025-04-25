namespace ASFEnhance.Data.Plugin;

/// <summary>
/// 地址信息
/// </summary>
public sealed record AddressConfig
{
    /// <summary>
    /// 姓
    /// </summary>
    public string? FirstName { get; set; }
    /// <summary>
    /// 名
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// 街道地址
    /// </summary>
    public string Address { get; set; } = "";

    /// <summary>
    /// 城市
    /// </summary>
    public string City { get; set; } = "";

    /// <summary>
    /// 国家
    /// </summary>
    public string Country { get; set; } = "";

    /// <summary>
    /// 省/州
    /// </summary>
    public string State { get; set; } = "";

    /// <summary>
    /// 邮编
    /// </summary>
    public string PostCode { get; set; } = "";
}
