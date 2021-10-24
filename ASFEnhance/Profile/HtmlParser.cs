#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Web.Responses;
using static Chrxw.ASFEnhance.Utils;
using System.Collections.Generic;

namespace Chrxw.ASFEnhance.Profile
{
    internal static class HtmlParser
    {        
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

            IElement? eleOnline = response.Content.SelectSingleNode("//div[@class='profile_in_game_name']");
            bool online = eleOnline == null;

            IElement? eleBadgesCount = response.Content.SelectSingleNode("//a[contains(@href,'/badges/')]/span[last()]");
            string? strBadgesCount = eleBadgesCount?.TextContent.Replace(",", "");

            IElement? eleGamesCount = response.Content.SelectSingleNode("//a[contains(@href,'/games/')]/span[last()]");
            string? strGamesCount = eleGamesCount?.TextContent.Trim().Replace(",", "");

            IElement? eleScreenshotsCount = response.Content.SelectSingleNode("//a[contains(@href,'/screenshots/')]/span[last()]");
            string? strScreenshotsCount = eleScreenshotsCount?.TextContent.Replace(",", "");

            IElement? eleVideosCount = response.Content.SelectSingleNode("//a[contains(@href,'/videos/')]/span[last()]");
            string? strVideosCount = eleVideosCount?.TextContent.Replace(",", "");

            IElement? eleWorkshopCount = response.Content.SelectSingleNode("//a[contains(@href,'/myworkshopfiles/')]/span[last()]");
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

            List<string> result = new();

            result.Add("个人资料:");
            result.Add(string.Format("昵称: {0}", nickName));
            result.Add(string.Format("状态: {0}", online ? "在线" : "离线"));

            if (uint.TryParse(strLevel, out uint level))
            {
                result.Add(string.Format("等级: {0}", level));
            }

            if (uint.TryParse(strBadgesCount, out uint badges))
            {
                result.Add(string.Format("徽章: {0}", badges));
            }

            if (uint.TryParse(strGamesCount, out uint games))
            {
                result.Add(string.Format("游戏: {0}", games));
            }

            if (uint.TryParse(strScreenshotsCount, out uint screenshots))
            {
                result.Add(string.Format("截图: {0}", screenshots));
            }

            if (uint.TryParse(strVideosCount, out uint videos))
            {
                result.Add(string.Format("视频: {0}", videos));
            }

            if (uint.TryParse(strWorkshopCount, out uint workshops))
            {
                result.Add(string.Format("创意工坊: {0}", workshops));
            }

            if (uint.TryParse(strRecommendedCount, out uint recommendeds))
            {
                result.Add(string.Format("评测: {0}", recommendeds));
            }

            if (uint.TryParse(strGuideCount, out uint guides))
            {
                result.Add(string.Format("指南: {0}", guides));
            }

            if (uint.TryParse(strImagesCount, out uint images))
            {
                result.Add(string.Format("艺术作品: {0}", images));
            }

            if (uint.TryParse(strGroupsCount, out uint groups))
            {
                result.Add(string.Format("组: {0}", groups));
            }

            if (uint.TryParse(strFriendsCount, out uint friends))
            {
                result.Add(string.Format("好友: {0}", friends));
            }

            return string.Join('\n', result);
        }
    }
}
