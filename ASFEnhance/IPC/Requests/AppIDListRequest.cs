using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ASFEnhance.IPC.Requests
{
    /// <summary>
    /// AppIDs列表请求
    /// </summary>
    public sealed class AppIDListRequest
    {
        [JsonProperty(Required = Required.Always)]
        [Required]
        public HashSet<uint> AppIDs { get; set; } = new();
    }
}
