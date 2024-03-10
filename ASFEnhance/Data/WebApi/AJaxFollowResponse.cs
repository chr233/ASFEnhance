using ArchiSteamFarm.Steam.Data;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed record AJaxFollowResponse
{
    [JsonPropertyName("success")]
    public AjaxFlollowSuccess Success { get; set; } = new();

    internal sealed class AjaxFlollowSuccess : ResultResponse
    {
    }
}
