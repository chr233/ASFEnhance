using SteamKit2;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed record AjaxCreateWalletAndCheckFundsResponse
{
    [JsonPropertyName("success")]
    public EResult Success { get; set; }
}
