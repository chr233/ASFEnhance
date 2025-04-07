using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;
using ASFEnhance.Data;
using ASFEnhance.Data.Plugin;


namespace ASFEnhance.Group;

internal static class WebRequest
{
    /// <summary>
    /// 获取群组信息
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="groupId"></param>
    /// <returns></returns>
    public static async Task<GroupData?> FetchGroupData(Bot bot, string groupId)
    {
        var request = new Uri(SteamCommunityURL, $"/groups/{groupId}?l=schinese");
        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamCommunityURL).ConfigureAwait(false);

        if (response?.Content == null)
        {
            return null;
        }

        var eleError = response.Content.QuerySelector("div.error_ctn");

        if (eleError != null)
        {
            return new GroupData(false, null, JoinGroupStatus.Failed);
        }

        var eleSecondName = response.Content.QuerySelector("div.grouppage_header_name>span");
        if (eleSecondName != null)
        {
            eleSecondName.TextContent = "";
        }

        var groupName = response.Content.QuerySelector("div.grouppage_header_name")?.TextContent.Trim();
        var eleLeave = response.Content.QuerySelector("div.content a[href^='javascript:ConfirmLeaveGroup']");
        var eleCancel = response.Content.QuerySelector("div.content a[href^='javascript:ConfirmCancelJoinRequest']");

        JoinGroupStatus status;

        if (eleLeave != null)
        {
            status = JoinGroupStatus.Joined;
        }
        else if (eleCancel != null)
        {
            status = JoinGroupStatus.Applied;
        }
        else
        {
            status = JoinGroupStatus.NotJoined;
        }

        return new GroupData(true, groupName, status);
    }


    /// <summary>
    /// 加入指定群组
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="groupName"></param>
    /// <returns></returns>
    internal static async Task<bool> JoinGroup(Bot bot, string groupName)
    {
        var request = new Uri(SteamCommunityURL, $"/groups/{groupName}");
        Dictionary<string, string> data = new(2, StringComparer.Ordinal)
        {
            { "action", "join" },
        };
        var result = await bot.ArchiWebHandler.UrlPostWithSession(request, data: data, referer: SteamCommunityURL, session: ArchiWebHandler.ESession.CamelCase).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// 离开群组
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="GroupId"></param>
    /// <returns></returns>
    internal static async Task<bool> LeaveGroup(Bot bot, ulong GroupId)
    {
        Uri request = new(SteamCommunityURL, $"/profiles/{bot.SteamID}/home_process");

        Dictionary<string, string> data = new(3, StringComparer.Ordinal)
        {
            { "action", "leaveGroup" },
            { "groupId", GroupId.ToString() }
        };

        await bot.ArchiWebHandler.UrlPostToHtmlDocumentWithSession(request, data: data, referer: SteamStoreURL, session: ArchiWebHandler.ESession.CamelCase).ConfigureAwait(false);

        return true;
    }

    /// <summary>
    /// 获取群组列表
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<HashSet<GroupItem>?> GetGroupList(Bot bot)
    {
        Uri request = new(SteamCommunityURL, $"/profiles/{bot.SteamID}/groups/");

        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL).ConfigureAwait(false);

        return HtmlParser.ParseGropuList(response);
    }
}
