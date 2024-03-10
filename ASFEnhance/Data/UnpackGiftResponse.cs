using ArchiSteamFarm.Steam.Data;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data;
internal sealed class UnpackGiftResponse : ResultResponse
{
    [JsonPropertyName("gidgiftnew")]
    public ulong GidGiftNew { get; set; }

    [JsonPropertyName("accepted")]
    public bool Accepted { get; set; }
}
