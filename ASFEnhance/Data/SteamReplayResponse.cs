using Newtonsoft.Json;

namespace ASFEnhance.Data
{
    internal sealed record SteamReplayResponse
    {
        [JsonProperty(PropertyName = "response", Required = Required.Always)]
        public ResponseData Response { get; set; } = new();

        public class ResponseData
        {
            [JsonProperty(PropertyName = "images", Required = Required.Always)]
            public List<ImagesData> Imanges { get; set; } = new();
        }
        public class ImagesData
        {
            [JsonProperty(PropertyName = "name", Required = Required.Always)]
            public string Name { get; set; } = "";

            [JsonProperty(PropertyName = "url_path", Required = Required.Always)]
            public string Path { get; set; } = "";
        }
    }

    internal sealed record SteamReplayPermissionsResponse
    {
        [JsonProperty(PropertyName = "response", Required = Required.Always)]
        public ResponseData Response { get; set; } = new();

        public class ResponseData
        {
            [JsonProperty(PropertyName = "privacy_state", Required = Required.Always)]
            public int Privacy { get; set; } = new();
        }
    }
}
