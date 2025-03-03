using AngleSharp.Dom;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;
using ASFEnhance.Data.Plugin;
using System.Text.RegularExpressions;

namespace ASFEnhance.Group;

internal static class HtmlParser
{
    /// <summary>
    /// 获取群组名
    /// </summary>
    /// <param name="response"></param>
    /// <returns>
    /// true,string:  群组名
    /// false,string: 群组未找到/网络错误
    /// </returns>
    internal static (bool, string) GetGroupName(HtmlDocumentResponse? response)
    {
        if (response?.Content == null)
        {
            return (false, Langs.NetworkError);
        }

        var groupNameNode = response.Content.QuerySelector("div.grouppage_resp_title ellipsis");

        if (groupNameNode != null)
        {
            string groupName = groupNameNode.TextContent.Trim().Replace("\t\t\t\t", " ");

            return (true, groupName);
        }
        else
        {
            var errorMessage = response.Content.QuerySelector("div.error_ctn h3");

            return (false, errorMessage?.TextContent.Trim() ?? Langs.NetworkError);
        }
    }

    /// <summary>
    /// 判断是否已加入群组
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    internal static JoinGroupStatus CheckJoinGroup(HtmlDocumentResponse? response)
    {
        if (response?.Content == null)
        {
            throw new ArgumentNullException(nameof(response));
        }

        var joinAction = response.Content.QuerySelector("div.grouppage_join_area>a");
        string? link = joinAction?.GetAttribute("href");

        if (link != null)
        {
            return link.StartsWith("javascript") ? JoinGroupStatus.Unjoined : JoinGroupStatus.Joined;
        }
        else
        {
            return JoinGroupStatus.Applied;
        }
    }



    /// <summary>
    /// 解析群组列表
    /// </summary>
    /// <returns></returns>
    internal static HashSet<GroupItem>? ParseGropuList(HtmlDocumentResponse? response)
    {
        if (response?.Content == null)
        {
            return null;
        }

        var groupNodes = response.Content.QuerySelectorAll("#search_results>div[id][class]");

        HashSet<GroupItem> groups = [];

        if (groupNodes.Any())
        {
            foreach (var groupNode in groupNodes)
            {
                var eleName = groupNode.QuerySelector("a.linkTitle");
                var eleAction = groupNode.QuerySelector("div.actions>a");

                string? groupName = eleName?.Text();

                if (string.IsNullOrEmpty(groupName))
                {
                    ASFLogger.LogGenericDebug(string.Format("{0} == NULL", nameof(groupName)));
                    continue;
                }

                string strOnlick = eleAction?.GetAttribute("onclick") ?? "( '0',";

                Match match = RegexUtils.MatchStrOnClick().Match(strOnlick);

                if (!match.Success)
                {
                    ASFLogger.LogGenericWarning(string.Format(Langs.SomethingIsNull, nameof(eleName)));
                    continue;
                }
                else
                {
                    string strGroupId = match.Groups[1].ToString();

                    if (!ulong.TryParse(strGroupId, out ulong groupId))
                    {
                        ASFLogger.LogGenericWarning(string.Format("{0} {1} cant parse to uint", nameof(strGroupId), strGroupId));
                        continue;
                    }

                    groups.Add(new(groupName, groupId));
                }
            }
        }

        return groups;
    }
}
