using System.Text.Json.Serialization;

namespace ASFEnhance.Data;
internal sealed record GetOwnedGamesResponse
{
    [JsonPropertyName("response")]
    public ResponseData? Response { get; set; }

    internal sealed record ResponseData
    {
        [JsonPropertyName("game_count")]
        public uint GameCount { get; set; }

        [JsonPropertyName("games")]
        public List<GameData>? Games { get; set; }
    }

    internal sealed record GameData
    {
        [JsonPropertyName("appid")]
        public uint AppId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("playtime_2weeks")]
        public uint PlayTime2Weeks { get; set; }

        [JsonPropertyName("playtime_forever")]
        public uint PlayTimeForever { get; set; }

        [JsonPropertyName("has_community_visible_stats")]
        public bool HasCommunityVisibleStats { get; set; }

        [JsonPropertyName("playtime_windows_forever")]
        public uint PlaytimeWindowsForever { get; set; }

        [JsonPropertyName("playtime_mac_forever")]
        public uint PlaytimeMacForever { get; set; }

        [JsonPropertyName("playtime_linux_forever")]
        public uint PlaytimeLinuxForever { get; set; }

        [JsonPropertyName("rtime_last_played")]
        public ulong RtimeLastPlayed { get; set; }

        [JsonPropertyName("playtime_disconnected")]
        public uint PlaytimeDisconnected { get; set; }

    }
}
