using ASFEnhance.Data;
using System.Reflection;

namespace ASFEnhance._Adapter_;
public static class Endpoint
{
    public static string RegisterModule(string pluginName, string? cmdPrefix, string? repoName, Version version, MethodInfo cmdHandler)
    {
        if (string.IsNullOrEmpty(pluginName))
        {
            throw new ArgumentNullException(nameof(pluginName));
        }

        var subModule = new SubModuleInfo
        {
            PluginName = pluginName,
            CmdPrefix = cmdPrefix?.ToUpperInvariant(),
            RepoName = repoName,
            PluginVersion = version,
            CommandHandler = cmdHandler
        };

        var pluginIdenty = pluginName.ToUpperInvariant();
        var success = Core.SubModules.TryAdd(pluginIdenty, subModule);

        return success ? "注册成功" : "注册失败, 重复的ID";
    }


}
