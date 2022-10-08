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
    /// 愿望单相关接口
    /// </summary>
    public sealed class WishlistController : ASFEController
    {
        /// <summary>
        /// 添加愿望单
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPost("{botNames:required}/AddWishlist")]
        [SwaggerOperation(Summary = "添加愿望单", Description = "需要指定AppIDs列表")]
        [ProducesResponseType(typeof(GenericResponse<IReadOnlyDictionary<string, BoolDictResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<GenericResponse>> AddWishlist(string botNames, [FromBody] AppIDListRequest request)
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

            if ((bots == null) || (bots.Count == 0))
            {
                return BadRequest(new GenericResponse(false, string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botNames)));
            }

            if ((request.AppIDs == null) || (request.AppIDs.Count == 0))
            {
                return BadRequest(new GenericResponse(false, "AppIDs 无效"));
            }

            Dictionary<string, BoolDictResponse> response = bots.ToDictionary(x => x.BotName, x => new BoolDictResponse());

            foreach (uint appid in request.AppIDs)
            {
                IList<(string, bool)> results = await Utilities.InParallel(bots.Select(
                    async bot => {
                        if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, false); }

                        bool result = await Wishlist.WebRequest.AddWishlist(bot, appid).ConfigureAwait(false);
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
        /// 移除愿望单
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPost("{botNames:required}/RemoveWishlist")]
        [SwaggerOperation(Summary = "移除愿望单", Description = "需要指定AppIDs列表")]
        [ProducesResponseType(typeof(GenericResponse<IReadOnlyDictionary<string, BoolDictResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<GenericResponse>> RemoveWishlist(string botNames, [FromBody] AppIDListRequest request)
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

            if (request.AppIDs == null || request.AppIDs.Count == 0)
            {
                return BadRequest(new GenericResponse(false, "AppIDs 无效"));
            }

            Dictionary<string, BoolDictResponse> response = bots.ToDictionary(x => x.BotName, x => new BoolDictResponse());

            foreach (uint appid in request.AppIDs)
            {
                IList<(string, bool)> results = await Utilities.InParallel(bots.Select(
                    async bot => {
                        if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, false); }

                        bool result = await Wishlist.WebRequest.RemoveWishlist(bot, appid).ConfigureAwait(false);
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
        /// 关注游戏
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPost("{botNames:required}/FollowGame")]
        [SwaggerOperation(Summary = "关注游戏", Description = "需要指定AppIDs列表")]
        [ProducesResponseType(typeof(GenericResponse<IReadOnlyDictionary<string, BoolDictResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<GenericResponse>> FollowGame(string botNames, [FromBody] AppIDListRequest request)
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

            if (request.AppIDs == null || request.AppIDs.Count == 0)
            {
                return BadRequest(new GenericResponse(false, "AppIDs 无效"));
            }

            Dictionary<string, BoolDictResponse> response = bots.ToDictionary(x => x.BotName, x => new BoolDictResponse());

            foreach (uint appid in request.AppIDs)
            {
                IList<(string, bool)> results = await Utilities.InParallel(bots.Select(
                    async bot => {
                        if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, false); }

                        bool result = await Wishlist.WebRequest.FollowGame(bot, appid, true).ConfigureAwait(false);
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
        [HttpPost("{botNames:required}/UnFollowGame")]
        [SwaggerOperation(Summary = "取消关注游戏", Description = "需要指定AppIDs列表")]
        [ProducesResponseType(typeof(GenericResponse<IReadOnlyDictionary<string, BoolDictResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<GenericResponse>> UnFollowGame(string botNames, [FromBody] AppIDListRequest request)
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

            if (request.AppIDs == null || request.AppIDs.Count == 0)
            {
                return BadRequest(new GenericResponse(false, "AppIDs 无效"));
            }

            Dictionary<string, BoolDictResponse> response = bots.ToDictionary(x => x.BotName, x => new BoolDictResponse());

            foreach (uint appid in request.AppIDs)
            {
                IList<(string, bool)> results = await Utilities.InParallel(bots.Select(
                    async bot => {
                        if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, false); }

                        bool result = await Wishlist.WebRequest.FollowGame(bot, appid, false).ConfigureAwait(false);
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
        [HttpPost("{botNames:required}/CheckGame")]
        [SwaggerOperation(Summary = "检查游戏关注/愿望单情况", Description = "需要指定AppIDs列表")]
        [ProducesResponseType(typeof(GenericResponse<IReadOnlyDictionary<string, CheckGameDictResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<GenericResponse>> CheckGame(string botNames, [FromBody] AppIDListRequest request)
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

            if (request.AppIDs == null || request.AppIDs.Count == 0)
            {
                return BadRequest(new GenericResponse(false, "AppIDs 无效"));
            }

            Dictionary<string, CheckGameDictResponse> response = bots.ToDictionary(x => x.BotName, x => new CheckGameDictResponse());

            foreach (uint appid in request.AppIDs)
            {
                IList<(string, CheckGameResponse)> results = await Utilities.InParallel(bots.Select(
                    async bot => {
                        if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, new(false, "机器人离线")); }

                        CheckGameResponse result = await Wishlist.WebRequest.CheckGame(bot, appid).ConfigureAwait(false);
                        return (bot.BotName, result);
                    }
                )).ConfigureAwait(false);

                foreach (var result in results)
                {
                    response[result.Item1].Add(appid.ToString(), result.Item2);
                }
            }

            return Ok(new GenericResponse<IReadOnlyDictionary<string, CheckGameDictResponse>>(response));
        }
    }
}
