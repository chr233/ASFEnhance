using System.Text.Json.Serialization;

namespace ASFEnhance.Data.ISteamNotificationService;

internal sealed record GetSteamNotificationsResponse
{
    [JsonPropertyName("response")]
    public ResponseData? Response { get; set; }

    public sealed record ResponseData
    {
        [JsonPropertyName("notifications")]
        public List<NotificationData>? Notifications { get; set; }

        [JsonPropertyName("confirmation_count")]
        public int ConfirmationCount { get; set; }

        [JsonPropertyName("pending_gift_count")]
        public int PendingGiftCount { get; set; }

        [JsonPropertyName("pending_friend_count")]
        public int PendingFriendCount { get; set; }

        [JsonPropertyName("unread_count")]
        public int UnreadCount { get; set; }

        [JsonPropertyName("pending_family_invite_count")]
        public int PendingFamilyInviteCount { get; set; }
    }

    public sealed record NotificationData
    {
        [JsonPropertyName("notification_id")]
        public string? NotificationId { get; set; }

        [JsonPropertyName("notification_targets")]
        public int NotificationTargets { get; set; }

        [JsonPropertyName("notification_type")]
        public int NotificationType { get; set; }

        [JsonPropertyName("body_data")]
        public string? BodyData { get; set; }

        [JsonPropertyName("read")]
        public bool Read { get; set; }

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("hidden")]
        public bool Hidden { get; set; }

        [JsonPropertyName("expiry")]
        public long Expiry { get; set; }

        [JsonPropertyName("viewed")]
        public long Viewed { get; set; }
    }
}