using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASFEnhance.Data;
internal sealed record ClaimItemResponse
{
    [JsonProperty(PropertyName = "response")]
    public ResponseData? Response { get; set; }

    public sealed record ResponseData
    {
        [JsonProperty(PropertyName = "communityitemid")]
        public string? CommunityItemId { get; set; }

        [JsonProperty(PropertyName = "next_claim_time")]
        public long NextClaimTime { get; set; }

        [JsonProperty(PropertyName = "reward_item")]
        public RewardItemData? RewardItem { get; set; }
    }



    public sealed record RewardItemData
    {
        [JsonProperty(PropertyName = "appid")]
        public int AppId { get; set; }

        [JsonProperty(PropertyName = "defid")]
        public int DefId { get; set; }

        [JsonProperty(PropertyName = "type")]
        public int Type { get; set; }

        [JsonProperty(PropertyName = "community_item_class")]
        public int CommunityItemClass { get; set; }

        [JsonProperty(PropertyName = "community_item_type")]
        public int CommunityItemType { get; set; }

        [JsonProperty(PropertyName = "point_cost")]
        public string? PointCost { get; set; }

        [JsonProperty(PropertyName = "timestamp_created")]
        public long TimestampCreated { get; set; }

        [JsonProperty(PropertyName = "timestamp_updated")]
        public long TimestampUpdated { get; set; }

        [JsonProperty(PropertyName = "timestamp_available")]
        public long TimestampAvailable { get; set; }

        [JsonProperty(PropertyName = "timestamp_available_end")]
        public long TimestampAvailableEnd { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public string? Quantity { get; set; }

        [JsonProperty(PropertyName = "internal_description")]
        public string? InternalDescription { get; set; }

        [JsonProperty(PropertyName = "active")]
        public bool Active { get; set; }

        [JsonProperty(PropertyName = "community_item_data")]
        public CommunityItemData? CommunityItemData { get; set; }

        [JsonProperty(PropertyName = "usable_duration")]
        public int UsableDuration { get; set; }

        [JsonProperty(PropertyName = "bundle_discount")]
        public int BundleDiscount { get; set; }
    }

    public sealed record CommunityItemData
    {
        [JsonProperty(PropertyName = "item_name")]
        public string? ItemName { get; set; }

        [JsonProperty(PropertyName = "item_title")]
        public string? ItemTitle { get; set; }

        [JsonProperty(PropertyName = "item_description")]
        public string? ItemDescription { get; set; }

        [JsonProperty(PropertyName = "item_image_small")]
        public string? ItemImageSmall { get; set; }

        [JsonProperty(PropertyName = "item_image_large")]
        public string? ItemImageLarge { get; set; }

        [JsonProperty(PropertyName = "animated")]
        public bool Animated { get; set; }
    }
}
