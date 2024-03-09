using System.Reflection;

namespace ASFEnhance.Data.Plugin;
internal sealed record SubModuleInfo
{
    /// <summary>
    /// 插件名称
    /// </summary>
    public string PluginName { get; set; } = null!;
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
    public Version PluginVersion { get; set; } = null!;
    /// <summary>
    /// 命令处理函数
    /// </summary>
    public MethodInfo CommandHandler { get; set; } = null!;
    /// <summary>
    /// 参数列表
    /// </summary>
    public List<string?> ParamList { get; set; } = null!;

    /// <summary>
    /// 判断CmdPrefix是否匹配
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public bool MatchCmdPrefix(string text) => !string.IsNullOrEmpty(CmdPrefix) && CmdPrefix == text;
}
