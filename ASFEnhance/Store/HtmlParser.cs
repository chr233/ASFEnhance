using AngleSharp.Dom;
using AngleSharp.XPath;
using ArchiSteamFarm.Core;
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

        var gameNodes = response.Content.SelectNodes<IElement>("//div[@id='search_resultsRows']/a[contains(@class,'search_result_row')]");

        if (!gameNodes.Any())
        {
            return Langs.SearchResultEmpty;
        }

        var result = new StringBuilder();

        result.AppendLine(Langs.MultipleLineResult);

        result.AppendLine(Langs.SearchResultTitle);

        var matchGameId = RegexUtils.MatchGameIds();

        foreach (var gameNode in gameNodes)
        {
            var eleTitle = gameNode.SelectSingleNode(".//span[@class='title']");

            if (eleTitle == null)
            {
                ASFLogger.LogGenericDebug(string.Format(Langs.SomethingIsNull, nameof(eleTitle)));
                continue;
            }

            string gameTitle = eleTitle.Text();

            string gameHref = gameNode?.GetAttribute("href") ?? "";

            var match = matchGameId.Match(gameHref);

            if (match.Success)
            {
                result.AppendLineFormat(Langs.AreaItem, match.Value, gameTitle);
            }
        }

        return result.ToString();
    }
}
