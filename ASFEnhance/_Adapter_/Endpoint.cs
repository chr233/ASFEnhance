using ASFEnhance.Data;
using System.Reflection;

namespace ASFEnhance._Adapter_;

/// <summary>
/// 子模块接入点
/// </summary>
public static class Endpoint
{
    /// <summary>
    /// 注册子模块
    /// </summary>
    /// <param name="pluginName">插件名称</param>
    /// <param name="pluginId">插件唯一标识符</param>
    /// <param name="cmdPrefix">命令前缀</param>
    /// <param name="repoName">自动更新仓库</param>
    /// <param name="version">版本</param>
    /// <param name="cmdHandler">命令处理函数</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string RegisterModule(string pluginName, string pluginId, string? cmdPrefix, string? repoName, Version version, MethodInfo cmdHandler)
    {
        if (string.IsNullOrEmpty(pluginName))
        {
            throw new ArgumentNullException(nameof(pluginName));
        }

        if (string.IsNullOrEmpty(pluginId))
        {
            throw new ArgumentNullException(nameof(pluginId));
        }

        if (version == null)
        {
            throw new ArgumentNullException(nameof(version));
        }

        if (cmdHandler == null)
        {
            throw new ArgumentNullException(nameof(cmdHandler));
        }

        var paramList = new List<string>();
        foreach (var paramInfo in cmdHandler.GetParameters())
        {
            paramList.Add(paramInfo.Name ?? "null");
        }

        var subModule = new SubModuleInfo
        {
            PluginName = pluginName,
            CmdPrefix = cmdPrefix?.ToUpperInvariant(),
            RepoName = repoName,
            PluginVersion = version,
            CommandHandler = cmdHandler,
            ParamList = paramList,
        };

        pluginId = pluginId.ToUpperInvariant();
        var success = ExtensionCore.SubModules.TryAdd(pluginId, subModule);

        if (!success)
        {
            ASFLogger.LogGenericWarning(string.Format("子模块 {0} 注册失败, 重复的 ID {1}", pluginName, pluginId));
        }

        return success ? pluginName : "注册失败, 重复的ID";
    }


}
