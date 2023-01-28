using AngleSharp.Dom;
using ArchiSteamFarm.Web.Responses;
using System.Text.RegularExpressions;

namespace ASFEnhance.Friend
{
    internal static partial class HtmlParser
    {

        [GeneratedRegex("\"steamid\":\"(\\d+)\"")]
        private static partial Regex MatchSteamId();

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
                Regex regex = MatchSteamId();

                var match = regex.Match(text);
                if (match.Success)
                {
                    var strId = match.Groups[1].Value;
                    return ulong.TryParse(strId, out ulong steamId) ? steamId : null;
                }
            }
            
            return null;
        }

    }
}
