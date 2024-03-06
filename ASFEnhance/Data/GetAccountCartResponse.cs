using Newtonsoft.Json;
using SteamKit2;

namespace ASFEnhance.Data;
/// <summary>
/// 获取购物车响应
/// </summary>
public record GetAccountCartResponse
{
    [JsonProperty("response")]
    public ResponseData? Response { get; set; }

    public sealed record ResponseData
    {
        [JsonProperty("cart")]
        public CartData? Cart { get; set; }
    }

    public sealed record CartData
    {
        [JsonProperty("line_items")]
        public List<CartLineItemData> LineItems { get; set; }
        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }
    }

    public sealed record CartLineItemData
    {
        [JsonProperty("line_item_id")]
        public string? LineItemId { get; set; }
        public int Type { get; set; }
        [JsonProperty("packageid")]
        public uint PackageId { get; set; }
        [JsonProperty("bundleid")]
        public uint BundleId { get; set; }
        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }
        [JsonProperty("time_added")]
        public ulong TimeAdded { get; set; }
        [JsonProperty("price_when_added")]
        public PriceWhenAddedData? PriceWhenAdded { get; set; }
        [JsonProperty("flags")]
        public FlagsData? Flags { get; set; }
        [JsonProperty("gift_info")]
        public GiftInfoData? GiftInfo { get; set; }
    }

    public sealed record PriceWhenAddedData
    {
        [JsonProperty("amount_in_cents")]
        public string? MyProperty { get; set; }
        [JsonProperty("currency_code")]
        public ECurrencyCode CurrencytCode { get; set; }
        [JsonProperty("formatted_amount")]
        public string? FprmattedAmount { get; set; }
    }

    public sealed record GiftInfoData
    {
        [JsonProperty("accountid_giftee")]
        public ulong AccountIdGiftee { get; set; }
        [JsonProperty("gift_message")]
        public GiftMessageData? GiftMessage { get; set; }
        [JsonProperty("time_scheduled_send")]
        public ulong TimeScheduledSend { get; set; }
    }

    public sealed record GiftMessageData
    {

        [JsonProperty("gifteename")]
        public string? GifteeName { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("sentiment")]
        public string? Sentiment { get; set; }

        [JsonProperty("signature")]
        public string? Signature { get; set; }
    }

    public sealed record FlagsData
    {
        [JsonProperty("is_gift")]
        public bool IsGift { get; set; }
        [JsonProperty("is_private")]
        public bool IsPrivate { get; set; }
    }
}
