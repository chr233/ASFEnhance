using ArchiSteamFarm.Core;
using ArchiSteamFarm.IPC.Responses;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Data;
using ASFEnhance.IPC.Controllers.Base;
using ASFEnhance.IPC.Data.Requests;
using ASFEnhance.IPC.Data.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace ASFEnhance.IPC.Controllers;

/// <summary>
/// 鉴赏家相关接口
/// </summary>
[Route("/Api/[controller]/[action]")]
public sealed class CuratorController : AbstractController
{
    /// <summary>
    /// 关注鉴赏家
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("{botNames:required}")]
    [EndpointDescription("需要指定ClanId")]
    [EndpointSummary("关注鉴赏家")]
    public async Task<ActionResult<GenericResponse>> FollowCurator(string botNames, [FromBody] ClanIdListRequest request)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }
        ArgumentNullException.ThrowIfNull(request);

        if (!Config.EULA)
        {
            return BadRequest(new GenericResponse(false, Langs.EulaFeatureUnavilable));
        }

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return BadRequest(new GenericResponse(false, string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botNames)));
        }

        if (request.ClanIds == null || request.ClanIds.Count == 0)
        {
            return BadRequest(new GenericResponse(false, "ClanIds 无效"));
        }

        var response = bots.ToDictionary(x => x.BotName, x => new BoolDictResponse());

        foreach (var clianid in request.ClanIds)
        {
            IList<(string, bool)> results = await Utilities.InParallel(bots.Select(
                async bot =>
                {
                    if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, false); }
                    var result = await Curator.WebRequest.FollowCurator(bot, clianid, true, null).ConfigureAwait(false);
                    return (bot.BotName, result);
                }
            )).ConfigureAwait(false);

            foreach (var result in results)
            {
                response[result.Item1].Add(clianid.ToString(), result.Item2);
            }
        }

        return Ok(new GenericResponse<IReadOnlyDictionary<string, BoolDictResponse>>(response));
    }

    /// <summary>
    /// 取消关注鉴赏家
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("{botNames:required}")]
    [EndpointDescription("需要指定ClanId")]
    [EndpointSummary("取消关注鉴赏家")]
    public async Task<ActionResult<GenericResponse>> UnFollowCurator(string botNames, [FromBody] ClanIdListRequest request)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        ArgumentNullException.ThrowIfNull(request);

        if (!Config.EULA)
        {
            return BadRequest(new GenericResponse(false, Langs.EulaFeatureUnavilable));
        }

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return BadRequest(new GenericResponse(false, string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botNames)));
        }

        if (request.ClanIds == null || request.ClanIds.Count == 0)
        {
            return BadRequest(new GenericResponse(false, "ClanIds 无效"));
        }

        var response = bots.ToDictionary(x => x.BotName, x => new BoolDictResponse());

        foreach (var clianId in request.ClanIds)
        {
            IList<(string, bool)> results = await Utilities.InParallel(bots.Select(
                async bot =>
                {
                    if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, false); }
                    var result = await Curator.WebRequest.FollowCurator(bot, clianId, false, null).ConfigureAwait(false);
                    return (bot.BotName, result);
                }
            )).ConfigureAwait(false);

            foreach (var result in results)
            {
                response[result.Item1].Add(clianId.ToString(), result.Item2);
            }
        }

        return Ok(new GenericResponse<IReadOnlyDictionary<string, BoolDictResponse>>(response));
    }

    /// <summary>
    /// 已关注的鉴赏家列表
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("{botNames:required}")]
    [EndpointDescription("Start:起始位置,Count:获取数量")]
    [EndpointSummary("获取已关注的鉴赏家列表")]
    public async Task<ActionResult<GenericResponse>> FollowingCurators(string botNames, [FromBody] CuratorsRequest request)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        ArgumentNullException.ThrowIfNull(request);

        if (!Config.EULA)
        {
            return BadRequest(new GenericResponse(false, Langs.EulaFeatureUnavilable));
        }

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return BadRequest(new GenericResponse(false, string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botNames)));
        }

        if (request.Count == 0)
        {
            return BadRequest(new GenericResponse(false, "Count 无效"));
        }

        var response = bots.ToDictionary(x => x.BotName, x => new List<CuratorItem>());

        var results = await Utilities.InParallel(bots.Select(
               async bot =>
               {
                   if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, []); }

                   var result = await Curator.WebRequest.GetFollowingCurators(bot, request.Start, request.Count).ConfigureAwait(false);

                   return (bot.BotName, result);
               }
           )).ConfigureAwait(false);

        foreach (var result in results)
        {
            response[result.BotName] = result.result ?? [];
        }

        return Ok(new GenericResponse<IReadOnlyDictionary<string, List<CuratorItem>>>(response));
    }
}
