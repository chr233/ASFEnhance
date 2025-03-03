using ArchiSteamFarm.Web.Responses;
using System.Text;

namespace ASFEnhance.Store;

internal static class HtmlParser
{
    /// <summary>
    /// 解析搜索页
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    internal static string? ParseSearchPage(HtmlDocumentResponse? response)
    {
        if (response?.Content == null)
        {
            return null;
        }

        var gameNodes = response.Content.QuerySelectorAll("#search_resultsRows>a.search_result_row");

        if (gameNodes.Length == 0)
        {
            return Langs.SearchResultEmpty;
        }

        var result = new StringBuilder();

        result.AppendLine(Langs.MultipleLineResult);

        result.AppendLine(Langs.SearchResultTitle);

        var matchGameId = RegexUtils.MatchGameIds();

        foreach (var gameNode in gameNodes)
        {
            var eleTitle = gameNode.QuerySelector("span.title");

            if (eleTitle == null)
            {
                ASFLogger.LogGenericDebug(string.Format(Langs.SomethingIsNull, nameof(eleTitle)));
                continue;
            }

            var gameTitle = eleTitle.TextContent.Trim();
            var gameHref = gameNode?.GetAttribute("href") ?? "";

            var match = matchGameId.Match(gameHref);

            if (match.Success)
            {
                result.AppendLineFormat(Langs.AreaItem, match.Value, gameTitle);
            }
        }

        return result.ToString();
    }
}
