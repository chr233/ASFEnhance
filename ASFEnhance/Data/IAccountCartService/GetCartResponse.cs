using Newtonsoft.Json;
using SteamKit2;

namespace ASFEnhance.Data.IAccountCartService;
/// <summary>
/// 获取购物车响应
/// </summary>
public sealed record GetCartResponse
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("cart")]
    public CartData? Cart { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public record CartData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("line_items")]
        public List<CartLineItemData>? LineItems { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed record CartLineItemData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("line_item_id")]
        public string? LineItemId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("packageid")]
        public uint PackageId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("bundleid")]
        public uint BundleId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("time_added")]
        public ulong TimeAdded { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("price_when_added")]
        public PriceWhenAddedData? PriceWhenAdded { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("flags")]
        public FlagsData? Flags { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("gift_info")]
        public AddItemsToCartRequest.GiftInfoData? GiftInfo { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed record PriceWhenAddedData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("amount_in_cents")]
        public string? AmountInCents { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("currency_code")]
        public ECurrencyCode CurrencytCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("formatted_amount")]
        public string? FormattedAmount { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed record FlagsData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("is_gift")]
        public bool IsGift { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("is_private")]
        public bool IsPrivate { get; set; }
    }
}
