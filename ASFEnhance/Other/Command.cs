#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Steam;
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
        internal static string ResponseASFEnhanceVersion(EAccess access)
        {
            if (access < EAccess.FamilySharing)
            {
                return null;
            }

            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            return string.Format(CurrentCulture, Langs.PluginVer, nameof(ASFEnhance), version.Major, version.Minor, version.Build, version.Revision);
        }

        /// <summary>
        /// 从文本提取Key
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        internal static string? ResponseExtractKeys(EAccess access, string message)
        {
            if (access < EAccess.FamilySharing)
            {
                return null;
            }

            List<string> keys = new();

            MatchCollection matches;
            matches = Regex.Matches(message, @"[A-Z0-9]{5}-?[A-Z0-9]{5}-?[A-Z0-9]{5}", RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                keys.Add(match.Value.ToUpperInvariant());
            }

            //matches = Regex.Matches(message, @"([A-Za-z0-9]{15})", RegexOptions.IgnoreCase);
            //foreach (Match match in matches)
            //{
            //    GroupCollection groups = match.Groups;
            //    keys.Add(groups[1].Value.ToUpperInvariant().Insert(10, "-").Insert(5, "-"));
            //}

            return keys.Count > 0 ? string.Join('\n', keys) : string.Format(CurrentCulture, Langs.KeyNotFound);
        }
    }
}
