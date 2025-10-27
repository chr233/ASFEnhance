using System.Text.Json.Serialization;

namespace ASFEnhance.Data.IPlayerService;

public sealed record ProfileModifierItemData
{
    [JsonPropertyName("communityitemid")]
    public string? CommunityItemId { get; init; }

    [JsonPropertyName("image_small")]
    public string? ImageSmall { get; init; }

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
}
