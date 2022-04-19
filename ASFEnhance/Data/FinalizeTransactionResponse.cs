using Newtonsoft.Json;

namespace Chrxw.ASFEnhance.Data
{

    internal sealed class FinalizeTransactionResponse
    {
        [JsonProperty(PropertyName = "success", Required = Required.Always)]
        public int Result { get; private set; }
    }

}
