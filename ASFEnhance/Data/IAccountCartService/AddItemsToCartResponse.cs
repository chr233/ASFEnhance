using System.Text.Json.Serialization;

namespace ASFEnhance.Data.IAccountCartService;
/// <summary>
/// 
/// </summary>
public sealed record AddItemsToCartResponse
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("line_item_ids")]
    public List<string>? LineItemIds { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("cart")]
    public CartData? Cart { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public sealed record CartData : GetCartResponse.CartData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("subtotal")]
        public GetCartResponse.PriceWhenAddedData? SubTotal { get; set; }
    }
}
