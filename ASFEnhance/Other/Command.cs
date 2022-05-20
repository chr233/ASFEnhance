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
        /// 查看插件版本
        /// </summary>
        /// <returns></returns>
        internal static string ResponseASFEnhanceVersion()
        {
            return string.Format(Langs.PluginVer, nameof(ASFEnhance), MyVersion.ToString());
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
        /// <param name="commands"></param>
        /// <returns></returns>
        internal static string? ResponseHelp(string[] commands)
        {
            StringBuilder sb = new();

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
                if (Response.ShortCommands.ContainsKey(cmd))
                {
                    cmd = Response.ShortCommands[cmd];
                }
                if (Response.CommandUsage.ContainsKey(cmd))
                {
                    count++;
                    string usage = Response.CommandUsage[cmd];

                    if (string.IsNullOrEmpty(usage))
                    {
                        usage = Langs.NoArgs;
                    }

                    if (Response.FullCommand.ContainsKey(cmd))
                    {
                        string shortCmd = Response.FullCommand[cmd];
                        sb.AppendLine(string.Format(Langs.CommandHelpWithShortName, cmd, shortCmd, usage));
                    }
                    else
                    {
                        sb.AppendLine(string.Format(Langs.CommandHelpNoShortName, cmd, usage));
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

        /// <summary>
        /// 获取插件最新版本
        /// </summary>
        /// <returns></returns>
        internal static async Task<string?> ResponseCheckLatestVersion()
        {
            GitHubReleaseResponse? response = await WebRequest.GetLatestRelease(true).ConfigureAwait(false);

            if (response == null)
            {
                return FormatStaticResponse(Langs.GetReleaseInfoFailed);
            }

            StringBuilder sb = new();
            sb.AppendLine(FormatStaticResponse(Langs.MultipleLineResult));

            sb.AppendLine(Langs.ASFECurrentVersion);
            sb.AppendLine(string.Format(Langs.ASFEPluginVersion, MyVersion.ToString()));

            sb.AppendLine(Langs.ASFEOnlineVersion);
            sb.AppendLine(string.Format(Langs.AppDetailName, response.Name));
            sb.AppendLine(string.Format(Langs.Online, response.TagName));
            sb.AppendLine(string.Format(Langs.Detail, response.Body));
            sb.AppendLine(Langs.Assert);

            foreach (GitHubAssetsData asset in response.Assets)
            {
                sb.AppendLine(string.Format(Langs.SubName, asset.Name));
                sb.AppendLine(string.Format(Langs.SubSize, asset.Size / 1024.0));
                sb.AppendLine(string.Format(Langs.SubLink, asset.DownloadUrl));
            }

            sb.AppendLine(Langs.UpdateTips);

            return sb.ToString();
        }

        /// <summary>
        /// 自动更新插件
        /// </summary>
        /// <returns></returns>
        internal static async Task<string?> ResponseUpdatePlugin()
        {
            GitHubReleaseResponse releaseResponse = await WebRequest.GetLatestRelease(true).ConfigureAwait(false);

            if (releaseResponse == null)
            {
                return FormatStaticResponse(Langs.GetReleaseInfoFailed);
            }

            if (MyVersion.ToString() == releaseResponse.TagName)
            {
                return FormatStaticResponse(Langs.AlreadyLatest);
            }

            string langVersion = Langs.CurrentLanguage;
            string downloadUrl = "";
            foreach (GitHubAssetsData asset in releaseResponse.Assets)
            {
                if (asset.Name.Contains(langVersion))
                {
                    downloadUrl = asset.DownloadUrl;
                    break;
                }
            }

            if (string.IsNullOrEmpty(downloadUrl) && releaseResponse.Assets.Any())
            {
                downloadUrl = releaseResponse.Assets.First().DownloadUrl;
            }

            BinaryResponse binResponse = await WebRequest.DownloadRelease(downloadUrl).ConfigureAwait(false);

            if (binResponse == null)
            {
                return FormatStaticResponse(Langs.DownloadFailed);
            }

            byte[] zipBytes = binResponse.Content as byte[] ?? binResponse.Content.ToArray(); ;

            MemoryStream ms = new(zipBytes);

            try
            {
                await using (ms.ConfigureAwait(false))
                {
                    using ZipArchive zipArchive = new(ms);

                    string currentPath = MyLocation;
                    string pluginFolder = Path.GetDirectoryName(currentPath);
                    string backupPath = Path.Combine(pluginFolder, $"{nameof(ASFEnhance)}.bak");

                    File.Move(currentPath, backupPath);

                    foreach (ZipArchiveEntry entry in zipArchive.Entries)
                    {
                        if (entry.FullName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                        {
                            entry.ExtractToFile(currentPath);
                            UpdateTips = true;
                            return FormatStaticResponse(Langs.UpdateSuccess);
                        }
                    }
                    File.Move(backupPath, currentPath);
                    return Langs.UpdateFiledWithZip;
                }
            }
            catch (Exception e)
            {
                ASFLogger.LogGenericException(e);
                return FormatStaticResponse(Langs.UpdateFiledWithZip);
            }
        }
    }
}
