using ArchiSteamFarm.Core;
using ArchiSteamFarm.IPC.Responses;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.IPC.Requests;
using ASFEnhance.IPC.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net;

namespace ASFEnhance.IPC.Controllers;

/// <summary>
/// 评测相关接口
/// </summary>
[Route("/Api/[controller]/[action]")]
public sealed class RecommendController : AbstractController
{
    /// <summary>
    /// 发布游戏评测
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("{botNames:required}")]
    [EndpointDescription("RateUp:true好评,AllowReply:true允许回复,ForFree:false非免费取得,Public:true评测公开可见,Comment:评测内容")]
    [EndpointSummary("发布游戏评测")]
    [ProducesResponseType(typeof(GenericResponse<IReadOnlyDictionary<string, BoolDictResponse>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<GenericResponse>> PublishReview(string botNames, [FromBody] RecommendRequest request)
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

        HashSet<Bot>? bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return BadRequest(new GenericResponse(false, string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botNames)));
        }

        if (request.Recommends == null || request.Recommends.Count == 0)
        {
            return BadRequest(new GenericResponse(false, "Recommends 无效"));
        }

        Dictionary<string, BoolDictResponse> response = bots.ToDictionary(x => x.BotName, x => new BoolDictResponse());

        foreach (var recommend in request.Recommends)
        {
            if (string.IsNullOrEmpty(recommend.Comment))
            {
                foreach (Bot bot in bots)
                {
                    response[bot.BotName].Add(recommend.AppId.ToString(), false);
                }
                continue;
            }

            IList<(string, bool)> results = await Utilities.InParallel(bots.Select(
                async bot =>
                {
                    if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, false); }
                    var result = await Store.WebRequest.PublishReview(bot, recommend.AppId, recommend.Comment, recommend.RateUp, recommend.Public, recommend.AllowReply, recommend.ForFree).ConfigureAwait(false);
                    return (bot.BotName, result?.Result ?? false);
                }
            )).ConfigureAwait(false);

            foreach (var result in results)
            {
                response[result.Item1].Add(recommend.AppId.ToString(), result.Item2);
            }
        }

        return Ok(new GenericResponse<IReadOnlyDictionary<string, BoolDictResponse>>(response));
    }

    /// <summary>
    /// 删除游戏评测
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("{botNames:required}")]
    [EndpointDescription("需要指定AppIds列表")]
    [EndpointSummary("删除游戏评测")]
    [ProducesResponseType(typeof(GenericResponse<IReadOnlyDictionary<string, BoolDictResponse>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<GenericResponse>> DeleteReview(string botNames, [FromBody] AppIdListRequest request)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        ArgumentNullException.ThrowIfNull(request);

        if (!Utils.Config.EULA)
        {
            return BadRequest(new GenericResponse(false, Langs.EulaFeatureUnavilable));
        }

        HashSet<Bot>? bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return BadRequest(new GenericResponse(false, string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botNames)));
        }

        if (request.AppIds == null || request.AppIds.Count == 0)
        {
            return BadRequest(new GenericResponse(false, "AppIds 无效"));
        }

        Dictionary<string, BoolDictResponse> response = bots.ToDictionary(x => x.BotName, x => new BoolDictResponse());

        foreach (uint appid in request.AppIds)
        {
            IList<(string, bool)> results = await Utilities.InParallel(bots.Select(
                async bot =>
                {
                    if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, false); }
                    bool result = await Store.WebRequest.DeleteRecommend(bot, appid).ConfigureAwait(false);
                    return (bot.BotName, result);
                }
            )).ConfigureAwait(false);

            foreach (var result in results)
            {
                response[result.Item1].Add(appid.ToString(), result.Item2);
            }
        }

        return Ok(new GenericResponse<IReadOnlyDictionary<string, BoolDictResponse>>(response));
    }
}
