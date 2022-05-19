using ArchiSteamFarm.Core;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;

namespace ASFEnhance.Other
{
    internal static class WebRequest
    {
        /// <summary>
        /// 获取最新的发行版
        /// </summary>
        /// <returns></returns>
        internal static async Task<GitHubReleaseResponse?> GetLatestRelease()
        {
            Uri request = new("https://api.github.com/repos/chr233/ASFenhance/releases/latest");

            ObjectResponse<GitHubReleaseResponse>? response = await ASF.WebBrowser.UrlGetToJsonObject<GitHubReleaseResponse>(request).ConfigureAwait(false);

            return response?.Content;
        }


        private static async Task<GitHubReleaseResponse?> DownloadRelease(string downloadUrl)
        {
            Uri request = new(downloadUrl);

            BinaryResponse response = await ASF.WebBrowser.UrlGetToBinary(request).ConfigureAwait(false);


            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (ASF.WebBrowser == null)
            {
                throw new InvalidOperationException(nameof(ASF.WebBrowser));
            }


        }

    }
}
