using ArchiSteamFarm.Core;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;

namespace ASFEnhance.Update
{
    internal static class WebRequest
    {
        /// <summary>
        /// 获取最新的发行版
        /// </summary>
        /// <returns></returns>
        internal static async Task<GitHubReleaseResponse?> GetLatestRelease(bool useMirror = true)
        {
            Uri request = new(
                useMirror ? "https://hub.chrxw.com/ASFenhance/releases/latest" : "https://api.github.com/repos/chr233/ASFenhance/releases/latest"
            );
            ObjectResponse<GitHubReleaseResponse>? response = await ASF.WebBrowser!.UrlGetToJsonObject<GitHubReleaseResponse>(request).ConfigureAwait(false);

            if (response == null && useMirror)
            {
                return await GetLatestRelease(false).ConfigureAwait(false);
            }

            return response?.Content;
        }

        /// <summary>
        /// 下载发行版
        /// </summary>
        /// <param name="downloadUrl"></param>
        /// <returns></returns>
        internal static async Task<BinaryResponse?> DownloadRelease(string downloadUrl)
        {
            Uri request = new(downloadUrl);
            BinaryResponse? response = await ASF.WebBrowser!.UrlGetToBinary(request).ConfigureAwait(false);
            return response;
        }
    }
}
