using ArchiSteamFarm.Steam;
using System.Text;

namespace ASFEnhance.Other;

internal static class Command
{
    /// <summary>
    /// 从文本提取Key
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    internal static string? ResponseExtractKeys(string message)
    {
        var keys = new HashSet<string>();

        var matches = RegexUtils.MatchGameKey().Matches(message);
        foreach (var match in matches.ToList())
        {
            keys.Add(match.Value.ToUpperInvariant());
        }

        return keys.Count > 0 ? string.Join('\n', keys) : Langs.KeyNotFound;
    }

    /// <summary>
    /// 查看所有命令
    /// </summary>
    /// <returns></returns>
    internal static string? ResponseAllCommands()
    {
        var sb = new StringBuilder();
        sb.AppendLine(Langs.MultipleLineResult);
        sb.AppendLine(Langs.CommandHelp);

        foreach (var item in CommandHelpData.CommandArges)
        {
            var cmd = item.Key;
            var args = string.IsNullOrEmpty(item.Value) ? Langs.NoArgs : item.Value;
            if (!CommandHelpData.CommandUsage.TryGetValue(cmd, out var usage))
            {
                usage = Langs.CommandHelpNoUsage;
            }

            sb.AppendLineFormat(Langs.CommandHelpNoShortName, cmd, args, usage);
            if (CommandHelpData.FullCmd2ShortCmd.TryGetValue(cmd, out var shortCmd))
            {
                sb.AppendLineFormat(Langs.CommandHelpWithShortName, shortCmd);
            }
        }

        return FormatStaticResponse(sb.ToString());
    }

    /// <summary>
    /// 命令帮助
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    internal static string? ResponseCommandHelp(string[] commands)
    {
        var sb = new StringBuilder();
        sb.AppendLine(Langs.MultipleLineResult);
        sb.AppendLine(Langs.CommandHelp);

        var call = commands[0].ToUpperInvariant();
        uint count = 0;
        if (commands.Length >= 2)
        {
            foreach (var command in commands[1..])
            {
                var upperCmd = command.ToUpperInvariant();
                if (!CommandHelpData.ShortCmd2FullCmd.TryGetValue(upperCmd, out var cmd))
                {
                    cmd = upperCmd;
                }
                // 精确匹配
                if (CommandHelpData.CommandArges.TryGetValue(cmd, out var args))
                {
                    count++;
                    if (string.IsNullOrEmpty(args))
                    {
                        args = Langs.NoArgs;
                    }

                    if (!CommandHelpData.CommandUsage.TryGetValue(cmd, out var usage))
                    {
                        usage = Langs.CommandHelpNoUsage;
                    }

                    sb.AppendLineFormat(Langs.CommandHelpNoShortName, cmd, args, usage);
                    if (CommandHelpData.FullCmd2ShortCmd.TryGetValue(cmd, out var shortCmd))
                    {
                        sb.AppendLineFormat(Langs.CommandHelpWithShortName, shortCmd);
                    }
                }
                else
                {
                    // 模糊匹配
                    foreach (var (key, value) in CommandHelpData.CommandArges)
                    {
                        if (key.Contains(upperCmd))
                        {
                            count++;
                            if (string.IsNullOrEmpty(value))
                            {
                                args = Langs.NoArgs;
                            }
                            else
                            {
                                args = value;
                            }

                            if (!CommandHelpData.CommandUsage.TryGetValue(key, out var usage))
                            {
                                usage = Langs.CommandHelpNoUsage;
                            }

                            sb.AppendLineFormat(Langs.CommandHelpNoShortName, key, args, usage);
                            if (CommandHelpData.FullCmd2ShortCmd.TryGetValue(cmd, out var shortCmd))
                            {
                                sb.AppendLineFormat(Langs.CommandHelpWithShortName, shortCmd);
                            }
                        }
                    }
                }
            }

            if (count > 0)
            {
                sb.AppendLine();
                sb.AppendLine(Langs.HelpArgsExplain);
            }
        }

        if (count == 0 && call != "HELP")
        {
            return FormatStaticResponse(Langs.CommandHelpCmdNotFound);
        }
        else if (count > 0)
        {
            return FormatStaticResponse(sb.ToString());
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 提示命令不可用
    /// </summary>
    /// <returns></returns>
    internal static string? ResponseDevFeatureUnavilable()
    {
        return FormatStaticResponse(Langs.DevFeatureNotEnabled);
    }

    /// <summary>
    /// 提示命令不可用
    /// </summary>
    /// <returns></returns>
    internal static string? ResponseEulaCmdUnavilable()
    {
        return FormatStaticResponse(Langs.EulaCmdUnavilable);
    }

    /// <summary>
    /// 可用时显示命令提示
    /// </summary>
    /// <param name="cmd"></param>
    /// <returns></returns>
    internal static string? ShowUsageIfAvilable(string cmd)
    {
        if (CommandHelpData.ShortCmd2FullCmd.TryGetValue(cmd, out var fullCmd))
        {
            cmd = fullCmd;
        }

        if (CommandHelpData.CommandArges.TryGetValue(cmd, out var cmdArgs))
        {
            if (string.IsNullOrEmpty(cmdArgs))
            {
                cmdArgs = Langs.NoArgs;
            }

            if (!CommandHelpData.CommandUsage.TryGetValue(cmd, out var usage))
            {
                usage = Langs.CommandHelpNoUsage;
            }

            var sb = new StringBuilder();
            sb.AppendLineFormat(Langs.CommandHelpNoShortName, cmd, cmdArgs, usage);
            if (CommandHelpData.FullCmd2ShortCmd.TryGetValue(cmd, out var shortCmd))
            {
                sb.AppendLineFormat(Langs.CommandHelpWithShortName, shortCmd);
            }
            return sb.ToString();
        }
        return null;
    }

    /// <summary>
    /// Dump执行结果到文件
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="access"></param>
    /// <param name="command"></param>
    /// <param name="steamId"></param>
    /// <returns></returns>
    internal static string? ResponseDumpToFile(Bot bot, EAccess access, string command, ulong steamId)
    {
        var filePath = Path.Combine(MyDirectory, $"ASFEDump_{DateTime.Now:yyyy-MM-dd}.txt");

        _ = Task.Run(async () =>
        {
            try
            {
                using var file = File.CreateText(filePath);
                string result;
                try
                {
                    result = await bot.Commands.Response(access, command, steamId).ConfigureAwait(false) ?? "NULL";
                }
                catch (Exception ex)
                {
                    result = string.Format("命令执行遇到内部错误: {0}, {1}", ex.Message, ex.StackTrace);
                }
                await file.WriteAsync(result).ConfigureAwait(false);
                await file.FlushAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ASFLogger.LogGenericError(string.Format("写入文件至 {0} 失败", filePath));
                ASFLogger.LogGenericException(ex);
            }
        });

        return string.Format("命令异步执行中, 执行结果将保存至 {0}", filePath);
    }
}
