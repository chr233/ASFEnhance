using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ASFEnhance.Data
{
    internal sealed record AjaxRedeemWalletCodeResponse
    {
        [JsonProperty("success", Required = Required.Always)]
        public int Success { get; set; }

        [JsonProperty("detail", Required = Required.Always)]
        internal int Detail { get; set; }

        [JsonProperty("formattednewwalletbalance", Required = Required.DisallowNull)]
        internal string WalletBalance { get; set; } = "";
    }
}
