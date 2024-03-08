using Newtonsoft.Json;

namespace ASFEnhance.Data.IStoreBrowseService;
/// <summary>
/// 
/// </summary>
public sealed record GetItemsResponse
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("store_items")]
    public List<StoreItemData>? StoreItems { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public sealed record StoreItemData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("item_type")]
        public int ItemType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("id")]
        public uint Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("success")]
        public byte Success { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("visible")]
        public bool Visiable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("store_url_path")]
        public string? StoreUrlPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("appid")]
        public uint AppId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("type")]
        public byte Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("is_free")]
        public bool IsFree { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("purchase_options")]
        public List<PurchaseOptionData>? PurchaseOptions { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("full_description")]
        public string? FullDescription { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed record PurchaseOptionData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("packageid")]
        public uint? PackageId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("bundleid")]
        public uint? BundleId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("purchase_option_name")]
        public string? PurchaseOptionName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("final_price_in_cents")]
        public string? FinalPriceInCents { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("formatted_final_price")]
        public string? FormattedFinalPrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("user_can_purchase_as_gift")]
        public bool UserCanPurchaseAsGift { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("hide_discount_pct_for_compliance")]
        public bool HideDiscountPctForCompliance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("included_game_count")]
        public int IncludedGameCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("requires_shipping")]
        public bool RequiresShipping { get; set; }
    }
}
