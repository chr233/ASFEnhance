using Newtonsoft.Json;
using SteamKit2;
using System.Diagnostics.CodeAnalysis;

namespace Chrxw.ASFEnhance.Data
{
    [SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]
    internal sealed class RecommendGameResponse
    {
        [JsonProperty(PropertyName = "success", Required = Required.Always)]
        public bool Result { get; private set; }

        [JsonProperty(PropertyName = "strError", Required = Required.Default)]
        public string ErrorMsg { get; private set; }
    }
}
