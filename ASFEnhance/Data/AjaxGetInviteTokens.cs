using ArchiSteamFarm.Steam.Data;
using Newtonsoft.Json;

namespace ASFEnhance.Data;

internal sealed class AjaxGetInviteTokens : ResultResponse
{
    [JsonProperty(PropertyName = "token", Required = Required.Always)]
    public string Token { get; private set; } = "";

    [JsonProperty(PropertyName = "invite", Required = Required.Always)]
    public InviteData Invite { get; set; } = new();

    [JsonIgnore]
    public string? Prefix { get; set; }
}

internal sealed record InviteData
{
    [JsonProperty(PropertyName = "invite_token", Required = Required.Always)]
    public string InviteToken { get; private set; } = "";

    [JsonProperty(PropertyName = "invite_limit", Required = Required.AllowNull)]
    public string? InviteLimit { get; private set; } = "";

    [JsonProperty(PropertyName = "invite_duration", Required = Required.AllowNull)]
    public string? InviteDuration { get; private set; } = "";

    [JsonProperty(PropertyName = "time_created", Required = Required.Always)]
    public long TimeCreated { get; private set; }

    [JsonProperty(PropertyName = "valid", Required = Required.Always)]
    public byte Valid { get; private set; }
}
