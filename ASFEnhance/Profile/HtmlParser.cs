using AngleSharp.Common;
using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Localization;
using System.Text;

namespace ASFEnhance.Profile
{
    internal static class HtmlParser
    {
        
        /// <summary>
        /// 解析个人资料页
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static string? ParseProfilePage(HtmlDocumentResponse? response)
        {
            if (response?.Content == null)
            {
                return null;
            }

            IDocument content = response.Content;

            var eleNickName = content.SelectSingleNode("//div[@class='persona_name']/span[1]");
            string nickName = eleNickName?.TextContent ?? "";

            var eleLevel = content.SelectSingleNode("//div[@class='profile_header_badgeinfo_badge_area']//span[@class='friendPlayerLevelNum']");
            string strLevel = eleLevel?.TextContent ?? "0";

            var eleOnline = content.SelectSingleNode("//div[@class='profile_in_game_name']");
            bool online = eleOnline == null;

            var eleBadgesCount = content.SelectSingleNode("//a[contains(@href,'/badges/')]/span[last()]");
            string? strBadgesCount = eleBadgesCount?.TextContent.Replace(",", "");

            var eleGamesCount = content.SelectSingleNode("//a[contains(@href,'/games/')]/span[last()]");
            string? strGamesCount = eleGamesCount?.TextContent.Trim().Replace(",", "");

            var eleScreenshotsCount = content.SelectSingleNode("//a[contains(@href,'/screenshots/')]/span[last()]");
            string? strScreenshotsCount = eleScreenshotsCount?.TextContent.Replace(",", "");

            var eleVideosCount = content.SelectSingleNode("//a[contains(@href,'/videos/')]/span[last()]");
            string? strVideosCount = eleVideosCount?.TextContent.Replace(",", "");

            var eleWorkshopCount = content.SelectSingleNode("//a[contains(@href,'/myworkshopfiles/')]/span[last()]");
            string? strWorkshopCount = eleWorkshopCount?.TextContent.Replace(",", "");

            var eleRecommendedCount = content.SelectSingleNode("//a[contains(@href,'/recommended/')]/span[last()]");
            string? strRecommendedCount = eleRecommendedCount?.TextContent.Replace(",", "");

            var eleGuideCount = content.SelectSingleNode("//a[contains(@href,'section=guides')]/span[last()]");
            string? strGuideCount = eleGuideCount?.TextContent.Replace(",", "");

            var eleImagesCount = content.SelectSingleNode("//a[contains(@href,'/images/')]/span[last()]");
            string? strImagesCount = eleImagesCount?.TextContent.Replace(",", "");

            var eleGroupsCount = content.SelectSingleNode("//a[contains(@href,'/groups/')]/span[last()]");
            string? strGroupsCount = eleGroupsCount?.TextContent.Replace(",", "");

            var eleFriendsCount = content.SelectSingleNode("//a[contains(@href,'/friends/')]/span[last()]");
            string? strFriendsCount = eleFriendsCount?.TextContent.Replace(",", "");

            StringBuilder result = new();
            result.AppendLine(Langs.MultipleLineResult);
            result.AppendLine(Langs.ProfileHeader);
            result.AppendLine(string.Format(Langs.ProfileNickname, nickName));
            result.AppendLine(string.Format(Langs.ProfileState, online ? Langs.Online : Langs.Offline));

            uint maxFriend = 0;
            if (uint.TryParse(strLevel, out uint level))
            {
                maxFriend = 5 * level + 250;
                result.AppendLine(string.Format(Langs.ProfileLevel, level));
            }

            if (uint.TryParse(strBadgesCount, out uint badges))
            {
                result.AppendLine(string.Format(Langs.ProfileBadges, badges));
            }

            if (uint.TryParse(strGamesCount, out uint games))
            {
                result.AppendLine(string.Format(Langs.ProfileGames, games));
            }

            if (uint.TryParse(strScreenshotsCount, out uint screenshots))
            {
                result.AppendLine(string.Format(Langs.ProfileScreenshots, screenshots));
            }

            if (uint.TryParse(strVideosCount, out uint videos))
            {
                result.AppendLine(string.Format(Langs.ProfileVideos, videos));
            }

            if (uint.TryParse(strWorkshopCount, out uint workshops))
            {
                result.AppendLine(string.Format(Langs.ProfileWorkshop, workshops));
            }

            if (uint.TryParse(strRecommendedCount, out uint recommendeds))
            {
                result.AppendLine(string.Format(Langs.ProfileRecommended, recommendeds));
            }

            if (uint.TryParse(strGuideCount, out uint guides))
            {
                result.AppendLine(string.Format(Langs.ProfileGuide, guides));
            }

            if (uint.TryParse(strImagesCount, out uint images))
            {
                result.AppendLine(string.Format(Langs.ProfileImages, images));
            }

            if (uint.TryParse(strGroupsCount, out uint groups))
            {
                result.AppendLine(string.Format(Langs.ProfileGroups, groups));
            }

            if (uint.TryParse(strFriendsCount, out uint friends))
            {
                result.AppendLine(string.Format(Langs.ProfileFriends, friends, maxFriend > 0 ? maxFriend : "-"));
            }

            return result.ToString();
        }

        /// <summary>
        /// 解析交易设置页
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static string? ParseTradeofferPrivacyPage(HtmlDocumentResponse? response)
        {
            if (response?.Content == null)
            {
                return null;
            }

            var inputEle = response.Content.SelectSingleNode<IElement>("//input[@id='trade_offer_access_url']");

            if (inputEle == null)
            {
                return null;
            }

            string? tradeLink = inputEle?.GetAttribute("value");
            return tradeLink;
        }
        
        /// <summary>
        /// 解析游戏头像页面
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static Dictionary<string, List<string>>? ParseGameAvatarsPage(HtmlDocumentResponse? response)
        {
            var avatarContainers = response?.Content?.GetElementsByClassName("avatarMedium");
            
            if (avatarContainers == null)
            {
                return null;
            }

            Dictionary<string, List<string>> result = new();

            foreach (var curContainer in avatarContainers)
            {
                foreach (var imgElement in curContainer.GetElementsByTagName("a"))
                {
                    string? imgUrl = imgElement.GetAttribute("href");

                    if (imgUrl == null)
                    {
                        continue;
                    }

                    string[] items = imgUrl.Split("/");

                    if (items.Length == 0)
                    {
                        continue;
                    }

                    string gameId = items.GetItemByIndex(4);
                    string avatarId = items.GetItemByIndex(7);
                    
                    
                    if (!result.ContainsKey(gameId))
                    {
                        result[gameId] = new List<string>();
                    }
                    result[gameId].Add(avatarId);
                }
            }
            return result;
        }
    }
}
