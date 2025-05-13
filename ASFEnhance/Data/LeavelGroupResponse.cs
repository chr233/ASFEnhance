using System.Text.Json.Serialization;

namespace ASFEnhance.Data;
internal sealed record LeaveGroupResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; init; }
}