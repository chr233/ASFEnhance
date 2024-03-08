using AngleSharp.Dom;
using AngleSharp.XPath;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data.Plugin;
using System.Text;
using System.Text.RegularExpressions;

namespace ASFEnhance.Store;

internal static class HtmlParser
{
    /// <summary>
    /// 解析商店页面
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    internal static GameStorePageResponse? ParseStorePage(HtmlDocumentResponse? response)
    {
        if (response?.Content == null)
        {
            return null;
        }

        var gameNodes = response.Content.SelectNodes<IElement>("//div[@id='game_area_purchase']/div[contains(@class,'purchase')]");

        HashSet<GameStorePageResponse.SingleSubData> subInfos = [];

        foreach (var gameNode in gameNodes)
        {
            var eleName = gameNode.SelectSingleNode(".//h1");
            var eleForm = gameNode.SelectSingleNode<IElement>(".//form");
            var elePrice = gameNode.SelectSingleNode<IElement>(".//div[@data-price-final]");

            if (eleName == null)
            {
                ASFLogger.LogGenericDebug(string.Format(Langs.SomethingIsNull, nameof(eleName)));
                continue;
            }

            string subName = eleName?.Text() ?? string.Format(Langs.GetStoreNameFailed);

            subName = RegexUtils.MatchSubNames().Replace(subName, " ").Trim();

            if (eleForm != null && elePrice != null) // 非免费游戏
            {
                string finalPrice = elePrice.GetAttribute("data-price-final") ?? "0";
                string formName = eleForm.GetAttribute("name") ?? "-1";
                Match match = RegexUtils.MatchSubPrice().Match(formName);

                uint subId = 0, price = 0;

                if (match.Success)
                {
                    if (!uint.TryParse(match.Value, out subId) || !uint.TryParse(finalPrice, out price))
                    {
                        ASFLogger.LogGenericWarning(string.Format("{0} or {1} cant parse to uint", nameof(formName), nameof(finalPrice)));
                    }
                }

                bool isBundle = formName.Contains("bundle");

                subInfos.Add(new(isBundle, subId, subName, price));
            }
        }
        var eleGameName = response.Content.SelectSingleNode("//div[@id='appHubAppName']|//div[@class='page_title_area game_title_area']/h2");
        string gameName = eleGameName?.Text() ?? string.Format(Langs.GetStoreNameFailed);

        if (subInfos.Count == 0)
        {
            var eleError = response.Content.SelectSingleNode("//div[@id='error_box']/span");

            gameName = eleError?.Text() ?? string.Format(Langs.StorePageNotFound);
        }

        return new GameStorePageResponse(subInfos, gameName);
    }

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
