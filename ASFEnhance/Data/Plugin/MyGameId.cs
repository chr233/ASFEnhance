namespace ASFEnhance.Data;
/// <summary>
/// 
/// </summary>
public sealed record MyGameId
{
    /// <summary>
    /// 类型
    /// </summary>
    public EGameIdType Type { get; set; }
    /// <summary>
    /// 值
    /// </summary>
    public uint Id { get; set; }
}

/// <summary>
/// 类型
/// </summary>
public enum EGameIdType : byte
{
    /// <summary>
    /// AppId
    /// </summary>
    AppId,
    /// <summary>
    /// SubId
    /// </summary>
    PackageId,
    /// <summary>
    /// BundleId
    /// </summary>
    BundleId,
}
