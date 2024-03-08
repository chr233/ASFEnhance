using Newtonsoft.Json;

namespace ASFEnhance.Data.IAccountCartService;

/// <summary>
/// 
/// </summary>
public sealed record AddItemsToCartRequest
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("user_country")]
    public string UserCountry { get; set; } = Langs.CountryCode;
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("items")]
    public List<ItemData>? Items { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("navdata")]
    public NavdataData? Navdata { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public sealed record ItemData
    {
        /// <summary>
        /// 
        /// </summary>
        public uint? PackageId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public uint? BundleId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public GiftInfoData? GIftInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public FlagsData? Flags { get; set; }
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
    public class FlagsData
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

    /// <summary>
    /// 
    /// </summary>
    public class NavdataData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("domain")]
        public string? Domain { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("controller")]
        public string ?Controller { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("method")]
        public string? Method { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("submethod")]
        public string? SubMethod { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("feature")]
        public string? Feature { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("depth")]
        public int Depth { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("countrycode")]
        public string CountryCode { get; set; } = Langs.CountryCode;
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("webkey")]
        public int WebKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("is_client")]
        public bool IsClient { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("curator_data")]
        public CuratorData? CuratorData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("is_likely_bot")]
        public bool IsLikelyBot { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("is_utm")]
        public bool IsUtm { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CuratorData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("clanid")]
        public string? ClanId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("listid")]
        public string? ListId { get; set; }
    }

}
