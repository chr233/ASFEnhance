#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using AngleSharp.Dom;
using AngleSharp.XPath;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;
using ASFEnhance.Localization;
using System.Text;
using System.Text.RegularExpressions;
using static ASFEnhance.Utils;

namespace ASFEnhance.Store
{
    internal static class HtmlParser
    {
        /// <summary>
        /// 解析商店页面
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static GameStorePageResponse? ParseStorePage(HtmlDocumentResponse response)
        {
            if (response == null)
            {
                return null;
            }

            IEnumerable<IElement> gameNodes = response.Content.SelectNodes("//div[@id='game_area_purchase']/div[contains(@class,'purchase')]");

            HashSet<GameStorePageResponse.SingleSubData> subInfos = new();

            foreach (var gameNode in gameNodes)
            {
                IElement? eleName = gameNode.SelectSingleElementNode(".//h1");
                IElement? eleForm = gameNode.SelectSingleElementNode(".//form");
                IElement? elePrice = gameNode.SelectSingleElementNode(".//div[@data-price-final]");

                if (eleName == null)
                {
                    ASFLogger.LogGenericDebug(string.Format(Langs.SomethingIsNull, nameof(eleName)));
                    continue;
                }

                string subName = eleName?.Text() ?? string.Format(Langs.GetStoreNameFailed);

                subName = Regex.Replace(subName, @"\s+|\(\?\)", " ").Trim();

                if (eleForm != null && elePrice != null) // 非免费游戏
                {
                    string finalPrice = elePrice.GetAttribute("data-price-final") ?? "0";
                    string formName = eleForm.GetAttribute("name") ?? "-1";
                    Match match = Regex.Match(formName, @"\d+$");

                    uint subID = 0, price = 0;

                    if (match.Success)
                    {
                        if (!uint.TryParse(match.Value, out subID) || !uint.TryParse(finalPrice, out price))
                        {
                            ASFLogger.LogGenericWarning(string.Format("{0} or {1} cant parse to uint", nameof(formName), nameof(finalPrice)));
                        }
                    }

                    bool isBundle = formName.Contains("bundle");

                    subInfos.Add(new(isBundle, subID, subName, price));
                }
            }
            IElement? eleGameName = response.Content.SelectSingleNode("//div[@id='appHubAppName']|//div[@class='page_title_area game_title_area']/h2");
            string gameName = eleGameName?.Text() ?? string.Format(Langs.GetStoreNameFailed);

            if (subInfos.Count == 0)
            {
                IElement? eleError = response.Content.SelectSingleNode("//div[@id='error_box']/span");

                gameName = eleError?.Text() ?? string.Format(Langs.StorePageNotFound);
            }

            return new GameStorePageResponse(subInfos, gameName);
        }

        /// <summary>
        /// 解析搜索页
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static string? ParseSearchPage(HtmlDocumentResponse response)
        {
            if (response == null)
            {
                return null;
            }

            IEnumerable<IElement> gameNodes = response.Content.SelectNodes("//div[@id='search_resultsRows']/a[contains(@class,'search_result_row')]");

            if (!gameNodes.Any())
            {
                return Langs.SearchResultEmpty;
            }

            StringBuilder result = new();

            result.AppendLine(Langs.MultipleLineResult);

            result.AppendLine(Langs.SearchResultTitle);

            Regex matchGameID = new(@"((app|sub|bundle)\/\d+)");

            foreach (var gameNode in gameNodes)
            {
                IElement? eleTitle = gameNode.SelectSingleElementNode(".//span[@class='title']");

                if (eleTitle == null)
                {
                    ASFLogger.LogGenericDebug(string.Format(Langs.SomethingIsNull, nameof(eleTitle)));
                    continue;
                }

                string gameTitle = eleTitle.Text();

                string gameHref = gameNode.GetAttribute("href");

                Match match = matchGameID.Match(gameHref);

                if (match.Success)
                {
                    result.AppendLine(string.Format(Langs.AreaItem, match.Value, gameTitle));
                }
            }

            return result.ToString();
        }
    }
}
