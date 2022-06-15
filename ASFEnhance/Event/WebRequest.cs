#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using AngleSharp.Dom;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Web.Responses;
using Newtonsoft.Json;
using static ASFEnhance.Utils;

namespace ASFEnhance.Event
{
    internal static class WebRequest
    {
        internal static async Task<HashSet<DemoAppResponse>?> FetchDemoAppIDs(Bot bot)
        {
            Uri request = new(SteamStoreURL, "/sale/nextfest");

            HtmlDocumentResponse response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

            if (response != null)
            {
                IElement? appconfigEle = response.Content.QuerySelector("#application_config");
                string? demoeventstore = appconfigEle?.GetAttribute("data-demoeventstore");

                if (!string.IsNullOrEmpty(demoeventstore))
                {
                    try
                    {
                        var result = JsonConvert.DeserializeObject<HashSet<DemoAppResponse>>(demoeventstore);
                        return result;
                    }
                    catch (Exception ex)
                    {
                        ASFLogger.LogGenericException(ex);
                        return null;
                    }
                }
            }

            return null;
        }
    }
}
