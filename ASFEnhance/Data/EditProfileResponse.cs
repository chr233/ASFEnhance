using Newtonsoft.Json;
using SteamKit2;

namespace ASFEnhance.Data;

/// <summary>
/// 编辑个人资料响应
/// </summary>
public class EditProfileResponse
{
    /// <summary>
    /// 是否成功
    /// </summary>
    [JsonProperty("success")]
    public EResult Success { get; set; }

    /// <summary>
    /// 错误消息
    /// </summary>
    [JsonProperty("errmsg")]
    public string? ErrMsg { get; set; }

    /// <summary>
    /// 重定向地址
    /// </summary>
    [JsonProperty("redirect")]
    public string? Redirect { get; set; }
}
