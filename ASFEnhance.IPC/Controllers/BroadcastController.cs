using ArchiSteamFarm.Core;
using ArchiSteamFarm.IPC.Responses;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.IPC.Controllers.Base;
using ASFEnhance.IPC.Data.Requests;
using ASFEnhance.IPC.Data.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net;

namespace ASFEnhance.IPC.Controllers;

/// <summary>
/// 直播相关接口
/// </summary>
[Route("/Api/[controller]/[action]")]
public sealed class BroadcastController : AbstractController
{
    /// <summary>
    /// 直播观众数
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("{botNames:required}")]
    [EndpointDescription("直播观众数")]
    [EndpointSummary("直播观众数")]
    [ProducesResponseType(typeof(GenericResponse<IReadOnlyDictionary<string, BoolDictResponse>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<GenericResponse>> Watch(string botNames, [FromBody] BroadcastWatchRequest request)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        ArgumentNullException.ThrowIfNull(request);

        HashSet<Bot>? bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return BadRequest(new GenericResponse(false, string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botNames)));
        }

        IList<(string BotName, GenericResponse<int?> Data)> results = await Utilities.InParallel(bots.Select(async bot =>
            {
                if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, new GenericResponse<int?>(false, Strings.BotNotConnected)); }

                var result = await Broadcast.WebRequest.Watch(bot, request.SteamId, request.Seconds).ConfigureAwait(false);
                return (bot.BotName, result);
            }
        )).ConfigureAwait(false);

        Dictionary<string, GenericResponse<int?>> response = results.ToDictionary(static x => x.BotName, static x => x.Data);
        return Ok(new GenericResponse<IReadOnlyDictionary<string, GenericResponse<int?>>>(response));
    }

    /// <summary>
    /// 直播发送消息
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("{botNames:required}")]
    [EndpointDescription("直播发送消息")]
    [EndpointSummary("直播发送消息")]
    [ProducesResponseType(typeof(GenericResponse<IReadOnlyDictionary<string, BoolDictResponse>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<GenericResponse>> Chat(string botNames, [FromBody] BroadcastChatRequest request)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        ArgumentNullException.ThrowIfNull(request);

        HashSet<Bot>? bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return BadRequest(new GenericResponse(false, string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botNames)));
        }

        if (string.IsNullOrEmpty(request.Message))
        {
            return BadRequest(new GenericResponse(false, "Message 无效"));
        }

        IList<(string BotName, GenericResponse<int?> Data)> results = await Utilities.InParallel(bots.Select(async bot =>
            {
                if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, new GenericResponse<int?>(false, Strings.BotNotConnected)); }

                var result = await Broadcast.WebRequest.Chat(bot, request.SteamId, request.Message).ConfigureAwait(false);
                return (bot.BotName, result);
            }
        )).ConfigureAwait(false);


        Dictionary<string, GenericResponse<int?>> response = results.ToDictionary(static x => x.BotName, static x => x.Data);
        return Ok(new GenericResponse<IReadOnlyDictionary<string, GenericResponse<int?>>>(response));
    }
}