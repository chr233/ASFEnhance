using ArchiSteamFarm.Steam.Data;
using Newtonsoft.Json;

namespace ASFEnhance.Data;
internal sealed class UnpackGiftResponse : ResultResponse
{
    [JsonProperty("gidgiftnew")]
    public ulong GidGiftNew { get; set; }

    [JsonProperty("accepted")]
    public bool Accepted { get; set; }
}
