namespace ASFEnhance.Data;

/// <summary>
/// 插件更新信息
/// </summary>
internal sealed record PluginUpdateResponse
{
    /// <summary>
    /// 插件名称
    /// </summary>
    public string PluginName { get; set; } = "";
    /// <summary>
    /// 有可用更新
    /// </summary>
    public bool CanUpdate => OnlineVersion != null && OnlineVersion > CurrentVersion;
    /// <summary>
    /// 已经为最新版本
    /// </summary>
    public bool IsLatest => OnlineVersion == CurrentVersion;

    public string Tips => CanUpdate ? Langs.CanUpdate : "";
    public Version? CurrentVersion { get; set; }
    public Version? OnlineVersion { get; set; }
    public string? UpdateLog { get; set; }
    public string ReleaseNote { get; set; } = "";
}
