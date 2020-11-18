using System;
using System.Collections.Generic;
using System.Composition;
using System.Threading.Tasks;
using ArchiSteamFarm;
using ArchiSteamFarm.Json;
using ArchiSteamFarm.Plugins;
using SteamKit2;

namespace ASFEnhance
{
    [Export(typeof(IPlugin))]
    internal sealed class ASFEnhance : IBotCommand
    {
        public string Name => nameof(ASFEnhance);
        public Version Version => typeof(ASFEnhance).Assembly.GetName().Version ?? throw new ArgumentNullException(nameof(Version));


        public async Task<string?> OnBotCommand(Bot bot, ulong steamID, string message, string[] args)
        {
            switch (args[0].ToUpperInvariant())
            {
                case "K" when bot.HasPermission(steamID, BotConfig.EPermission.FamilySharing):
                case "KEY" when bot.HasPermission(steamID, BotConfig.EPermission.FamilySharing):
                    ASF.ArchiLogger.LogGenericInfo(message);
                    string rs = GrubKeys.String2keysString(message);
                    return !string.IsNullOrEmpty(rs) ? rs : "出了点小问题";
                case "ST" when bot.HasPermission(steamID, BotConfig.EPermission.FamilySharing):
                case "SETU" when bot.HasPermission(steamID, BotConfig.EPermission.FamilySharing):
                    string picURL = await SetuAPI.GetRandomAnimateURL(bot.ArchiWebHandler.WebBrowser).ConfigureAwait(false);
                    return !string.IsNullOrEmpty(picURL) ? picURL : "出了点小问题";
                default:
                    return null;
            }
        }


        public void OnLoaded()
        {
            //ASF.ArchiLogger.LogGenericInfo("插件已加载");
        }
    }

}
