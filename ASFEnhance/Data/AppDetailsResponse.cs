using Newtonsoft.Json;

namespace ASFEnhance.Data
{
    internal sealed record AppDetailResponse
    {
        [JsonProperty(PropertyName = "success", Required = Required.Always)]
        public bool Success { get; private set; }

        [JsonProperty(PropertyName = "data", Required = Required.DisallowNull)]
        public AppDetailData Data { get; private set; } = new();
    }

    internal sealed record AppDetailData
    {
        [JsonProperty(PropertyName = "type", Required = Required.Always)]
        public string Type { get; private set; }

        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public string Name { get; private set; }

        [JsonProperty(PropertyName = "steam_appid", Required = Required.Always)]
        public uint AppId { get; private set; }

        [JsonProperty(PropertyName = "required_age", Required = Required.Always)]
        public uint RequiredAge { get; private set; }

        [JsonProperty(PropertyName = "is_free", Required = Required.Always)]
        public bool IsFree { get; private set; }

        [JsonProperty(PropertyName = "dlc", Required = Required.DisallowNull)]
        public HashSet<uint> Dlc { get; private set; }

        [JsonProperty(PropertyName = "detailed_description", Required = Required.DisallowNull)]
        public string DetailedDescription { get; private set; }

        [JsonProperty(PropertyName = "about_the_game", Required = Required.DisallowNull)]
        public string AboutTheGame { get; private set; }

        [JsonProperty(PropertyName = "short_description", Required = Required.DisallowNull)]
        public string ShortDescription { get; private set; }

        [JsonProperty(PropertyName = "fullgame", Required = Required.DisallowNull)]
        public FullGameData FullGame { get; private set; }

        [JsonProperty(PropertyName = "supported_languages", Required = Required.DisallowNull)]
        public string SupportedLanguages { get; private set; }

        [JsonProperty(PropertyName = "header_image", Required = Required.DisallowNull)]
        public string HeaderImage { get; private set; }

        [JsonProperty(PropertyName = "website", Required = Required.Default)]
        public string Website { get; private set; }

        [JsonProperty(PropertyName = "developers", Required = Required.DisallowNull)]
        public HashSet<string> Developers { get; private set; }

        [JsonProperty(PropertyName = "publishers", Required = Required.DisallowNull)]
        public HashSet<string> Publishers { get; private set; }

        [JsonProperty(PropertyName = "demos", Required = Required.DisallowNull)]
        public HashSet<DemoData> Demos { get; private set; }

        [JsonProperty(PropertyName = "price_overview", Required = Required.DisallowNull)]
        public PriceOverviewData PriceOverview { get; private set; }

        [JsonProperty(PropertyName = "packages", Required = Required.DisallowNull)]
        public HashSet<uint> Packages { get; private set; }

        [JsonProperty(PropertyName = "package_groups", Required = Required.DisallowNull)]
        public HashSet<PackageGroupsData> PackageGroups { get; private set; }

        [JsonProperty(PropertyName = "platforms", Required = Required.DisallowNull)]
        public PlatformsData Platforms { get; private set; }

        [JsonProperty(PropertyName = "metacritic", Required = Required.DisallowNull)]
        public MetacriticData Metacritic { get; private set; }

        [JsonProperty(PropertyName = "categories", Required = Required.DisallowNull)]
        public HashSet<CategoryData> Categories { get; private set; }

        [JsonProperty(PropertyName = "genres", Required = Required.DisallowNull)]
        public HashSet<GenreData> Genres { get; private set; }

        [JsonProperty(PropertyName = "screenshots", Required = Required.DisallowNull)]
        public HashSet<ScreenshotData> Screenshots { get; private set; }

        [JsonProperty(PropertyName = "movies", Required = Required.DisallowNull)]
        public HashSet<MovieData> Movies { get; private set; }

        [JsonProperty(PropertyName = "recommendations", Required = Required.DisallowNull)]
        public RecommendationsData Recommendations { get; private set; }

        [JsonProperty(PropertyName = "release_date", Required = Required.DisallowNull)]
        public ReleaseDateData ReleaseDate { get; private set; }

        [JsonProperty(PropertyName = "support_info", Required = Required.DisallowNull)]
        public SupportInfoData SupportInfo { get; private set; }

        internal sealed record FullGameData
        {
            [JsonProperty(PropertyName = "appid", Required = Required.Always)]
            public string AppId { get; private set; }

            [JsonProperty(PropertyName = "name", Required = Required.Always)]
            public string Name { get; private set; }
        }

        internal sealed record DemoData
        {
            [JsonProperty(PropertyName = "appid", Required = Required.Always)]
            public uint AppId { get; private set; }

            [JsonProperty(PropertyName = "description", Required = Required.Always)]
            public string Description { get; private set; }
        }

        internal sealed record PriceOverviewData
        {
            [JsonProperty(PropertyName = "currency", Required = Required.Always)]
            public string Currency { get; private set; }

            [JsonProperty(PropertyName = "initial", Required = Required.Always)]
            public uint Initial { get; private set; }

            [JsonProperty(PropertyName = "final", Required = Required.Always)]
            public uint Final { get; private set; }

            [JsonProperty(PropertyName = "discount_percent", Required = Required.Always)]
            public uint DiscountPercent { get; private set; }

            [JsonProperty(PropertyName = "initial_formatted", Required = Required.Always)]
            public string InitialFormatted { get; private set; }

            [JsonProperty(PropertyName = "final_formatted", Required = Required.Always)]
            public string FinalFormatted { get; private set; }
        }

        internal sealed record PackageGroupsData
        {
            [JsonProperty(PropertyName = "name", Required = Required.Default)]
            public string Name { get; private set; }

            [JsonProperty(PropertyName = "title", Required = Required.Default)]
            public string Title { get; private set; }

            [JsonProperty(PropertyName = "description", Required = Required.Default)]
            public string Description { get; private set; }

            [JsonProperty(PropertyName = "subs", Required = Required.Default)]
            public HashSet<SubData> Subs { get; private set; }

            internal sealed record SubData
            {
                [JsonProperty(PropertyName = "packageid", Required = Required.Always)]
                public uint SubId { get; private set; }

                [JsonProperty(PropertyName = "percent_savings", Required = Required.Always)]
                public string PercentSavingsText { get; private set; }

                [JsonProperty(PropertyName = "option_text", Required = Required.Always)]
                public string OptionText { get; private set; }

                [JsonProperty(PropertyName = "is_free_license", Required = Required.Always)]
                public bool IsFreeLicense { get; private set; }

                [JsonProperty(PropertyName = "price_in_cents_with_discount", Required = Required.Always)]
                public uint PriceInCentsWithDiscount { get; private set; }
            }

        }

        internal sealed record PlatformsData
        {
            [JsonProperty(PropertyName = "windows", Required = Required.Always)]
            public bool Windows { get; private set; }

            [JsonProperty(PropertyName = "mac", Required = Required.Always)]
            public bool Mac { get; private set; }

            [JsonProperty(PropertyName = "linux", Required = Required.Always)]
            public bool Linux { get; private set; }
        }

        internal sealed record MetacriticData
        {
            [JsonProperty(PropertyName = "score", Required = Required.Always)]
            public uint Score { get; private set; }

            [JsonProperty(PropertyName = "url", Required = Required.Always)]
            public string Url { get; private set; }
        }

        internal sealed record CategoryData
        {
            [JsonProperty(PropertyName = "id", Required = Required.Always)]
            public uint Id { get; private set; }

            [JsonProperty(PropertyName = "description", Required = Required.Always)]
            public string Description { get; private set; }

            public override string ToString()
            {
                return Description;
            }
        }

        internal sealed record GenreData
        {
            [JsonProperty(PropertyName = "id", Required = Required.Always)]
            public string Id { get; private set; }

            [JsonProperty(PropertyName = "description", Required = Required.Always)]
            public string Description { get; private set; }

            public override string ToString()
            {
                return Description;
            }
        }

        internal sealed record ScreenshotData
        {
            [JsonProperty(PropertyName = "id", Required = Required.Always)]
            public uint Id { get; private set; }

            [JsonProperty(PropertyName = "path_thumbnail", Required = Required.Always)]
            public string PathThumbnail { get; private set; }

            [JsonProperty(PropertyName = "path_full", Required = Required.Always)]
            public string PathFull { get; private set; }
        }

        internal sealed record MovieData
        {
            [JsonProperty(PropertyName = "id", Required = Required.Always)]
            public uint Id { get; private set; }

            [JsonProperty(PropertyName = "name", Required = Required.Always)]
            public string Name { get; private set; }

            [JsonProperty(PropertyName = "thumbnail", Required = Required.Always)]
            public string Thumbnail { get; private set; }

            [JsonProperty(PropertyName = "webm", Required = Required.Always)]
            public MovieFormatData Webm { get; private set; }

            [JsonProperty(PropertyName = "mp4", Required = Required.Always)]
            public MovieFormatData Mp4 { get; private set; }

            [JsonProperty(PropertyName = "highlight", Required = Required.Always)]
            public bool Highlight { get; private set; }

            internal sealed record MovieFormatData
            {
                [JsonProperty(PropertyName = "480", Required = Required.Always)]
                public string M480 { get; private set; }

                [JsonProperty(PropertyName = "max", Required = Required.Always)]
                public string Max { get; private set; }
            }
        }

        internal sealed record RecommendationsData
        {
            [JsonProperty(PropertyName = "total", Required = Required.Always)]
            public uint Total { get; private set; }
        }

        internal sealed record ReleaseDateData
        {
            [JsonProperty(PropertyName = "coming_soon", Required = Required.Always)]
            public bool ComingSoon { get; private set; }

            [JsonProperty(PropertyName = "date", Required = Required.Always)]
            public string Date { get; private set; }
        }

        internal sealed record SupportInfoData
        {
            [JsonProperty(PropertyName = "url", Required = Required.Always)]
            public string Url { get; private set; }

            [JsonProperty(PropertyName = "email", Required = Required.Always)]
            public string Email { get; private set; }
        }
    }
}

