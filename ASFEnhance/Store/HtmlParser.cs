#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Web.Responses;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using static Chrxw.ASFEnhance.Store.Response;

namespace Chrxw.ASFEnhance.Store
{
    internal static class HtmlParser
    {
        //解析商店页
        internal static StoreResponse? ParseStorePage(HtmlDocumentResponse response)
        {
            if (response == null)
            {
                return null;
            }

            IEnumerable<IElement> gameNodes = response.Content.SelectNodes("//div[@id='game_area_purchase']/div[contains(@class,'purchase')]");

            List<SubData> subInfos = new();

            foreach (IElement gameNode in gameNodes)
            {
                IElement? eleName = gameNode.SelectSingleElementNode(".//h1");
                IElement? eleForm = gameNode.SelectSingleElementNode(".//form");
                IElement? elePrice = gameNode.SelectSingleElementNode(".//div[@data-price-final]");

                if (eleName == null)
                {
                    ASF.ArchiLogger.LogGenericDebug(string.Format("{0} == NULL", nameof(eleName)));
                    continue;
                }

                string subName = eleName?.TextContent ?? "读取名称出错";

                subName = Regex.Replace(subName, @"\s+|\(\?\)", " ").Trim();

                if (eleForm != null && elePrice != null) // 非免费游戏
                {
                    string formName = eleForm.GetAttribute("name") ?? "-1";
                    string finalPrice = elePrice.GetAttribute("data-price-final") ?? "0";
                    Match match = Regex.Match(formName, @"\d+$");

                    uint subID = 0, price = 0;

                    if (match == null)
                    {
                        ASF.ArchiLogger.LogGenericWarning(string.Format("{0} == NULL", nameof(eleName)));
                    }
                    else
                    {
                        if (!uint.TryParse(match.Value, out subID) || uint.TryParse(finalPrice, out price))
                        {
                            ASF.ArchiLogger.LogGenericWarning(string.Format("{0} or {1} cant parse to uint", nameof(formName), nameof(finalPrice)));
                        }
                    }

                    bool isBundle = CultureInfo.InvariantCulture.CompareInfo.IndexOf(formName, "bundle") != -1;

                    subInfos.Add(new SubData(isBundle, subID, subName, price));
                }
            }

            IElement? eleGameName = response.Content.SelectSingleNode("//div[@id='appHubAppName']|//div[@class='page_title_area game_title_area']/h2");
            string gameName = eleGameName?.TextContent.Trim() ?? "读取名称失败";

            if (subInfos.Count == 0)
            {
                IElement? eleError = response.Content.SelectSingleNode("//div[@id='error_box']/span");

                gameName = eleError?.TextContent.Trim() ?? "未找到商店页";
            }

            return new StoreResponse(subInfos, gameName);
        }
    }
}
