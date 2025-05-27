using System.ComponentModel.DataAnnotations;

namespace ASFEnhance.IPC.Data.Requests;

/// <summary>
/// 直播观众数
/// </summary>
public sealed record BroadcastWatchRequest
{
    /// <summary>
    /// 直播间ID
    /// </summary>
    [Required]
    public ulong SteamId { get; set; }

    /// <summary>
    /// 在线时长（秒）
    /// </summary>
    [Required]
    public ulong Seconds { get; set; }
}

/// <summary>
/// 直播弹幕
/// </summary>
public sealed record BroadcastChatRequest
{
    /// <summary>
    /// 直播间ID
    /// </summary>
    [Required]
    public ulong SteamId { get; set; }

    /// <summary>
    /// 在线时长（秒）
    /// </summary>
    [Required]
    public string? Message { get; set; }
}