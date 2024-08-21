using System.Text.Json.Serialization;

namespace ASFEnhance.Data.WebApi;

/// <summary>
/// 添加愿望单响应
/// </summary>
public record AddWishlistResponse
{
    /// <summary>
    /// 结果
    /// </summary>
    [JsonPropertyName("success")]
    public bool Result { get; set; }

    /// <summary>
    /// 愿望单数量
    /// </summary>
    [JsonPropertyName("wishlistCount")]
    public int WishlistCount { get; set; }
}