using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

/// <summary>
/// 编辑个人资料实体类
/// </summary>
public class EditProfilePayload
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("strPersonaName")]
    public string? PersonaName { get; set; }
    /// <summary>
    /// 
    /// </summary>

    [JsonPropertyName("strCustomURL")]
    public string? CustomURL { get; set; }
    /// <summary>
    /// 
    /// </summary>

    [JsonPropertyName("strRealName")]
    public string? RealName { get; set; }
    /// <summary>
    /// 
    /// </summary>

    [JsonPropertyName("strSummary")]
    public string? Summary { get; set; }
    /// <summary>
    /// 
    /// </summary>

    [JsonPropertyName("strAvatarHash")]
    public string? AvatarHash { get; set; }
    /// <summary>
    /// 
    /// </summary>

    [JsonPropertyName("rtPersonaNameBannedUntil")]
    public long PersonaNameBannedUntil { get; set; }
    /// <summary>
    /// 
    /// </summary>

    [JsonPropertyName("rtProfileSummaryBannedUntil")]
    public long ProfileSummaryBannedUntil { get; set; }
    /// <summary>
    /// 
    /// </summary>

    [JsonPropertyName("rtAvatarBannedUntil")]
    public long AvatarBannedUntil { get; set; }
    /// <summary>
    /// 
    /// </summary>

    [JsonPropertyName("LocationData")]
    public LocationData? Location { get; set; }
    /// <summary>
    /// 
    /// </summary>

    [JsonPropertyName("ProfilePreferences")]
    public ProfilePreferencesData? ProfilePreferences { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public class LocationData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("locCountry")]
        public string? Country { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("locCountryCode")]
        public string? CountryCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("locState")]
        public string? State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("locStateCode")]
        public string? StateCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("locCity")]
        public string? City { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("locCityCode")]
        public string? CityCode { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ProfilePreferencesData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("hide_profile_awards")]
        public int HideProfileAwards { get; set; }
    }

}
