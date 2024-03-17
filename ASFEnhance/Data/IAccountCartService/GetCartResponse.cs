using ASFEnhance.Data.Common;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data.IAccountCartService;
/// <summary>
/// 获取购物车响应
/// </summary>
public sealed record GetCartResponse
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("cart")]
    public CartData? Cart { get; set; }
}
