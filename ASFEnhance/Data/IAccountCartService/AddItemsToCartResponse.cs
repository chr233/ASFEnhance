using ArchiSteamFarm.Steam;
using ASFEnhance.Data.Common;
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
}
