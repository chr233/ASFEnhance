#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Web.Responses;
using Chrxw.ASFEnhance.Localization;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static Chrxw.ASFEnhance.Cart.Response;
using static Chrxw.ASFEnhance.Utils;

namespace Chrxw.ASFEnhance.Cart
{
    internal static class HtmlParser
    {
        //解析购物车页
        internal static CartResponse? ParseCertPage(HtmlDocumentResponse response)
        {
            if (response == null)
            {
                return null;
            }

            string error = string.Format(CurrentCulture, Langs.Error);

            IEnumerable<IElement?> gameNodes = response.Content.SelectNodes("//div[@class='cart_item_list']/div");

            bool dotMode = true;

            foreach (IElement gameNode in gameNodes)
            {
                IElement? elePrice = gameNode.SelectSingleElementNode(".//div[@class='price']");

                Match match = Regex.Match(elePrice.TextContent, @"([.,])\d?\d?$");
                if (match.Success)
                {
                    dotMode = ".".Equals(match.Groups[1].ToString());
                    break;
                }
            }

            List<CartData> cartGames = new();

            foreach (IElement gameNode in gameNodes)
            {
                IElement? eleName = gameNode.SelectSingleElementNode(".//div[@class='cart_item_desc']/a");
                IElement? elePrice = gameNode.SelectSingleElementNode(".//div[@class='price']");

                string gameName = eleName.TextContent.Trim() ?? error;
                string gameLink = eleName.GetAttribute("href") ?? error;

                Match match = Regex.Match(gameLink, @"\w+\/\d+");
                string gamePath = match.Success ? match.Value : error;

                match = Regex.Match(elePrice.TextContent, @"[,.\d]+");
                string strPrice = match.Success ? match.Value : "-1";

                if (!dotMode)
                {
                    strPrice = strPrice.Replace(".", "").Replace(",", ".");
                }

                bool success = float.TryParse(strPrice, out float gamePrice);
                if (!success)
                {
                    gamePrice = -1;
                }

                cartGames.Add(new CartData(gamePath, gameName, (int)(gamePrice * 100)));
            }

            int totalPrice = 0;
            bool purchaseSelf = false, purchaseGift = false;

            if (cartGames.Count > 0)
            {
                IElement? eleTotalPrice = response.Content.SelectSingleNode("//div[@id='cart_estimated_total']");

                Match match = Regex.Match(eleTotalPrice.TextContent, @"\d+([.,]\d+)?");

                string strPrice = match.Success ? match.Value : "0";

                if (!dotMode)
                {
                    strPrice = strPrice.Replace(".", "").Replace(",", ".");
                }

                bool success = float.TryParse(strPrice, out float totalProceFloat);
                if (!success)
                {
                    totalProceFloat = -1;
                }
                totalPrice = (int)(totalProceFloat * 100);

                purchaseSelf = response.Content.SelectSingleNode("//a[@id='btn_purchase_self']") != null;
                purchaseGift = response.Content.SelectSingleNode("//a[@id='btn_purchase_gift']") != null;
            }

            return new CartResponse(cartGames, totalPrice, purchaseSelf, purchaseGift);
        }
        //解析购物车可用区域
        internal static List<CartCountryData>? ParseCertCountries(HtmlDocumentResponse response)
        {
            if (response == null)
            {
                return null;
            }

            IElement? currentCountry = response.Content.SelectSingleNode("//input[@id='usercountrycurrency']");

            IEnumerable<IElement?> availableCountries = response.Content.SelectNodes("//ul[@id='usercountrycurrency_droplist']/li/a");

            List<CartCountryData> ccDatas = new();

            if (currentCountry != null)
            {
                string currentCode = currentCountry.GetAttribute("value");

                ASF.ArchiLogger.LogGenericInfo(currentCode);

                foreach (IElement availableCountrie in availableCountries)
                {
                    string? countryCode = availableCountrie.GetAttribute("id");
                    string countryName = availableCountrie.TextContent ?? "";

                    if (!string.IsNullOrEmpty(countryCode)) //过滤null
                    {
                        ccDatas.Add(new CartCountryData(countryName, countryCode, countryCode == currentCode));
                    }
                }
            }
            return ccDatas;
        }
    }
}
