using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Web.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Chrxw.ASFEnhance.Data;

namespace Chrxw.ASFEnhance
{
    class HtmlParser
    {
        //解析购物车页
        internal static List<CartData>? ParseCertPage(HtmlDocumentResponse response)
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

                string? gameName = eleName.TextContent;
                string? gameLink = eleName.GetAttribute("href");
                string? gamePrice = elePrice.TextContent;

                Match match = Regex.Match(gameLink, @"\w+\/\d+", RegexOptions.IgnoreCase);

                if (!match.Success)
                {
                    continue;
                }
                string gamePath = match.Groups[0].Value;

                cartGames.Add(new CartData(gamePath, gameName, gamePrice));
            }

            if (cartGames.Count > 0)
            {
                IElement? totalPrice = response.Content.SelectSingleNode("//div[@id='cart_estimated_total']");

                cartGames.Add(new CartData("预计总额", "", totalPrice.TextContent));

            }
            return cartGames;
        }

        //解析商店页
        internal static List<SubData> ParseStorePage(HtmlDocumentResponse response)
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
                IElement? eleAddCart = gameNode.SelectSingleElementNode("//div[@class='btn_addtocart']/a");
                IElement? elePrice = gameNode.SelectSingleElementNode("//div[@class='btn_addtocart']/../div[1]");

                string gameName = eleName.TextContent;
                string gameLink = eleAddCart.GetAttribute("href") ?? "无";
                string finalPrice = elePrice?.GetAttribute("data-price-final") ?? "-1";


                Match match = Regex.Match(gameLink, @"\d+", RegexOptions.IgnoreCase);

                if (!match.Success)
                {
                    continue;
                }

                uint subID, gamePrice;

                if (uint.TryParse(match.Groups[0].Value, out subID) && uint.TryParse(finalPrice, out gamePrice))
                {
                    subInfos.Add(new SubData(subID, gameName, gamePrice));
                }
            }

            if (subInfos.Count == 0)
            {
                IElement eleError = response.Content.SelectSingleNode("//div[@id=;error_box']/span");
                if (eleError != null)
                {
                    throw new ApplicationException(eleError.TextContent);
                }

            }

            return subInfos;
        }

    }
}
