using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ASFEnhance.Data;

/// <summary>
/// 插件更新信息
/// </summary>
internal sealed record PluginUpdateResponse
{
    public string PluginName { get; set; } = "";
    public bool CanUpdate => OnlineVersion != null && OnlineVersion > CurrentVersion;
    public bool IsLatest => OnlineVersion == CurrentVersion;

    public string Tips => !IsLatest && CanUpdate ? "可更新" : "";
    public Version CurrentVersion { get; set; }
    public Version? OnlineVersion { get; set; }
    public string ReleaseNote { get; set; } = "";
}
