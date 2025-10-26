using System.Text.Json.Serialization;

namespace ASFEnhance.Data.IFamilyGroupsService;
internal sealed record JoinFamilyGroupResponse
{
    [JsonPropertyName("two_factor_method")]
    public int TwoFactorMethod { get; init; }

    [JsonPropertyName("cooldown_skip_granted")]
    public bool CooldownSkipGranted { get; init; }
}
