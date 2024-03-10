using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed record AjaxGetCuratorsResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("pagesize")]
    public uint PageSize { get; set; }

    [JsonPropertyName("total_count")]
    public uint TotalCount { get; set; }

    [JsonPropertyName("start")]
    public uint Start { get; set; }

    [JsonPropertyName("results_html")]
    public string Html { get; set; } = "";
}
