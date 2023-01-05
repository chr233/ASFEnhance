using ASFEnhance.Data;
using ASFEnhance.Localization;
using System.IO.Compression;
using System.Text;
using static ASFEnhance.Utils;

namespace ASFEnhance.Update
{
    internal static class Command
    {
        /// <summary>
        /// 查看插件版本
        /// </summary>
        /// <returns></returns>
        internal static string ResponseASFEnhanceVersion()
        {
            return FormatStaticResponse(string.Format(Langs.PluginVer, nameof(ASFEnhance), MyVersion.ToString()));
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

            sb.AppendLine(string.Format(Langs.ASFECurrentVersion, MyVersion.ToString()));
            sb.AppendLine(string.Format(Langs.ASFEOnlineVersion, response.TagName));
            sb.AppendLine(string.Format(Langs.Detail, response.Body));
            sb.AppendLine(Langs.Assert);

            foreach (var asset in response.Assets)
            {
                sb.AppendLine(string.Format(Langs.SubName, asset.Name));
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
            var releaseResponse = await WebRequest.GetLatestRelease(true).ConfigureAwait(false);

            if (releaseResponse == null)
            {
                return FormatStaticResponse(Langs.GetReleaseInfoFailed);
            }

            if (MyVersion.ToString() == releaseResponse.TagName)
            {
                return FormatStaticResponse(Langs.AlreadyLatest);
            }

            string langVersion = Langs.CurrentLanguage;
            string? downloadUrl = null;

            foreach (var asset in releaseResponse.Assets)
            {
                if (asset.Name.Contains(langVersion))
                {
                    downloadUrl = asset.DownloadUrl;
                    break;
                }
            }

            if (string.IsNullOrEmpty(downloadUrl) && releaseResponse.Assets.Any())
            {
                downloadUrl = releaseResponse.Assets?.First().DownloadUrl;
            }

            var binResponse = await WebRequest.DownloadRelease(downloadUrl).ConfigureAwait(false);

            if (binResponse == null)
            {
                return FormatStaticResponse(Langs.DownloadFailed);
            }

            var zipBytes = binResponse?.Content as byte[] ?? binResponse?.Content?.ToArray();

            if (zipBytes == null)
            {
                return FormatStaticResponse(Langs.DownloadFailed);
            }

            MemoryStream ms = new(zipBytes);

            try
            {
                await using (ms.ConfigureAwait(false))
                {
                    using ZipArchive zipArchive = new(ms);

                    string currentPath = MyLocation ?? ".";
                    string pluginFolder = Path.GetDirectoryName(currentPath) ?? ".";
                    string backupPath = Path.Combine(pluginFolder, $"{nameof(ASFEnhance)}.bak");

                    File.Move(currentPath, backupPath, true);

                    foreach (var entry in zipArchive.Entries)
                    {
                        if (entry.FullName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                        {
                            entry.ExtractToFile(currentPath);
                            UpdatePadding = true;

                            StringBuilder sb = new();
                            sb.AppendLine(Langs.UpdateSuccess);

                            sb.AppendLine();
                            sb.AppendLine(string.Format(Langs.ASFECurrentVersion, MyVersion.ToString()));
                            sb.AppendLine(string.Format(Langs.ASFEOnlineVersion, releaseResponse.TagName));
                            sb.AppendLine(string.Format(Langs.Detail, releaseResponse.Body));

                            return sb.ToString();
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
