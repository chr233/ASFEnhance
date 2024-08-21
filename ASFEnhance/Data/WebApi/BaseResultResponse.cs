using SteamKit2;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data.WebApi;
/// <summary>
/// 结果响应
/// </summary>
public record BaseResultResponse
{
    /// <summary>
    /// 结果
    /// </summary>
    [JsonPropertyName("success")]
    public EResult Result { get; set; }
}
