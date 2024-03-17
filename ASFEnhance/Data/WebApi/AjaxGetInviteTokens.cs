using ArchiSteamFarm.Steam.Data;
using ASFEnhance.Data.Common;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed record AjaxGetInviteTokens : BaseResultResponse
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = "";

    [JsonPropertyName("invite")]
    public InviteData Invite { get; set; } = new();

    [JsonIgnore]
    public string? Prefix { get; set; }
}

internal sealed record InviteData
{
    [JsonPropertyName("invite_token")]
    public string? InviteToken { get; set; }

    [JsonPropertyName("invite_limit")]
    public string? InviteLimit { get; set; }

    [JsonPropertyName("invite_duration")]
    public string? InviteDuration { get; set; }

    [JsonPropertyName("time_created")]
    public long TimeCreated { get; set; }

    [JsonPropertyName("valid")]
    public byte Valid { get; set; }
}
