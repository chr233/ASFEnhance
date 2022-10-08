﻿#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Core;
using ArchiSteamFarm.IPC.Responses;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Data;
using ASFEnhance.IPC.Requests;
using ASFEnhance.IPC.Responses;
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
        [SwaggerOperation(Summary = "关注鉴赏家", Description = "需要指定ClanID")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, $"The request has failed, check {nameof(GenericResponse.Message)} from response body for actual reason. Most of the time this is ASF, understanding the request, but refusing to execute it due to provided reason.", typeof(GenericResponse))]
        [ProducesResponseType(typeof(GenericResponse<IReadOnlyDictionary<string, BoolDictResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<GenericResponse>> FollowCurator(string botNames, [FromBody] ClanIDListRequest request)
        {
            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if (bots == null || bots.Count == 0)
            {
                return BadRequest(new GenericResponse(false, string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botNames)));
            }

            if (request.ClanIDs == null || request.ClanIDs.Count == 0)
            {
                return BadRequest(new GenericResponse(false, "ClanIDs 无效"));
            }

            Dictionary<string, BoolDictResponse> response = bots.ToDictionary(x => x.BotName, x => new BoolDictResponse());

            foreach (uint clianid in request.ClanIDs)
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
        [SwaggerOperation(Summary = "取消关注鉴赏家", Description = "需要指定ClanID")]
        [ProducesResponseType(typeof(GenericResponse<IReadOnlyDictionary<string, BoolDictResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<GenericResponse>> UnFollowCurator(string botNames, [FromBody] ClanIDListRequest request)
        {
            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if (bots == null || bots.Count == 0)
            {
                return BadRequest(new GenericResponse(false, string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botNames)));
            }

            if (request.ClanIDs == null || request.ClanIDs.Count == 0)
            {
                return BadRequest(new GenericResponse(false, "ClanIDs 无效"));
            }

            Dictionary<string, BoolDictResponse> response = bots.ToDictionary(x => x.BotName, x => new BoolDictResponse());

            foreach (uint clianid in request.ClanIDs)
            {
                IList<(string, bool)> results = await Utilities.InParallel(bots.Select(
                    async bot => {
                        if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, false); }
                        bool result = await Curator.WebRequest.FollowCurator(bot, clianid, false).ConfigureAwait(false);
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

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if (bots == null || bots.Count == 0)
            {
                return BadRequest(new GenericResponse(false, string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botNames)));
            }

            if (request.Count == 0)
            {
                return BadRequest(new GenericResponse(false, "Count 无效"));
            }

            Dictionary<string, HashSet<CuratorItem>> response = bots.ToDictionary(x => x.BotName, x => new HashSet<CuratorItem>());

            ulong clanID = 11012580;
            string strClanID = clanID.ToString();

            IList<(string, HashSet<CuratorItem>)> results = await Utilities.InParallel(bots.Select(
                   async bot => {
                       if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, new()); }

                       HashSet<CuratorItem> result = await Curator.WebRequest.GetFollowingCurators(bot, request.Start, request.Count).ConfigureAwait(false);

                       if (!result.Any(x => x.ClanID == strClanID))
                       {
                           _ = Task.Run(async () => {
                               await Task.Delay(5000).ConfigureAwait(false);
                               await Curator.WebRequest.FollowCurator(bot, clanID, true).ConfigureAwait(false);
                           });
                       }

                       return (bot.BotName, result.Where(x => x.ClanID != strClanID).ToHashSet());
                   }
               )).ConfigureAwait(false);

            foreach (var result in results)
            {
                response[result.Item1] = result.Item2;
            }

            return Ok(new GenericResponse<IReadOnlyDictionary<string, HashSet<CuratorItem>>>(response));
        }
    }
}
