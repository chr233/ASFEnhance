#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Localization;
using static ASFEnhance.Utils;

namespace ASFEnhance.Event
{
    internal static class Command
    {
        internal static async Task<string?> ResponseEvent(Bot bot, bool endless)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            HashSet<DemoAppResponse>? demoAppIDs = await Event.WebRequest.FetchDemoAppIDs(bot).ConfigureAwait(false);

            if (demoAppIDs == null)
            {
                return bot.FormatBotResponse(Langs.NetworkError);

            }

            bool Paused = bot.CardsFarmer.Paused;

            if (!Paused)
            {
                await bot.Commands.Response(EAccess.Master, "pause").ConfigureAwait(false);
            }

            _ = Task.Run(async () => {
                int i = 0;
                foreach (var demo in demoAppIDs)
                {
                    uint appID = demo.DemoAppID;

                    if (appID != 0)
                    {
                        ASFLogger.LogGenericInfo($"{bot.BotName} 入库 Demo: {appID}");
                        await bot.Commands.Response(EAccess.Master, $"addlicense {bot.BotName} a/{appID}").ConfigureAwait(false);
                        await Task.Delay(TimeSpan.FromSeconds(10)).ConfigureAwait(false);

                        ASFLogger.LogGenericInfo($"{bot.BotName} 游玩 Demo: {appID}");
                        await bot.Commands.Response(EAccess.Master, $"play {bot.BotName} {appID}").ConfigureAwait(false);
                        await Task.Delay(TimeSpan.FromSeconds(10)).ConfigureAwait(false);

                        if (endless)
                        {
                            if (i++ >= 40)
                            {
                                i = 0;
                                await bot.Commands.Response(EAccess.Master, $"resume {bot.BotName}").ConfigureAwait(false);
                                await Task.Delay(TimeSpan.FromHours(1)).ConfigureAwait(false);
                                if (!Paused)
                                {
                                    await bot.Commands.Response(EAccess.Master, "pause").ConfigureAwait(false);
                                }
                            }
                        }
                        else
                        {
                            if (i++ >= 10)
                            {
                                break;
                            }
                        }

                    }

                    if (!bot.IsConnectedAndLoggedOn)
                    {
                        break;
                    }
                }

                if (!Paused)
                {
                    await bot.Commands.Response(EAccess.Master, "resume").ConfigureAwait(false);
                }
            });

            return bot.FormatBotResponse("任务将在后台运行");
        }

        internal static async Task<string?> ResponseEvent(string botNames, bool endless)
        {
            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseEvent(bot, endless))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }
    }
}
