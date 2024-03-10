using System.Text.Json.Serialization;

namespace ASFEnhance.Data;
/// <summary>
/// 基础响应
/// </summary>
/// <typeparam name="T"></typeparam>
public record AbstractResponse<T> where T : notnull
{
    /// <summary>
    /// 响应
    /// </summary>
    [JsonPropertyName("response")]
    public T? Response { get; set; }
}

/// <inheritdoc/>
public record AbstractResponse : AbstractResponse<Dictionary<string, string>>
{
}