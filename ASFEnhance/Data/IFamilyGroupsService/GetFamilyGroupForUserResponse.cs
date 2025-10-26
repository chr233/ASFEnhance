using System.Text.Json.Serialization;

namespace ASFEnhance.Data.IFamilyGroupsService;
internal sealed record GetFamilyGroupForUserResponse
{
    [JsonPropertyName("family_groupid")]
    public string? FamilyGroupId { get; init; }

    [JsonPropertyName("is_not_member_of_any_group")]
    public bool IsNotMemberOfAnyGroup { get; init; }

    [JsonPropertyName("latest_time_joined")]
    public long LatestTimeJoined { get; init; }

    [JsonPropertyName("latest_joined_family_groupid")]
    public string? LatestJoinedFamilyGroupId { get; init; }

    [JsonPropertyName("pending_group_invites")]
    public List<PendingGroupInviteData>? PendingGroupInvites { get; init; }

    [JsonPropertyName("role")]
    public int Role { get; init; }

    [JsonPropertyName("cooldown_seconds_remaining")]
    public int CooldownSecondsRemaining { get; init; }

    [JsonPropertyName("family_group")]
    public FamilyGroupData? FamilyGroup { get; init; }

    [JsonPropertyName("can_undelete_last_joined_family")]
    public bool CanUndeleteLastJoinedFamily { get; init; }

    [JsonPropertyName("membership_history")]
    public List<MembershipHistoryData>? MembershipHistory { get; init; }

    internal sealed record PendingGroupInviteData
    {
        [JsonPropertyName("family_groupid")]
        public string? FamilyGroupId { get; init; }

        [JsonPropertyName("role")]
        public int Role { get; init; }

        [JsonPropertyName("inviter_steamid")]
        public string? InviterSteamId { get; init; }

        [JsonPropertyName("awaiting_2fa")]
        public bool Awaiting2FA { get; init; }
    }

    internal sealed record FamilyGroupData
    {
        [JsonPropertyName("name")]
        public string? Name { get; init; }

        [JsonPropertyName("members")]
        public List<FamilyMemberData>? Members { get; init; }

        [JsonPropertyName("free_spots")]
        public int FreeSpots { get; init; }

        [JsonPropertyName("country")]
        public string? Country { get; init; }

        [JsonPropertyName("slot_cooldown_remaining_seconds")]
        public int SlotCooldownRemainingSeconds { get; init; }

        [JsonPropertyName("slot_cooldown_overrides")]
        public int SlotCooldownOverrides { get; init; }
    }

    internal sealed record FamilyMemberData
    {
        [JsonPropertyName("steamid")]
        public string? SteamId { get; init; }

        [JsonPropertyName("role")]
        public int Role { get; init; }

        [JsonPropertyName("time_joined")]
        public long TimeJoined { get; init; }

        [JsonPropertyName("cooldown_seconds_remaining")]
        public int CooldownSecondsRemaining { get; init; }
    }

    internal sealed record MembershipHistoryData
    {
        [JsonPropertyName("family_groupid")]
        public string? FamilyGroupId { get; init; }

        [JsonPropertyName("rtime_joined")]
        public long RTimeJoined { get; init; }

        [JsonPropertyName("rtime_left")]
        public long RTimeLeft { get; init; }

        [JsonPropertyName("role")]
        public int Role { get; init; }

        [JsonPropertyName("participated")]
        public bool Participated { get; init; }
    }
}

