#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using Newtonsoft.Json;

namespace ASFEnhance.Event
{
    internal sealed class CapsuleinsertResponse
    {
        [JsonProperty("appid")]
        public uint Appid { get; set; }

        [JsonProperty("action")]
        public uint Action { get; set; } 

        [JsonProperty("payload")]
        public uint Payload { get; set; }

        [JsonProperty("datarecord")]
        public string DataRecord { get; set; } = "";
    }
}
