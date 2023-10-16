using ArchiSteamFarm.Core;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;

namespace ASFEnhance.ExpansionUtils;

/// <summary>
/// 插件更新相关
/// </summary>
public static class Update
{
    /// <summary>
    /// 获取链接
    /// </summary>
    /// <param name="repo"></param>
    /// <param name="useMirror"></param>
    /// <returns></returns>
    public static Uri GetGitHubApiUri(string repo, bool useMirror)
    {
        var addr = useMirror ?
            $"https://hub.chrxw.com/{repo}/releases/latest" :
            $"https://api.github.com/repos/chr233/{repo}/releases/latest";

        return new Uri(addr);
    }

    /// <summary>
    /// 获取最新的发行版
    /// </summary>
    /// <returns></returns>
    public static async Task<GitHubReleaseResponse?> GetLatestRelease(string repo, bool useMirror)
    {
        var request = GetGitHubApiUri(repo, useMirror);
        var response = await ASF.WebBrowser!.UrlGetToJsonObject<GitHubReleaseResponse>(request).ConfigureAwait(false);

        if (response == null && useMirror)
        {
            return await GetLatestRelease(repo, false).ConfigureAwait(false);
        }

        return response?.Content;
    }

    /// <summary>
    /// 下载发行版
    /// </summary>
    /// <param name="downloadUrl"></param>
    /// <returns></returns>
    public static async Task<BinaryResponse?> DownloadRelease(string? downloadUrl)
    {
        if (string.IsNullOrEmpty(downloadUrl))
        {
            return null;
        }

        var request = new Uri(downloadUrl);
        var response = await ASF.WebBrowser!.UrlGetToBinary(request).ConfigureAwait(false);
        return response;
    }
}
