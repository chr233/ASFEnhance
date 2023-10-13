using ASFEnhance.Data;
using System.Collections.Generic;
using System.Reflection;

namespace ASFEnhance._Adapter_;
public static class Endpoint
{

    public static string RegisterModule(string pluginName, string? cmdPrefix, string? repoName, Version version, MethodInfo cmdHandler)
    {
        var subModule = new SubModuleInfo
        {
            PluginName = pluginName,
            CmdPrefix = cmdPrefix,
            RepoName = repoName,
            PluginVersion = version,
            CommandHandler = cmdHandler
        };

        var success = Core.SubModules.TryAdd(pluginName, subModule);

        return success ? "注册成功" : "注册失败, 重复的ID";
    }
}
