using ASFEnhance.Data.WebApi;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed record AJaxFollowResponse
{
    [JsonPropertyName("success")]
    public BaseResultResponse? Success { get; set; }
}
