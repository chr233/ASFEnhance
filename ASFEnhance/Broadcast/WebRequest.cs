using ArchiSteamFarm.IPC.Responses;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;
using System.Net;
using System.Text.Json.Nodes;

namespace ASFEnhance.Broadcast;

/// <summary>
/// 直播间
/// </summary>
public static class WebRequest
{
    /// <summary>
    /// 直播间人气
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="steamId"></param>
    /// <param name="seconds"></param>
    /// <returns></returns>
    public static async Task<GenericResponse<int?>> Watch(this Bot bot, ulong steamId, ulong seconds = 900)
    {
        // 获取直播间信息
        var broadcastInfo = await GetBroadcastMpd(bot, steamId).ConfigureAwait(false);
        if (broadcastInfo == null)
        {
            return new GenericResponse<int?>(false, "获取直播间信息失败", null);
        }

        var broadcastId = broadcastInfo["broadcastid"]?.GetValue<string>();
        var viewertoken = broadcastInfo["viewertoken"]?.GetValue<string>();
        var viewersNum = broadcastInfo["num_viewers"]?.GetValue<int>();
        if (string.IsNullOrEmpty(broadcastId) || string.IsNullOrEmpty(viewertoken))
        {
            return new GenericResponse<int?>(false, "获取直播间信息失败", null);
        }

        // 发送心跳包
        var firstHeartbeat = await Heartbeat(bot, steamId, broadcastId, viewertoken).ConfigureAwait(false);
        if (!firstHeartbeat)
        {
            return new GenericResponse<int?>(false, "第一个心跳包发送失败，直接结束", viewersNum);
        }

        // 保持在线
        const ulong intervalSecond = 30;
        const int failCountLimit = 5;
        var heartbeatTimes = seconds / intervalSecond;

        async void Start()
        {
            var failCount = 0;
            for (ulong i = 0; i < heartbeatTimes; i++)
            {
                Thread.Sleep((int)(intervalSecond * 1000));
                var success = await Heartbeat(bot, steamId, broadcastId, viewertoken).ConfigureAwait(false);
                if (!success)
                {
                    failCount++;
                    if (failCount >= failCountLimit)
                    {
                        broadcastInfo = await GetBroadcastMpd(bot, steamId).ConfigureAwait(false);
                        if (broadcastInfo == null)
                        {
                            return;
                        }

                        broadcastId = broadcastInfo["broadcastid"]?.GetValue<string>();
                        viewertoken = broadcastInfo["viewertoken"]?.GetValue<string>();
                        if (string.IsNullOrEmpty(broadcastId) || string.IsNullOrEmpty(viewertoken))
                        {
                            return;
                        }
                    }
                }
            }
        }

        var thread = new Thread(Start);
        thread.Start();
        return new GenericResponse<int?>(true, Strings.Success, viewersNum);
    }


    /// <summary>
    /// 获取直播间核心信息
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="steamId"></param>
    /// <returns></returns>
    public static async Task<JsonObject?> GetBroadcastMpd(this Bot bot, ulong steamId)
    {
        Uri referer = new(ArchiWebHandler.SteamCommunityURL, $"/broadcast/watch/{steamId}");
        Uri request = new(ArchiWebHandler.SteamCommunityURL, "/broadcast/getbroadcastmpd");
        var data = new Dictionary<string, string>(5) { { "steamid", steamId.ToString() }, { "broadcastid", "0" }, { "viewertoken", "" }, { "watchlocation", "5" } };
        var response = await bot.ArchiWebHandler
            .UrlPostToJsonObjectWithSession<JsonObject>(request, data: data, referer: referer).ConfigureAwait(false);

        var result = response?.Content;
        if (result == null || result["success"]?.GetValue<string>() != "ready")
        {
            bot.ArchiLogger.LogGenericWarning("获取直播信息失败:" + result);
            return null;
        }

        return result;
    }

    /// <summary>
    /// 直播间人气维持心跳
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="steamId"></param>
    /// <param name="broadcastid"></param>
    /// <param name="viewertoken"></param>
    /// <returns></returns>
    public static async Task<bool> Heartbeat(this Bot bot, ulong steamId, string broadcastid, string viewertoken)
    {
        Uri referer = new(ArchiWebHandler.SteamCommunityURL, $"/broadcast/watch/{steamId}");
        Uri request = new(ArchiWebHandler.SteamCommunityURL, "/broadcast/heartbeat/");
        var data = new Dictionary<string, string>(3) { { "steamid", steamId.ToString() }, { "broadcastid", broadcastid }, { "viewertoken", viewertoken } };
        var response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<JsonObject>(request, data: data, referer: referer).ConfigureAwait(false);
        if (response is not { StatusCode: HttpStatusCode.OK })
        {
            return false;
        }

        var result = response.Content;
        if (result == null || result["success"]!.GetValue<int>() != 1)
        {
            bot.ArchiLogger.LogGenericWarning("直播心跳失败:" + result);
            return false;
        }

        return true;
    }

    /// <summary>
    /// 直播发送消息
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="steamId"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static async Task<GenericResponse<int?>> Chat(this Bot bot, ulong steamId, string message)
    {
        // 获取直播间信息
        var broadcastInfo = await GetBroadcastMpd(bot, steamId).ConfigureAwait(false);
        if (broadcastInfo == null)
        {
            return new GenericResponse<int?>(false, "获取直播间信息失败", null);
        }

        var broadcastId = broadcastInfo["broadcastid"]?.GetValue<string>();
        var viewertoken = broadcastInfo["viewertoken"]?.GetValue<string>();
        var viewersNum = broadcastInfo["num_viewers"]?.GetValue<int>();
        if (string.IsNullOrEmpty(broadcastId) || string.IsNullOrEmpty(viewertoken))
        {
            return new GenericResponse<int?>(false, "获取直播间信息失败", null);
        }

        var chatInfo = await GetChatInfo(bot, steamId, broadcastId).ConfigureAwait(false);
        if (chatInfo == null)
        {
            return new GenericResponse<int?>(false, "获取聊天服务器信息失败", null);
        }

        var token = chatInfo["token"]?.GetValue<string>();
        var chatId = chatInfo["chat_id"]?.GetValue<string>();
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(chatId))
        {
            return new GenericResponse<int?>(false, "获取聊天服务器信息失败", null);
        }

        var sendSuccess = await SendChatMsg(bot, token, chatId, message).ConfigureAwait(false);

        return sendSuccess ? new GenericResponse<int?>(sendSuccess, Strings.Success, viewersNum) : new GenericResponse<int?>(sendSuccess, "发送弹幕失败", viewersNum);
    }

    /// <summary>
    /// 获取直播聊天室信息
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="steamId"></param>
    /// <param name="broadcastid"></param>
    /// <returns></returns>
    public static async Task<JsonObject?> GetChatInfo(this Bot bot, ulong steamId, string broadcastid)
    {
        Uri request = new(ArchiWebHandler.SteamCommunityURL, $"/broadcast/getchatinfo?steamid={steamId}&broadcastid={broadcastid}");
        Uri referer = new(ArchiWebHandler.SteamCommunityURL, $"/broadcast/watch/{steamId}");
        var response = await bot.ArchiWebHandler.UrlGetToJsonObjectWithSession<JsonObject>(request, referer: referer).ConfigureAwait(false);
        var result = response?.Content;
        if (result == null || result["success"]!.GetValue<int>() != 1)
        {
            bot.ArchiLogger.LogGenericWarning("获取聊天室信息失败:" + result);
            return null;
        }

        return response?.Content;
    }

    /// <summary>
    /// 获取直播聊天室信息
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="token"></param>
    /// <param name="chatId"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static async Task<bool> SendChatMsg(this Bot bot, string token, string chatId, string message)
    {
        Uri request = new(SteamApiURL, $"/IBroadcastService/PostChatMessage/v0001?access_token={token}");
        Dictionary<string, string> data = new(3) { { "chat_id", chatId }, { "message", message }, { "instance_id", "" }, };
        var response = await bot.ArchiWebHandler
            .UrlPostToJsonObjectWithSession<JsonObject>(request, referer: ArchiWebHandler.SteamCommunityURL, data: data, session: ArchiWebHandler.ESession.None)
            .ConfigureAwait(false);
        var result = response?.Content;
        if (result == null || result["response"]?["result"]?.GetValue<int>() != 1)
        {
            return false;
        }

        return true;
    }
}