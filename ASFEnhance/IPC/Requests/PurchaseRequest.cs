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
        public HashSet<uint> SubIDs { get; set; } = new();

        [JsonProperty(Required = Required.Default)]
        [Required]
        public HashSet<uint> BundleIDs { get; set; } = new();

        [JsonProperty(Required = Required.DisallowNull)]
        [Required]
        public bool SkipOwned { get; set; } = true;
    }
}
