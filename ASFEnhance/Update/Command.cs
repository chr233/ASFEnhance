using ArchiSteamFarm.Core;
using ArchiSteamFarm.Plugins;
using ArchiSteamFarm.Plugins.Interfaces;
using ArchiSteamFarm.Steam;
using ASFEnhance.Data;
using ASFEnhance.Explorer;
using System;
using System.Collections.Immutable;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using static SteamKit2.GC.Dota.Internal.CMsgDOTABotDebugInfo;
using static SteamKit2.GC.Underlords.Internal.CMsgIndividualPostMatchStats;

namespace ASFEnhance.Update;

internal static class Command
{
    /// <summary>
    /// 获取已安装的插件列表
    /// </summary>
    /// <returns></returns>
    internal static string? ResponsePluginList()
    {
        var activePlugins = typeof(PluginsCore).GetStaticPrivateProperty<ImmutableHashSet<IPlugin>>("ActivePlugins");

        if (activePlugins != null)
        {
            var sb = new StringBuilder();
            sb.AppendLine(FormatStaticResponse(Langs.MultipleLineResult));
            sb.AppendLineFormat("已安装 {0} 个外部模块, 末尾带 [] 的为 ASFEnhance 和子模块", activePlugins.Count);

            var subModules = new Dictionary<string, SubModuleInfo>();
            foreach (var subModule in _Adapter_.ExtensionCore.SubModules.Values)
            {
                subModules.TryAdd(subModule.PluginName, subModule);
            }

            var index = 1;
            foreach (var plugin in activePlugins)
            {
                if (plugin.Name == "ASFEnhance")
                {
                    sb.AppendLineFormat("{0}: {1,-20} {2} [{3,-}]", index++, plugin.Name, plugin.Version, "ASFE");
                }
                else if (subModules.TryGetValue(plugin.Name, out var subModule))
                {
                    sb.AppendLineFormat("{0}: {1,-20} {2} [{3,-}]", index++, subModule.PluginName, subModule.PluginVersion, subModule.CmdPrefix ?? "---");
                }
                else
                {
                    sb.AppendLineFormat("{0}: {1,-20} {2}", index++, plugin.Name, plugin.Version);
                }
            }

            return sb.ToString();
        }
        else
        {
            //大概不可能会执行到这里
            return FormatStaticResponse("未加载外部模块");
        }
    }

    /// <summary>
    /// 获取插件最新版本
    /// </summary>
    /// <param name="pluginNames"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetPluginLatestVersion(string? pluginNames = null)
    {
        var entries = pluginNames?.ToUpperInvariant().Split(',', StringSplitOptions.RemoveEmptyEntries);

        var tasks = new List<Task<PluginUpdateResponse>>();

        if (entries?.Any() == true)
        {
            foreach (var entry in entries)
            {
                if (entry == "ASFE" || entry == "ASFENHANCE")
                {
                    tasks.Add(WebRequest.GetPluginReleaseNote("ASFEnhance", MyVersion, "ASFEnhance"));
                }
                else if (_Adapter_.ExtensionCore.SubModules.TryGetValue(entry, out var subModule))
                {
                    tasks.Add(WebRequest.GetPluginReleaseNote(subModule.PluginName, subModule.PluginVersion, subModule.RepoName));
                }
            }
        }
        else
        {
            tasks.Add(WebRequest.GetPluginReleaseNote("ASFEnhance", MyVersion, "ASFEnhance"));
            foreach (var subModule in _Adapter_.ExtensionCore.SubModules.Values)
            {
                tasks.Add(WebRequest.GetPluginReleaseNote(subModule.PluginName, subModule.PluginVersion, subModule.RepoName));
            }
        }

        if (!tasks.Any())
        {
            return FormatStaticResponse("获取版本信息失败, 未找到插件 {0}", pluginNames);
        }

        var results = await Utilities.InParallel(tasks).ConfigureAwait(false);

        var sb = new StringBuilder();
        sb.AppendLine(FormatStaticResponse("插件更新信息:"));

        foreach (var info in results)
        {
            sb.AppendLine(Static.Line);
            sb.AppendLineFormat("插件名称: {0} {1}", info.PluginName, info.Tips);
            sb.AppendLineFormat("当前版本: {0} 在线版本: {1}", info.CurrentVersion, info.OnlineVersion?.ToString() ?? "-.-.-.-");
            sb.AppendLineFormat("更新日志: \n{0}", info.ReleaseNote);
        }

        return sb.ToString();
    }

    internal static async Task<string?> ResponsePluginUpdate(string? pluginNames = null)
    {
        var entries = pluginNames?.ToUpperInvariant().Split(',', StringSplitOptions.RemoveEmptyEntries);

        var tasks = new List<Task<PluginUpdateResponse>>();

        if (entries?.Any() == true)
        {
            foreach (var entry in entries)
            {
                if (entry == "ASFE" || entry == "ASFENHANCE")
                {
                    tasks.Add(WebRequest.UpdatePluginFile("ASFEnhance", MyVersion, "ASFEnhance"));
                }
                else if (_Adapter_.ExtensionCore.SubModules.TryGetValue(entry, out var subModule))
                {
                    tasks.Add(WebRequest.UpdatePluginFile(subModule.PluginName, subModule.PluginVersion, subModule.RepoName));
                }
            }
        }
        else
        {
            tasks.Add(WebRequest.UpdatePluginFile("ASFEnhance", MyVersion, "ASFEnhance"));
            foreach (var subModule in _Adapter_.ExtensionCore.SubModules.Values)
            {
                tasks.Add(WebRequest.UpdatePluginFile(subModule.PluginName, subModule.PluginVersion, subModule.RepoName));
            }
        }

        if (!tasks.Any())
        {
            return FormatStaticResponse("获取版本信息失败, 未找到插件 {0}", pluginNames);
        }

        var results = await Utilities.InParallel(tasks).ConfigureAwait(false);

        var sb = new StringBuilder();
        sb.AppendLine(FormatStaticResponse("插件更新信息:"));

        foreach (var info in results)
        {
            sb.AppendLine(Static.Line);
            sb.AppendLineFormat("插件名称: {0} {1}", info.PluginName, info.Tips);
            sb.AppendLineFormat("当前版本: {0} 在线版本: {1}", info.CurrentVersion, info.OnlineVersion?.ToString() ?? "-.-.-.-");
            sb.AppendLineFormat("更新日志: \n{0}", info.ReleaseNote);
        }

        return sb.ToString();
    }
}
