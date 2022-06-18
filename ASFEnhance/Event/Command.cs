#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using System.Globalization;
using System.Text;
using System.Collections.Concurrent;
using static ASFEnhance.Utils;
using ASFEnhance.Localization;

namespace ASFEnhance.Event
{
    internal static class Command
    {
        internal static async Task<string?> ResponseEvent(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            bool Paused = bot.CardsFarmer.Paused;

            if (!Paused)
            {
                await bot.Commands.Response(EAccess.Master, $"PAUSE {bot.BotName}").ConfigureAwait(false);
            }

            List<uint> demos = new() { 1431000, 1805630, 1937760, 1923740, 1993130, 1490520, 2015730, 1991740, 2012980, 1402220 };
            string demoStr = string.Join(',', demos.Select(x => $"app/{x}"));
            string demoStr2 = string.Join(',', demos);

            await bot.Commands.Response(EAccess.Master, $"ADDLICENSE {bot.BotName} {demoStr}").ConfigureAwait(false);
            await bot.Commands.Response(EAccess.Master, $"PLAY {bot.BotName} {demoStr2}").ConfigureAwait(false);

            if (!Paused)
            {
                await bot.Commands.Response(EAccess.Master, $"RESUME {bot.BotName}").ConfigureAwait(false);
            }

            return bot.FormatBotResponse("Done!");
        }

        internal static async Task<string?> ResponseEvent(string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseEvent(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        private static ConcurrentDictionary<string, HashSet<uint>> FailedDemos = new();
        private static ConcurrentDictionary<string, HashSet<uint>> AddedDemos = new();
        private static ConcurrentDictionary<string, string> Status = new();
        private static ConcurrentDictionary<string, DateTime> NextTime = new();

        internal static Task<string?> ResponseEventEndless(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return Task.FromResult(bot.FormatBotResponse(Strings.BotNotConnected));
            }

            _ = Task.Run(async () => {
                string ownStr = bot.FormatBotResponse(string.Format(CultureInfo.CurrentCulture, Strings.BotOwnedAlready, "")[..^1]);

                bool Paused = bot.CardsFarmer.Paused;

                string botName = bot.BotName;

                try
                {
                    List<uint> demos = new();

                    HashSet<uint> failedDemos = new();
                    HashSet<uint> addedDemos = new();

                    FailedDemos[botName] = failedDemos;
                    AddedDemos[botName] = addedDemos;
                    NextTime[botName] = DateTime.Now;

                    int index = 0;
                    while (index < DemosDB.Demos.Count)
                    {
                        Status[botName] = "Running";

                        demos.Clear();

                        int error = 5;
                        while (!bot.IsConnectedAndLoggedOn)
                        {
                            await Task.Delay(10000).ConfigureAwait(false);
                            if (error-- == 0)
                            {
                                return;
                            }
                        }

                        int count = 0;
                        while (count < 45 && index < DemosDB.Demos.Count)
                        {
                            if (!Paused)
                            {
                                await bot.Commands.Response(EAccess.Master, $"PAUSE {botName}").ConfigureAwait(false);
                            }

                            uint appid = DemosDB.Demos[index++];

                            //获取Appdetail
                            int tries = 5;
                            Data.AppDetailData? data = null;

                            while (tries-- > 0)
                            {
                                var detail = await Store.WebRequest.GetAppDetails(bot, appid).ConfigureAwait(false);
                                if (detail.Success)
                                {
                                    data = detail.Data;
                                    break;
                                }
                            }

                            if (data == null || data.Demos == null || data.Demos?.Count == 0)
                            {
                                failedDemos.Add(appid);
                                continue;
                            }

                            appid = data.Demos.First().AppID;

                            //检查是否已拥有
                            string result = await bot.Commands.Response(EAccess.Owner, $"OWNS {botName} {appid}").ConfigureAwait(false) ?? "";
                            bool owned = result.Contains(ownStr);
                            ASFLogger.LogGenericInfo(result);


                            if (!owned)
                            {
                                count++;
                                string r1 = await bot.Commands.Response(EAccess.Owner, $"ADDLICENSE {botName} app/{appid}").ConfigureAwait(false);
                                ASFLogger.LogGenericInfo(r1);
                                await Task.Delay(1000).ConfigureAwait(false);
                                result = await bot.Commands.Response(EAccess.Owner, $"OWNS {botName} {appid}").ConfigureAwait(false) ?? "";
                                ASFLogger.LogGenericInfo(result);
                                owned = result.Contains(ownStr);
                            }

                            if (!owned)
                            {
                                failedDemos.Add(appid);
                            }

                            demos.Add(appid);

                            //游玩Demo
                            if (demos.Count >= 20)
                            {
                                string arg = string.Join(',', demos);
                                await bot.Commands.Response(EAccess.Owner, $"PLAY {botName} {arg}").ConfigureAwait(false);
                                await Task.Delay(5000).ConfigureAwait(false);

                                foreach (var i in demos)
                                {
                                    addedDemos.Add(i);
                                }

                                if (!Paused)
                                {
                                    await bot.Commands.Response(EAccess.Master, $"RESUME {botName}").ConfigureAwait(false);
                                }

                                demos.Clear();
                            }
                        }

                        Status[botName] = "Running[Waiting]";
                        NextTime[botName] = DateTime.Now + TimeSpan.FromMinutes(65);
                        await Task.Delay(TimeSpan.FromMinutes(65)).ConfigureAwait(false);
                    }

                    if (demos.Count > 0)
                    {
                        string arg = string.Join(',', demos);
                        await bot.Commands.Response(EAccess.Owner, $"PLAY {botName} {arg}").ConfigureAwait(false);
                        await Task.Delay(5000).ConfigureAwait(false);

                        foreach (var i in demos)
                        {
                            addedDemos.Add(i);
                        }

                        if (!Paused)
                        {
                            await bot.Commands.Response(EAccess.Master, $"RESUME {botName}").ConfigureAwait(false);
                        }
                    }
                }
                finally
                {
                    Status[botName] = "Stopped";
                }
            });

            return Task.FromResult(bot.FormatBotResponse("Task will running in background"));
        }

        internal static async Task<string?> ResponseEventEndless(string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseEventEndless(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        internal static Task<string?> ResponseEventStatus(Bot bot)
        {
            string botName = bot.BotName;

            if (!FailedDemos.ContainsKey(botName))
            {
                return Task.FromResult(bot.FormatBotResponse("No record"));
            }

            var failedDemos = FailedDemos[botName];
            var addedDemos = AddedDemos[botName];
            var status = Status[botName];
            var runTime = NextTime[botName];

            StringBuilder sb = new();
            sb.AppendLine(bot.FormatBotResponse(Langs.MultipleLineResult));
            sb.AppendLine(string.Format("Task Status: {0}", status));
            sb.AppendLine(string.Format("Task Will Run At: {0}", runTime));
            sb.AppendLine(string.Format("Added Demos Count: {0}", addedDemos.Count));
            sb.AppendLine(string.Format("Failed Demos Count: {0}", failedDemos.Count));
            sb.AppendLine(string.Format("Total Demos Count: {0}", DemosDB.Demos.Count));

            return Task.FromResult(sb.ToString());
        }

        internal static async Task<string?> ResponseEventStatus(string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseEventStatus(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        internal static Task<string?> ResponseFailedDemos(Bot bot)
        {
            string botName = bot.BotName;

            if (!FailedDemos.ContainsKey(botName))
            {
                return Task.FromResult(bot.FormatBotResponse("No record"));
            }

            var failedDemos = FailedDemos[botName];

            string data = string.Join(',', failedDemos);

            return Task.FromResult(data);
        }

        internal static async Task<string?> ResponseFailedDemos(string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseFailedDemos(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }
    }
}
