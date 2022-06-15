using ArchiSteamFarm.Steam.Data;
using Newtonsoft.Json;

namespace ASFEnhance.Data
{
    internal sealed class PurchaseResponse : ResultResponse
    {
        [JsonProperty(PropertyName = "transid", Required = Required.Default)]
        public string TransID { get; private set; }

        [JsonProperty(PropertyName = "transactionid", Required = Required.Default)]
        public string TransActionID { get; private set; }
    }
}
