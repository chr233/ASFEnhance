using Newtonsoft.Json;

namespace ASFEnhance.Data.IAccountCartService;
/// <summary>
/// 
/// </summary>
public sealed record AddItemsToCartResponse
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("line_item_ids")]
    public List<string>? LineItemIds { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("cart")]
    public CartData? Cart { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public sealed record CartData : GetCartResponse.CartData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("subtotal")]
        public GetCartResponse.PriceWhenAddedData? SubTotal { get; set; }
    }
}
