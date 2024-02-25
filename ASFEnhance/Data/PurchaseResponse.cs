using ArchiSteamFarm.Steam.Data;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed class PurchaseResponse : ResultResponse
{
    [JsonPropertyName("transid")]
    public string? TransId { get; private set; }

    [JsonPropertyName("transactionid")]
    public string? TransActionId { get; private set; }
}
