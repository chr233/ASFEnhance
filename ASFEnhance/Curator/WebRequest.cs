#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;
using SteamKit2;
using static ASFEnhance.Utils;


namespace ASFEnhance.Curator

{
    internal static class WebRequest
    {
        /// <summary>
        /// 关注或者取关鉴赏家
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="clanID"></param>
        /// <param name="isFollow"></param>
        /// <returns></returns>
        internal static async Task<bool> FollowCurator(Bot bot, ulong clanID, bool isFollow)
        {
            Uri request = new(SteamStoreURL, "/curators/ajaxfollow");
            Uri referer = new(SteamStoreURL, $"curator/{clanID}");

            Dictionary<string, string> data = new(3) {
                { "clanid", clanID.ToString() },
                { "follow", isFollow ? "1" : "0" },
            };

            ObjectResponse<AJaxFollowResponse>? response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<AJaxFollowResponse>(request, data: data, referer: referer).ConfigureAwait(false);

            return response?.Content.Success.Result == EResult.OK;
        }

        /// <summary>
        /// 获取关注的鉴赏家列表
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        internal static async Task<HashSet<CuratorItem>?> GetFollowingCurators(Bot bot, uint start, uint count)
        {
            Uri request = new(SteamStoreURL, $"/curators/ajaxgetcurators//?query=&start={start}&count={count}&dynamic_data=&filter=mycurators&appid=0");
            Uri referer = new(SteamStoreURL, "/curators/mycurators/");

            ObjectResponse<AjaxGetCuratorsResponse> response = await bot.ArchiWebHandler.UrlGetToJsonObjectWithSession<AjaxGetCuratorsResponse>(request, referer: referer).ConfigureAwait(false);

            HashSet<CuratorItem>? result = HtmlParser.ParseCuratorListPage(response?.Content);

            return result;
        }
    }
}
