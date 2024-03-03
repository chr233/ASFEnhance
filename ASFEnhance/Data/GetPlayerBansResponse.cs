using Newtonsoft.Json;

namespace ASFEnhance.Data;
/// <summary>
/// 获取封禁状态
/// </summary>
internal sealed record GetPlayerBansResponse
{
    [JsonProperty("players")]
    public List<PlayerData>? Players { get; set; }

    public sealed record PlayerData
    {
        public ulong SteamId { get; set; }
        public bool CommunityBanned { get; set; }
        public bool VACBanned { get; set; }
        public int NumberOfVACBans { get; set; }
        public int DaysSinceLastBan { get; set; }
        public int NumberOfGameBans { get; set; }
        public string EconomyBan { get; set; } = "none";
    }
}

