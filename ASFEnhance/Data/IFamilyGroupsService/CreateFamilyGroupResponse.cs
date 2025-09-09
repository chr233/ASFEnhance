using System.Text.Json.Serialization;

namespace ASFEnhance.Data.IFamilyGroupsService;
internal sealed record CreateFamilyGroup
{
    [JsonPropertyName("family_groupid")]
    public string? FamilyGroupId { get; init; }

    [JsonPropertyName("cooldown_skip_granted")]
    public bool CooldownSkipGranted { get; init; }
}

