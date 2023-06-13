using AngleSharp.Dom;
using ArchiSteamFarm.Web.Responses;
using System.Text.RegularExpressions;

namespace ASFEnhance.Friend;

internal static class HtmlParser
{
    /// <summary>
    /// 解析个人资料页面
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    internal static ulong? ParseProfilePage(HtmlDocumentResponse? response)
    {
        if (response?.Content == null)
        {
            return null;
        }

        var scriptNode = response.Content.QuerySelector<IElement>("#responsive_page_template_content>script");
        var text = scriptNode?.Text();

        if (!string.IsNullOrEmpty(text))
        {
            Regex regex = RegexUtils.MatchSteamId();

            var match = regex.Match(text);
            if (match.Success)
            {
                var strId = match.Groups[1].Value;
                return ulong.TryParse(strId, out ulong steamId) ? steamId : null;
            }
        }

        return null;
    }

    /// <summary>
    /// 解析添加好友链接前缀
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    internal static string? ParseInviteLinkPrefix(HtmlDocumentResponse? response)
    {
        if (response?.Content == null)
        {
            return null;
        }

        var configEle = response.Content.QuerySelector<IElement>("#application_config");

        string? configJson = configEle?.GetAttribute("data-userinfo");

        if (string.IsNullOrEmpty(configJson))
        {
            return null;
        }

        var match = RegexUtils.MatchShortLink().Match(configJson);
        return match.Success ? match.Groups[1].Value.Replace("\\/", "/") : null;
    }
}
