using Newtonsoft.Json;

namespace ASFEnhance.Data;

/// <summary>
/// 应用配置
/// </summary>
public sealed record PluginConfig
{
    /// <summary>
    /// 是否同意使用协议
    /// </summary>
    [JsonProperty(Required = Required.DisallowNull)]
    public bool EULA { get; set; } = true;

    /// <summary>
    /// 是否启用统计
    /// </summary>
    [JsonProperty(Required = Required.DisallowNull)]
    public bool Statistic { get; set; } = true;

    /// <summary>
    /// 是否启用开发者特性
    /// </summary>
    [JsonProperty(Required = Required.DisallowNull)]
    public bool DevFeature { get; set; }

    /// <summary>
    /// 禁用命令表
    /// </summary>
    [JsonProperty(Required = Required.Default)]
    public HashSet<string>? DisabledCmds { get; set; }

    /// <summary>
    /// 单条地址信息
    /// </summary>
    [JsonProperty(Required = Required.Default)]
    public AddressConfig? Address { get; set; }

    /// <summary>
    /// 多条地址信息
    /// </summary>
    [JsonProperty(Required = Required.Default)]
    public List<AddressConfig>? Addresses { get; set; }

    /// <summary>
    /// Api Key, 用于获取封禁信息, 非必须
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// 自动领取物品的机器人名, 逗号分隔
    /// </summary>
    public string? AutoClaimItemBotNames { get; set; }
    /// <summary>
    /// 自动领取物品周期, 单位小时
    /// </summary>
    public uint AutoClaimItemPeriod { get; set; } = 23;

    /// <summary>
    /// 地址信息
    /// </summary>
    public sealed record AddressConfig
    {
        /// <summary>
        /// 街道地址
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string Address { get; set; } = "";

        /// <summary>
        /// 城市
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string City { get; set; } = "";

        /// <summary>
        /// 国家
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string Country { get; set; } = "";

        /// <summary>
        /// 省/州
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string State { get; set; } = "";

        /// <summary>
        /// 邮编
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string PostCode { get; set; } = "";
    }
}
