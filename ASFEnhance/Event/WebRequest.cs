#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Web;
using ProtoBuf;
using static ASFEnhance.Utils;

namespace ASFEnhance.Event
{
    internal static class WebRequest
    {
        /// <summary>
        /// 获取探索队列
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        internal static async Task<GetDiscoveryQueueResponse?> GetDiscoveryQueue(Bot bot, string token)
        {
            Uri request = new($"https://api.steampowered.com/IStoreService/GetDiscoveryQueue/v1?access_token={token}&input_protobuf_encoded=CAESAkNOGAEwAWIGCgQI/7VL");

            WebBrowser webBrowser = bot.ArchiWebHandler.WebBrowser;

            try
            {
                var response = await webBrowser.UrlGetToStream(request).ConfigureAwait(false);
                var data = Serializer.Deserialize<GetDiscoveryQueueResponse>(response.Content);

                return data;
            }
            catch (Exception ex)
            {
                ASFLogger.LogGenericException(ex);
                return null;
            }
        }

        /// <summary>
        /// 模拟探索队列
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        internal static async Task SkipDiscoveryQueueItem(Bot bot, string token, uint appid)
        {
            Uri request = new($"https://api.steampowered.com/IStoreService/SkipDiscoveryQueueItem/v1?access_token={token}");

            using MemoryStream ms = new MemoryStream();

            SkipDiscoveryQueueItemRequest payload = new() { Appid = appid };
            Serializer.Serialize(ms, payload);

            byte[] buffer = ms.ToArray();
            string b64 = Convert.ToBase64String(buffer);

            //ASFLogger.LogGenericInfo(b64);

            Dictionary<string, string> data = new() {
                {"input_protobuf_encoded", b64}
            };

            WebBrowser webBrowser = bot.ArchiWebHandler.WebBrowser;

            var response = await webBrowser.UrlPost(request, data: data).ConfigureAwait(false);
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> FetchEventToken(Bot bot)
        {
            Uri request = new(SteamStoreURL, "/sale/nextfest");

            var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

            if (response == null)
            {
                return null;
            }

            var configEle = response.Content.SelectSingleNode<IElement>("//div[@id='application_config']");

            var token = configEle?.GetAttribute("data-loyalty_webapi_token").Replace("\"", "");

            return token;
        }
    }
}
