using System.Text.Json.Serialization;

namespace ASFEnhance.Data.Common;

/// <summary>
/// SteamAward投票内容
/// </summary>
internal sealed record SteamAwardVoteData
{
    [JsonPropertyName("definitions")]
    public DefinitionData? Definitions { get; init; }
    [JsonPropertyName("user_votes")]
    public List<UserVoteData>? UserVotes { get; init; }

    internal sealed record DefinitionData
    {
        [JsonPropertyName("votes")]
        public List<VoteData>? Votes { get; set; }
    }

    internal sealed record VoteData
    {
        [JsonPropertyName("voteid")]
        public byte VoteId { get; set; }
        [JsonPropertyName("active")]
        public byte Active { get; set; }
    }

    internal sealed record UserVoteData
    {
        [JsonPropertyName("voteid")]
        public byte VoteId { get; set; }
        [JsonPropertyName("appid")]
        public uint Appid { get; set; }
        [JsonPropertyName("communityitemid")]
        public string? Communityitemid { get; set; }
    }
}
