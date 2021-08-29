using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Web.Responses;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using static Chrxw.ASFEnhance.Response;

namespace Chrxw.ASFEnhance
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

                string gameName = eleName.TextContent.Trim() ?? "出错";
                string gameLink = eleName.GetAttribute("href") ?? "出错";

                Match match = Regex.Match(gameLink, @"\w+\/\d+");
                string gamePath = match.Success ? match.Value : "出错";

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

                    bool isBundle = CultureInfo.InvariantCulture.CompareInfo.IndexOf(formName,"bundle") != -1;

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

        //解析个人资料
        internal static string? ParseProfilePage(HtmlDocumentResponse response)
        {
            if (response == null)
            {
                return null;
            }

            IElement? eleNickName = response.Content.SelectSingleNode("//div[@class='persona_name']/span[1]");
            string nickName = eleNickName?.TextContent ?? "";

            IElement? eleLevel = response.Content.SelectSingleNode("//div[@class='profile_header_badgeinfo_badge_area']//span[@class='friendPlayerLevelNum']");
            string strLevel = eleLevel?.TextContent ?? "0";

            IElement? eleOnline = response.Content.SelectSingleNode("//div[@class='profile_in_game persona online']");
            bool online = eleOnline != null;

            IElement? eleBadgesCount = response.Content.SelectSingleNode("//a[contains(@href,'/badges/')]/span[last()]");
            string? strBadgesCount = eleBadgesCount?.TextContent.Replace(",", "");

            IElement? eleGamesCount = response.Content.SelectSingleNode("//a[contains(@href,'/games/')]/span[last()]");
            string? strGamesCount = eleGamesCount?.TextContent.Trim().Replace(",", "");

            IElement? eleScreenshotsCount = response.Content.SelectSingleNode("//a[contains(@href,'/screenshots/')]/span[last()]");
            string? strScreenshotsCount = eleScreenshotsCount?.TextContent.Replace(",", "");

            IElement? eleVideosCount = response.Content.SelectSingleNode("//a[contains(@href,'/videos/')]/span[last()]");
            string? strVideosCount = eleVideosCount?.TextContent.Replace(",", "");

            IElement? eleWorkshopCount = response.Content.SelectSingleNode("//a[ends-with(@href,'/myworkshopfiles/')]/span[last()]");
            string? strWorkshopCount = eleWorkshopCount?.TextContent.Replace(",", "");
            
            IElement? eleRecommendedCount = response.Content.SelectSingleNode("//a[contains(@href,'/recommended/')]/span[last()]");
            string? strRecommendedCount = eleRecommendedCount?.TextContent.Replace(",", "");

            IElement? eleGuideCount = response.Content.SelectSingleNode("//a[contains(@href,'section=guides')]/span[last()]");
            string? strGuideCount = eleGuideCount?.TextContent.Replace(",", "");
            
            IElement? eleImagesCount = response.Content.SelectSingleNode("//a[contains(@href,'/images/')]/span[last()]");
            string? strImagesCount = eleImagesCount?.TextContent.Replace(",", "");

            IElement? eleGroupsCount = response.Content.SelectSingleNode("//a[contains(@href,'/groups/')]/span[last()]");
            string? strGroupsCount = eleGroupsCount?.TextContent.Replace(",", "");

            IElement? eleFriendsCount = response.Content.SelectSingleNode("//a[contains(@href,'/friends/')]/span[last()]");
            string? strFriendsCount = eleFriendsCount?.TextContent.Replace(",", "");

            uint level, badges, games, screenshots, videos, workshops, recommendeds, guides, images, groups, friends;

            List<string> result = new();

            result.Add("个人资料:");
            result.Add(string.Format("昵称: {0}", nickName));
            result.Add(string.Format("状态: {0}", online ? "在线" : "离线"));

            if (uint.TryParse(strLevel, out level))
            {
                result.Add(string.Format("等级: {0}", level));
            }

            if (uint.TryParse(strBadgesCount, out badges))
            {
                result.Add(string.Format("徽章: {0}", badges));
            }

            if (uint.TryParse(strGamesCount, out games))
            {
                result.Add(string.Format("游戏: {0}", games));
            }

            if (uint.TryParse(strScreenshotsCount, out screenshots))
            {
                result.Add(string.Format("截图: {0}", screenshots));
            }

            if (uint.TryParse(strVideosCount, out videos))
            {
                result.Add(string.Format("视频: {0}", videos));
            }

            if (uint.TryParse(strWorkshopCount, out workshops))
            {
                result.Add(string.Format("创意工坊: {0}", workshops));
            }

            if (uint.TryParse(strRecommendedCount, out recommendeds))
            {
                result.Add(string.Format("评测: {0}", recommendeds));
            }
            
            if (uint.TryParse(strGuideCount, out guides))
            {
                result.Add(string.Format("指南: {0}", guides));
            }

            if (uint.TryParse(strImagesCount, out images))
            {
                result.Add(string.Format("艺术作品: {0}", images));
            }

            if (uint.TryParse(strGroupsCount, out groups))
            {
                result.Add(string.Format("组: {0}", groups));
            }

            if (uint.TryParse(strFriendsCount, out friends))
            {
                result.Add(string.Format("好友: {0}", friends));
            }

            return string.Join('\n', result);
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
                    string countryCode = availableCountrie.GetAttribute("id") ?? "help";
                    string countryName = availableCountrie.TextContent ?? "Null";

                    if (countryCode == "help") //过滤“其他”
                    {
                        continue;
                    }

                    ccDatas.Add(new CartCountryData(countryName, countryCode, countryCode == currentCode));
                }
            }
            return ccDatas;
        }
    }
}
