using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Web.Responses;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static Chrxw.ASFEnhance.Response;

namespace Chrxw.ASFEnhance
{
    class HtmlParser
    {
        //解析购物车页
        internal static CartResponse? ParseCertPage(HtmlDocumentResponse response)
        {
            if (response == null)
            {
                return null;
            }

            IEnumerable<IElement?> gameNodes = response.Content.SelectNodes("//div[@class='cart_item_list']/div");

            List<CartData> cartGames = new();

            foreach (IElement gameNode in gameNodes)
            {
                IElement? eleName = gameNode.SelectSingleElementNode("//div[@class='cart_item_desc']/a");
                IElement? elePrice = gameNode.SelectSingleElementNode("//div[@class='price']");

                string gameName = eleName.TextContent.Trim() ?? "出错";
                string gameLink = eleName.GetAttribute("href") ?? "出错";

                Match match = Regex.Match(gameLink, @"\w+\/\d+");
                string gamePath = match.Success ? match.Value : "出错";

                match = Regex.Match(elePrice.TextContent, @"\d+([.,]\d+)?");
                string strPrice = match.Success ? match.Value : "-1";

                bool success = float.TryParse(strPrice, out float gamePrice);
                if (!success)
                {
                    gamePrice = -1;
                }

                cartGames.Add(new CartData(gamePath, gameName, (int)gamePrice * 100));
            }

            int totalPrice = 0;
            bool purchaseSelf = false, purchaseGift = false;

            if (cartGames.Count > 0)
            {
                IElement? eleTotalPrice = response.Content.SelectSingleNode("//div[@id='cart_estimated_total']");

                Match match = Regex.Match(eleTotalPrice.TextContent, @"\d+([.,]\d+)?");

                string strPrice = match.Success ? match.Value : "0";

                bool success = float.TryParse(strPrice, out float totalProceFloat);
                if (!success)
                {
                    totalProceFloat = -1;
                }
                totalPrice = (int)totalProceFloat * 100;

                purchaseSelf = response.Content.SelectSingleNode("//a[@id='btn_purchase_self']") != null;
                purchaseGift = response.Content.SelectSingleNode("//a[@id='btn_purchase_gift']") != null;
            }

            return new CartResponse(cartGames, totalPrice, purchaseSelf, purchaseGift);
        }

        //解析商店页
        internal static StoreResponse? ParseStorePage(HtmlDocumentResponse response)
        {
            if (response == null)
            {
                return null;
            }

            IEnumerable<IElement> gameNodes = response.Content.SelectNodes("//div[@id='game_area_purchase']/div");

            List<SubData> subInfos = new();

            foreach (IElement gameNode in gameNodes)
            {
                IElement? eleName = gameNode.SelectSingleElementNode("//h1");
                IElement? eleForm = gameNode.SelectSingleElementNode("//form");
                IElement? elePrice = gameNode.SelectSingleElementNode("//div[@data-price-final]");

                if (elePrice == null)//DLC的按钮,无价格
                {
                    continue;
                }

                string subName = eleName.TextContent.Trim() ?? "出错";
                string formName = eleForm.GetAttribute("name") ?? "出错";
                string finalPrice = elePrice.GetAttribute("data-price-final") ?? "出错";

                Match match = Regex.Match(formName, @"\d+$");

                if (uint.TryParse(match.Value, out uint subID) && uint.TryParse(finalPrice, out uint gamePrice))
                {
                    bool bundle = formName.IndexOf("bundle") != -1;
                    subInfos.Add(new SubData(bundle, subID, subName, gamePrice));
                }
                else
                {
                    ASF.ArchiLogger.LogGenericWarning(subName);
                    ASF.ArchiLogger.LogGenericWarning(formName);
                    ASF.ArchiLogger.LogGenericWarning(finalPrice);
                }
            }

            IElement? eleGameName = response.Content.SelectSingleNode("//div[@id='appHubAppName']");
            string gameName = eleGameName?.TextContent.Trim() ?? "出错";

            if (subInfos.Count == 0)
            {
                IElement eleError = response.Content.SelectSingleNode("//div[@id='error_box']/span");
                if (eleError != null)
                {
                    gameName = eleError.TextContent.Trim();
                }
            }

            return new StoreResponse(subInfos, gameName);
        }
    }
}
