using System.Text.Json.Serialization;

namespace ASFEnhance.Data.WebApi;

/// <summary>
/// 来自ASF
/// </summary>
internal sealed record TradeOfferAcceptResponse
{
    [JsonPropertyName("strError")]
    public string? ErrorText { get; set; }

    [JsonPropertyName("needs_mobile_confirmation")]
    public bool RequiresMobileConfirmation { get; set; }
}