using SteamKit2;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data.WebApi;

internal sealed record CreateBuyOrderResponse
{
    [JsonPropertyName("success")]
    public EResult Success { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("buy_orderid")]
    public string? BuyOrderId { get; set; }

    [JsonPropertyName("need_confirmation")]
    public bool NeedConfirmation { get; set; }

    [JsonPropertyName("confirmation")]
    public ConfirmationData? Confirmation { get; set; }

    public sealed record ConfirmationData
    {
        [JsonPropertyName("confirmation_id")]
        public string? ConfirmationId { get; set; }
    }
}
