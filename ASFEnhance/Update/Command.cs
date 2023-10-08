using System.IO.Compression;
using System.Text;

namespace ASFEnhance.Update;

internal static class Command
{
    /// <summary>
    /// 查看插件版本
    /// </summary>
    /// <returns></returns>
    internal static string? ResponseASFEnhanceVersion()
    {
        return FormatStaticResponse(Langs.PluginVer, nameof(ASFEnhance), MyVersion);
    }

    /// <summary>
    /// 获取插件最新版本
    /// </summary>
    /// <returns></returns>
    internal static async Task<string?> ResponseCheckLatestVersion()
    {
        var response = await WebRequest.GetLatestRelease(true).ConfigureAwait(false);

        if (response == null)
        {
            return FormatStaticResponse(Langs.GetReleaseInfoFailed);
        }

        var sb = new StringBuilder();
        sb.AppendLine(FormatStaticResponse(Langs.MultipleLineResult));

        sb.AppendLineFormat(Langs.ASFECurrentVersion, MyVersion);
        sb.AppendLineFormat(Langs.ASFEOnlineVersion, response.TagName);
        sb.AppendLineFormat(Langs.Detail, response.Body);
        sb.AppendLine(Langs.Assert);

        foreach (var asset in response.Assets)
        {
            sb.AppendLineFormat(Langs.SubName, asset.Name);
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

                        var sb = new StringBuilder();
                        sb.AppendLine(Langs.UpdateSuccess);

                        sb.AppendLine();
                        sb.AppendLineFormat(Langs.ASFECurrentVersion, MyVersion.ToString());
                        sb.AppendLineFormat(Langs.ASFEOnlineVersion, releaseResponse.TagName);
                        sb.AppendLineFormat(Langs.Detail, releaseResponse.Body);

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
