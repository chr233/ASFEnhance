namespace ASFEnhance.IPC.Requests;

/// <summary>
/// AppIds列表请求
/// </summary>
public sealed record AppIdListRequest
{
    /// <summary>
    /// AppId列表
    /// </summary>
    public HashSet<uint>? AppIds { get; set; }
    /// <summary>
    /// SubId列表
    /// </summary>
    public HashSet<uint>? PackageIds { get; set; }
    /// <summary>
    /// BundleId列表
    /// </summary>
    public HashSet<uint>? BundleIds { get; set; }
}
