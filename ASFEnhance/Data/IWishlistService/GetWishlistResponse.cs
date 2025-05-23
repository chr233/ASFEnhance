using System.Text.Json.Serialization;

namespace ASFEnhance.Data.IWishlistService;
/// <summary>
/// 愿望单响应
/// </summary>
public sealed record GetWishlistResponse
{
    /// <summary>
    /// 愿望单物品
    /// </summary>
    [JsonPropertyName("items")]
    public List<ItemData>? Items { get; set; }

    /// <summary>
    /// 愿望单信息
    /// </summary>
    public sealed record ItemData
    {
        /// <summary>
        /// appid
        /// </summary>
        [JsonPropertyName("appid")]
        public uint AppId { get; set; }
        /// <summary>
        /// 优先级
        /// </summary>
        [JsonPropertyName("priority")]
        public uint Priority { get; set; }
        /// <summary>
        /// 添加日期
        /// </summary>
        [JsonPropertyName("date_added")]
        public ulong DateAdded { get; set; }
    }
}
