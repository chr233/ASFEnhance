#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;
using ASFEnhance.Localization;
using System.IO.Compression;
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
                if (CommandHelpData.FullCmd2ShortCmd.ContainsKey(cmd))
                {
                    string shortCmd = CommandHelpData.FullCmd2ShortCmd[cmd];
                    sb.AppendLine(string.Format(Langs.CommandHelpWithShortName, cmd, shortCmd, args, usage));
                }
                else
                {
                    sb.AppendLine(string.Format(Langs.CommandHelpNoShortName, cmd, args, usage));
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
            int count = 0;
            bool skip = true;
            foreach (string command in commands)
            {
                if (skip)
                {
                    skip = false;
                    continue;
                }
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

                    if (CommandHelpData.FullCmd2ShortCmd.ContainsKey(cmd))
                    {
                        string shortCmd = CommandHelpData.FullCmd2ShortCmd[cmd];
                        sb.AppendLine(string.Format(Langs.CommandHelpWithShortName, cmd, shortCmd, args, usage));
                    }
                    else
                    {
                        sb.AppendLine(string.Format(Langs.CommandHelpNoShortName, cmd, args, usage));
                    }
                }
            }

            if (count > 0)
            {
                sb.AppendLine();
                sb.AppendLine(Langs.HelpArgsExplain);
            }

            return FormatStaticResponse(count > 0 ? sb.ToString() : Langs.CommandHelpCmdNotFound);
        }
    }
}
