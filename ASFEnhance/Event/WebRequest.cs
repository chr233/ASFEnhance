#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Steam;
using static ASFEnhance.Utils;

namespace ASFEnhance.Event
{
    internal static class WebRequest
    {
        /// <summary>
        /// 调用外部API解析Protobuf
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="content">base64编码后的protobuf</param>
        /// <returns></returns>
        internal static async Task<APIResponse?> ExternalAPI(Bot bot, string content)
        {
            Uri request = new($"http://e.chrxw.com/event?content={content}");

            var response = await bot.ArchiWebHandler.UrlGetToJsonObjectWithSession<APIResponse>(request).ConfigureAwait(false);

            var result = response?.Content;

            if (result?.Success == true)
            {
                return result;
            }
            else
            {
                ASFLogger.LogGenericError($"API response error: {result}");
                return null;
            }
        }

        /// <summary>
        /// 获取探索队列
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        internal static async Task<string?> GetDiscoveryQueue(Bot bot, string token)
        {
            Uri request = new($"https://api.steampowered.com/IStoreService/GetDiscoveryQueue/v1?access_token={token}&input_protobuf_encoded=CAESAkhLGAEwAWIGCgQI/7VL");

            HttpClient httpClient = new();

            try
            {
                var response = await httpClient.GetByteArrayAsync(request).ConfigureAwait(false);
                string b64Result = Convert.ToBase64String(response);
                return b64Result;
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
        internal static async Task SkipDiscoveryQueueItem(Bot bot, string token, string payload)
        {
            Uri request = new($"https://api.steampowered.com/IStoreService/SkipDiscoveryQueueItem/v1?access_token={token}");

            Dictionary<string, string> data = new() {
                    {"input_protobuf_encoded", payload}
            };

            var response = await bot.ArchiWebHandler.UrlPostToHtmlDocumentWithSession(request, data: data).ConfigureAwait(false);

        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="index"></param>
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
