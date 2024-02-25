using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

/// <summary>
/// 编辑个人资料实体类
/// </summary>
public class EditProfilePayload
{
    /// <summary>
    /// 昵称
    /// </summary>
    [JsonPropertyName("strPersonaName")]
    public string? PersonaName { get; set; }

    [JsonPropertyName("strCustomURL")]
    public string? CustomURL { get; set; }

    [JsonPropertyName("strRealName")]
    public string? RealName { get; set; }

    [JsonPropertyName("strSummary")]
    public string? Summary { get; set; }

    [JsonPropertyName("strAvatarHash")]
    public string? AvatarHash { get; set; }

    [JsonPropertyName("rtPersonaNameBannedUntil")]
    public long PersonaNameBannedUntil { get; set; }

    [JsonPropertyName("rtProfileSummaryBannedUntil")]
    public long ProfileSummaryBannedUntil { get; set; }

    [JsonPropertyName("rtAvatarBannedUntil")]
    public long AvatarBannedUntil { get; set; }

    [JsonPropertyName("LocationData")]
    public LocationData? Location { get; set; }

    [JsonPropertyName("ProfilePreferences")]
    public ProfilePreferencesData? ProfilePreferences { get; set; }

    public class LocationData
    {
        [JsonPropertyName("locCountry")]
        public string? Country { get; set; }

        [JsonPropertyName("locCountryCode")]
        public string? CountryCode { get; set; }

        [JsonPropertyName("locState")]
        public string? State { get; set; }

        [JsonPropertyName("locStateCode")]
        public string? StateCode { get; set; }

        [JsonPropertyName("locCity")]
        public string? City { get; set; }

        [JsonPropertyName("locCityCode")]
        public string? CityCode { get; set; }
    }

    public class ProfilePreferencesData
    {
        [JsonPropertyName("hide_profile_awards")]
        public int HideProfileAwards { get; set; }
    }

}
