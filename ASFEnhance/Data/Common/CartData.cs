using ArchiSteamFarm.Steam;
using ASFEnhance.Account;
using ASFEnhance.Data.Plugin;
using ASFEnhance.Store;
using SteamKit2;
using System.Text;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data.Common;
/// <summary>
/// 
/// </summary>
public record CartData
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("line_items")]
    public List<CartLineItemData>? LineItems { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("is_valid")]
    public bool IsValid { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("subtotal")]
    public PriceWhenAddedData? SubTotal { get; set; }

    /// <summary>
    /// 生成购物车概要
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="skipLoadName"></param>
    /// <returns></returns>
    public async Task<string?> ToSummary(Bot bot, bool skipLoadName = false)
    {
        if (LineItems == null || LineItems.Count == 0)
        {
            return bot.FormatBotResponse(Langs.CartIsEmpty);
        }

        var gameNameDict = new Dictionary<uint, string>();

        // 加载游戏名称
        if (!skipLoadName)
        {
            var gameIds = new HashSet<SteamGameId>();
            foreach (var item in LineItems)
            {
                if (item.PackageId > 0)
                {
                    gameIds.Add(new SteamGameId(ESteamGameIdType.Sub, item.PackageId.Value));
                }
                else if (item.BundleId > 0)
                {
                    gameIds.Add(new SteamGameId(ESteamGameIdType.Bundle, item.BundleId.Value));
                }
            }

            var getItemResponse = await bot.GetStoreItems(gameIds).ConfigureAwait(false);
            var gameInfos = getItemResponse?.StoreItems;

            if (gameInfos != null)
            {
                foreach (var info in gameInfos)
                {
                    gameNameDict.TryAdd(info.Id, info.Name ?? "Unknown");
                }
            }
        }

        var walletCurrency = bot.WalletCurrency != ECurrencyCode.Invalid ? bot.WalletCurrency.ToString() : "";
        if (!CurrencyHelper.Currency2Symbol.TryGetValue(walletCurrency, out var currencySymbol))
        {
            currencySymbol = walletCurrency;
        }

        var sb = new StringBuilder();

        sb.AppendLine(bot.FormatBotResponse(Langs.MultipleLineResult));
        sb.AppendLineFormat(Langs.CartIsValid, Bool2Str(IsValid));

        decimal cartValue = 0;
        int count = 1;
        foreach (var item in LineItems)
        {
            if (string.IsNullOrEmpty(item.LineItemId) || (item.BundleId == 0 && item.PackageId == 0))
            {
                sb.AppendLineFormat(Langs.CartItem, item.LineItemId, Langs.CanNotParse);
                continue;
            }

            if (decimal.TryParse(item.PriceWhenAdded?.AmountInCents, out var coast))
            {
                cartValue += coast;
            }

            var price = item.PriceWhenAdded?.FormattedAmount ?? "??";
            if (!gameNameDict.TryGetValue(item.PackageId ?? item.BundleId ?? 0, out var gameName))
            {
                gameName = Langs.KeyNotFound;
            }

            if (item.PackageId > 0)
            {
                sb.AppendLineFormat(Langs.CartItemSub, count++, item.PackageId);
            }
            else if (item.BundleId > 0)
            {
                sb.AppendLineFormat(Langs.CartItemBundle, count++, item.PackageId);
            }

            sb.AppendLineFormat(Langs.CartItemId, item.LineItemId);
            sb.AppendLineFormat(Langs.AppDetailName, gameName);
            sb.AppendLineFormat(Langs.AppDetailPrice, price);

            if (item.Flags?.IsPrivate == true)
            {
                sb.AppendLine(Langs.CartPrivate);
            }

            if (item.Flags?.IsGift == true)
            {
                if (item.GiftInfo?.AccountIdGiftee > 0)
                {
                    sb.AppendLineFormat(Langs.CartGift, GetGifteeProfile(item.GiftInfo.AccountIdGiftee));
                    if (item.GiftInfo != null)
                    {
                        sb.AppendLineFormat(Langs.CartGiftGifteeName, item.GiftInfo.GiftMessage?.GifteeName);
                        sb.AppendLineFormat(Langs.CartGiftMessage, item.GiftInfo.GiftMessage?.Message);
                        sb.AppendLineFormat(Langs.CartGiftSignature, item.GiftInfo.GiftMessage?.Signature);
                        sb.AppendLineFormat(Langs.CartGiftSentiment, item.GiftInfo.GiftMessage?.Sentiment);
                    }
                }
                else
                {
                    sb.AppendLine(Langs.CartGifteeNotSet);
                }
            }

            sb.AppendLineFormat("有效: {0}", Bool2Str(item.IsValid));
        }

        sb.AppendLine();
        if (SubTotal != null)
        {
            sb.AppendLineFormat(Langs.CartTotalValue, SubTotal.FormattedAmount);
        }
        else
        {
            sb.AppendLineFormat(Langs.CartTotalPrice, cartValue / 100, currencySymbol);
        }

        return sb.ToString();
    }


    /// <summary>
    /// 
    /// </summary>
    public sealed record CartLineItemData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("line_item_id")]
        public string? LineItemId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("packageid")]
        public uint? PackageId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("bundleid")]
        public uint? BundleId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("is_valid")]
        public bool IsValid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("time_added")]
        public ulong TimeAdded { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("price_when_added")]
        public PriceWhenAddedData? PriceWhenAdded { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("flags")]
        public FlagsData? Flags { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("gift_info")]
        public GiftInfoData? GiftInfo { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed record PriceWhenAddedData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("amount_in_cents")]
        public string? AmountInCents { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("currency_code")]
        public ECurrencyCode CurrencytCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("formatted_amount")]
        public string? FormattedAmount { get; set; }
    }
}
