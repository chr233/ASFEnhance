using ArchiSteamFarm.Plugins;
using ArchiSteamFarm.Plugins.Interfaces;
using ASFEnhance.Data.Plugin;
using ASFEnhance.Explorer;
using System.Collections.Frozen;
using System.Text;

namespace ASFEnhance.Update;

internal static class Command
{
    /// <summary>
    /// 获取已安装的插件列表
    /// </summary>
    /// <returns></returns>
    internal static string? ResponsePluginList()
    {
        FrozenSet<IPlugin>? activePlugins;
        try
        {
            activePlugins = typeof(PluginsCore).GetStaticPrivateProperty<FrozenSet<IPlugin>>("ActivePlugins");
        }
        catch (Exception ex)
        {
            ASFLogger.LogGenericException(ex);
            return FormatStaticResponse(Langs.SubModuleLoadFailed);
        }

        if (activePlugins != null)
        {
            var sb = new StringBuilder();
            sb.AppendLine(FormatStaticResponse(Langs.MultipleLineResult));
            sb.AppendLineFormat(Langs.PluginListTips, activePlugins.Count);

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
                    sb.AppendLineFormat(Langs.PluginListItem, index++, plugin.Name, plugin.Version, "ASFE");
                }
                else if (subModules.TryGetValue(plugin.Name, out var subModule))
                {
                    sb.AppendLineFormat(Langs.PluginListItem, index++, subModule.PluginName, subModule.PluginVersion, subModule.CmdPrefix ?? "---");
                }
                else
                {
                    sb.AppendLineFormat(Langs.PluginListItem2, index++, plugin.Name, plugin.Version);
                }
            }

            return sb.ToString();
        }
        else
        {
            //大概不可能会执行到这里
            return FormatStaticResponse(Langs.SubModuleNoModule);
        }
    }
}
