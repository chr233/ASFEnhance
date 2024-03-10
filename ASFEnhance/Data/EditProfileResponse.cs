using SteamKit2;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

/// <summary>
/// 编辑个人资料响应
/// </summary>
public class EditProfileResponse
{
    /// <summary>
    /// 是否成功
    /// </summary>
    [JsonPropertyName("success")]
    public EResult Success { get; set; }

    /// <summary>
    /// 错误消息
    /// </summary>
    [JsonPropertyName("errmsg")]
    public string? ErrMsg { get; set; }

    /// <summary>
    /// 重定向地址
    /// </summary>
    [JsonPropertyName("redirect")]
    public string? Redirect { get; set; }
}
