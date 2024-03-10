using System.Text.Json.Serialization;

namespace ASFEnhance.Data.IStoreBrowseService;
/// <summary>
/// 
/// </summary>
public sealed record GetItemsResponse
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("store_items")]
    public List<StoreItemData>? StoreItems { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public sealed record StoreItemData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("item_type")]
        public int ItemType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("id")]
        public uint Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("success")]
        public byte Success { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("visible")]
        public bool Visiable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("store_url_path")]
        public string? StoreUrlPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("appid")]
        public uint AppId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("type")]
        public byte Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("is_free")]
        public bool IsFree { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("purchase_options")]
        public List<PurchaseOptionData>? PurchaseOptions { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("full_description")]
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
        [JsonPropertyName("purchase_option_name")]
        public string? PurchaseOptionName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("final_price_in_cents")]
        public string? FinalPriceInCents { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("formatted_final_price")]
        public string? FormattedFinalPrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("user_can_purchase_as_gift")]
        public bool UserCanPurchaseAsGift { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("hide_discount_pct_for_compliance")]
        public bool HideDiscountPctForCompliance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("included_game_count")]
        public int IncludedGameCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("requires_shipping")]
        public bool RequiresShipping { get; set; }
    }
}
