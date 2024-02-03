using ASFEnhance.Data;
using System.Composition;
using System.Reflection;

namespace ASFEnhance._Adapter_;

/// <summary>
/// 子模块接入点
/// </summary>
[Export]
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

        ArgumentNullException.ThrowIfNull(version);

        ArgumentNullException.ThrowIfNull(cmdHandler);

        var paramList = new List<string?>();
        foreach (var paramInfo in cmdHandler.GetParameters())
        {
            paramList.Add(paramInfo.Name?.ToUpperInvariant());
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
            ASFLogger.LogGenericWarning(string.Format(Langs.SubModuleRegisterFailedLog, pluginName, pluginId));
        }

        return success ? pluginName : Langs.SubModuleRegisterFailed;
    }
}
