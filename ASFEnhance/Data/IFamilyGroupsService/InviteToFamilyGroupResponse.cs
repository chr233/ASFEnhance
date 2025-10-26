using System.Text.Json.Serialization;

namespace ASFEnhance.Data.IFamilyGroupsService;
internal sealed record InviteToFamilyGroupResponse
{
    [JsonPropertyName("two_factor_method")]
    public int TwoFactorMethod { get; init; }
}
