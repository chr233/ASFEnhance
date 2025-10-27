using System.Text.Json.Serialization;

namespace ASFEnhance.Data.IPlayerService;

internal sealed record MiniProfileBackgroundData
{
    [JsonPropertyName("communityitemid")]
    public string? CommunityItemId { get; init; }

    [JsonPropertyName("image_large")]
    public string? ImageLarge { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("item_title")]
    public string? ItemTitle { get; init; }

    [JsonPropertyName("item_description")]
    public string? ItemDescription { get; init; }

    [JsonPropertyName("appid")]
    public uint AppId { get; init; }

    [JsonPropertyName("item_type")]
    public int ItemType { get; init; }

    [JsonPropertyName("item_class")]
    public int ItemClass { get; init; }

    [JsonPropertyName("movie_webm")]
    public string? MovieWebm { get; init; }

    [JsonPropertyName("movie_mp4")]
    public string? MovieMp4 { get; init; }

    [JsonPropertyName("movie_webm_small")]
    public string? MovieWebmSmall { get; init; }

    [JsonPropertyName("movie_mp4_small")]
    public string? MovieMp4Small { get; init; }
}
