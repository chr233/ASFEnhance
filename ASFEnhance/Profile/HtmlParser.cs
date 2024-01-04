using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Web.Responses;
using System.Text;
using System.Text.RegularExpressions;

namespace ASFEnhance.Profile;

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
        result.AppendLineFormat(Langs.ProfileNickname, nickName);
        result.AppendLineFormat(Langs.ProfileState, online ? Langs.Online : Langs.Offline);

        uint maxFriend = 0;
        if (uint.TryParse(strLevel, out uint level))
        {
            maxFriend = 5 * level + 250;
            result.AppendLineFormat(Langs.ProfileLevel, level);
        }

        if (uint.TryParse(strBadgesCount, out uint badges))
        {
            result.AppendLineFormat(Langs.ProfileBadges, badges);
        }

        if (uint.TryParse(strGamesCount, out uint games))
        {
            result.AppendLineFormat(Langs.ProfileGames, games);
        }

        if (uint.TryParse(strScreenshotsCount, out uint screenshots))
        {
            result.AppendLineFormat(Langs.ProfileScreenshots, screenshots);
        }

        if (uint.TryParse(strVideosCount, out uint videos))
        {
            result.AppendLineFormat(Langs.ProfileVideos, videos);
        }

        if (uint.TryParse(strWorkshopCount, out uint workshops))
        {
            result.AppendLineFormat(Langs.ProfileWorkshop, workshops);
        }

        if (uint.TryParse(strRecommendedCount, out uint recommendeds))
        {
            result.AppendLineFormat(Langs.ProfileRecommended, recommendeds);
        }

        if (uint.TryParse(strGuideCount, out uint guides))
        {
            result.AppendLineFormat(Langs.ProfileGuide, guides);
        }

        if (uint.TryParse(strImagesCount, out uint images))
        {
            result.AppendLineFormat(Langs.ProfileImages, images);
        }

        if (uint.TryParse(strGroupsCount, out uint groups))
        {
            result.AppendLineFormat(Langs.ProfileGroups, groups);
        }

        if (uint.TryParse(strFriendsCount, out uint friends))
        {
            result.AppendLineFormat(Langs.ProfileFriends, friends, maxFriend > 0 ? maxFriend : "-");
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

        string? tradeLink = inputEle?.GetAttribute("value");
        return tradeLink;
    }

    /// <summary>
    /// 解析游戏头像GameIds
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    internal static List<int>? ParseAvatarsPageToGameIds(HtmlDocumentResponse? response)
    {
        if (response?.Content == null)
        {
            return null;
        }

        var avatarViewAllEles = response.Content.SelectNodes<IElement>("//div[@id='avatarViewAll']/a");

        if (avatarViewAllEles == null)
        {
            return null;
        }

        List<int> result = new();

        Regex matchGameId = RegexUtils.MatchGameId();

        foreach (var avatarViewEle in avatarViewAllEles)
        {
            string? url = avatarViewEle.GetAttribute("href") ?? "";
            Match match = matchGameId.Match(url);

            if (match.Success && int.TryParse(match.Groups[1].Value, out int gameId))
            {
                result.Add(gameId);
            }
        }
        return result;
    }

    /// <summary>
    /// 解析单个游戏的AvatarIds
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    internal static List<int>? ParseSingleGameToAvatarIds(HtmlDocumentResponse? response)
    {
        if (response?.Content == null)
        {
            return null;
        }

        var avatarBucketEles = response.Content.SelectNodes<IElement>("//div[@class='avatarBucket']/div/a");

        if (avatarBucketEles == null)
        {
            return null;
        }

        List<int> result = [];
        foreach (var avatarEle in avatarBucketEles)
        {
            var imgUrl = avatarEle.GetAttribute("href") ?? "";
            string[] items = imgUrl.Split("/");

            if (items.Length > 0)
            {
                if (int.TryParse(items.Last(), out int avatarId))
                {
                    result.Add(avatarId);
                }
            }
        }
        return result;
    }

    /// <summary>
    /// 解析徽章页信息
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    internal static IDictionary<uint, int>? ParseCraftableBadgeDict(HtmlDocumentResponse? response)
    {
        if (response?.Content == null)
        {
            return null;
        }

        var badgeEles = response.Content.QuerySelectorAll<IElement>("div.badge_row.is_link");

        if (badgeEles == null)
        {
            return null;
        }

        Dictionary<uint, int> result = [];
        var regAppId = RegexUtils.MatchBadgeAppId();
        var regLevel = RegexUtils.MatchLevel();
        foreach (var badgeEle in badgeEles)
        {
            var craftableEle = badgeEle.QuerySelector("a.badge_craft_button");
            var href = craftableEle?.GetAttribute("href");
            if (!string.IsNullOrEmpty(href))
            {
                bool foil = href.EndsWith("border=1");
                int level = 0;
                var match = regAppId.Match(href);
                if (!match.Success || !uint.TryParse(match.Groups[1].Value, out var appid))
                {
                    continue;
                }

                var levelEle = badgeEle.QuerySelector("div.badge_info_description>div:nth-child(2)");
                var desc = levelEle?.TextContent;
                if (!string.IsNullOrEmpty(desc))
                {
                    match = regLevel.Match(desc);
                    if (!match.Success || !int.TryParse(match.Groups[1].Value, out level))
                    {
                        level = 0;
                    }
                }
                result.Add(appid, foil ? -level : level);
            }
        }
        return result;
    }
}
