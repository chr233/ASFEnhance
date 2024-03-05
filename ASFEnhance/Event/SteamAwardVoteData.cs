using Newtonsoft.Json;

namespace ASFEnhance.Event;

/// <summary>
/// SteamAward投票内容
/// </summary>
internal sealed record SteamAwardVoteData
{
    [JsonProperty("definitions")]
    public DefinitionData? Definitions { get; init; }
    [JsonProperty("user_votes")]
    public List<UserVoteData>? UserVotes { get; init; }

    internal sealed record DefinitionData
    {
        [JsonProperty("votes")]
        public List<VoteData>? Votes { get; set; }
    }

    internal sealed record VoteData
    {
        [JsonProperty("voteid")]
        public byte VoteId { get; set; }
        [JsonProperty("active")]
        public byte Active { get; set; }
    }

    internal sealed record UserVoteData
    {
        [JsonProperty("voteid")]
        public byte VoteId { get; set; }
        [JsonProperty("appid")]
        public uint Appid { get; set; }
        [JsonProperty("communityitemid")]
        public string? Communityitemid { get; set; }
    }
}
