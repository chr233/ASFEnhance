using ArchiSteamFarm.Core;
using ASFEnhance.Data;
using ASFEnhance.Data.Plugin;
using System.IO.Compression;

namespace ASFEnhance.Update;

internal static class WebRequest
{
    /// <summary>
    /// 获取最新的发行版
    /// </summary>
    /// <returns></returns>
    private static async Task<GitHubReleaseResponse?> GetLatestRelease(Uri request)
    {
        var response = await ASF.WebBrowser!.UrlGetToJsonObject<GitHubReleaseResponse>(request).ConfigureAwait(false);
        var content = response?.Content;
        if (content != null)
        {
            var splits = content.Body.Split("---", StringSplitOptions.RemoveEmptyEntries);
            content.Body = (splits.Length >= 2 ? splits[1] : content.Body).Trim();
        }

        return content;
    }

    /// <summary>
    /// 获取最新的发行版
    /// </summary>
    /// <returns></returns>
    private static async Task<GitHubReleaseResponse?> GetLatestRelease(string repoPath)
    {
        var splits = repoPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (splits.Length == 0)
        {
            return null;
        }

        string user;
        string repo;

        if (splits.Length >= 2)
        {
            user = splits[0];
            repo = splits[1];
        }
        else
        {
            user = "chr233";
            repo = splits[0];
        }

        GitHubReleaseResponse? data = null;

        if (user == "chr233")
        {
            //使用镜像站
            data = await GetLatestRelease(new Uri($"https://hub.chrxw.com/{repo}/releases/latest")).ConfigureAwait(false);
        }

        data ??= await GetLatestRelease(new Uri($"https://api.github.com/repos/{user}/{repo}/releases/latest")).ConfigureAwait(false);

        return data;
    }

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
            var relesaeData = await GetLatestRelease(pluginRepo).ConfigureAwait(false);
            if (relesaeData == null)
            {
                response.ReleaseNote = Langs.GetReleaseInfoFailedNetworkError;
            }
            else
            {
                response.ReleaseNote = relesaeData.Body;
                response.OnlineVersion = Version.TryParse(relesaeData.TagName, out var version) ? version : null;
            }
        }

        return response;
    }

    /// <summary>
    /// 获取更新文件链接
    /// </summary>
    /// <param name="relesaeData"></param>
    /// <returns></returns>
    internal static string? FetchDownloadUrl(GitHubReleaseResponse relesaeData)
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
                return asset.DownloadUrl;
            }
        }

        //优先下载英文版本
        foreach (var asset in relesaeData.Assets)
        {
            if (asset.Name.Contains("en-US"))
            {
                return asset.DownloadUrl;
            }
        }

        //如果没有找到当前语言的版本, 则下载第一个
        return relesaeData.Assets.First().DownloadUrl;
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
            var relesaeData = await GetLatestRelease(pluginRepo).ConfigureAwait(false);
            if (relesaeData == null)
            {
                response.UpdateLog = Langs.GetReleaseInfoFailedNetworkError;
            }
            else
            {
                response.ReleaseNote = relesaeData.Body;
                response.OnlineVersion = Version.TryParse(relesaeData.TagName, out var version) ? version : null;

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
                        response.UpdateLog = await DownloadRelease(downloadUri).ConfigureAwait(false);
                    }
                }
            }
        }

        return response;
    }

    /// <summary>
    /// 下载发行版
    /// </summary>
    /// <param name="downloadUrl"></param>
    /// <returns></returns>
    internal static async Task<string> DownloadRelease(string downloadUrl)
    {
        var request = new Uri(downloadUrl);
        var binResponse = await ASF.WebBrowser!.UrlGetToBinary(request).ConfigureAwait(false);

        if (binResponse == null)
        {
            return Langs.DownloadPluginFailed;
        }

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

                foreach (var entry in zipArchive.Entries)
                {
                    if (entry.FullName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                    {
                        var targetPath = Path.Combine(pluginFolder, entry.FullName);

                        if (File.Exists(targetPath))
                        {
                            var backupPath = $"{targetPath}.autobak";
                            int i = 1;
                            while (File.Exists(backupPath) && i < 10)
                            {
                                backupPath = $"{targetPath}.{i++}.autobak";

                                if (i >= 10)
                                {
                                    return Langs.DownloadFailedFileConflict;
                                }
                            }

                            File.Move(targetPath, backupPath, true);
                        }

                        entry.ExtractToFile(targetPath);
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
