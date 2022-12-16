using Newtonsoft.Json;

namespace ASFEnhance.Data
{
    internal sealed record RecommendGameResponse
    {
        [JsonProperty(PropertyName = "success", Required = Required.Always)]
        public bool? Result { get; private set; }

        [JsonProperty(PropertyName = "strError", Required = Required.Default)]
        public string? ErrorMsg { get; private set; }
    }
}
