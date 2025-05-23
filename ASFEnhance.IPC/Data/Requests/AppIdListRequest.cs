namespace ASFEnhance.IPC.Data.Requests;

/// <summary>
/// AppIds列表请求
/// </summary>
public sealed record AppIdListRequest
{
    /// <summary>
    /// AppId列表
    /// </summary>
    public uint[]? AppIds { get; set; }
    /// <summary>
    /// SubId列表
    /// </summary>
    public uint[]? PackageIds { get; set; }
    /// <summary>
    /// BundleId列表
    /// </summary>
    public uint[]? BundleIds { get; set; }
}
