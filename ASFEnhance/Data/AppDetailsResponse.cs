using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed record AppDetailResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; private set; }

    [JsonPropertyName("data")]
    public AppDetailData Data { get; private set; } = new();
}

internal sealed record AppDetailData
{
    [JsonPropertyName("type")]
    public string Type { get; private set; } = "";

    [JsonPropertyName("name")]
    public string Name { get; private set; } = "";

    [JsonPropertyName("steam_appid")]
    public uint AppId { get; private set; }

    [JsonPropertyName("required_age")]
    public uint RequiredAge { get; private set; }

    [JsonPropertyName("is_free")]
    public bool IsFree { get; private set; }

    [JsonPropertyName("dlc")]
    public HashSet<uint> Dlc { get; private set; } = [];

    [JsonPropertyName("detailed_description")]
    public string DetailedDescription { get; private set; } = "";

    [JsonPropertyName("about_the_game")]
    public string AboutTheGame { get; private set; } = "";

    [JsonPropertyName("short_description")]
    public string ShortDescription { get; private set; } = "";

    [JsonPropertyName("fullgame")]
    public FullGameData FullGame { get; private set; } = new();

    [JsonPropertyName("supported_languages")]
    public string SupportedLanguages { get; private set; } = "";

    [JsonPropertyName("header_image")]
    public string HeaderImage { get; private set; } = "";

    [JsonPropertyName("website")]
    public string Website { get; private set; } = "";

    [JsonPropertyName("developers")]
    public HashSet<string> Developers { get; private set; } = [];

    [JsonPropertyName("publishers")]
    public HashSet<string> Publishers { get; private set; } = [];

    [JsonPropertyName("demos")]
    public HashSet<DemoData> Demos { get; private set; } = [];

    [JsonPropertyName("price_overview")]
    public PriceOverviewData PriceOverview { get; private set; } = new();

    [JsonPropertyName("packages")]
    public HashSet<uint> Packages { get; private set; } = [];

    [JsonPropertyName("package_groups")]
    public HashSet<PackageGroupsData> PackageGroups { get; private set; } = [];

    [JsonPropertyName("platforms")]
    public PlatformsData Platforms { get; private set; } = new();

    [JsonPropertyName("metacritic")]
    public MetacriticData Metacritic { get; private set; } = new();

    [JsonPropertyName("categories")]
    public HashSet<CategoryData> Categories { get; private set; } = [];

    [JsonPropertyName("genres")]
    public HashSet<GenreData> Genres { get; private set; } = [];

    [JsonPropertyName("screenshots")]
    public HashSet<ScreenshotData> Screenshots { get; private set; } = [];

    [JsonPropertyName("movies")]
    public HashSet<MovieData> Movies { get; private set; } = [];

    [JsonPropertyName("recommendations")]
    public RecommendationsData Recommendations { get; private set; } = new();

    [JsonPropertyName("release_date")]
    public ReleaseDateData ReleaseDate { get; private set; } = new();

    [JsonPropertyName("support_info")]
    public SupportInfoData SupportInfo { get; private set; } = new();

    internal sealed record FullGameData
    {
        [JsonPropertyName("appid")]
        public string AppId { get; private set; } = "";

        [JsonPropertyName("name")]
        public string Name { get; private set; } = "";
    }

    internal sealed record DemoData
    {
        [JsonPropertyName("appid")]
        public uint AppId { get; private set; }

        [JsonPropertyName("description")]
        public string Description { get; private set; } = "";
    }

    internal sealed record PriceOverviewData
    {
        [JsonPropertyName("currency")]
        public string Currency { get; private set; } = "";

        [JsonPropertyName("initial")]
        public uint Initial { get; private set; }

        [JsonPropertyName("final")]
        public uint Final { get; private set; }

        [JsonPropertyName("discount_percent")]
        public uint DiscountPercent { get; private set; }

        [JsonPropertyName("initial_formatted")]
        public string InitialFormatted { get; private set; } = "";

        [JsonPropertyName("final_formatted")]
        public string FinalFormatted { get; private set; } = "";
    }

    internal sealed record PackageGroupsData
    {
        [JsonPropertyName("name")]
        public string? Name { get; private set; }

        [JsonPropertyName("title")]
        public string? Title { get; private set; }

        [JsonPropertyName("description")]
        public string? Description { get; private set; }

        [JsonPropertyName("subs")]
        public HashSet<SubData>? Subs { get; private set; }

        internal sealed record SubData
        {
            [JsonPropertyName("packageid")]
            public uint SubId { get; private set; }

            [JsonPropertyName("percent_savings")]
            public string PercentSavingsText { get; private set; } = "";

            [JsonPropertyName("option_text")]
            public string OptionText { get; private set; } = "";

            [JsonPropertyName("is_free_license")]
            public bool IsFreeLicense { get; private set; }

            [JsonPropertyName("price_in_cents_with_discount")]
            public uint PriceInCentsWithDiscount { get; private set; }
        }

    }

    internal sealed record PlatformsData
    {
        [JsonPropertyName("windows")]
        public bool Windows { get; private set; }

        [JsonPropertyName("mac")]
        public bool Mac { get; private set; }

        [JsonPropertyName("linux")]
        public bool Linux { get; private set; }
    }

    internal sealed record MetacriticData
    {
        [JsonPropertyName("score")]
        public uint Score { get; private set; }

        [JsonPropertyName("url")]
        public string Url { get; private set; } = "";
    }

    internal sealed record CategoryData
    {
        [JsonPropertyName("id")]
        public uint Id { get; private set; }

        [JsonPropertyName("description")]
        public string Description { get; private set; } = "";
    }

    internal sealed record GenreData
    {
        [JsonPropertyName("id")]
        public string Id { get; private set; } = "";

        [JsonPropertyName("description")]
        public string Description { get; private set; } = "";
    }

    internal sealed record ScreenshotData
    {
        [JsonPropertyName("id")]
        public uint Id { get; private set; }

        [JsonPropertyName("path_thumbnail")]
        public string PathThumbnail { get; private set; } = "";

        [JsonPropertyName("path_full")]
        public string PathFull { get; private set; } = "";
    }

    internal sealed record MovieData
    {
        [JsonPropertyName("id")]
        public uint Id { get; private set; }

        [JsonPropertyName("name")]
        public string Name { get; private set; } = "";

        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; private set; } = "";

        [JsonPropertyName("webm")]
        public MovieFormatData? Webm { get; private set; }

        [JsonPropertyName("mp4")]
        public MovieFormatData? Mp4 { get; private set; }

        [JsonPropertyName("highlight")]
        public bool Highlight { get; private set; }

        internal sealed record MovieFormatData
        {
            [JsonPropertyName("480")]
            public string M480 { get; private set; } = "";

            [JsonPropertyName("max")]
            public string Max { get; private set; } = "";
        }
    }

    internal sealed record RecommendationsData
    {
        [JsonPropertyName("total")]
        public uint Total { get; private set; }
    }

    internal sealed record ReleaseDateData
    {
        [JsonPropertyName("coming_soon")]
        public bool ComingSoon { get; private set; }

        [JsonPropertyName("date")]
        public string Date { get; private set; } = "";
    }

    internal sealed record SupportInfoData
    {
        [JsonPropertyName("url")]
        public string Url { get; private set; } = "";

        [JsonPropertyName("email")]
        public string Email { get; private set; } = "";
    }
}

