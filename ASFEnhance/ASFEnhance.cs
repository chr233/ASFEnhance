using ArchiSteamFarm.Core;
using ArchiSteamFarm.Plugins.Interfaces;
using ArchiSteamFarm.Steam;
using ASFEnhance.Localization;
using System;
using System.Composition;
using System.Globalization;
using System.Threading.Tasks;

namespace Chrxw.ASFEnhance
{
    [Export(typeof(IPlugin))]
    internal sealed class ASFEnhance : IBotCommand
    {
        public string Name => nameof(ASFEnhance);
        public Version Version => typeof(ASFEnhance).Assembly.GetName().Version ?? throw new ArgumentNullException(nameof(Version));

        public void OnLoaded()
        {
            Langs.Culture = CultureInfo.CurrentCulture;

            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            ASF.ArchiLogger.LogGenericInfo(string.Format(Langs.PluginVer, version.Major, version.Minor, version.Build, version.Revision));
            ASF.ArchiLogger.LogGenericInfo("作者 Chr_, 联系方式 chr@chrxw.com");
        }

        public async Task<string?> OnBotCommand(Bot bot, ulong steamID, string message, string[] args)
        {
            return await Command.ProcessCommand(bot, steamID, message, args).ConfigureAwait(false);
        }
    }
}
