using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SteamKit2;

namespace ASFEnhance.Data;

internal sealed record AjaxRedeemWalletCodeResponse
{
    [JsonProperty("success", Required = Required.Always)]
    public EResult Success { get; set; }

    [JsonProperty("detail", Required = Required.Always)]
    internal int Detail { get; set; }

    [JsonProperty("formattednewwalletbalance", Required = Required.DisallowNull)]
    internal string WalletBalance { get; set; } = "";
}
