using AngleSharp.Dom;
using ArchiSteamFarm.Steam;
using ASFEnhance.Data;
using System.Net;

namespace ASFEnhance.Friend;

internal static class WebRequest
{
    /// <summary>
    /// 验证个人资料链接
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    internal static async Task<ulong?> GetSteamIdByProfileLink(Bot bot, string path)
    {
        var request = new Uri(SteamCommunityURL, path);

        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

        return HtmlParser.ParseProfilePage(response);
    }

    /// <summary>
    /// 读取好友邀请链接
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<AjaxGetInviteTokens?> GetAddFriendPage(Bot bot)
    {
        var request = new Uri(SteamCommunityURL, $"/profiles/{bot.SteamID}/friends/add");
        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamStoreURL).ConfigureAwait(false);

        var prefix = HtmlParser.ParseInviteLinkPrefix(response);
        if (string.IsNullOrEmpty(prefix))
        {
            return null;
        }

        request = new Uri(SteamCommunityURL, $"/invites/ajaxcreate");
        var response2 = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<AjaxGetInviteTokens>(request, data: null).ConfigureAwait(false);

        if (response2?.Content != null)
        {
            var result = response2.Content;
            result.Prefix = prefix;
            return result;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 使用邀请链接添加好友
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="identity"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    internal static async Task<string> AddFriendViaInviteLink(Bot bot, string identity, string token)
    {
        var request = new Uri(SteamCommunityURL, $"/user/{identity}/{token}");
        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamCommunityURL).ConfigureAwait(false);

        if (response?.Content == null)
        {
            return Langs.NetworkError;
        }

        var previewLink = response.Content?.QuerySelector("a.btn_profile_action[href^='https://store']")?.GetAttribute("href");
        var match = RegexUtils.MatchSteamIdFromLink().Match(previewLink ?? "");
        if (!match.Success)
        {
            return Langs.GetProfileFailed;
        }

        string? steamId = match.Groups[1].Value;
        if (steamId == bot.SteamID.ToString())
        {
            return Langs.CanNotAddMyself;
        }

        var request2 = new Uri(SteamCommunityURL, $"/invites/ajaxredeem");
        var data = new Dictionary<string, string>(3)
        {
            { "steamid_user", steamId },
            { "invite_token", token },
        };

        response = await bot.ArchiWebHandler.UrlPostToHtmlDocumentWithSession(request2, data: data, referer: request).ConfigureAwait(false);

        return response?.StatusCode == HttpStatusCode.OK ? Langs.AddFriendSuccess : Langs.AddFriendFailed;
    }
}
