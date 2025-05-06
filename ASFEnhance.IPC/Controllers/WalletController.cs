using ArchiSteamFarm.IPC.Responses;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Data;
using ASFEnhance.Data.Plugin;
using ASFEnhance.IPC.Controllers.Base;
using ASFEnhance.IPC.Data.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SteamKit2;
using System.Globalization;
using System.Net;

namespace ASFEnhance.IPC.Controllers;

/// <summary>
/// 鉴赏家相关接口
/// </summary>
[Route("/Api/[controller]/[action]")]
public sealed class WalletController : AbstractController
{
    /// <summary>
    /// 充值钱包兑换码
    /// </summary>
    /// <param name="botName"></param>
    /// <param name="payload"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("{botName:required}")]
    [EndpointDescription("Code: 重置码, Address: 地址, 可为空, 为空时使用 ASF.json 中配置的地址替代")]
    [EndpointSummary("充值钱包兑换码")]
    [ProducesResponseType(typeof(GenericResponse<IReadOnlyDictionary<string, HashSet<CuratorItem>>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<GenericResponse>> RedeemWalletCode(string botName, [FromBody] RedeemWalletCodeRequest payload)
    {
        if (string.IsNullOrEmpty(botName))
        {
            throw new ArgumentNullException(nameof(botName));
        }

        if (string.IsNullOrEmpty(payload.Code))
        {
            return BadRequest(new GenericResponse(false, "参数错误, Code无效"));
        }

        if (!Config.EULA)
        {
            return BadRequest(new GenericResponse(false, Langs.EulaFeatureUnavilable));
        }

        var bot = Bot.GetBot(botName);
        if (bot == null)
        {
            return BadRequest(new GenericResponse(false, string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botName)));
        }

        var result = await Wallet.WebRequest.RedeemWalletCode(bot, payload.Code).ConfigureAwait(false);

        if (result != null)
        {
            if (result.Success == EResult.OK)
            {
                return Ok(new GenericResponse(true, Langs.WalletCodeRedeemSuccess));
            }
            else if (result.Success == EResult.InvalidState)
            {
                AddressConfig? address = null;

                if (payload.Address != null)
                {
                    address = payload.Address;
                }
                else if (Config.Addresses?.Count > 0)
                {
                    address = Config.Addresses[Random.Shared.Next(0, Config.Addresses.Count)];
                }

                if (address == null)
                {
                    return Ok(new GenericResponse(false, Langs.NoAvilableAddressError));
                }

                var result2 = await Wallet.WebRequest.RedeemWalletCode(bot, payload.Code, address).ConfigureAwait(false);

                if (result2 != null)
                {
                    if (result2.Success == EResult.OK)
                    {
                        return Ok(new GenericResponse(true, Langs.WalletCodeRedeemSuccess));
                    }

                    return Ok(new GenericResponse(false, string.Format(Langs.WalletCodeRedeemFailed, result2.Success)));
                }
            }
            else
            {
                return Ok(new GenericResponse(false, string.Format(Langs.WalletCodeRedeemFailed, result.Success)));
            }
        }

        return Ok(new GenericResponse(false, Langs.NetworkError));
    }
}
