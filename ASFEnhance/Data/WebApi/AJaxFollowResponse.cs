using ArchiSteamFarm.Steam.Data;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed record AJaxFollowResponse
{
    [JsonPropertyName("success")]
    public ResultResponse? Success { get; set; }
}
