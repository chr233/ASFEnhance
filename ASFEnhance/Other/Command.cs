#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using Chrxw.ASFEnhance.Localization;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static Chrxw.ASFEnhance.Utils;

namespace Chrxw.ASFEnhance.Other
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
            return string.Format(CurrentCulture, Langs.PluginVer, nameof(ASFEnhance), version.Major, version.Minor, version.Build, version.Revision);
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

            return keys.Count > 0 ? string.Join('\n', keys) : string.Format(CurrentCulture, Langs.KeyNotFound);
        }
    }
}
