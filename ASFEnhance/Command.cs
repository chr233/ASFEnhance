using System;
using System.Collections.Generic;
using System.Composition;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ArchiSteamFarm;
using ArchiSteamFarm.Json;
using ArchiSteamFarm.Localization;
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
                case "K":
                case "KEY":
                    return ResponseGrubKeys(message);

                case "PA":
                    return await bot.Commands.Response(steamID, "POINTS ASF").ConfigureAwait(false);

                case "LA":
                    return await bot.Commands.Response(steamID, "LEVEL ASF").ConfigureAwait(false);

                case "BA":
                    return await bot.Commands.Response(steamID, "BALANCE ASF").ConfigureAwait(false);

                //case "ADD" when args.Length > 1:
                //    return await bot.Commands.Response(steamID, "ADDLICENSE" + string.Join(",", args)).ConfigureAwait(false);


                default:
                    return null;
            }
        }

        // 提取KEY
        private static string? ResponseGrubKeys(string message)
        {
            string rs = GrubKeys.String2keysString(message);
            return !string.IsNullOrEmpty(rs) ? rs : "未找到结果";
        }

        public void OnLoaded()
        {
            ASF.ArchiLogger.LogGenericInfo("欢迎使用 ASFEnhance By Chr_ , 联系方式 chr@@chrxw.com");
        }
    }

}
