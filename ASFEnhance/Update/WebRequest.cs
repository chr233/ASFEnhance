using ArchiSteamFarm.Core;
using ArchiSteamFarm.Web.GitHub;
using ArchiSteamFarm.Web.GitHub.Data;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;
using System.IO.Compression;

namespace ASFEnhance.Update;

internal static class WebRequest
{
    /// <summary>
    /// 获取发行版信息
    /// </summary>
    /// <param name="pluginName"></param>
    /// <param name="pluginVersion"></param>
    /// <param name="pluginRepo"></param>
    /// <returns></returns>
    internal static async Task<PluginUpdateResponse> GetPluginReleaseNote(string pluginName, Version pluginVersion, string? pluginRepo)
    {
        var response = new PluginUpdateResponse
        {
            PluginName = pluginName,
            CurrentVersion = pluginVersion,
            OnlineVersion = null,
            ReleaseNote = "",
        };

        if (string.IsNullOrEmpty(pluginRepo))
        {
            response.ReleaseNote = Langs.PluginUpdateNotSupport;
        }
        else
        {
            if (!pluginRepo.Contains('/'))
            {
                pluginRepo = "chr233/" + pluginRepo;
            }

            var relesaeData = await GitHubService.GetLatestRelease(pluginRepo, true, default).ConfigureAwait(false);
            if (relesaeData == null)
            {
                response.ReleaseNote = Langs.GetReleaseInfoFailedNetworkError;
            }
            else
            {
                response.ReleaseNote = relesaeData.MarkdownBody;
                response.OnlineVersion = Version.TryParse(relesaeData.Tag, out var version) ? version : null;
            }
        }

        return response;
    }

    /// <summary>
    /// 获取更新文件链接
    /// </summary>
    /// <param name="relesaeData"></param>
    /// <returns></returns>
    internal static Uri? FetchDownloadUrl(ReleaseResponse relesaeData)
    {
        if (relesaeData.Assets.Count == 0)
        {
            return null;
        }

        //优先下载当前语言的版本
        foreach (var asset in relesaeData.Assets)
        {
            if (asset.Name.Contains(Langs.CurrentLanguage))
            {
                return asset.DownloadURL;
            }
        }

        //优先下载英文版本
        foreach (var asset in relesaeData.Assets)
        {
            if (asset.Name.Contains("en-US"))
            {
                return asset.DownloadURL;
            }
        }

        //如果没有找到当前语言的版本, 则下载第一个
        return relesaeData.Assets.FirstOrDefault()?.DownloadURL;
    }

    /// <summary>
    /// 自动更新插件
    /// </summary>
    /// <param name="pluginName"></param>
    /// <param name="pluginVersion"></param>
    /// <param name="pluginRepo"></param>
    /// <returns></returns>
    internal static async Task<PluginUpdateResponse> UpdatePluginFile(string pluginName, Version pluginVersion, string? pluginRepo)
    {
        var response = new PluginUpdateResponse
        {
            PluginName = pluginName,
            CurrentVersion = pluginVersion,
            OnlineVersion = null,
            ReleaseNote = "",
        };

        if (string.IsNullOrEmpty(pluginRepo))
        {
            response.UpdateLog = Langs.PluginUpdateNotSupport;
        }
        else
        {
            if (!pluginRepo.Contains('/'))
            {
                pluginRepo = "chr233/" + pluginRepo;
            }

            var relesaeData = await GitHubService.GetLatestRelease(pluginRepo, true, default).ConfigureAwait(false);
            if (relesaeData == null)
            {
                response.UpdateLog = Langs.GetReleaseInfoFailedNetworkError;
            }
            else
            {
                response.ReleaseNote = relesaeData.MarkdownBody;
                response.OnlineVersion = Version.TryParse(relesaeData.Tag, out var version) ? version : null;

                if (!response.CanUpdate)
                {
                    response.UpdateLog = response.IsLatest ? Langs.AlreadyLatest : Langs.VersionHigher;
                }
                else
                {
                    var downloadUri = FetchDownloadUrl(relesaeData);
                    if (downloadUri == null)
                    {
                        response.UpdateLog = Langs.NoAssetFoundInReleaseInfo;
                    }
                    else
                    {
                        var mirrorUrl = new Uri(downloadUri.AbsoluteUri.Replace("https://github.com/chr233", "https://dl.chrxw.com"));
                        var binResponse = await DownloadRelease(mirrorUrl).ConfigureAwait(false) ?? await DownloadRelease(downloadUri).ConfigureAwait(false);

                        if (binResponse == null)
                        {
                            response.UpdateLog = Langs.DownloadFailed;
                        }
                        else
                        {
                            response.UpdateLog = await UnzipRelease(binResponse).ConfigureAwait(false);
                        }
                    }
                }
            }
        }

        return response;
    }

    /// <summary>
    /// 下载发行版
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    internal static Task<BinaryResponse?> DownloadRelease(Uri request)
    {
        return ASF.WebBrowser?.UrlGetToBinary(request) ?? Task.FromResult<BinaryResponse?>(null);
    }

    /// <summary>
    /// 下载发行版
    /// </summary>
    /// <param name="binResponse"></param>
    /// <returns></returns>
    internal static async Task<string> UnzipRelease(BinaryResponse binResponse)
    {
        var zipBytes = binResponse?.Content as byte[] ?? binResponse?.Content?.ToArray();
        if (zipBytes == null)
        {
            return Langs.DownloadPluginFailed;
        }

        var ms = new MemoryStream(zipBytes);
        try
        {
            await using (ms.ConfigureAwait(false))
            {
                using var zipArchive = new ZipArchive(ms);
                string pluginFolder = MyDirectory;

                var updateFolder = Path.Combine(pluginFolder, "_ASFEnhanceTemp_");
                if (Directory.Exists(updateFolder))
                {
                    Directory.Delete(updateFolder, true);
                }

                zipArchive.ExtractToDirectory(updateFolder);

                foreach (var filePath in Directory.GetFiles(updateFolder, "*.dll"))
                {
                    var pluginName = Path.GetFileName(filePath);

                    var oldPluginPath = Path.Combine(pluginFolder, pluginName);

                    if (File.Exists(oldPluginPath))
                    {
                        var originPath = Path.Combine(pluginFolder, pluginName);
                        var backupPath = $"{oldPluginPath}.autobak";

                        int i = 1;
                        while (File.Exists(backupPath))
                        {
                            backupPath = $"{oldPluginPath}.{i++}.autobak";

                            if (i >= 10)
                            {
                                return Langs.DownloadFailedFileConflict;
                            }
                        }

                        File.Move(oldPluginPath, backupPath, true);
                        File.Move(filePath, oldPluginPath, true);
                    }
                }

                return Langs.DownloadPluginSuccess;
            }
        }
        catch (Exception ex)
        {
            ASFLogger.LogGenericException(ex);
            return Langs.DownloadPluginFailedUnzipError;
        }
    }
}
