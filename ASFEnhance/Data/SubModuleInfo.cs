using System.Reflection;

namespace ASFEnhance.Data;
internal sealed record SubModuleInfo
{
    /// <summary>
    /// 插件名称
    /// </summary>
    public string PluginName { get; set; } = "";
    /// <summary>
    /// 插件命令前缀
    /// </summary>
    public string? CmdPrefix { get; set; }
    /// <summary>
    /// 更新仓库名称
    /// </summary>
    public string? RepoName { get; set; }
    /// <summary>
    /// 插件版本
    /// </summary>
    public Version? PluginVersion { get; set; }

    public MethodInfo? CommandHandler { get; set; }
}
