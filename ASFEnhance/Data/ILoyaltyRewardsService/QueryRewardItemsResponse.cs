using ASFEnhance.Data.Common;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data.ILoyaltyRewardsService;
internal sealed record QueryRewardItemsResponse
{
    [JsonPropertyName("definitions")]
    public List<RewardItemData>? Definitions { get; set; }
}
