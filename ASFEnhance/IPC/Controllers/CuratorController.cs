using ArchiSteamFarm.Core;
using ArchiSteamFarm.IPC.Responses;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Data;
using ASFEnhance.IPC.Requests;
using ASFEnhance.IPC.Responses;
using ASFEnhance.Localization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Globalization;
using System.Net;

namespace ASFEnhance.IPC.Controllers
{
    /// <summary>
    /// 鉴赏家相关接口
    /// </summary>
    public sealed class CuratorController : ASFEController
    {
        /// <summary>
        /// 关注鉴赏家
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPost("{botNames:required}/FollowCurator")]
        [SwaggerOperation(Summary = "关注鉴赏家", Description = "需要指定ClanId")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, $"The request has failed, check {nameof(GenericResponse.Message)} from response body for actual reason. Most of the time this is ASF, understanding the request, but refusing to execute it due to provided reason.", typeof(GenericResponse))]
        [ProducesResponseType(typeof(GenericResponse<IReadOnlyDictionary<string, BoolDictResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<GenericResponse>> FollowCurator(string botNames, [FromBody] ClanIdListRequest request)
        {
            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (!Utils.Config.EULA)
            {
                return BadRequest(new GenericResponse(false, Langs.EulaFeatureUnavilable));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if (bots == null || bots.Count == 0)
            {
                return BadRequest(new GenericResponse(false, string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botNames)));
            }

            if (request.ClanIds == null || request.ClanIds.Count == 0)
            {
                return BadRequest(new GenericResponse(false, "ClanIds 无效"));
            }

            Dictionary<string, BoolDictResponse> response = bots.ToDictionary(x => x.BotName, x => new BoolDictResponse());

            foreach (uint clianid in request.ClanIds)
            {
                IList<(string, bool)> results = await Utilities.InParallel(bots.Select(
                    async bot => {
                        if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, false); }
                        bool result = await Curator.WebRequest.FollowCurator(bot, clianid, true).ConfigureAwait(false);
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
        [HttpPost("{botNames:required}/UnFollowCurator")]
        [SwaggerOperation(Summary = "取消关注鉴赏家", Description = "需要指定ClanId")]
        [ProducesResponseType(typeof(GenericResponse<IReadOnlyDictionary<string, BoolDictResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<GenericResponse>> UnFollowCurator(string botNames, [FromBody] ClanIdListRequest request)
        {
            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (!Utils.Config.EULA)
            {
                return BadRequest(new GenericResponse(false, Langs.EulaFeatureUnavilable));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if (bots == null || bots.Count == 0)
            {
                return BadRequest(new GenericResponse(false, string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botNames)));
            }

            if (request.ClanIds == null || request.ClanIds.Count == 0)
            {
                return BadRequest(new GenericResponse(false, "ClanIds 无效"));
            }

            Dictionary<string, BoolDictResponse> response = bots.ToDictionary(x => x.BotName, x => new BoolDictResponse());

            foreach (uint clianId in request.ClanIds)
            {
                IList<(string, bool)> results = await Utilities.InParallel(bots.Select(
                    async bot => {
                        if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, false); }
                        bool result = await Curator.WebRequest.FollowCurator(bot, clianId, false).ConfigureAwait(false);
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
        [HttpPost("{botNames:required}/FollowingCurators")]
        [SwaggerOperation(Summary = "获取已关注的鉴赏家列表", Description = "Start:起始位置,Count:获取数量")]
        [ProducesResponseType(typeof(GenericResponse<IReadOnlyDictionary<string, HashSet<CuratorItem>>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<GenericResponse>> FollowingCurators(string botNames, [FromBody] CuratorsRequest request)
        {
            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (!Utils.Config.EULA)
            {
                return BadRequest(new GenericResponse(false, Langs.EulaFeatureUnavilable));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if (bots == null || bots.Count == 0)
            {
                return BadRequest(new GenericResponse(false, string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botNames)));
            }

            if (request.Count == 0)
            {
                return BadRequest(new GenericResponse(false, "Count 无效"));
            }
            
            var response = bots.ToDictionary(x => x.BotName, x => new HashSet<CuratorItem>());
            
            var results = await Utilities.InParallel(bots.Select(
                   async bot => {
                       if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, new()); }

                       var result = await Curator.WebRequest.GetFollowingCurators(bot, request.Start, request.Count).ConfigureAwait(false);

                       return (bot.BotName, result);
                   }
               )).ConfigureAwait(false);

            foreach (var result in results)
            {
                response[result.Item1] = result.Item2 ?? new();
            }

            return Ok(new GenericResponse<IReadOnlyDictionary<string, HashSet<CuratorItem>>>(response));
        }
    }
}
