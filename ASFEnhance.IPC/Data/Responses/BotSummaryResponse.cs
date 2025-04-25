namespace ASFEnhance.IPC.Responses;

/// <summary>
/// 机器人信息响应
/// </summary>
public sealed record BotSummaryResponse
{
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Success { get; set; }
    /// <summary>
    /// 货币
    /// </summary>
    public string Currency { get; set; } = "";
    /// <summary>
    /// 钱包余额
    /// </summary>
    public long Balance { get; set; }
    /// <summary>
    /// 格式化钱包余额
    /// </summary>
    public string FormatBalance { get; set; } = "";
    /// <summary>
    /// 昵称
    /// </summary>
    public string Nick { get; set; } = "";
    /// <summary>
    /// 个人资料链接
    /// </summary>
    public string ProfileLink { get; set; } = "";
    /// <summary>
    /// SteamId
    /// </summary>
    public ulong SteamId { get; set; }
    /// <summary>
    /// 好友代码
    /// </summary>
    public ulong FriendCode { get; set; }
}
