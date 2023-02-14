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
        [SwaggerOperation(Summary = "获取游戏详情", Description = "需要指定AppIds列表")]
        [ProducesResponseType(typeof(GenericResponse<IReadOnlyDictionary<string, AppDetailDictResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<GenericResponse>> GetAppDetail(string botNames, [FromBody] AppIdListRequest request)
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

            if (request.AppIds == null || request.AppIds.Count == 0)
            {
                return BadRequest(new GenericResponse(false, "AppIds 无效"));
            }

            var response = bots.ToDictionary(x => x.BotName, x => new AppDetailDictResponse());

            foreach (uint appid in request.AppIds)
            {
                var results = await Utilities.InParallel(bots.Select(
                    async bot => {
                        if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, new()); }

                        var detail = await Store.WebRequest.GetAppDetails(bot, appid).ConfigureAwait(false);

                        if (detail == null)
                        {
                            return (bot.BotName, new() { Success = false });
                        }

                        var data = detail.Data;

                        AppDetail result = new() {
                            Success = detail.Success,
                            AppId = appid,
                            Name = data.Name,
                            Type = data.Type,
                            Desc = data.ShortDescription,
                            IsFree = data.IsFree,
                            Released = !data.ReleaseDate.ComingSoon,
                            Subs = new(),
                        };

                        foreach (var subs in data.PackageGroups)
                        {
                            foreach (var sub in subs.Subs!)
                            {
                                result.Subs.Add(new() {
                                    SubId = sub.SubId,
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
        [SwaggerOperation(Summary = "购买指定游戏", Description = "SubIds和BundleIds可省略,但是必须指定一种,也可以都指定")]
        [ProducesResponseType(typeof(GenericResponse<IReadOnlyDictionary<string, BoolDictResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<GenericResponse>> PurchaseGame(string botNames, [FromBody] PurchaseRequest request)
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

            if ((request.SubIds == null && request.BundleIds == null) || request.SubIds?.Count + request.BundleIds?.Count <= 0)
            {
                return BadRequest(new GenericResponse(false, "SubIds 和 BundleIds 不能同时为 null"));
            }

            var response = bots.ToDictionary(x => x.BotName, x => new PurchaseResultResponse());

            //清空购物车
            await Utilities.InParallel(bots.Select(bot => Cart.WebRequest.ClearCart(bot))).ConfigureAwait(false);

            if (request.BundleIds?.Count > 0)
            {
                foreach (uint bundleid in request.BundleIds)
                {
                    var results = await Utilities.InParallel(bots.Select(
                        async bot => {
                            if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, false); }

                            bool? result = await Cart.WebRequest.AddCart(bot, bundleid, true).ConfigureAwait(false);
                            return (bot.BotName, result ?? false);
                        }
                    )).ConfigureAwait(false);

                    foreach (var result in results)
                    {
                        response[result.Item1].AddCartResult.BundleIds.Add(bundleid.ToString(), result.Item2);
                    }
                }
            }

            if (request.SubIds?.Count > 0)
            {
                foreach (uint subid in request.SubIds)
                {
                    var results = await Utilities.InParallel(bots.Select(
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
                        response[result.Item1].AddCartResult.SubIds.Add(subid.ToString(), result.Item2);
                    }
                }
            }

            //记录购物车
            {
                var results = await Utilities.InParallel(bots.Select(
                        async bot => {
                            if (!bot.IsConnectedAndLoggedOn) { return (bot.BotName, new()); }

                            var cartData = await Cart.WebRequest.GetCartGames(bot).ConfigureAwait(false);

                            PurchaseResult result = new() {
                                Currency = bot.WalletCurrency.ToString(),
                            };

                            if (cartData == null)
                            {
                                result.CartItems.Add(new() {
                                    Type = "Error",
                                    Id = 0,
                                    Name = Langs.NetworkError,
                                });
                            }
                            else
                            {
                                foreach (var c in cartData.CartItems)
                                {
                                    result.CartItems.Add(new() {
                                        Type = c.GameId.Type.ToString(),
                                        Id = c.GameId.GameId,
                                        Name = c.Name,
                                    });
                                }
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
                var results = await Utilities.InParallel(bots.Select(
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

                        string? transId = response2.TransId ?? response2.TransActionId;

                        if (string.IsNullOrEmpty(transId))
                        {
                            return (bot.BotName, result);
                        }

                        var response3 = await Cart.WebRequest.GetFinalPrice(bot, transId, false).ConfigureAwait(false);

                        if (response3 == null || response2.TransId == null)
                        {
                            return (bot.BotName, result);
                        }

                        if (!request.FakePurchase)
                        {
                            var response4 = await Cart.WebRequest.FinalizeTransaction(bot, transId).ConfigureAwait(false);

                            if (response4 == null)
                            {
                                return (bot.BotName, result);
                            }

                            await Task.Delay(2000).ConfigureAwait(false);
                        }
                        else
                        {
                            var response4 = await Cart.WebRequest.CancelTransaction(bot, transId).ConfigureAwait(false);

                            if (response4 == null)
                            {
                                return (bot.BotName, result);
                            }
                            result.Success = true;
                        }

                        long balanceNow = bot.WalletBalance;
                        result.BalanceNow = balanceNow;
                        result.Cost = balancePrev - balanceNow;

                        //自动清空购物车
                        await Cart.WebRequest.ClearCart(bot).ConfigureAwait(false);

                        return (bot.BotName, result);
                    }
                    )).ConfigureAwait(false);

                foreach (var (botName, result) in results)
                {
                    var purchaseResponse = response[botName].PurchaseResult;
                    purchaseResponse.Success = result.Success;
                    purchaseResponse.BalancePrev = result.BalancePrev;
                    purchaseResponse.BalanceNow = result.BalanceNow;
                    purchaseResponse.Cost = result.Cost;
                }
            }

            return Ok(new GenericResponse<IReadOnlyDictionary<string, PurchaseResultResponse>>(response));
        }
    }
}
