using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed record SteamReplayResponse
{
    [JsonPropertyName("response")]
    public ResponseData Response { get; set; } = new();

    public class ResponseData
    {
        [JsonPropertyName("images")]
        public List<ImagesData> Imanges { get; set; } = [];
    }
    public class ImagesData
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("url_path")]
        public string Path { get; set; } = "";
    }
}

internal sealed record SteamReplayPermissionsResponse
{
    [JsonPropertyName("response")]
    public ResponseData Response { get; set; } = new();

    public class ResponseData
    {
        [JsonPropertyName("privacy_state")]
        public int Privacy { get; set; }
    }
}
