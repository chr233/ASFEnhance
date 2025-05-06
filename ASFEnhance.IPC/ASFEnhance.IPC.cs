using ArchiSteamFarm.Plugins.Interfaces;
using System.Composition;
using System.Text;

namespace ASFEnhance.IPC;

[Export(typeof(IPlugin))]
internal sealed class ASFEnhance_IPC : IGitHubPluginUpdates
{
    public string Name => "ASFEnhance.IPC";

    public Version Version => MyVersion;

    public bool CanUpdate => true;
    public string RepositoryName => "chr233/ASFEnhance";

    /// <summary>
    /// 插件加载事件
    /// </summary>
    /// <returns></returns>
    public Task OnLoaded()
    {
        var message = new StringBuilder("\n");
        message.AppendLine(Static.Line);
        message.AppendLine(Name);
        message.AppendLine(Static.Line);

        ASFLogger.LogGenericInfo(message.ToString());

        return Task.CompletedTask;
    }
}