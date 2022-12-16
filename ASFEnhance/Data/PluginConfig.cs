using Newtonsoft.Json;

namespace ASFEnhance.Data
{
    /// <summary>应用配置</summary>
    internal sealed record PluginConfig
    {
        [JsonProperty(Required = Required.DisallowNull)]
        internal bool EULA { get; set; } = true;

        [JsonProperty(Required = Required.DisallowNull)]
        internal bool Statistic { get; set; } = true;

        [JsonProperty(Required = Required.DisallowNull)]
        internal bool DevFeature { get; private set; }
    }
}
