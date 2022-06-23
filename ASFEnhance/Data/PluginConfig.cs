using Newtonsoft.Json;

namespace ASFEnhance.Data
{
    /// <summary>应用配置</summary>
    internal sealed class PluginConfig
    {
        [JsonProperty(Required = Required.DisallowNull)]
        internal bool Statistic { get; set; } = true;

        [JsonProperty(Required = Required.DisallowNull)]
        internal bool DevFeature { get; private set; } = false;

        [JsonProperty(Required = Required.DisallowNull)]
        internal int CheckUpdatePeriod { get; private set; } = 24;

        [JsonProperty(Required = Required.DisallowNull)]
        internal bool AutoUpdate { get; private set; } = false;
    }
}
