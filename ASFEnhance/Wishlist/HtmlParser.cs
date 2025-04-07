using AngleSharp.Dom;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;

namespace ASFEnhance.Wishlist;

internal static class HtmlParser
{
    /// <summary>
    /// 解析商店页面
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    internal static CheckGameResponse ParseStorePage(HtmlDocumentResponse? response)
    {
        if (response?.Content == null)
        {
            return new(false, "网络错误");
        }

        var eleGameName = response.Content.QuerySelector("#appHubAppName");
        string gameName = eleGameName?.Text() ?? string.Format(Langs.GetStoreNameFailed);

        bool owned = response.Content.QuerySelector("div.already_in_library") != null;

        var eleWishlist = response.Content.QuerySelector("#add_to_wishlist_area_success");
        bool inWishlist = eleWishlist != null && string.IsNullOrEmpty(eleGameName?.GetAttribute("style") ?? null);

        var eleFollow = response.Content.QuerySelector("#queueBtnFollow>div:first-child");
        bool isFollow = eleFollow != null && string.IsNullOrEmpty(eleFollow.GetAttribute("style"));

        return new CheckGameResponse(true, gameName, owned, inWishlist, isFollow);
    }
}
