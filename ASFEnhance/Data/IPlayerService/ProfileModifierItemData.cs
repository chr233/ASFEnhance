using System.Text.Json.Serialization;

namespace ASFEnhance.Data.IPlayerService;

/// <summary>
/// 
/// </summary>
public sealed record ProfileModifierItemData
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("communityitemid")]
    public string? CommunityItemId { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("image_small")]
    public string? ImageSmall { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("image_large")]
    public string? ImageLarge { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("item_title")]
    public string? ItemTitle { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("item_description")]
    public string? ItemDescription { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("appid")]
    public uint AppId { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("item_type")]
    public int ItemType { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("item_class")]
    public int ItemClass { get; init; }
}
