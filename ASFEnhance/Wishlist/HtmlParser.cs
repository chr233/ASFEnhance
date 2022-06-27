#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using AngleSharp.Dom;
using AngleSharp.XPath;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;
using ASFEnhance.Localization;

namespace ASFEnhance.Wishlist
{
    internal static class HtmlParser
    {
        /// <summary>
        /// 解析商店页面
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static CheckGameResponse? ParseStorePage(HtmlDocumentResponse response)
        {
            if (response == null)
            {
                return null;
            }

            IElement? eleGameName = response.Content.SelectSingleNode("//div[@id='appHubAppName']|//div[@class='page_title_area game_title_area']/h2");
            string gameName = eleGameName?.Text() ?? string.Format(Langs.GetStoreNameFailed);

            bool owned = response.Content.SelectSingleNode("//div[@class='already_in_library']") != null;

            IElement? eleWishlist = response.Content.SelectSingleNode("//div[@id='add_to_wishlist_area_success']");
            bool inWishlist = eleWishlist != null ? string.IsNullOrEmpty(eleGameName.GetAttribute("style")) : false;

            IElement eleFollow = response.Content.SelectSingleNode("//div[@id='queueBtnFollow']/div[1]");
            bool isFollow = eleFollow != null ? string.IsNullOrEmpty(eleFollow.GetAttribute("style")) : false;

            return new(true, gameName, owned, inWishlist, isFollow);
        }
    }
}
