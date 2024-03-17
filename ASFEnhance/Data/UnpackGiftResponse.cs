using ArchiSteamFarm.Steam.Data;
using ASFEnhance.Data.Common;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data;
internal sealed record UnpackGiftResponse : BaseResultResponse
{
    [JsonPropertyName("gidgiftnew")]
    public ulong GidGiftNew { get; set; }

    [JsonPropertyName("accepted")]
    public bool Accepted { get; set; }
}
