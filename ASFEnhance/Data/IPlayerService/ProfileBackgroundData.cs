using System.Text.Json.Serialization;

namespace ASFEnhance.Data.IPlayerService;
public sealed record ProfileBackgroundData
{
    [JsonPropertyName("communityitemid")]
    public string? CommunityItemId { get; set; }

    [JsonPropertyName("image_large")]
    public string? ImageLarge { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("item_title")]
    public string? ItemTitle { get; set; }

    [JsonPropertyName("item_description")]
    public string? ItemDescription { get; set; }

    [JsonPropertyName("appid")]
    public uint AppId { get; set; }

    [JsonPropertyName("item_type")]
    public int ItemType { get; set; }

    [JsonPropertyName("item_class")]
    public int ItemClass { get; set; }

    [JsonPropertyName("movie_webm")]
    public string? MovieWebm { get; set; }

    [JsonPropertyName("movie_mp4")]
    public string? MovieMp4 { get; set; }

    [JsonPropertyName("movie_webm_small")]
    public string? MovieWebmSmall { get; set; }

    [JsonPropertyName("movie_mp4_small")]
    public string? MovieMp4Small { get; set; }
}
