
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ArchiSteamFarm;
using ArchiSteamFarm.Json;
using ArchiSteamFarm.Plugins;

namespace ASFEnhance
{
    internal static class SetuAPI
    {
        private const string URL = "https://api.dongmanxingkong.com/suijitupian/acg/1080p/index.php?return=json";

        internal static async Task<string?> GetRandomAnimateURL(WebBrowser webBrowser)
        {
            if (webBrowser == null)
            {
                throw new ArgumentNullException(nameof(webBrowser));
            }
            WebBrowser.ObjectResponse<MeowResponse>? response = await webBrowser.UrlGetToJsonObject<MeowResponse>(URL).ConfigureAwait(false);
            if (response?.Content == null)
            {
                return null;
            }
            if (string.IsNullOrEmpty(response.Content.Link))
            {
                throw new ArgumentNullException(nameof(response.Content.Link));
            }
            return Uri.EscapeUriString(response.Content!.Link!);
        }

        private sealed class MeowResponse
        {
#pragma warning disable 649
            [JsonProperty(PropertyName = "imgurl", Required = Required.Always)]
            internal readonly string? Link;
#pragma warning restore 649
            [JsonConstructor]
            private MeowResponse() { }
        }
    }
}
