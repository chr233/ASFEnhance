using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace ASFEnhance.Cart
{
    internal static partial class HtmlParser
    {
        [GeneratedRegex("\\d+([.,]\\d+)?")]
        private static partial Regex MatchTotalPrice();

        [GeneratedRegex("[0-9,.]+")]
        private static partial Regex MatchPrice();

        [GeneratedRegex("([.,])\\d\\d?$")]
        private static partial Regex MatchPriceValue();

        [GeneratedRegex("(\\w+)\\/(\\d+)")]
        private static partial Regex MatchGameLink();

        [GeneratedRegex("[,.\\d]+")]
        private static partial Regex MatchStrPrice();

        /// <summary>
        /// 解析购物车页面
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static CartItemResponse? ParseCartPage(HtmlDocumentResponse? response)
        {
            if (response?.Content == null)
            {
                return null;
            }

            var gameNodes = response.Content.SelectNodes<IElement>("//div[@class='cart_item_list']/div");

            bool dotMode = true;

            foreach (var gameNode in gameNodes)
            {
                var strPrice = gameNode.SelectSingleNode<IElement>(".//div[@class='price']")?.TextContent ?? "";

                Match matchPrice = MatchPrice().Match(strPrice);

                if (matchPrice.Success)
                {
                    Match match = MatchPriceValue().Match(matchPrice.Value);
                    if (match.Success)
                    {
                        dotMode = ".".Equals(match.Groups[1].ToString());
                        break;
                    }
                }
            }

            HashSet<CartItemResponse.CartItem> cartGames = new();

            foreach (var gameNode in gameNodes)
            {
                var eleName = gameNode.SelectSingleNode<IElement>(".//div[@class='cart_item_desc']/a");
                var elePrice = gameNode.SelectSingleNode<IElement>(".//div[@class='price']");

                string gameName = eleName?.TextContent.Trim() ?? Langs.Error;
                string gameLink = eleName?.GetAttribute("href") ?? Langs.Error;

                Match match = MatchGameLink().Match(gameLink);

                SteamGameId gameId;
                if (match.Success)
                {
                    if (uint.TryParse(match.Groups[2].Value, out uint id))
                    {
                        SteamGameIdType type = match.Groups[1].Value.ToUpperInvariant() switch {
                            "APP" => SteamGameIdType.App,
                            "SUB" => SteamGameIdType.Sub,
                            "BUNDLE" => SteamGameIdType.Bundle,
                            _ => SteamGameIdType.Error
                        };

                        gameId = new(type, id);
                    }
                    else
                    {
                        gameId = new(SteamGameIdType.Error, 0);
                    }
                }
                else
                {
                    gameId = new(SteamGameIdType.Error, 0);
                }

                match = MatchStrPrice().Match(elePrice?.TextContent ?? "");
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

                cartGames.Add(new CartItemResponse.CartItem(gameId, gameName, (int)(gamePrice * 100)));
            }

            if (cartGames.Count > 0)
            {
                var text = response.Content.SelectSingleNode<IElement>("//div[@id='cart_estimated_total']")?.TextContent;

                Match match = MatchTotalPrice().Match(text ?? "");

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
                int totalPrice = (int)(totalProceFloat * 100);

                bool purchaseSelf = response.Content.SelectSingleNode("//a[@id='btn_purchase_self']") != null;
                bool purchaseGift = response.Content.SelectSingleNode("//a[@id='btn_purchase_gift']") != null;

                return new(cartGames, totalPrice, purchaseSelf, purchaseGift);
            }
            else
            {
                return new();
            }
        }

        /// <summary>
        /// 解析购物车可用区域
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static string? ParseCartCountries(HtmlDocumentResponse? response)
        {
            if (response?.Content == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            var currentCountry = response.Content.SelectSingleNode<IElement>("//input[@id='usercountrycurrency']");

            var availableCountries = response.Content.SelectNodes<IElement>("//ul[@id='usercountrycurrency_droplist']/li/a");

            StringBuilder message = new();
            message.AppendLine(Langs.MultipleLineResult);

            if (currentCountry != null)
            {
                message.AppendLine(Langs.MultipleLineResult);
                message.AppendLine(Langs.AvailableAreaHeader);

                string? currentCode = currentCountry.GetAttribute("value");

                foreach (var availableCountrie in availableCountries)
                {
                    string? countryCode = availableCountrie.GetAttribute("id");
                    string countryName = availableCountrie.TextContent ?? "";

                    if (!string.IsNullOrEmpty(countryCode) && countryCode != "help")
                    {
                        message.AppendLine(string.Format(currentCode == countryCode ? Langs.AreaItemCurrent : Langs.AreaItem, countryCode, countryName));
                    }
                }
            }
            else
            {
                message.AppendLine(Langs.NoAvailableArea);
            }

            return message.ToString();
        }
    }
}
