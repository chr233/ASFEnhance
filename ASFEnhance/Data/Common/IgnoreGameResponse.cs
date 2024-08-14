using System.Text.Json.Serialization;

namespace ASFEnhance.Data.Common;

/// <summary>
/// 添加愿望单响应
/// </summary>
public record IgnoreGameResponse
{
    /// <summary>
    /// 结果
    /// </summary>
    [JsonPropertyName("success")]
    public bool Result { get; set; }
}