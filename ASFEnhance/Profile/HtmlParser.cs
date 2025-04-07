using AngleSharp.Dom;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;
using System.Text.RegularExpressions;

namespace ASFEnhance.Profile;

internal static class HtmlParser
{
    public static FetchProfileSummaryData? ParseProfilePage(HtmlDocumentResponse? response)
    {
        var document = response?.Content;

        if (document == null)
        {
            return null;
        }

        var eleError = document.QuerySelector("div.error_ctn");
        if (eleError != null)
        {
            ASFLogger.LogGenericWarning("获取个人资料失败: {msg}", eleError.QuerySelector("h3")?.TextContent);
            return null;
        }

        var script = document.QuerySelector("#responsive_page_template_content>script")?.TextContent
            .Replace("\\/", "/");
        if (string.IsNullOrEmpty(script))
        {
            return null;
        }

        var matchSteamId = RegexUtils.MatchSteamId().Match(script);
        if (!matchSteamId.Success || !ulong.TryParse(matchSteamId.Groups[1].Value, out var steamId))
        {
            return null;
        }

        var result = new FetchProfileSummaryData
        {
            SteamId = steamId,
            NickName = document.QuerySelector("div.persona_name>span.actual_persona_name")?.TextContent,
            RealName = document.QuerySelector("div.header_real_name>bdi")?.TextContent.Trim(),
            Description = document.QuerySelector("head>meta[name='Description']")?.GetAttribute("content"),
            Level = document.QuerySelector("span.friendPlayerLevelNum").ReadTextContentAsInt(-1),
            CommentCount = document.QuerySelector("a.commentthread_allcommentslink>span")?.ReadTextContentAsLong(-1) ??
                       document.QuerySelectorAll("div.commentthread_comments>div").Length,
        };

        var countryFlagUrl = document.QuerySelector("div.header_real_name>img")?.GetAttribute("src");
        if (!string.IsNullOrEmpty(countryFlagUrl))
        {
            var flag = RegexUtils.MatchCountryFlag().Match(countryFlagUrl);
            if (flag.Success)
            {
                result.Region = flag.Groups[1].Value.ToUpperInvariant();
            }
        }

        var eleCounts = document.QuerySelectorAll("div.profile_count_link.ellipsis>a");
        foreach (var eleCount in eleCounts)
        {
            var href = eleCount.GetAttribute("href");

            if (string.IsNullOrEmpty(href))
            {
                continue;
            }

            var count = eleCount.QuerySelector("span.profile_count_link_total").ReadTextContentAsLong(-1);
            if (count < 0)
            {
                count = 0;
            }

            if (href.Contains("/screenshots/"))
            {
                result.ScreenshotCount = count;
            }
            else if (href.Contains("/videos/"))
            {
                result.VideoCount = count;
            }
            else if (href.Contains("/images/"))
            {
                result.ArtworkCount = count;
            }
            else if (href.Contains("/myworkshopfiles/?section=guides"))
            {
                result.GuideCount = count;
            }
            else if (href.Contains("/myworkshopfiles/"))
            {
                result.WorkshopCount = count;
            }
            else if (href.Contains("/recommended/"))
            {
                result.ReviewCount = count;
            }
            else if (href.Contains("/games/"))
            {
                result.GameCount = count;
            }
            else if (href.Contains("/badges/"))
            {
                result.BadgeCount = count;
            }
            else if (href.Contains("/friends/"))
            {
                result.FriendCount = count;
            }
            else if (href.Contains("/groups/"))
            {
                result.GroupCount = count;
            }
        }

        return result;
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

        var inputEle = response.Content.QuerySelector("#trade_offer_access_url");

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

        var avatarViewAllEles = response.Content.QuerySelectorAll("#avatarViewAll>a");

        if (avatarViewAllEles == null)
        {
            return null;
        }

        List<int> result = [];

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

        var avatarBucketEles = response.Content.QuerySelectorAll("div.avatarBucket>div>a");

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
    internal static IDictionary<int, int>? ParseCraftableBadgeDict(HtmlDocumentResponse? response)
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

        Dictionary<int, int> result = [];
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
                if (!match.Success || !int.TryParse(match.Groups[1].Value, out var appid))
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
