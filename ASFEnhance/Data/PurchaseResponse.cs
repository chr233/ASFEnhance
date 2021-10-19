using Newtonsoft.Json;
using SteamKit2;
using System.Diagnostics.CodeAnalysis;

namespace Chrxw.ASFEnhance.Data
{
    [SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]
    internal sealed class PurchaseResponse
    {
        [JsonProperty(PropertyName = "success", Required = Required.Always)]
        public EResult Result { get; private set; }


        [JsonProperty(PropertyName = "transid", Required = Required.Default)]
        public string TransID { get; private set; }


        [JsonProperty(PropertyName = "transactionid", Required = Required.Default)]
        public string TransActionID { get; private set; }
    }
}
