using ASFEnhance.Data.Common;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data;
internal sealed record ClaimItemResponse
{
    [JsonPropertyName("communityitemid")]
    public string? CommunityItemId { get; set; }

    [JsonPropertyName("next_claim_time")]
    public long NextClaimTime { get; set; }

    [JsonPropertyName("reward_item")]
    public RewardItemData? RewardItem { get; set; }
}
