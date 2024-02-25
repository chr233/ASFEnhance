using SteamKit2;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed record AjaxRedeemWalletCodeResponse
{
    [JsonPropertyName("success")]
    public EResult Success { get; set; }

    [JsonPropertyName("detail")]
    internal int Detail { get; set; }

    [JsonPropertyName("formattednewwalletbalance")]
    internal string WalletBalance { get; set; } = "";
}
