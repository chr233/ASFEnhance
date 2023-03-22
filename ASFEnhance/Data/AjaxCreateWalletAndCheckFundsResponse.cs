using Newtonsoft.Json;

namespace ASFEnhance.Data
{
    internal sealed record AjaxCreateWalletAndCheckFundsResponse
    {
        [JsonProperty("success", Required = Required.Always)]
        public int Success { get; set; }
    }
}
