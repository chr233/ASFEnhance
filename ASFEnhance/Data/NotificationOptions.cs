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
    public NotificationPayload(ENotificationType notificationType, NotificationTarget notificationTargets)
    {
        NotificationType = notificationType;
        NotificationTargets = notificationTargets;
    }

    [JsonPropertyName("notification_type")]
    public ENotificationType NotificationType { get; set; }

    [JsonPropertyName("notification_targets")]
    public NotificationTarget NotificationTargets { get; set; }
}