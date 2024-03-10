using ArchiSteamFarm.Steam.Data;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed class AjaxGetInviteTokens : ResultResponse
{
    [JsonPropertyName("token")]
    public string Token { get; private set; } = "";

    [JsonPropertyName("invite")]
    public InviteData Invite { get; set; } = new();

    [JsonIgnore]
    public string? Prefix { get; set; }
}

internal sealed record InviteData
{
    [JsonPropertyName("invite_token")]
    public string? InviteToken { get; private set; }

    [JsonPropertyName("invite_limit")]
    public string? InviteLimit { get; private set; }

    [JsonPropertyName("invite_duration")]
    public string? InviteDuration { get; private set; }

    [JsonPropertyName("time_created")]
    public long TimeCreated { get; private set; }

    [JsonPropertyName("valid")]
    public byte Valid { get; private set; }
}
