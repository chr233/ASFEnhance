using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed record AppDetailResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("data")]
    public AppDetailData Data { get; set; } = new();
}

internal sealed record AppDetailData
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "";

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("steam_appid")]
    public uint AppId { get; set; }

    [JsonPropertyName("required_age")]
    public uint RequiredAge { get; set; }

    [JsonPropertyName("is_free")]
    public bool IsFree { get; set; }

    [JsonPropertyName("dlc")]
    public HashSet<uint> Dlc { get; set; } = [];

    [JsonPropertyName("detailed_description")]
    public string DetailedDescription { get; set; } = "";

    [JsonPropertyName("about_the_game")]
    public string AboutTheGame { get; set; } = "";

    [JsonPropertyName("short_description")]
    public string ShortDescription { get; set; } = "";

    [JsonPropertyName("fullgame")]
    public FullGameData FullGame { get; set; } = new();

    [JsonPropertyName("supported_languages")]
    public string SupportedLanguages { get; set; } = "";

    [JsonPropertyName("header_image")]
    public string HeaderImage { get; set; } = "";

    [JsonPropertyName("website")]
    public string Website { get; set; } = "";

    [JsonPropertyName("developers")]
    public HashSet<string> Developers { get; set; } = [];

    [JsonPropertyName("publishers")]
    public HashSet<string> Publishers { get; set; } = [];

    [JsonPropertyName("demos")]
    public HashSet<DemoData> Demos { get; set; } = [];

    [JsonPropertyName("price_overview")]
    public PriceOverviewData PriceOverview { get; set; } = new();

    [JsonPropertyName("packages")]
    public HashSet<uint> Packages { get; set; } = [];

    [JsonPropertyName("package_groups")]
    public HashSet<PackageGroupsData> PackageGroups { get; set; } = [];

    [JsonPropertyName("platforms")]
    public PlatformsData Platforms { get; set; } = new();

    [JsonPropertyName("metacritic")]
    public MetacriticData Metacritic { get; set; } = new();

    [JsonPropertyName("categories")]
    public HashSet<CategoryData> Categories { get; set; } = [];

    [JsonPropertyName("genres")]
    public HashSet<GenreData> Genres { get; set; } = [];

    [JsonPropertyName("screenshots")]
    public HashSet<ScreenshotData> Screenshots { get; set; } = [];

    [JsonPropertyName("movies")]
    public HashSet<MovieData> Movies { get; set; } = [];

    [JsonPropertyName("recommendations")]
    public RecommendationsData Recommendations { get; set; } = new();

    [JsonPropertyName("release_date")]
    public ReleaseDateData ReleaseDate { get; set; } = new();

    [JsonPropertyName("support_info")]
    public SupportInfoData SupportInfo { get; set; } = new();

    internal sealed record FullGameData
    {
        [JsonPropertyName("appid")]
        public string AppId { get; set; } = "";

        [JsonPropertyName("name")]
        public string Name { get; set; } = "";
    }

    internal sealed record DemoData
    {
        [JsonPropertyName("appid")]
        public uint AppId { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; } = "";
    }

    internal sealed record PriceOverviewData
    {
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = "";

        [JsonPropertyName("initial")]
        public uint Initial { get; set; }

        [JsonPropertyName("final")]
        public uint Final { get; set; }

        [JsonPropertyName("discount_percent")]
        public uint DiscountPercent { get; set; }

        [JsonPropertyName("initial_formatted")]
        public string InitialFormatted { get; set; } = "";

        [JsonPropertyName("final_formatted")]
        public string FinalFormatted { get; set; } = "";
    }

    internal sealed record PackageGroupsData
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("subs")]
        public HashSet<SubData>? Subs { get; set; }

        internal sealed record SubData
        {
            [JsonPropertyName("packageid")]
            public uint SubId { get; set; }

            [JsonPropertyName("percent_savings")]
            public string PercentSavingsText { get; set; } = "";

            [JsonPropertyName("option_text")]
            public string OptionText { get; set; } = "";

            [JsonPropertyName("is_free_license")]
            public bool IsFreeLicense { get; set; }

            [JsonPropertyName("price_in_cents_with_discount")]
            public uint PriceInCentsWithDiscount { get; set; }
        }

    }

    internal sealed record PlatformsData
    {
        [JsonPropertyName("windows")]
        public bool Windows { get; set; }

        [JsonPropertyName("mac")]
        public bool Mac { get; set; }

        [JsonPropertyName("linux")]
        public bool Linux { get; set; }
    }

    internal sealed record MetacriticData
    {
        [JsonPropertyName("score")]
        public uint Score { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; } = "";
    }

    internal sealed record CategoryData
    {
        [JsonPropertyName("id")]
        public uint Id { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; } = "";
    }

    internal sealed record GenreData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = "";

        [JsonPropertyName("description")]
        public string Description { get; set; } = "";
    }

    internal sealed record ScreenshotData
    {
        [JsonPropertyName("id")]
        public uint Id { get; set; }

        [JsonPropertyName("path_thumbnail")]
        public string PathThumbnail { get; set; } = "";

        [JsonPropertyName("path_full")]
        public string PathFull { get; set; } = "";
    }

    internal sealed record MovieData
    {
        [JsonPropertyName("id")]
        public uint Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; } = "";

        [JsonPropertyName("webm")]
        public MovieFormatData? Webm { get; set; }

        [JsonPropertyName("mp4")]
        public MovieFormatData? Mp4 { get; set; }

        [JsonPropertyName("highlight")]
        public bool Highlight { get; set; }

        internal sealed record MovieFormatData
        {
            [JsonPropertyName("480")]
            public string M480 { get; set; } = "";

            [JsonPropertyName("max")]
            public string Max { get; set; } = "";
        }
    }

    internal sealed record RecommendationsData
    {
        [JsonPropertyName("total")]
        public uint Total { get; set; }
    }

    internal sealed record ReleaseDateData
    {
        [JsonPropertyName("coming_soon")]
        public bool ComingSoon { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; } = "";
    }

    internal sealed record SupportInfoData
    {
        [JsonPropertyName("url")]
        public string Url { get; set; } = "";

        [JsonPropertyName("email")]
        public string Email { get; set; } = "";
    }
}

