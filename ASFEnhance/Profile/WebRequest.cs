using AngleSharp.Dom;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;
using ArchiSteamFarm.Web;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;
using ASFEnhance.Localization;
using static ASFEnhance.Utils;

namespace ASFEnhance.Profile
{
    internal static class WebRequest
    {
        /// <summary>
        /// 读取Steam个人资料
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> GetSteamProfile(Bot bot)
        {
            Uri request = new(SteamCommunityURL, $"/profiles/{bot.SteamID}/?l=english");
            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL).ConfigureAwait(false);
            return HtmlParser.ParseProfilePage(response);
        }

        /// <summary>
        /// 读取交易链接
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> GetTradeofferPrivacyPage(Bot bot)
        {
            Uri request = new(SteamCommunityURL, $"/profiles/{bot.SteamID}/tradeoffers/privacy");
            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL).ConfigureAwait(false);
            return HtmlParser.ParseTradeofferPrivacyPage(response);
        }

        /// <summary>
        /// 清除昵称历史
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task ClearAliasHisrory(Bot bot)
        {
            Uri request = new(SteamCommunityURL, $"/profiles/{bot.SteamID}/ajaxclearaliashistory/");
            await bot.ArchiWebHandler.UrlPostWithSession(request, referer: SteamStoreURL).ConfigureAwait(false);
        }


        /// <summary>
        /// 获取年度总结Token
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> GetReplayToken(Bot bot)
        {
            Uri request = new(SteamStoreURL, "/replay/");
            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL).ConfigureAwait(false);
            var token = response?.Content?.QuerySelector<IElement>("#application_config")?.GetAttribute("data-sale_feature_webapi_token");
            return token?.Replace("\"", "");
        }

        /// <summary>
        /// 获取年度总结图片
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="year"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        internal static async Task<string?> GetReplayPic(Bot bot, int year, string token)
        {
            Uri request = new($"https://api.steampowered.com/ISaleFeatureService/GetUserYearInReviewShareImage/v1/?access_token={token}&steamid={bot.SteamID}&year={year}&language={Langs.Language}");
            var response = await bot.ArchiWebHandler.UrlGetToJsonObjectWithSession<SteamReplayResponse>(request, referer: SteamStoreURL).ConfigureAwait(false);

            var payload = response?.Content?.Response.Imanges;
            if (payload?.Count > 0)
            {
                string path = payload[0].Path;
                return $"https://shared.akamai.steamstatic.com/social_sharing/{path}";
            }
            else
            {
                return Langs.NetworkError;
            }
        }

        /// <summary>
        /// 设置年度总结可见性
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="year"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        internal static async Task<string> SetReplayPermission(Bot bot, int year, string token, int privacy)
        {
            Uri request = new($"https://api.steampowered.com/ISaleFeatureService/SetUserSharingPermissions/v1/");

            Dictionary<string, string> data = new(4) {
                { "access_token", token },
                { "steamid", bot.SteamID.ToString() },
                { "year", year.ToString() },
                { "privacy_state", privacy.ToString() },
            };

            var response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<SteamReplayPermissionsResponse>(request, referer: SteamStoreURL, data: data, session: ArchiWebHandler.ESession.None).ConfigureAwait(false);

            string result = response?.Content?.Response?.Privacy switch {
                1 => Langs.ReplayPrivate,
                2 => Langs.ReplayFriend,
                3 => Langs.ReplayPublic,
                _ => Langs.NetworkError
            };

            return result;
        }

        /// <summary>
        /// 从游戏头像设置个人资料头像
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="gameId"></param>
        /// <param name="avatarId"></param>
        /// <returns></returns>
        internal static async Task<string?> SetProfileGameAvatar(Bot bot, int gameId, int avatarId)
        {
            Uri request = new(SteamCommunityURL, $"/games/{gameId.ToString()}/selectAvatar");
            Uri referer = new(SteamCommunityURL, $"/games/{gameId.ToString()}/Avatar/Preview/{gameId.ToString()}");

            Dictionary<string, string> data = new(1) {
                { "selectedAvatar", $"{avatarId.ToString()}" },
            };

            bool response = await bot.ArchiWebHandler.UrlPostWithSession(request, referer: referer, data: data, requestOptions: WebBrowser.ERequestOptions.ReturnRedirections).ConfigureAwait(false);
            return response ? Langs.Done : Langs.NetworkError;

        }

        /// <summary>
        /// 从您的游戏中获取头像
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<Dictionary<string, List<string>>?> GetGameAvatars(Bot bot)
        {
            Uri request = new(SteamCommunityURL, "/actions/GameAvatars/");
            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamCommunityURL).ConfigureAwait(false);
            return HtmlParser.ParseGameAvatarsPage(response);
        }
    }
}
