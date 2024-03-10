using ASFEnhance.Data.Common;
using SteamKit2;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data.IAccountCartService;
/// <summary>
/// 获取购物车响应
/// </summary>
public sealed record GetCartResponse
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("cart")]
    public CartData? Cart { get; set; }

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
