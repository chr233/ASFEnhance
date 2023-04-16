using Newtonsoft.Json;
using SteamKit2;

namespace ASFEnhance.Data
{
    internal sealed record AjaxCreateWalletAndCheckFundsResponse
    {
        [JsonProperty("success", Required = Required.Always)]
        public EResult Success { get; set; }
    }
}
