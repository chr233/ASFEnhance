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

                case "P" when args.Length > 1:
                case "POINT" when args.Length > 1:
                    ASF.ArchiLogger.LogGenericInfo("多个bot");
                    return await ResponseGetPointSummary(steamID, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);
                case "P":
                case "POINT":
                    ASF.ArchiLogger.LogGenericInfo("单个bot");
                    return await ResponseGetPointSummary(bot, steamID).ConfigureAwait(false);

                case "PA":
                    return await ResponseGetPointSummary(steamID, "ASF").ConfigureAwait(false);

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


        // 获取账号点数余额
        private static async Task<string?> ResponseGetPointSummary(Bot bot, ulong steamID)
        {
            if (steamID == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(steamID));
            }

            if (!bot.IsConnectedAndLoggedOn)
            {
                return FormatBotResponse(bot, Strings.BotNotConnected);
            }

            string? message = await WebRequest.GetPointSummary(bot).ConfigureAwait(false);

            return FormatBotResponse(bot, message ?? Properties.Resources.BotPointFailed);
        }

        // 获取账号点数余额(多个bot)
        private static async Task<string?> ResponseGetPointSummary(ulong steamID, string botNames)
        {
            ASF.ArchiLogger.LogGenericInfo(botNames);

            if (steamID == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(steamID));
            }

            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return ASF.IsOwner(steamID) ? FormatStaticResponse(string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botNames)) : null;
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseGetPointSummary(bot, steamID))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        internal static string FormatStaticResponse(string response) => Commands.FormatStaticResponse(response);
        internal static string FormatBotResponse(Bot bot, string response) => bot.Commands.FormatBotResponse(response);

        public void OnLoaded()
        {
            ASF.ArchiLogger.LogGenericInfo("欢迎使用 ASFEnhance By Chr_ , 联系方式 chr@@chrxw.com");
        }
    }

}
