using System.Text.Json.Serialization;

namespace ASFEnhance.Data;
internal sealed record CombineItemStacksResponse
{
    [JsonPropertyName("response")]
    public ResponseData? Response { get; set; }

    public sealed record ResponseData
    {
        [JsonPropertyName("item_json")]
        public string? ItemJson { get; set; }
    }
}
