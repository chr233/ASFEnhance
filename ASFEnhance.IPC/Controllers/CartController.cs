using ArchiSteamFarm.Core;
using ArchiSteamFarm.IPC.Responses;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Data.Common;
using ASFEnhance.Data.IAccountCartService;
using ASFEnhance.Data.Plugin;
using ASFEnhance.IPC.Controllers.Base;
using ASFEnhance.IPC.Data.Requests;
using ASFEnhance.IPC.Data.Responses;
using ASFEnhance.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SteamKit2;
using System.Globalization;
using System.Net;
using WebRequest = ASFEnhance.Cart.WebRequest;

namespace ASFEnhance.IPC.Controllers;

/// <summary>
/// 购物车相关接口
/// </summary>
[Route("/Api/[controller]/[action]")]
public sealed class CartController : AbstractController
{
    /// <summary>
    /// 获取游戏详情
    /// </summary>
    /// <param name="botName"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("{botName:required}")]
    [EndpointDescription("需要指定AppIds列表")]
    [EndpointSummary("获取游戏详情")]
    public async Task<ActionResult<GenericResponse>> GetAppDetail(string botName, [FromBody] AppIdListRequest request)
    {
        if (string.IsNullOrEmpty(botName))
        {
            throw new ArgumentNullException(nameof(botName));
        }

        ArgumentNullException.ThrowIfNull(request);

        if (!Config.EULA)
        {
            return BadRequest(new GenericResponse(false, Langs.EulaFeatureUnavilable));
        }

        var bot = Bot.GetBot(botName);
        if (bot == null)
        {
            return BadRequest(new GenericResponse(false, string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botName)));
        }

        var items = new List<IdData>();
        if (request.AppIds != null)
        {
            foreach (var appId in request.AppIds)
            {
                items.Add(new IdData { AppId = appId });
            }
        }
        if (request.PackageIds != null)
        {
            foreach (var subId in request.PackageIds)
            {
                items.Add(new IdData { PackageId = subId });
            }
        }
        if (request.BundleIds != null)
        {
            foreach (var bundleId in request.BundleIds)
            {
                items.Add(new IdData { BundleId = bundleId });
            }
        }

        if (items.Count == 0)
        {
            return BadRequest(new GenericResponse(false, "AppIds 或 PackageIds 或 BundleId 无效"));
        }

        var result = await bot.GetStoreItems(items).ConfigureAwait(false);

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

        var response = new Dictionary<string, AppDetailDictResponse>
       {
           { botName, dict },
       };

        return Ok(new GenericResponse<IReadOnlyDictionary<string, AppDetailDictResponse>>(response));
    }

    /// <summary>
    /// 清空购物车
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("{botNames:required}")]
    [EndpointDescription("清除购物车所有内容")]
    [EndpointSummary("清空购物车")]
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

        var results = await Utilities.InParallel(bots.Select(x => WebRequest.ClearAccountCart(x))).ConfigureAwait(false);

        var response = new BoolDictResponse();
        var i = 0;
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
    [EndpointDescription("读取机器人购物车内容")]
    [EndpointSummary("读取机器人购物车")]
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

        var results = await Utilities.InParallel(bots.Select(x => WebRequest.GetAccountCart(x))).ConfigureAwait(false);

        var response = new Dictionary<string, BotCartResponse?>();
        var i = 0;
        foreach (var result in results)
        {
            if (i >= bots.Count)
            {
                break;
            }

            var bot = bots.ElementAt(i++);


            var cart = result?.Cart;
            if (cart == null)
            {
                response.Add(bot.BotName, null);
            }
            else
            {
                var data = new List<BotCartResponse.CartItemData>();
                if (cart.LineItems?.Count > 0)
                {
                    var ids = new HashSet<SteamGameId>();
                    foreach (var item in cart.LineItems)
                    {
                        if (item.PackageId > 0)
                        {
                            ids.Add(new SteamGameId(ESteamGameIdType.Sub, item.PackageId.Value));
                        }
                        else if (item.BundleId > 0)
                        {
                            ids.Add(new SteamGameId(ESteamGameIdType.Bundle, item.BundleId.Value));
                        }
                    }

                    var getItemResponse = await bot.GetStoreItems(ids).ConfigureAwait(false);
                    var gameInfos = getItemResponse?.StoreItems;

                    var gameNameDict = new Dictionary<uint, string>();

                    if (gameInfos != null)
                    {
                        foreach (var info in gameInfos)
                        {
                            gameNameDict.TryAdd(info.Id, info.Name ?? "Unknown");
                        }
                    }

                    foreach (var item in cart.LineItems)
                    {
                        if (!gameNameDict.TryGetValue(item.PackageId ?? item.BundleId ?? 0, out var gameName))
                        {
                            gameName = Langs.KeyNotFound;
                        }

                        data.Add(new BotCartResponse.CartItemData
                        {
                            PackageId = item.PackageId,
                            BundleId = item.BundleId,
                            LineItemId = item.LineItemId,
                            Name = gameName,
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
                response.Add(bot.BotName, new BotCartResponse
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
    [EndpointDescription("IsGift为True时需要定义GiftInfo")]
    [EndpointSummary("购物车添加项目")]
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
                if (giftInfo != null && giftInfo.AccountIdGiftee != 0)
                {
                    payload.GIftInfo = new GiftInfoData
                    {
                        AccountIdGiftee = giftInfo.AccountIdGiftee,
                        GiftMessage = new GiftMessageData
                        {
                            GifteeName = giftInfo.GifteeName ?? "",
                            Message = giftInfo.Message ?? Config.CustomGifteeMessage ?? "Send via ASFEnhance",
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

        var results = await Utilities.InParallel(bots.Select(bot => WebRequest.AddItemsToAccountCart(bot, payloads))).ConfigureAwait(false);

        var response = new Dictionary<string, BotCartResponse?>();
        var i = 0;
        foreach (var result in results)
        {
            if (i >= bots.Count)
            {
                break;
            }

            var bot = bots.ElementAt(i++);

            var cart = result?.Cart;
            if (cart == null)
            {
                response.Add(bot.BotName, null);
            }
            else
            {
                var data = new List<BotCartResponse.CartItemData>();
                if (cart.LineItems?.Count > 0)
                {
                    var ids = new HashSet<SteamGameId>();
                    foreach (var item in cart.LineItems)
                    {
                        if (item.PackageId > 0)
                        {
                            ids.Add(new SteamGameId(ESteamGameIdType.Sub, item.PackageId.Value));
                        }
                        else if (item.BundleId > 0)
                        {
                            ids.Add(new SteamGameId(ESteamGameIdType.Bundle, item.BundleId.Value));
                        }
                    }

                    var getItemResponse = await bot.GetStoreItems(ids).ConfigureAwait(false);
                    var gameInfos = getItemResponse?.StoreItems;

                    var gameNameDict = new Dictionary<uint, string>();

                    if (gameInfos != null)
                    {
                        foreach (var info in gameInfos)
                        {
                            gameNameDict.TryAdd(info.Id, info.Name ?? "Unknown");
                        }
                    }

                    foreach (var item in cart.LineItems)
                    {
                        if (!gameNameDict.TryGetValue(item.PackageId ?? item.BundleId ?? 0, out var gameName))
                        {
                            gameName = Langs.KeyNotFound;
                        }

                        data.Add(new BotCartResponse.CartItemData
                        {
                            PackageId = item.PackageId,
                            BundleId = item.BundleId,
                            LineItemId = item.LineItemId,
                            Name = gameName,
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
                response.Add(bot.BotName, new BotCartResponse
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
    [EndpointDescription("结算当前购物车")]
    [EndpointSummary("购物车下单")]
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

                var balancePrev = bot.WalletBalance;

                result.BalanceNow = balancePrev;
                result.BalancePrev = balancePrev;

                if (balancePrev < 1)
                {
                    return result;
                }

                var response1 = await WebRequest.CheckOut(bot).ConfigureAwait(false);

                if (response1 == null)
                {
                    return result;
                }

                AddressConfig? address = null;
                if (Config.Addresses?.Count > 0)
                {
                    address = Config.Addresses[Random.Shared.Next(0, Config.Addresses.Count)];
                }

                var response2 = await WebRequest.InitTransaction(bot, "steamaccount", address).ConfigureAwait(false);

                if (response2 == null)
                {
                    return result;
                }

                var transId = response2.TransId ?? response2.TransActionId;

                if (string.IsNullOrEmpty(transId))
                {
                    return result;
                }

                var response3 = await WebRequest.GetFinalPrice(bot, transId).ConfigureAwait(false);

                if (response3 == null || response2.TransId == null)
                {
                    return result;
                }

                if (!request.FakePurchase)
                {
                    var response4 = await WebRequest.FinalizeTransaction(bot, transId).ConfigureAwait(false);

                    if (response4 == null)
                    {
                        return result;
                    }

                    await Task.Delay(2000).ConfigureAwait(false);
                }
                else
                {
                    var response4 = await WebRequest.CancelTransaction(bot, transId).ConfigureAwait(false);

                    if (response4 == null)
                    {
                        return result;
                    }
                    result.Success = true;
                }

                var balanceNow = bot.WalletBalance;
                result.BalanceNow = balanceNow;
                result.Cost = balancePrev - balanceNow;

                //自动清空购物车
                await WebRequest.ClearAccountCart(bot).ConfigureAwait(false);

                return result;
            }
            )).ConfigureAwait(false);

        var response = new Dictionary<string, OnlyPurchaseResponse>();

        var i = 0;
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

    /// <summary>
    /// 使用外部支付结算购物车
    /// </summary>
    /// <param name="botName"></param>
    /// <param name="tranId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("{botName:required}")]
    [EndpointDescription("使用外部支付结算购物车")]
    [EndpointSummary("购物车下单(外部支付方式)")]
    public async Task<ActionResult<GenericResponse>> CancelPayment(string botName, string tranId)
    {

        if (!Config.EULA)
        {
            return BadRequest(new GenericResponse(false, Langs.EulaFeatureUnavilable));
        }

        if (string.IsNullOrEmpty(tranId))
        {
            return BadRequest(new GenericResponse(false, "TranId 不能为空"));
        }

        var bot = Bot.GetBot(botName);
        if (bot == null)
        {
            return BadRequest(new GenericResponse(false, string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botName)));
        }

        if (!bot.IsConnectedAndLoggedOn)
        {
            return Ok(new GenericResponse(false, string.Format(CultureInfo.CurrentCulture, Strings.BotNotConnected)));
        }

        var result = await WebRequest.CancelTransaction(bot, tranId).ConfigureAwait(false);
        if (result?.Result == EResult.OK)
        {
            return Ok(new GenericResponse(true, "Ok"));
        }

        return Ok(new GenericResponse(false, "取消订单失败"));
    }

    /// <summary>
    /// 使用外部支付结算购物车
    /// </summary>
    /// <param name="botName"></param>
    /// <param name="payment"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("{botName:required}")]
    [EndpointDescription("使用外部支付结算购物车")]
    [EndpointSummary("购物车下单(外部支付方式)")]
    public async Task<ActionResult<GenericResponse>> PurchaseExternal(string botName, string payment = "alipay")
    {
        if (string.IsNullOrEmpty(botName))
        {
            throw new ArgumentNullException(nameof(botName));
        }

        if (!Config.EULA)
        {
            return BadRequest(new GenericResponse(false, Langs.EulaFeatureUnavilable));
        }

        if (string.IsNullOrEmpty(payment))
        {
            return BadRequest(new GenericResponse(false, "Payment 不能为空"));
        }

        var bot = Bot.GetBot(botName);
        if (bot == null)
        {
            return BadRequest(new GenericResponse(false, string.Format(CultureInfo.CurrentCulture, Strings.BotNotFound, botName)));
        }

        //下单
        if (!bot.IsConnectedAndLoggedOn)
        {
            return Ok(new GenericResponse(false, string.Format(CultureInfo.CurrentCulture, Strings.BotNotConnected)));
        }

        var response1 = await WebRequest.CheckOut(bot).ConfigureAwait(false);

        if (response1 == null)
        {
            return Ok(new GenericResponse(false, "CheckOut 失败"));
        }

        AddressConfig? address = null;
        if (Config.Addresses?.Count > 0)
        {
            address = Config.Addresses[Random.Shared.Next(0, Config.Addresses.Count)];
        }

        var response2 = await WebRequest.InitTransaction(bot, payment, address).ConfigureAwait(false);
        if (string.IsNullOrEmpty(response2?.TransId))
        {
            return Ok(new GenericResponse(false, "InitTransaction 失败"));
        }

        var transId = response2.TransId;
        var response3 = await WebRequest.GetFinalPrice(bot, transId).ConfigureAwait(false);

        if (string.IsNullOrEmpty(response3?.ExternalUrl) || string.IsNullOrEmpty(response2.TransId))
        {
            return Ok(new GenericResponse(false, "GetFinalPrice 失败"));
        }

        var response4 = await WebRequest.TransactionStatusAddFunds(bot, null, transId).ConfigureAwait(false);
        if (response4?.Result != EResult.Pending)
        {
            return Ok(new GenericResponse(false, "TransactionStatus 失败"));
        }

        var response5 = await WebRequest.CheckoutExternalLinkAddFunds(bot, null, transId).ConfigureAwait(false);
        if (response5 == null)
        {
            return Ok(new GenericResponse(false, "Checkout 失败"));
        }

        var finalUrl = await WebRequest.GetRealExternalPaymentLink(bot, response5).ConfigureAwait(false);
        if (finalUrl == null)
        {
            return Ok(new GenericResponse(false, "获取支付链接失败"));
        }

        return Ok(new GenericResponse<ExternalPurchaseResponse>(true, "Ok", new ExternalPurchaseResponse(transId, response3.FormattedTotal, finalUrl.ToString())));
    }
}
