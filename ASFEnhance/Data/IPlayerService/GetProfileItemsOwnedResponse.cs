using System.Text.Json.Serialization;

namespace ASFEnhance.Data.IPlayerService;
public sealed record GetProfileItemsOwnedResponse
{
    [JsonPropertyName("profile_backgrounds")]
    public List<ProfileBackgroundData>? ProfileBackgrounds { get; init; }

    [JsonPropertyName("mini_profile_backgrounds")]
    public List<MiniProfileBackgroundData>? MiniProfileBackgrounds { get; init; }

    [JsonPropertyName("avatar_frames")]
    public List<AvatarFrameData>? AvatarFrames { get; init; }

    [JsonPropertyName("animated_avatars")]
    public List<AvatarFrameData>? AnimatedAvatars { get; init; }

    [JsonPropertyName("profile_modifiers")]
    public List<ProfileModifierData>? ProfileModifiers { get; init; }

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

    public sealed record MiniProfileBackgroundData
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

    public sealed record AvatarFrameData
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

    public sealed record ProfileModifierData
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
}
