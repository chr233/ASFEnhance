using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ASFEnhance.IPC.Requests
{
    /// <summary>
    /// 下单请求
    /// </summary>
    public sealed class PurchaseRequest
    {
        [JsonProperty(Required = Required.Default)]
        [Required]
        public HashSet<uint> SubIds { get; set; } = new();

        [JsonProperty(Required = Required.Default)]
        [Required]
        public HashSet<uint> BundleIds { get; set; } = new();

        [JsonProperty(Required = Required.DisallowNull)]
        [Required]
        public bool SkipOwned { get; set; } = true;
    }
}
