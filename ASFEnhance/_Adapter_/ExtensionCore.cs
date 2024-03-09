using ArchiSteamFarm.Steam;
using ASFEnhance.Data.Plugin;

namespace ASFEnhance._Adapter_;

/// <summary>
/// 调用外部模块命令
/// </summary>
public static class ExtensionCore
{
    /// <summary>
    /// 子模块字典
    /// </summary>
    internal static Dictionary<string, SubModuleInfo> SubModules { get; } = [];

    internal static bool HasSubModule => SubModules.Count != 0;

    /// <summary>
    /// 调用子模块命令处理函数
    /// </summary>
    /// <param name="subModule"></param>
    /// <param name="bot"></param>
    /// <param name="access"></param>
    /// <param name="cmd"></param>
    /// <param name="message"></param>
    /// <param name="args"></param>
    /// <param name="steamId"></param>
    /// <returns></returns>
    private static Task<string?>? Invoke(this SubModuleInfo subModule, Bot bot, EAccess access, string cmd, string message, string[] args, ulong steamId)
    {
        // 根据参数名称填入参数
        var objList = new List<object?>();
        foreach (var paramName in subModule.ParamList)
        {
            object? obj = paramName switch
            {
                "BOT" => bot,
                "ACCESS" => access,
                "CMD" => cmd,
                "MESSAGE" => message,
                "ARGS" => args,
                "STEAMID" => steamId,
                _ => null,
            };
            objList.Add(obj);
        }

        var response = subModule.CommandHandler.Invoke(null, [.. objList]);
        if (response != null)
        {
            if (response is Task<string?> task)
            {
                return task;
            }
            else if (response is string str)
            {
                return Task.FromResult<string?>(str);
            }
        }
        return null;
    }

    /// <summary>
    /// 调用外部模块命令
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="bot"></param>
    /// <param name="access"></param>
    /// <param name="message"></param>
    /// <param name="args"></param>
    /// <param name="steamId"></param>
    /// <returns></returns>
    internal static Task<string?>? ExecuteCommand(string cmd, Bot bot, EAccess access, string message, string[] args, ulong steamId)
    {
        foreach (var (pluginId, subModule) in SubModules)
        {
            if (cmd == pluginId || subModule.MatchCmdPrefix(cmd))
            {
                // 响应 Plugin Info 命令
                var pluginInfo = string.Format("{0} {1} [ASFEnhance]", subModule.PluginName, subModule.PluginVersion);
                return Task.FromResult<string?>(pluginInfo);
            }
            else
            {
                // 响应命令
                var response = subModule.Invoke(bot, access, cmd, message, args, steamId);
                if (response != null)
                {
                    return response;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// 调用外部模块命令 (限定插件名)
    /// </summary>
    /// <param name="pluginName"></param>
    /// <param name="cmd"></param>
    /// <param name="bot"></param>
    /// <param name="access"></param>
    /// <param name="message"></param>
    /// <param name="args"></param>
    /// <param name="steamId"></param>
    /// <returns></returns>
    internal static Task<string?>? ExecuteCommand(string pluginName, string cmd, Bot bot, EAccess access, string message, string[] args, ulong steamId)
    {
        foreach (var (pluginId, subModule) in SubModules)
        {
            if (cmd == pluginId || subModule.MatchCmdPrefix(cmd))
            {
                // 响应 Plugin Info 命令
                var pluginInfo = string.Format("{0} {1} [ASFEnhance]", subModule.PluginName, subModule.PluginVersion);
                return Task.FromResult<string?>(pluginInfo);
            }
            else if (pluginId == pluginName || subModule.MatchCmdPrefix(pluginName))
            {
                // PluginIdenty 和 CmdPrefix 匹配时响应命令
                var response = subModule.Invoke(bot, access, cmd, message, args, steamId);
                if (response != null)
                {
                    return response;
                }
            }
        }

        return null;
    }
}
