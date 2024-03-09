using ArchiSteamFarm.Core;
using ArchiSteamFarm.IPC.Responses;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Cart;
using ASFEnhance.Data.Common;
using ASFEnhance.Data.IAccountCartService;
using ASFEnhance.IPC.Requests;
using ASFEnhance.IPC.Responses;
using ASFEnhance.Store;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;
using System.Globalization;
using System.Net;

namespace ASFEnhance.IPC.Controllers;

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
    [HttpPost("{botNames:required}")]
    [SwaggerOperation(Summary = "获取游戏详情", Description = "需要指定AppIds列表")]
    [ProducesResponseType(typeof(GenericResponse<IReadOnlyDictionary<string, AppDetailDictResponse>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<GenericResponse>> GetAppDetail(string botNames, [FromBody] AppIdListRequest request)
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

        var items = new List<IdData>();
        if (request.AppIds != null)
        {
            foreach (var appid in request.AppIds)
            {
                items.Add(new IdData { AppId = appid });
            }
        }
        if (request.PackageIds != null)
        {
            foreach (var subid in request.PackageIds)
            {
                items.Add(new IdData { PackageId = subid });
            }
        }
        if (request.BundleIds != null)
        {
            foreach (var bundleid in request.BundleIds)
            {
                items.Add(new IdData { BundleId = bundleid });
            }
        }

        if (items.Count == 0)
        {
            return BadRequest(new GenericResponse(false, "AppIds 或 PackageIds 或 BundleId 无效"));
        }

        var results = await Utilities.InParallel(bots.Select(x => x.GetStoreItems(items))).ConfigureAwait(false);

        var response = new Dictionary<string, AppDetailDictResponse>();
        int i = 0;
        foreach (var result in results)
        {
            if (i >= bots.Count)
            {
                break;
            }

            var dict = new AppDetailDictResponse();
            if (result?.StoreItems?.Count > 0)
            {
                foreach (var storeItem in result.StoreItems)
                {
                    var prefix = storeItem.ItemType switch
                    {
                        0 => "app",
                        1 => "sub",
                        2 => "bundle",
                        _ => storeItem.ItemType.ToString()
                    };

                    var key = $"{prefix}/{storeItem.Id}";

                    var subs = new List<SubInfo>();
                    if (storeItem.PurchaseOptions != null)
                    {
                        foreach (var purchase in storeItem.PurchaseOptions)
                        {
                            subs.Add(new SubInfo
                            {
                                PackageId = purchase.PackageId,
                                BundleId = purchase.BundleId,
                                Name = purchase.PurchaseOptionName,
                                PriceInCents = purchase.FinalPriceInCents,
                                PriceFormatted = purchase.FormattedFinalPrice,
                                CanPurchaseAsGift = purchase.UserCanPurchaseAsGift,
                                IncludeGameCount = purchase.IncludedGameCount,
                                RequiresShipping = purchase.RequiresShipping,
                            });
                        }
                    }
                    var value = new AppDetail
                    {
                        Success = storeItem.Success == 1,
                        AppId = storeItem.AppId,
                        Name = storeItem.Name ?? "",
                        Type = prefix,
                        Desc = storeItem.FullDescription ?? "",
                        IsFree = storeItem.IsFree,
                        Released = false,
                        Subs = subs,
                    };

                    dict.TryAdd(key, value);
                }
            }
            response.TryAdd(bots.ElementAt(i++).BotName, dict);
        }

        return Ok(new GenericResponse<IReadOnlyDictionary<string, AppDetailDictResponse>>(response));
    }

    /// <summary>
    /// 清空购物车
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("{botNames:required}")]
    [SwaggerOperation(Summary = "清空购物车", Description = "清除购物车所有内容")]
    [ProducesResponseType(typeof(GenericResponse<BoolDictResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<GenericResponse>> ClearCart(string botNames)
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

        var results = await Utilities.InParallel(bots.Select(x => x.ClearAccountCart())).ConfigureAwait(false);

        var response = new BoolDictResponse();
        int i = 0;
        foreach (var result in results)
        {
            if (i >= bots.Count)
            {
                break;
            }

            response.Add(bots.ElementAt(i++).BotName, result ?? false);
        }

        return Ok(new GenericResponse<BoolDictResponse>(true, "Ok", response));
    }

    /// <summary>
    /// 清空购物车
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("{botNames:required}")]
    [SwaggerOperation(Summary = "读取机器人购物车", Description = "读取机器人购物车内容")]
    [ProducesResponseType(typeof(GenericResponse<IReadOnlyDictionary<string, BotCartResponse?>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<GenericResponse>> GetCart(string botNames)
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

        var results = await Utilities.InParallel(bots.Select(x => x.GetAccountCart())).ConfigureAwait(false);

        var response = new Dictionary<string, BotCartResponse?>();
        int i = 0;
        foreach (var result in results)
        {
            if (i >= bots.Count)
            {
                break;
            }

            var cart = result?.Cart;
            if (cart == null)
            {
                response.Add(bots.ElementAt(i++).BotName, null);
            }
            else
            {
                var data = new List<BotCartResponse.CartItemData>();
                if (cart.LineItems?.Count > 0)
                {
                    foreach (var item in cart.LineItems)
                    {
                        data.Add(new BotCartResponse.CartItemData
                        {
                            PackageId = item.PackageId,
                            BundleId = item.BundleId,
                            LineItemId = item.LineItemId,
                            IsValid = item.IsValid,
                            TimeAdded = item.TimeAdded,
                            PriceCents = item.PriceWhenAdded?.AmountInCents,
                            CurrencyCode = item.PriceWhenAdded?.CurrencytCode ?? SteamKit2.ECurrencyCode.Invalid,
                            PriceFormatted = item.PriceWhenAdded?.FormattedAmount,
                            IsGift = item.Flags?.IsGift ?? false,
                            IsPrivate = item.Flags?.IsPrivate ?? false,
                            GiftInfo = item.GiftInfo,
                        });
                    }
                }
                response.Add(bots.ElementAt(i++).BotName, new BotCartResponse
                {
                    Items = data,
                    IsValid = cart.IsValid,
                });
            }
        }

        return Ok(new GenericResponse<IReadOnlyDictionary<string, BotCartResponse?>>(true, "Ok", response));
    }


    /// <summary>
    /// 向购物车添加内容
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("{botNames:required}")]
    [SwaggerOperation(Summary = "购物车添加项目", Description = "IsGift为True时需要定义GiftInfo")]
    [ProducesResponseType(typeof(GenericResponse<IReadOnlyDictionary<string, BotCartResponse>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<GenericResponse>> AddCart(string botNames, [FromBody] AddCartRequest request)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Items);

        if (!Config.EULA)
        {
            return BadRequest(new GenericResponse(false, Langs.EulaFeatureUnavilable));
        }

        var bots = Bot.GetBots(botNames);
        if (bots == null || bots.Count == 0)
        {
            return BadRequest(new GenericResponse(false, string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botNames)));
        }

        var payloads = new List<AddItemsToCartRequest.ItemData>();
        foreach (var item in request.Items)
        {
            if (item.BundleId == 0 && item.PackageId == 0)
            {
                continue;
            }

            var payload = new AddItemsToCartRequest.ItemData
            {
                PackageId = item.PackageId == 0 ? null : item.PackageId,
                BundleId = item.BundleId == 0 ? null : item.BundleId,
                Flags = new FlagsData
                {
                    IsPrivate = item.IsPrivate,
                    IsGift = item.IsGift,
                }
            };

            var giftInfo = item.GiftInfo;
            if (item.IsGift)
            {
                if (giftInfo != null && giftInfo.AccountIdGiftee == 0)
                {
                    payload.GIftInfo = new GiftInfoData
                    {
                        AccountIdGiftee = giftInfo.AccountIdGiftee,
                        GiftMessage = new GiftMessageData
                        {
                            GifteeName = giftInfo.GifteeName ?? "",
                            Message = giftInfo.Message ?? "Send via ASFEnhance",
                            Sentiment = giftInfo.Sentiment ?? "",
                            Signature = giftInfo.Signature ?? "ASFEnhance",
                        },
                        TimeScheduledSend = giftInfo.TimeScheduledSend,
                    };
                }
                else
                {
                    return BadRequest(new GenericResponse(false, "IsGift为true时, GiftInfo不能为空"));
                }
            }
            payloads.Add(payload);
        }

        if (payloads.Count <= 0)
        {
            return BadRequest(new GenericResponse(false, "SubIds 和 BundleIds 不能同时为 null"));
        }

        var results = await Utilities.InParallel(bots.Select(bot => Cart.WebRequest.AddItemsToAccountCart(bot, payloads))).ConfigureAwait(false);

        var response = new Dictionary<string, BotCartResponse?>();
        int i = 0;
        foreach (var result in results)
        {
            if (i >= bots.Count)
            {
                break;
            }

            var cart = result?.Cart;
            if (cart == null)
            {
                response.Add(bots.ElementAt(i++).BotName, null);
            }
            else
            {
                var data = new List<BotCartResponse.CartItemData>();
                if (cart.LineItems?.Count > 0)
                {
                    foreach (var item in cart.LineItems)
                    {
                        data.Add(new BotCartResponse.CartItemData
                        {
                            PackageId = item.PackageId,
                            BundleId = item.BundleId,
                            LineItemId = item.LineItemId,
                            IsValid = item.IsValid,
                            TimeAdded = item.TimeAdded,
                            PriceCents = item.PriceWhenAdded?.AmountInCents,
                            CurrencyCode = item.PriceWhenAdded?.CurrencytCode ?? SteamKit2.ECurrencyCode.Invalid,
                            PriceFormatted = item.PriceWhenAdded?.FormattedAmount,
                            IsGift = item.Flags?.IsGift ?? false,
                            IsPrivate = item.Flags?.IsPrivate ?? false,
                            GiftInfo = item.GiftInfo,
                        });
                    }
                }
                response.Add(bots.ElementAt(i++).BotName, new BotCartResponse
                {
                    Items = data,
                    IsValid = cart.IsValid,
                });
            }
        }

        return Ok(new GenericResponse<IReadOnlyDictionary<string, BotCartResponse?>>(true, "Ok", response));
    }

    /// <summary>
    /// 购买指定游戏
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("{botNames:required}")]
    [SwaggerOperation(Summary = "购物车下单", Description = "结算当前购物车")]
    [ProducesResponseType(typeof(GenericResponse<IReadOnlyDictionary<string, OnlyPurchaseResponse>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<GenericResponse>> Purchase(string botNames, [FromBody] OnlyPurchaseRequest request)
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

        //下单
        var results = await Utilities.InParallel(bots.Select(
            async bot =>
            {
                var result = new OnlyPurchaseResponse
                {
                    Success = false,
                    BalanceNow = -1,
                    BalancePrev = -1,
                    Currency = bot.WalletCurrency.ToString(),
                };

                if (!bot.IsConnectedAndLoggedOn)
                {
                    return result;
                }

                long balancePrev = bot.WalletBalance;

                result.BalanceNow = balancePrev;
                result.BalancePrev = balancePrev;

                if (balancePrev < 1)
                {
                    return result;
                }

                var response1 = await Cart.WebRequest.CheckOut(bot).ConfigureAwait(false);

                if (response1 == null)
                {
                    return result;
                }

                var response2 = await Cart.WebRequest.InitTransaction(bot).ConfigureAwait(false);

                if (response2 == null)
                {
                    return result;
                }

                string? transId = response2.TransId ?? response2.TransActionId;

                if (string.IsNullOrEmpty(transId))
                {
                    return result;
                }

                var response3 = await Cart.WebRequest.GetFinalPrice(bot, transId, false).ConfigureAwait(false);

                if (response3 == null || response2.TransId == null)
                {
                    return result;
                }

                if (!request.FakePurchase)
                {
                    var response4 = await Cart.WebRequest.FinalizeTransaction(bot, transId).ConfigureAwait(false);

                    if (response4 == null)
                    {
                        return result;
                    }

                    await Task.Delay(2000).ConfigureAwait(false);
                }
                else
                {
                    var response4 = await Cart.WebRequest.CancelTransaction(bot, transId).ConfigureAwait(false);

                    if (response4 == null)
                    {
                        return result;
                    }
                    result.Success = true;
                }

                long balanceNow = bot.WalletBalance;
                result.BalanceNow = balanceNow;
                result.Cost = balancePrev - balanceNow;

                //自动清空购物车
                await Cart.WebRequest.ClearAccountCart(bot).ConfigureAwait(false);

                return result;
            }
            )).ConfigureAwait(false);

        var response = new Dictionary<string, OnlyPurchaseResponse>();

        int i = 0;
        foreach (var bot in bots)
        {
            if (i >= results.Count)
            {
                break;
            }

            response.Add(bot.BotName, results[i++]);
        }

        return Ok(new GenericResponse<IDictionary<string, OnlyPurchaseResponse>>(true, "Ok", response));
    }
}
