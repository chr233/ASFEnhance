#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Core;
using ArchiSteamFarm.IPC.Responses;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.IPC.Requests;
using ASFEnhance.IPC.Responses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Globalization;
using System.Net;

namespace ASFEnhance.IPC.Controllers
{
    /// <summary>
    /// 购物车相关接口
    /// </summary>
    public sealed class PurchaseController : ASFEController
    {
        /// <summary>
        /// 获取游戏详情
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPost("{botNames:required}/GetAppDetail")]
        [SwaggerOperation(Summary = "获取游戏详情", Description = "需要指定AppIDs列表")]
        [ProducesResponseType(typeof(GenericResponse<IReadOnlyDictionary<string, AppDetailDictResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<GenericResponse>> GetAppDetail(string botNames, [FromBody] AppIDListRequest request)
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

            Dictionary<string, AppDetailDictResponse> response = bots.ToDictionary(x => x.BotName, x => new AppDetailDictResponse());

            foreach (uint appid in request.AppIDs)
            {
                IList<(string, AppDetail)> results = await Utilities.InParallel(bots.Select(
                    async bot => {
                        if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, new()); }

                        var detail = await Store.WebRequest.GetAppDetails(bot, appid).ConfigureAwait(false);
                        var data = detail.Data;

                        AppDetail result = new() {
                            Success = detail.Success,
                            AppID = appid,
                            Name = data.Name,
                            Type = data.Type,
                            Desc = data.ShortDescription,
                            IsFree = data.IsFree,
                            Released = !data.ReleaseDate.ComingSoon,
                            Subs = new(),
                        };

                        foreach (var subs in data.PackageGroups)
                        {
                            foreach (var sub in subs.Subs)
                            {
                                result.Subs.Add(new() {
                                    SubID = sub.SubID,
                                    IsFree = sub.IsFreeLicense,
                                    Name = sub.OptionText,
                                });
                            }
                        }

                        return (bot.BotName, result);
                    }
                )).ConfigureAwait(false);

                foreach (var result in results)
                {
                    response[result.Item1].Add(appid.ToString(), result.Item2);
                }
            }

            return Ok(new GenericResponse<IReadOnlyDictionary<string, AppDetailDictResponse>>(response));
        }

        /// <summary>
        /// 购买指定游戏
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPost("{botNames:required}/Purchase")]
        [SwaggerOperation(Summary = "购买指定游戏", Description = "SubIDs和BundleIDs可省略,但是必须指定一种,也可以都指定")]
        [ProducesResponseType(typeof(GenericResponse<IReadOnlyDictionary<string, BoolDictResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<GenericResponse>> PublishReview(string botNames, [FromBody] PurchaseRequest request)
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

            if ((request.SubIDs == null && request.BundleIDs == null) || request.SubIDs.Count + request.BundleIDs.Count == 0)
            {
                return BadRequest(new GenericResponse(false, "SubIDs 和 BundleIDs 不能同时为 null"));
            }

            Dictionary<string, PurchaseResultResponse> response = bots.ToDictionary(x => x.BotName, x => new PurchaseResultResponse());

            //清空购物车
            await Utilities.InParallel(bots.Select(bot => Cart.WebRequest.ClearCart(bot))).ConfigureAwait(false);

            if (request.BundleIDs?.Count > 0)
            {
                foreach (uint bundleid in request.BundleIDs)
                {
                    IList<(string, bool)> results = await Utilities.InParallel(bots.Select(
                        async bot => {
                            if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, false); }

                            bool? result = await Cart.WebRequest.AddCart(bot, bundleid, true).ConfigureAwait(false);
                            return (bot.BotName, result ?? false);
                        }
                    )).ConfigureAwait(false);

                    foreach (var result in results)
                    {
                        response[result.Item1].AddCartResult.BundleIDs.Add(bundleid.ToString(), result.Item2);
                    }
                }
            }

            if (request.SubIDs?.Count > 0)
            {
                foreach (uint subid in request.SubIDs)
                {
                    IList<(string, bool)> results = await Utilities.InParallel(bots.Select(
                        async bot => {
                            if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, false); }

                            if (request.SkipOwned)
                            {
                                if (bot.OwnedPackageIDs.ContainsKey(subid))
                                {
                                    return (bot.BotName, true);
                                }
                            }

                            bool? result = await Cart.WebRequest.AddCart(bot, subid, false).ConfigureAwait(false);
                            return (bot.BotName, result ?? false);
                        }
                    )).ConfigureAwait(false);

                    foreach (var result in results)
                    {
                        response[result.Item1].AddCartResult.SubIDs.Add(subid.ToString(), result.Item2);
                    }
                }
            }

            //记录购物车
            {
                IList<(string, PurchaseResult)> results = await Utilities.InParallel(bots.Select(
                        async bot => {
                            if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, new()); }

                            var cartData = await Cart.WebRequest.GetCartGames(bot).ConfigureAwait(false);

                            PurchaseResult result = new() {
                                Currency = bot.WalletCurrency.ToString(),
                            };

                            foreach (var c in cartData.CartItems)
                            {
                                result.CartItems.Add(new() {
                                    Type = c.GameID.Type.ToString(),
                                    ID = c.GameID.GameID,
                                    Name = c.Name,
                                });
                            }

                            return (bot.BotName, result);
                        }
                    )).ConfigureAwait(false);

                foreach (var result in results)
                {
                    response[result.Item1].PurchaseResult = result.Item2;
                }
            }

            //下单
            {
                IList<(string, PurchaseResult)> results = await Utilities.InParallel(bots.Select(
                    async bot => {
                        if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, new()); }

                        long balancePrev = bot.WalletBalance;

                        PurchaseResult result = new() {
                            Success = false,
                            BalanceNow = balancePrev,
                            BalancePrev = balancePrev,
                        };

                        if (balancePrev < 1)
                        {
                            return (bot.BotName, result);
                        }

                        var response1 = await Cart.WebRequest.CheckOut(bot, false).ConfigureAwait(false);

                        if (response1 == null)
                        {
                            return (bot.BotName, result);
                        }

                        var response2 = await Cart.WebRequest.InitTransaction(bot).ConfigureAwait(false);

                        if (response2 == null)
                        {
                            return (bot.BotName, result);
                        }

                        string? transID = response2.Content.TransID ?? response2.Content.TransActionID;

                        if (string.IsNullOrEmpty(transID))
                        {
                            return (bot.BotName, result);
                        }

                        var response3 = await Cart.WebRequest.GetFinalPrice(bot, transID, false).ConfigureAwait(false);

                        if (response3 == null || response2.Content.TransID == null)
                        {
                            return (bot.BotName, result);
                        }

                        var response4 = await Cart.WebRequest.FinalizeTransaction(bot, transID).ConfigureAwait(false);

                        if (response4 == null)
                        {
                            return (bot.BotName, result);
                        }

                        await Task.Delay(2000).ConfigureAwait(false);

                        long balanceNow = bot.WalletBalance;
                        result.BalanceNow = balanceNow;
                        result.Cost = balancePrev - balanceNow;

                        //自动清空购物车
                        await Cart.WebRequest.ClearCart(bot).ConfigureAwait(false);

                        return (bot.BotName, result);
                    }
                    )).ConfigureAwait(false);

                foreach (var result in results)
                {
                    var p1 = response[result.Item1].PurchaseResult;
                    var p2 = result.Item2;
                    p1.Success = p2.Success;
                    p1.BalancePrev = p2.BalancePrev;
                    p1.BalanceNow = p2.BalanceNow;
                    p1.Cost = p2.Cost;
                }
            }

            return Ok(new GenericResponse<IReadOnlyDictionary<string, PurchaseResultResponse>>(response));
        }
    }
}
