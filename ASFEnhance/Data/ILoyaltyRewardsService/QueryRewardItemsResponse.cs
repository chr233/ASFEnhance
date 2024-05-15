using System.Text.Json.Serialization;
using static ASFEnhance.Data.ClaimItemResponse;

namespace ASFEnhance.Data.ILoyaltyRewardsService;
internal sealed record QueryRewardItemsResponse
{
    [JsonPropertyName("response")]
    public ResponseData? Response { get; set; }
    public sealed record ResponseData
    {
        [JsonPropertyName("definitions")]
        public List<DefinitionData>? Definitions { get; set; }
    }

    public sealed record DefinitionData
    {
        [JsonPropertyName("appid")]
        public int AppId { get; set; }

        [JsonPropertyName("defid")]
        public int DefId { get; set; }

        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonPropertyName("community_item_class")]
        public int CommunityItemClass { get; set; }

        [JsonPropertyName("community_item_type")]
        public int CommunityItemType { get; set; }

        [JsonPropertyName("point_cost")]
        public string? PointCost { get; set; }

        [JsonPropertyName("timestamp_created")]
        public long TimestampCreated { get; set; }

        [JsonPropertyName("timestamp_updated")]
        public long TimestampUpdated { get; set; }

        [JsonPropertyName("timestamp_available")]
        public long TimestampAvailable { get; set; }

        [JsonPropertyName("timestamp_available_end")]
        public long TimestampAvailableEnd { get; set; }

        [JsonPropertyName("quantity")]
        public string? Quantity { get; set; }

        [JsonPropertyName("internal_description")]
        public string? InternalDescription { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("community_item_data")]
        public CommunityItemData? CommunityItem { get; set; }

        [JsonPropertyName("usable_duration")]
        public int UsableDuration { get; set; }

        [JsonPropertyName("bundle_discount")]
        public int BundleDiscount { get; set; }
    }
}
