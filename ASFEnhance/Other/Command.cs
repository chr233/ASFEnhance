using ASFEnhance.Localization;
using System.Text;
using System.Text.RegularExpressions;
using static ASFEnhance.Utils;

namespace ASFEnhance.Other
{
    internal static class Command
    {
        /// <summary>
        /// 从文本提取Key
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        internal static string? ResponseExtractKeys(string message)
        {
            HashSet<string> keys = new();

            MatchCollection matches;
            matches = Regex.Matches(message, @"[A-Z0-9]{5}-?[A-Z0-9]{5}-?[A-Z0-9]{5}", RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                keys.Add(match.Value.ToUpperInvariant());
            }

            return keys.Count > 0 ? string.Join('\n', keys) : string.Format(Langs.KeyNotFound);
        }

        /// <summary>
        /// 查看所有命令
        /// </summary>
        /// <returns></returns>
        internal static string? ResponseAllCommands()
        {
            StringBuilder sb = new();
            sb.AppendLine(Langs.MultipleLineResult);
            sb.AppendLine(Langs.CommandHelp);

            foreach (var item in CommandHelpData.CommandArges)
            {
                string cmd = item.Key;
                string args = string.IsNullOrEmpty(item.Value) ? Langs.NoArgs : item.Value;
                string usage;
                if (CommandHelpData.CommandUsage.ContainsKey(cmd))
                {
                    usage = CommandHelpData.CommandUsage[cmd];
                }
                else
                {
                    usage = Langs.CommandHelpNoUsage;
                }

                sb.AppendLine(string.Format(Langs.CommandHelpNoShortName, cmd, args, usage));
                if (CommandHelpData.FullCmd2ShortCmd.ContainsKey(cmd))
                {
                    string shortCmd = CommandHelpData.FullCmd2ShortCmd[cmd];
                    sb.AppendLine(string.Format(Langs.CommandHelpWithShortName, shortCmd));
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
            StringBuilder sb = new();
            sb.AppendLine(Langs.MultipleLineResult);
            sb.AppendLine(Langs.CommandHelp);

            string call = commands[0].ToUpperInvariant();
            uint count = 0;
            if (commands.Length >= 2)
            {
                foreach (string command in commands[1..])
                {
                    string cmd = command.ToUpperInvariant();
                    if (CommandHelpData.ShortCmd2FullCmd.ContainsKey(cmd))
                    {
                        cmd = CommandHelpData.ShortCmd2FullCmd[cmd];
                    }
                    if (CommandHelpData.CommandArges.ContainsKey(cmd))
                    {
                        count++;
                        string args = CommandHelpData.CommandArges[cmd];
                        if (string.IsNullOrEmpty(args))
                        {
                            args = Langs.NoArgs;
                        }

                        string usage;
                        if (CommandHelpData.CommandUsage.ContainsKey(cmd))
                        {
                            usage = CommandHelpData.CommandUsage[cmd];
                        }
                        else
                        {
                            usage = Langs.CommandHelpNoUsage;
                        }

                        sb.AppendLine(string.Format(Langs.CommandHelpNoShortName, cmd, args, usage));
                        if (CommandHelpData.FullCmd2ShortCmd.ContainsKey(cmd))
                        {
                            string shortCmd = CommandHelpData.FullCmd2ShortCmd[cmd];
                            sb.AppendLine(string.Format(Langs.CommandHelpWithShortName, shortCmd));
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
        internal static string ResponseDevFeatureUnavilable()
        {
            return FormatStaticResponse(Langs.DevFeatureNotEnabled);
        }

        /// <summary>
        /// 提示命令不可用
        /// </summary>
        /// <returns></returns>
        internal static string ResponseEulaCmdUnavilable()
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
            if (CommandHelpData.ShortCmd2FullCmd.ContainsKey(cmd))
            {
                cmd = CommandHelpData.ShortCmd2FullCmd[cmd];
            }
            if (CommandHelpData.CommandArges.ContainsKey(cmd))
            {
                string cmdArgs = CommandHelpData.CommandArges[cmd];
                if (string.IsNullOrEmpty(cmdArgs))
                {
                    cmdArgs = Langs.NoArgs;
                }

                string usage;
                if (CommandHelpData.CommandUsage.ContainsKey(cmd))
                {
                    usage = CommandHelpData.CommandUsage[cmd];
                }
                else
                {
                    usage = Langs.CommandHelpNoUsage;
                }

                StringBuilder sb = new();
                sb.AppendLine(string.Format(Langs.CommandHelpNoShortName, cmd, cmdArgs, usage));
                if (CommandHelpData.FullCmd2ShortCmd.ContainsKey(cmd))
                {
                    string shortCmd = CommandHelpData.FullCmd2ShortCmd[cmd];
                    sb.AppendLine(string.Format(Langs.CommandHelpWithShortName, shortCmd));
                }
                return sb.ToString();
            }
            return null;
        }
    }
}
