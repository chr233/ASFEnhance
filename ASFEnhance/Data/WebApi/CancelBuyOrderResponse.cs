using SteamKit2;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data.WebApi;

internal sealed record CancelBuyOrderResponse
{
    [JsonPropertyName("success")]
    public EResult Success { get; set; }
}
