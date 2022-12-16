using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ASFEnhance.IPC.Requests
{
    /// <summary>
    /// 发布评测请求
    /// </summary>
    public sealed class RecommendRequest
    {
        [JsonProperty(Required = Required.Always)]
        [Required]
        public HashSet<RecommendOption> Recommends { get; set; } = new();
    }
    /// <summary>
    /// 评测选项
    /// </summary>
    public sealed class RecommendOption
    {
        [JsonProperty(Required = Required.Always)]
        [Required]
        public uint AppId { get; set; }

        [JsonProperty(Required = Required.DisallowNull)]
        public bool RateUp { get; set; } = true;

        [JsonProperty(Required = Required.DisallowNull)]
        public bool AllowReply { get; set; } = true;

        [JsonProperty(Required = Required.DisallowNull)]
        public bool ForFree { get; set; }

        [JsonProperty(Required = Required.DisallowNull)]
        public bool Public { get; set; } = true;

        [JsonProperty(Required = Required.Always)]
        [Required]
        public string Comment { get; set; } = "";
    }
}
