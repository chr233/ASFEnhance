using System.Text.Json.Serialization;

namespace ASFEnhance.Data.IPlayerService;
internal sealed partial record GetProfileItemsOwnedResponse
{
    [JsonPropertyName("profile_backgrounds")]
    public List<ProfileBackgroundData>? ProfileBackgrounds { get; init; }

    [JsonPropertyName("mini_profile_backgrounds")]
    public List<MiniProfileBackgroundData>? MiniProfileBackgrounds { get; init; }

    [JsonPropertyName("avatar_frames")]
    public List<ProfileModifierItemData>? AvatarFrames { get; init; }

    [JsonPropertyName("animated_avatars")]
    public List<ProfileModifierItemData>? AnimatedAvatars { get; init; }

    [JsonPropertyName("profile_modifiers")]
    public List<ProfileModifierItemData>? ProfileModifiers { get; init; }

    [JsonPropertyName("steam_deck_keyboard_skins")]
    public List<ProfileModifierItemData>? SteamDeckKeyboardSkins { get; init; }
}
