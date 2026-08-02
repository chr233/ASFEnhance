using SteamKit2;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data.WebApi;

internal sealed record AjaxCreateBoosterResponse
{
    [JsonPropertyName("purchase_eresult")]
    public int EResult { get; set; }

    [JsonPropertyName("purchase_eresult")]
    public PurchaseResultData? PurchaseResult { get; set; }

    public class PurchaseResultData
    {
        [JsonPropertyName("communityitemid")]
        public string? CommunityItemId { get; set; }

        [JsonPropertyName("appid")]
        public int AppId { get; set; }

        [JsonPropertyName("item_type")]
        public int ItemType { get; set; }

        [JsonPropertyName("purchaseid")]
        public string? PurchaseId { get; set; }

        [JsonPropertyName("success")]
        public EResult Success { get; set; }

        [JsonPropertyName("rwgrsn")]
        public int Rwgrsn { get; set; }
    }

}
