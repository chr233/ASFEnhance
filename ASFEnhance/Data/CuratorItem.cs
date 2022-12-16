using Newtonsoft.Json;

namespace ASFEnhance.Data
{
    public sealed record CuratorItem
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("curator_description", Required = Required.Always)]
        public string Description { get; set; }

        [JsonProperty("clanId", Required = Required.Always)]
        public string ClanId { get; set; }

        [JsonProperty("total_followers", Required = Required.Always)]
        public uint TotalFollowers { get; set; }

        [JsonProperty("total_reviews", Required = Required.Always)]
        public uint TotalReviews { get; set; }

        [JsonProperty("total_recommended", Required = Required.Always)]
        public uint TotalRecommanded { get; set; }

        [JsonProperty("total_not_recommended", Required = Required.Always)]
        public uint TotalNotRecommanded { get; set; }

        [JsonProperty("total_informative", Required = Required.Always)]
        public uint TotalInformative { get; set; }
    }
}
