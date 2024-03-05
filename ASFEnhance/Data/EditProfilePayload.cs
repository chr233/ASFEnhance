using Newtonsoft.Json;

namespace ASFEnhance.Data;

/// <summary>
/// 编辑个人资料实体类
/// </summary>
public class EditProfilePayload
{
    [JsonProperty("strPersonaName")]
    public string? PersonaName { get; set; }

    [JsonProperty("strCustomURL")]
    public string? CustomURL { get; set; }

    [JsonProperty("strRealName")]
    public string? RealName { get; set; }

    [JsonProperty("strSummary")]
    public string? Summary { get; set; }

    [JsonProperty("strAvatarHash")]
    public string? AvatarHash { get; set; }

    [JsonProperty("rtPersonaNameBannedUntil")]
    public long PersonaNameBannedUntil { get; set; }

    [JsonProperty("rtProfileSummaryBannedUntil")]
    public long ProfileSummaryBannedUntil { get; set; }

    [JsonProperty("rtAvatarBannedUntil")]
    public long AvatarBannedUntil { get; set; }

    [JsonProperty("LocationData")]
    public LocationData? Location { get; set; }

    [JsonProperty("ProfilePreferences")]
    public ProfilePreferencesData? ProfilePreferences { get; set; }

    public class LocationData
    {
        [JsonProperty("locCountry")]
        public string? Country { get; set; }

        [JsonProperty("locCountryCode")]
        public string? CountryCode { get; set; }

        [JsonProperty("locState")]
        public string? State { get; set; }

        [JsonProperty("locStateCode")]
        public string? StateCode { get; set; }

        [JsonProperty("locCity")]
        public string? City { get; set; }

        [JsonProperty("locCityCode")]
        public string? CityCode { get; set; }
    }

    public class ProfilePreferencesData
    {
        [JsonProperty("hide_profile_awards")]
        public int HideProfileAwards { get; set; }
    }

}
