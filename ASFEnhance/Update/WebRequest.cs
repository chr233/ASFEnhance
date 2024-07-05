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
}
