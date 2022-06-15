using Newtonsoft.Json;

namespace ASFEnhance.Event
{
    internal sealed class DemoAppResponse
    {
        [JsonProperty(PropertyName = "appid", Required = Required.DisallowNull)]
        public uint AppID { get; private set; }

        [JsonProperty(PropertyName = "demo_appid", Required = Required.DisallowNull)]
        public uint DemoAppID { get; private set; }
    }
}
