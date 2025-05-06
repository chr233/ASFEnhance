using AngleSharp.Dom;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;
using ASFEnhance.Data.Plugin;

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
            var groupName = groupNameNode.TextContent.Trim().Replace("\t\t\t\t", " ");

            return (true, groupName);
        }

        var errorMessage = response.Content.QuerySelector("div.error_ctn h3");

        return (false, errorMessage?.TextContent.Trim() ?? Langs.NetworkError);
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
        var link = joinAction?.GetAttribute("href");

        if (link != null)
        {
            return link.StartsWith("javascript") ? JoinGroupStatus.NotJoined : JoinGroupStatus.Joined;
        }

        return JoinGroupStatus.Applied;
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

        if (groupNodes.Length != 0)
        {
            foreach (var groupNode in groupNodes)
            {
                var eleName = groupNode.QuerySelector("a.linkTitle");
                var eleAction = groupNode.QuerySelector("div.actions>a");

                var groupName = eleName?.Text();

                if (string.IsNullOrEmpty(groupName))
                {
                    ASFLogger.LogGenericDebug(string.Format("{0} == NULL", nameof(groupName)));
                    continue;
                }

                var strOnlick = eleAction?.GetAttribute("onclick") ?? "( '0',";

                var match = RegexUtils.MatchStrOnClick().Match(strOnlick);

                if (!match.Success)
                {
                    ASFLogger.LogGenericWarning(string.Format(Langs.SomethingIsNull, nameof(eleName)));
                }
                else
                {
                    var strGroupId = match.Groups[1].ToString();

                    if (!ulong.TryParse(strGroupId, out var groupId))
                    {
                        ASFLogger.LogGenericWarning(string.Format("{0} {1} cant parse to uint", nameof(strGroupId),
                            strGroupId));
                    }
                    else
                    {
                        groups.Add(new GroupItem(groupName, groupId));
                    }
                }
            }
        }

        return groups;
    }
}