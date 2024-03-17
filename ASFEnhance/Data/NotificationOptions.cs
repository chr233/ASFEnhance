using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed record NotificationOptions
{
    public NotificationTarget ReceivedGift { get; set; } = NotificationTarget.OFF;
    public NotificationTarget SubscribedDissionReplyed { get; set; } = NotificationTarget.OFF;
    public NotificationTarget ReceivedNewItem { get; set; } = NotificationTarget.OFF;
    public NotificationTarget ReceivedFriendInvitation { get; set; } = NotificationTarget.OFF;
    public NotificationTarget MajorSaleStart { get; set; } = NotificationTarget.OFF;
    public NotificationTarget ItemInWishlistOnSale { get; set; } = NotificationTarget.OFF;
    public NotificationTarget ReceivedTradeOffer { get; set; } = NotificationTarget.OFF;
    public NotificationTarget ReceivedSteamSupportReply { get; set; } = NotificationTarget.OFF;
    public NotificationTarget SteamTurnNotification { get; set; } = NotificationTarget.OFF;
}

/// <summary>
/// 通知类型
/// </summary>
internal enum NotificationType : int
{
    /// <summary>
    /// 收到礼物
    /// </summary>
    ReceivedGift = 2,
    /// <summary>
    /// 订阅的讨论回复
    /// </summary>
    SubscribedDissionReplyed,
    /// <summary>
    /// 有新物品
    /// </summary>
    ReceivedNewItem,
    /// <summary>
    /// 收到好友邀请
    /// </summary>
    ReceivedFriendInvitation,
    /// <summary>
    /// 大促开始
    /// </summary>
    MajorSaleStart,
    /// <summary>
    /// 愿望单物品打折
    /// </summary>
    ItemInWishlistOnSale = 8,
    /// <summary>
    /// 收到交易报价
    /// </summary>
    ReceivedTradeOffer,
    /// <summary>
    /// 收到客服回复
    /// </summary>
    ReceivedSteamSupportReply = 11,
    /// <summary>
    /// Steam回合通知
    /// </summary>
    SteamTurnNotification,
}

[Flags]
internal enum NotificationTarget : int
{
    OFF = 0,
    On = 1,
    SteamClient = 2,
    OnAndSteamClient = On | SteamClient,
    MobileApp = 8,
    OnAndMobileApp = On | MobileApp,
    All = On | SteamClient | MobileApp
}

internal sealed record NotificationPayload
{
    public NotificationPayload(NotificationType notificationType, NotificationTarget notificationTargets)
    {
        NotificationType = notificationType;
        NotificationTargets = notificationTargets;
    }

    [JsonPropertyName("notification_type")]
    public NotificationType NotificationType { get; set; }

    [JsonPropertyName("notification_targets")]
    public NotificationTarget NotificationTargets { get; set; }
}