using AngleSharp.Io;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;
using SteamKit2.GC.Dota.Internal;
using System.IO.Compression;
using System.Numerics;
using System.Text;
using static SteamKit2.GC.Underlords.Internal.CMsgIndividualPostMatchStats;

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
        return response?.Content;
    }

    /// <summary>
    /// 获取最新的发行版
    /// </summary>
    /// <returns></returns>
    private static async Task<GitHubReleaseResponse?> GetLatestRelease(string repoPath)
    {
        var splits = repoPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (!splits.Any())
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
            response.ReleaseNote = "插件不支持在线更新";
        }
        else
        {
            var relesaeData = await GetLatestRelease(pluginRepo).ConfigureAwait(false);
            if (relesaeData == null)
            {
                response.ReleaseNote = "网络错误, 无法获取发行版信息";
            }
            else
            {
                var splits = relesaeData.Body.Split("---", StringSplitOptions.RemoveEmptyEntries);
                response.ReleaseNote = (splits.Length >= 2 ? splits[1] : relesaeData.Body).Trim();
                response.OnlineVersion = Version.TryParse(relesaeData.TagName, out var version) ? version : null;
            }
        }

        return response;
    }

    /// <summary>
    /// 自动更新插件
    /// </summary>
    /// <param name="relesaeData"></param>
    /// <returns></returns>
    internal static async Task<string?> FetchDownloadUrl(GitHubReleaseResponse relesaeData)
    {
        if (!relesaeData.Assets.Any())
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
            response.ReleaseNote = "插件不支持在线更新";
        }
        else
        {
            var relesaeData = await GetLatestRelease(pluginRepo).ConfigureAwait(false);
            if (relesaeData == null)
            {
                response.ReleaseNote = "网络错误, 无法获取发行版信息";
            }
            else
            {
                response.OnlineVersion = Version.TryParse(relesaeData.TagName, out var version) ? version : null;

                if (!response.CanUpdate)
                {
                    response.ReleaseNote = response.IsLatest ? "已经是最新版本" : "当前版本号更高, 无需更新";
                }
                else
                {
                    var downloadUri = await FetchDownloadUrl(relesaeData).ConfigureAwait(false);
                    if (downloadUri == null)
                    {
                        response.ReleaseNote = "该发行版没有可下载的文件";
                    }
                    else
                    {
                        response.ReleaseNote = await DownloadRelease(downloadUri).ConfigureAwait(false);
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
            return "下载插件失败";
        }

        var zipBytes = binResponse?.Content as byte[] ?? binResponse?.Content?.ToArray();
        if (zipBytes == null)
        {
            return "下载插件失败";
        }

        var ms = new MemoryStream(zipBytes);
        try
        {
            await using (ms.ConfigureAwait(false))
            {
                using var zipArchive = new ZipArchive(ms);
                string pluginFolder = Path.GetDirectoryName(MyLocation) ?? ".";

                //string backupPath = Path.Combine(pluginFolder, $"{nameof(ASFEnhance)}.bak");
                //File.Move(currentPath, backupPath, true);

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
                                    return "下载插件失败, 存在文件冲突, 请尝试重启 ASF 后再尝试更新";
                                }
                            }

                            File.Move(targetPath, backupPath, true);
                        }

                        entry.ExtractToFile(targetPath);
                    }
                }

                return "更新插件成功";
            }
        }
        catch (Exception ex)
        {
            ASFLogger.LogGenericException(ex);
            return FormatStaticResponse("更新插件失败, 解压缩遇到错误");
        }
    }
}
