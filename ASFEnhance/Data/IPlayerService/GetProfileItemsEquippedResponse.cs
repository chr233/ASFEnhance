using System.Text.Json.Serialization;

namespace ASFEnhance.Data.IPlayerService;
internal sealed record GetProfileItemsEquippedResponse
{
    [JsonPropertyName("profile_background")]
    public ProfileBackgroundData? ProfileBackgrounds { get; init; }

    [JsonPropertyName("mini_profile_background")]
    public MiniProfileBackgroundData? MiniProfileBackgrounds { get; init; }

    [JsonPropertyName("avatar_frame")]
    public ProfileModifierItemData? AvatarFrames { get; init; }

    [JsonPropertyName("animated_avatar")]
    public ProfileModifierItemData? AnimatedAvatars { get; init; }

    [JsonPropertyName("profile_modifier")]
    public ProfileModifierItemData? ProfileModifiers { get; init; }

    [JsonPropertyName("steam_deck_keyboard_skin")]
    public List<ProfileModifierItemData>? SteamDeckKeyboardSkins { get; init; }
}
