using Newtonsoft.Json;

namespace ASFEnhance.Data;

public sealed record GitHubReleaseResponse
{
    [JsonProperty(PropertyName = "html_url", Required = Required.Always)]
    public string Url { get; set; } = "";

    [JsonProperty(PropertyName = "name", Required = Required.Always)]
    public string Name { get; set; } = "";

    [JsonProperty(PropertyName = "body", Required = Required.Always)]
    public string Body { get; set; } = "";

    [JsonProperty(PropertyName = "tag_name", Required = Required.Always)]
    public string TagName { get; set; } = "";

    [JsonProperty(PropertyName = "created_at", Required = Required.Always)]
    public string CreatedAt { get; set; } = "";

    [JsonProperty(PropertyName = "published_at", Required = Required.Always)]
    public string PublicAt { get; set; } = "";

    [JsonProperty(PropertyName = "assets", Required = Required.Always)]
    public HashSet<GitHubAssetsData> Assets { get; set; } = new();

    public sealed record GitHubAssetsData
    {
        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public string Name { get; set; } = "";

        [JsonProperty(PropertyName = "size", Required = Required.Always)]
        public uint Size { get; set; }

        [JsonProperty(PropertyName = "created_at", Required = Required.Always)]
        public string CreatedAt { get; set; } = "";

        [JsonProperty(PropertyName = "updated_at", Required = Required.Always)]
        public string UpdatedAt { get; set; } = "";

        [JsonProperty(PropertyName = "browser_download_url", Required = Required.Always)]
        public string DownloadUrl { get; set; } = "";
    }
}
