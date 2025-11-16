using System.Text.Json.Serialization;

namespace ASFEnhance.Data.IPlayerService;
/// <summary>
/// 
/// </summary>
public sealed record ProfileBackgroundData
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("communityitemid")]
    public string? CommunityItemId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("image_large")]
    public string? ImageLarge { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("item_title")]
    public string? ItemTitle { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("item_description")]
    public string? ItemDescription { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("appid")]
    public uint AppId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("item_type")]
    public int ItemType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("item_class")]
    public int ItemClass { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("movie_webm")]
    public string? MovieWebm { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("movie_mp4")]
    public string? MovieMp4 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("movie_webm_small")]
    public string? MovieWebmSmall { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("movie_mp4_small")]
    public string? MovieMp4Small { get; set; }
}
