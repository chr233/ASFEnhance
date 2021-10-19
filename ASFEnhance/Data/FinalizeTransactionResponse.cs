using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace Chrxw.ASFEnhance.Data
{
    [SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]
    internal sealed class FinalizeTransactionResponse
    {
        [JsonProperty(PropertyName = "success", Required = Required.Always)]
        public int Result { get; private set; }
    }

}
