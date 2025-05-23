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
using WebRequest = ASFEnhance.WishList.WebRequest;

namespace ASFEnhance.IPC.Controllers;

/// <summary>
/// 愿望单相关接口
/// </summary>
[Route("/Api/[controller]/[action]")]
public sealed class WishlistController : AbstractController
{
    /// <summary>
    /// 获取愿望单列表
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpGet("{botNames:required}")]
    [EndpointDescription("获取机器人愿望单列表")]
    [EndpointSummary("获取愿望单列表")]
    public async Task<ActionResult<Dictionary<string, MixWishlistResponse?>>> GetWishlist(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        if (!Config.EULA)
        {
            return BadRequest(new GenericResponse(false, Langs.EulaFeatureUnavilable));
        }

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return BadRequest(new GenericResponse(false, string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botNames)));
        }

        var resultsItems = await Utilities.InParallel(bots.Select(WebRequest.GetWishlistGames)).ConfigureAwait(false);

        Dictionary<string, MixWishlistResponse?> result = new(bots.Count);

        foreach (Bot bot in bots)
        {
            result[bot.BotName] = new MixWishlistResponse(resultsItems[result.Count]);
        }

        return Ok(new GenericResponse<IReadOnlyDictionary<string, MixWishlistResponse?>>(result));
    }

    /// <summary>
    /// 添加愿望单
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("{botNames:required}")]
    [EndpointDescription("需要指定AppIds列表")]
    [EndpointSummary("添加愿望单")]
    public async Task<ActionResult<GenericResponse>> AddWishlist(string botNames, [FromBody] AppIdListRequest request)
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

        if (request.AppIds == null || request.AppIds.Length == 0)
        {
            return BadRequest(new GenericResponse(false, "AppIds 无效"));
        }

        var response = bots.ToDictionary(x => x.BotName, x => new BoolDictResponse());

        foreach (var appid in request.AppIds)
        {
            IList<(string, bool)> results = await Utilities.InParallel(bots.Select(
                async bot =>
                {
                    if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, false); }

                    var result = await WebRequest.AddWishlist(bot, appid, true).ConfigureAwait(false);
                    return (bot.BotName, result?.Result == true);
                }
            )).ConfigureAwait(false);

            foreach (var result in results)
            {
                response[result.Item1].Add(appid.ToString(), result.Item2);
            }
        }

        return Ok(new GenericResponse<IReadOnlyDictionary<string, BoolDictResponse>>(response));
    }

    /// <summary>
    /// 移除愿望单
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("{botNames:required}")]
    [EndpointDescription("需要指定AppIds列表")]
    [EndpointSummary("移除愿望单")]
    public async Task<ActionResult<GenericResponse>> RemoveWishlist(string botNames, [FromBody] AppIdListRequest request)
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

        if (request.AppIds == null || request.AppIds.Length == 0)
        {
            return BadRequest(new GenericResponse(false, Langs.AppIdsInvalid));
        }

        var response = bots.ToDictionary(x => x.BotName, x => new BoolDictResponse());

        foreach (var appid in request.AppIds)
        {
            IList<(string, bool)> results = await Utilities.InParallel(bots.Select(
                async bot =>
                {
                    if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, false); }

                    var result = await WebRequest.AddWishlist(bot, appid, false).ConfigureAwait(false);
                    return (bot.BotName, result?.Result == true);
                }
            )).ConfigureAwait(false);

            foreach (var result in results)
            {
                response[result.Item1].Add(appid.ToString(), result.Item2);
            }
        }

        return Ok(new GenericResponse<IReadOnlyDictionary<string, BoolDictResponse>>(response));
    }

    /// <summary>
    /// 关注游戏
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("{botNames:required}")]
    [EndpointDescription("需要指定AppIds列表")]
    [EndpointSummary("关注游戏")]
    public async Task<ActionResult<GenericResponse>> FollowGame(string botNames, [FromBody] AppIdListRequest request)
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

        if (request.AppIds == null || request.AppIds.Length == 0)
        {
            return BadRequest(new GenericResponse(false, Langs.AppIdsInvalid));
        }

        var response = bots.ToDictionary(x => x.BotName, x => new BoolDictResponse());

        foreach (var appid in request.AppIds)
        {
            IList<(string, bool)> results = await Utilities.InParallel(bots.Select(
                async bot =>
                {
                    if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, false); }

                    var result = await WebRequest.FollowGame(bot, appid, true).ConfigureAwait(false);
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

    /// <summary>
    /// 取消关注游戏
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("{botNames:required}")]
    [EndpointDescription("需要指定AppIds列表")]
    [EndpointSummary("取消关注游戏")]
    public async Task<ActionResult<GenericResponse>> UnFollowGame(string botNames, [FromBody] AppIdListRequest request)
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

        if (request.AppIds == null || request.AppIds.Length == 0)
        {
            return BadRequest(new GenericResponse(false, "AppIds 无效"));
        }

        var response = bots.ToDictionary(x => x.BotName, x => new BoolDictResponse());

        foreach (var appid in request.AppIds)
        {
            IList<(string, bool)> results = await Utilities.InParallel(bots.Select(
                async bot =>
                {
                    if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, false); }

                    var result = await WebRequest.FollowGame(bot, appid, false).ConfigureAwait(false);
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

    /// <summary>
    /// 检查游戏关注/愿望单情况
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("{botNames:required}")]
    [EndpointDescription("需要指定AppIds列表")]
    [EndpointSummary("检查游戏关注/愿望单情况")]
    public async Task<ActionResult<GenericResponse>> CheckGame(string botNames, [FromBody] AppIdListRequest request)
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

        if (request.AppIds == null || request.AppIds.Length == 0)
        {
            return BadRequest(new GenericResponse(false, "AppIds 无效"));
        }

        var response = bots.ToDictionary(x => x.BotName, x => new CheckGameDictResponse());

        foreach (var appid in request.AppIds)
        {
            var results = await Utilities.InParallel(bots.Select(
                async bot =>
                {
                    if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, new(false, "机器人离线")); }

                    var result = await WebRequest.CheckGame(bot, appid).ConfigureAwait(false);
                    return (bot.BotName, result);
                }
            )).ConfigureAwait(false);

            if (results != null)
            {
                foreach (var result in results)
                {
                    response[result.BotName].Add(appid.ToString(), result.result);
                }
            }
        }

        return Ok(new GenericResponse<IReadOnlyDictionary<string, CheckGameDictResponse>>(response));
    }
}
