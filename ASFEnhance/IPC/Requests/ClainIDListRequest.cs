using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ASFEnhance.IPC.Requests
{
    /// <summary>
    /// 鉴赏家ID列表请求
    /// </summary>
    public sealed class ClanIDListRequest
    {
        [JsonProperty(Required = Required.Always)]
        [Required]
        public HashSet<uint> ClanIDs { get; set; } = new();
    }
}
