using ArchiSteamFarm.IPC.Responses;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;
using ASFEnhance.IPC.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ASFEnhance.IPC.Controllers;

/// <summary>
/// 购物车相关接口
/// </summary>
[Route("/Api/[controller]/[action]")]
public sealed class CookiesController : AbstractController
{
    /// <summary>
    /// 向购物车添加内容
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("{botNames:required}")]
    [EndpointDescription("获取 Cookies")]
    [EndpointSummary("获取 Cookies")]
    [ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GenericResponse<Dictionary<string, string>>>> GetCookies(string botNames)
    {
        if (!Config.DevFeature)
        {
            return Ok(new GenericResponse(false, "未启用 DevFeature"));
        }

        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);
        if (bots == null || bots.Count == 0)
        {
            return Ok(new GenericResponse(false, Strings.BotNotFound));
        }

        Dictionary<string, string> results = [];
        foreach (var bot in bots)
        {
            var cookies = bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookieHeader(ArchiWebHandler.SteamStoreURL);
            results.Add(bot.BotName, cookies);
        }

        return Ok(new GenericResponse<Dictionary<string, string>>(true, "Ok", results));
    }
}
