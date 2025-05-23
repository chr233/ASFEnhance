using ArchiSteamFarm.IPC.Responses;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.IPC.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASFEnhance.IPC.Controllers;

/// <summary>
/// 购物车相关接口
/// </summary>
[Route("/Api/[controller]/[action]")]
public sealed class ProfileController : AbstractController
{
    /// <summary>
    /// 批量设置用户名
    /// </summary>
    /// <param name="nicknames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost]
    [EndpointDescription("botNames 设定范围, nameList 一行一个昵称")]
    [EndpointSummary("批量设置用户名")]
    public async Task<ActionResult<GenericResponse>> BatchEditNickname(Dictionary<string, string> nicknames)
    {
        if (nicknames.Count == 0)
        {
            throw new ArgumentNullException(nameof(nicknames));
        }

        if (!Config.EULA)
        {
            return BadRequest(new GenericResponse(false, Langs.EulaFeatureUnavilable));
        }

        Dictionary<string, string> response = [];

        foreach (var (botName, nickname) in nicknames)
        {
            var bot = Bot.GetBot(botName);

            if (bot == null || !bot.IsConnectedAndLoggedOn)
            {
                response.Add(botName, bot == null ? Strings.BotNotFound : Strings.BotNotConnected);
                continue;
            }

            bot.SteamFriends.SetPersonaName(nickname);
            await Task.Delay(100).ConfigureAwait(false);

            response.Add(botName, Langs.Success);
        }

        return Ok(new GenericResponse<Dictionary<string, string>>(true, "Ok", response));
    }
}
