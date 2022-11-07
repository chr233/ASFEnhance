#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

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
    /// 评测相关接口
    /// </summary>
    public sealed class RecommendController : ASFEController
    {
        /// <summary>
        /// 发布游戏评测
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPost("{botNames:required}/PublishReview")]
        [SwaggerOperation(Summary = "发布游戏评测", Description = "RateUp:true好评,AllowReply:true允许回复,ForFree:false非免费取得,Public:true评测公开可见,Comment:评测内容")]
        [ProducesResponseType(typeof(GenericResponse<IReadOnlyDictionary<string, BoolDictResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<GenericResponse>> PublishReview(string botNames, [FromBody] RecommendRequest request)
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
                        response[bot.BotName].Add(recommend.AppID.ToString(), false);
                    }
                    continue;
                }

                IList<(string, bool)> results = await Utilities.InParallel(bots.Select(
                    async bot => {
                        if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, false); }
                        RecommendGameResponse result = await Store.WebRequest.PublishReview(bot, recommend.AppID, recommend.Comment, recommend.RateUp, recommend.Public, recommend.AllowReply, recommend.ForFree).ConfigureAwait(false);
                        return (bot.BotName, result?.Result ?? false);
                    }
                )).ConfigureAwait(false);

                foreach (var result in results)
                {
                    response[result.Item1].Add(recommend.AppID.ToString(), result.Item2);
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
        [HttpPost("{botNames:required}/DeleteReview")]
        [SwaggerOperation(Summary = "删除游戏评测", Description = "需要指定AppIDs列表")]
        [ProducesResponseType(typeof(GenericResponse<IReadOnlyDictionary<string, BoolDictResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<GenericResponse>> DeleteReview(string botNames, [FromBody] AppIDListRequest request)
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
}
