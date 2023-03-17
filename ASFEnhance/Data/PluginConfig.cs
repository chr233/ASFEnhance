using Newtonsoft.Json;

namespace ASFEnhance.Data
{
    /// <summary>应用配置</summary>
    public sealed record PluginConfig
    {
        [JsonProperty(Required = Required.DisallowNull)]
        public bool EULA { get; set; } = true;

        [JsonProperty(Required = Required.DisallowNull)]
        public bool Statistic { get; set; } = true;

        [JsonProperty(Required = Required.DisallowNull)]
        public bool DevFeature { get; set; }

        [JsonProperty(Required = Required.Default)]
        public List<string>? DisabledCmds { get; set; }
    }
}
