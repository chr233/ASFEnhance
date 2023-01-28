using AngleSharp.Dom;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;
using ArchiSteamFarm.Web;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;
using ASFEnhance.Localization;
using System.Net;
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
            Uri request = new(SteamApiURL, $"/ISaleFeatureService/GetUserYearInReviewShareImage/v1/?access_token={token}&steamid={bot.SteamID}&year={year}&language={Langs.Language}");
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
            Uri request = new(SteamApiURL, $"/ISaleFeatureService/SetUserSharingPermissions/v1/");

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
        /// 获取游戏的可用头像列表
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        internal static async Task<List<int>?> GetAvilableAvatarsOfGame(Bot bot, int gameId)
        {
            Uri request = new(SteamCommunityURL, $"/ogg/{gameId}/Avatar/List");
            var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamCommunityURL).ConfigureAwait(false);
            return HtmlParser.ParseSingleGameToAvatarIds(response);
        }

        /// <summary>
        /// 设置个人资料游戏头像
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="gameId"></param>
        /// <param name="avatarId"></param>
        /// <returns></returns>
        internal static async Task<bool> ApplyGameAvatar(Bot bot, int gameId, int avatarId)
        {
            Uri request = new(SteamCommunityURL, $"/games/{gameId}/selectAvatar");
            Uri referer = new(SteamCommunityURL, $"/games/{gameId}/Avatar/Preview/{gameId}");

            Dictionary<string, string> data = new(1) {
                { "selectedAvatar", $"{avatarId}" },
            };

            bool response = await bot.ArchiWebHandler.UrlPostWithSession(request, referer: referer, data: data, requestOptions: WebBrowser.ERequestOptions.ReturnRedirections).ConfigureAwait(false);
            return response;

        }

        /// <summary>
        /// 获取可用游戏头像列表
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<List<int>?> GetGamdIdsOfAvatarList(Bot bot)
        {
            Uri request = new(SteamCommunityURL, "/actions/GameAvatars/");
            var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamCommunityURL).ConfigureAwait(false);
            return HtmlParser.ParseAvatarsPageToGameIds(response);
        }

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private static async Task<IEnumerable<byte>?> DownloadImage(Bot bot, string url)
        {
            Uri request = new Uri(url);
            var response = await bot.ArchiWebHandler.WebBrowser.UrlGetToBinary(request).ConfigureAwait(false);
            return response?.Content;
        }

        /// <summary>
        /// 设置自定义头像
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="imgUrl"></param>
        /// <returns></returns>
        internal static async Task<string> ApplyCustomAvatar(Bot bot, string imgUrl)
        {
            var bytes = await DownloadImage(bot, imgUrl).ConfigureAwait(false);

            if (bytes == null)
            {
                return Langs.DownloadImageFailed;
            }

            var content = new MultipartFormDataContent
            {
                { new ByteArrayContent(bytes.ToArray()), "file", "Test.txt" }
            };

            var cookies = bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookies(SteamStoreURL)
                .Select(x => $"{x.Name}={x.Value}");

            Dictionary<string, string> headers = new() {
                { "Accept", "application/json, text/plain, */*" },
                { "Cookie", string.Join(';',cookies) }
            };

            Uri request = new(SteamCommunityURL, "/actions/FileUploader/");
            var response = await bot.ArchiWebHandler.WebBrowser.UrlPost(request, headers, data: content).ConfigureAwait(false);

            return response?.StatusCode == HttpStatusCode.OK ? Langs.ChangeAvatarSuccess : Langs.ChangeAvatarFailed;
        }
    }
}
