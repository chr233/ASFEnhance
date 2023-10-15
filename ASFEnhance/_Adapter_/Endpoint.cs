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

        var pluginIdenty = pluginName.ToUpperInvariant();
        var success = Core.SubModules.TryAdd(pluginIdenty, subModule);

        return success ? pluginName : "注册失败, 重复的ID";
    }


}
