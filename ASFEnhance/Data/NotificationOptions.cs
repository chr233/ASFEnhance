using Newtonsoft.Json;

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

internal enum NotificationType : int
{
    ReceivedGift = 2,
    SubscribedDissionReplyed,
    ReceivedNewItem,
    ReceivedFriendInvitation,
    MajorSaleStart,
    ItemInWishlistOnSale = 8,
    ReceivedTradeOffer,
    ReceivedSteamSupportReply = 11,
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

    [JsonProperty(PropertyName = "notification_type", Required = Required.Always)]
    public NotificationType NotificationType { get; set; }

    [JsonProperty(PropertyName = "notification_targets", Required = Required.Always)]
    public NotificationTarget NotificationTargets { get; set; }
}