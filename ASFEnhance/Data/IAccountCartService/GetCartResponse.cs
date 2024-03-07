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
    public sealed record CartData
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
    public sealed record GiftInfoData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("accountid_giftee")]
        public ulong AccountIdGiftee { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("gift_message")]
        public GiftMessageData? GiftMessage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("time_scheduled_send")]
        public ulong TimeScheduledSend { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed record GiftMessageData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("gifteename")]
        public string? GifteeName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("message")]
        public string? Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("sentiment")]
        public string? Sentiment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("signature")]
        public string? Signature { get; set; }
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
