#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;
using static ASFEnhance.Utils;


namespace ASFEnhance.Group
{
    internal static class WebRequest
    {

        /// <summary>
        /// 加入指定群组
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        internal static async Task<(JoinGroupStatus, string?)> JoinGroup(Bot bot, string groupID)
        {
            Uri request = new(SteamCommunityURL, $"/groups/{groupID}");

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamCommunityURL).ConfigureAwait(false);

            if (response == null)
            {
                return (JoinGroupStatus.Failed, null);
            }

            (bool success, string message) = HtmlParser.GetGroupName(response);

            if (success)
            {
                Dictionary<string, string> data = new(2, StringComparer.Ordinal)
                {
                    { "action", "join" },
                };
                response = await bot.ArchiWebHandler.UrlPostToHtmlDocumentWithSession(request, data: data, referer: SteamCommunityURL, session: ArchiWebHandler.ESession.CamelCase).ConfigureAwait(false);

                return (HtmlParser.CheckJoinGroup(response), message);
            }
            else
            {
                return (JoinGroupStatus.Failed, message);
            }
        }

        /// <summary>
        /// 离开群组
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        internal static async Task<bool> LeaveGroup(Bot bot, ulong GroupID)
        {
            Uri request = new(SteamCommunityURL, $"/profiles/{bot.SteamID}/home_process");

            Dictionary<string, string> data = new(3, StringComparer.Ordinal)
            {
                { "action", "leaveGroup" },
                { "groupId", GroupID.ToString() }
            };

            await bot.ArchiWebHandler.UrlPostToHtmlDocumentWithSession(request, data: data, referer: SteamStoreURL, session: ArchiWebHandler.ESession.CamelCase).ConfigureAwait(false);

            return true;
        }

        /// <summary>
        /// 获取群组列表
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> GetGroupList(Bot bot)
        {
            Uri request = new(SteamCommunityURL, $"/profiles/{bot.SteamID}/groups/");

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL).ConfigureAwait(false);

            return HtmlParser.ParseGropuList(response);
        }
    }
}
