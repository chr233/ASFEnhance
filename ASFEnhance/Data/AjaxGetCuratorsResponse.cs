using Newtonsoft.Json;

namespace ASFEnhance.Data
{
    internal sealed record AjaxGetCuratorsResponse
    {
        [JsonProperty("success", Required = Required.Always)]
        public bool Success { get; set; }

        [JsonProperty("pagesize", Required = Required.Always)]
        public uint PageSize { get; set; }

        [JsonProperty("total_count", Required = Required.Always)]
        public uint TotalCount { get; set; }

        [JsonProperty("start", Required = Required.Always)]
        public uint Start { get; set; }

        [JsonProperty("results_html", Required = Required.Always)]
        public string Html { get; set; } = "";
    }
}
