using System.Text.Json.Serialization;

namespace ASFEnhance.Data.WebApi;

internal sealed record AJaxFollowResponse
{
    [JsonPropertyName("success")]
    public BaseResultResponse? Success { get; set; }
}
