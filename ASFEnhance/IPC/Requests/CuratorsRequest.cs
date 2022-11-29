using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ASFEnhance.IPC.Requests
{
    /// <summary>
    /// 鉴赏家列表请求
    /// </summary>
    public sealed class CuratorsRequest
    {
        /// <summary>
        /// 起始位置
        /// </summary>
        [JsonProperty(Required = Required.DisallowNull)]
        [Required]
        public uint Start { get; set; }

        /// <summary>
        /// 获取数量
        /// </summary>
        [JsonProperty(Required = Required.DisallowNull)]
        [Required]
        public uint Count { get; set; } = 30;
    }
}
