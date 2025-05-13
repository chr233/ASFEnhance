using System.Text.Json.Serialization;

namespace ASFEnhance.Data.IWishlistService;
/// <summary>
/// 愿望单响应
/// </summary>
public sealed record GetWishlistCountResponse
{
    /// <summary>
    /// 愿望单物品
    /// </summary>
    [JsonPropertyName("count")]
    public int Count { get; set; }
}
