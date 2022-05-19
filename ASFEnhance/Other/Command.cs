#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ASFEnhance.Localization;
using System.Text.RegularExpressions;
using static ASFEnhance.Utils;

namespace ASFEnhance.Other
{
    internal static class Command
    {
        /// <summary>
        /// 查看插件版本
        /// </summary>
        /// <returns></returns>
        internal static string ResponseASFEnhanceVersion()
        {
            Version version = MyVersion;
            return string.Format(Langs.PluginVer, nameof(ASFEnhance), version.Major, version.Minor, version.Build, version.Revision);
        }

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
        /// 命令帮助
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        internal static string? ResponseHelp(string command)
        {
            return null;
        }

        internal static string? ResponseCheckUpdate()
        {
            return null;
        }

        internal static string? ResponseUpdatePlugin()
        {
            return null;
        }
    }
}
