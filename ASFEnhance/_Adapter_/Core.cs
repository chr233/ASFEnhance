using AngleSharp.Text;
using ArchiSteamFarm.Steam;
using ASFEnhance.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASFEnhance._Adapter_;
public static class Core
{
    internal static Dictionary<string, SubModuleInfo> SubModules { get; } = new();

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
        if (SubModules.Count == 0)
        {
            return null;
        }

        foreach (var (pluginIdenty, subModule) in SubModules)
        {
            //PluginIdenty 和 CmdPrefix 匹配时响应命令
            if (pluginIdenty == pluginName || (!string.IsNullOrEmpty(subModule.CmdPrefix) && subModule.CmdPrefix == pluginName))
            {
                var response = subModule.CommandHandler?.Invoke(null, new object?[] { bot, access, cmd, message, args, steamId });
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
        if (SubModules.Count == 0)
        {
            return null;
        }

        foreach (var (_, subModule) in SubModules)
        {
            var response = subModule.CommandHandler?.Invoke(null, new object?[] { bot, access, cmd, message, args, steamId });
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
        }

        return null;
    }
}
