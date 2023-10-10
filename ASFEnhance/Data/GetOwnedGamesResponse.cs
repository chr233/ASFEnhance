using Newtonsoft.Json;

namespace ASFEnhance.Data;
internal sealed record GetOwnedGamesResponse
{
    [JsonProperty("response")]
    public ResponseData? Response { get; set; }

    internal sealed record ResponseData
    {
        [JsonProperty("game_count")]
        public uint GameCount { get; set; }

        [JsonProperty("games")]
        public List<GameData>? Games { get; set; }
    }

    internal sealed record GameData
    {
        [JsonProperty("appid")]
        public uint AppId { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("playtime_2weeks")]
        public uint PlayTime2Weeks { get; set; }

        [JsonProperty("playtime_forever")]
        public uint PlayTimeForever { get; set; }

        [JsonProperty("has_community_visible_stats")]
        public bool HasCommunityVisibleStats { get; set; }

        [JsonProperty("playtime_windows_forever")]
        public uint PlaytimeWindowsForever { get; set; }

        [JsonProperty("playtime_mac_forever")]
        public uint PlaytimeMacForever { get; set; }

        [JsonProperty("playtime_linux_forever")]
        public uint PlaytimeLinuxForever { get; set; }

        [JsonProperty("rtime_last_played")]
        public ulong RtimeLastPlayed { get; set; }

        [JsonProperty("playtime_disconnected")]
        public uint PlaytimeDisconnected { get; set; }

    }
}
