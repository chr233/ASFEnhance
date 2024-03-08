using Newtonsoft.Json;

namespace ASFEnhance.Data;

/// <summary>
/// 编辑个人资料实体类
/// </summary>
public class EditProfilePayload
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("strPersonaName")]
    public string? PersonaName { get; set; }
    /// <summary>
    /// 
    /// </summary>

    [JsonProperty("strCustomURL")]
    public string? CustomURL { get; set; }
    /// <summary>
    /// 
    /// </summary>

    [JsonProperty("strRealName")]
    public string? RealName { get; set; }
    /// <summary>
    /// 
    /// </summary>

    [JsonProperty("strSummary")]
    public string? Summary { get; set; }
    /// <summary>
    /// 
    /// </summary>

    [JsonProperty("strAvatarHash")]
    public string? AvatarHash { get; set; }
    /// <summary>
    /// 
    /// </summary>

    [JsonProperty("rtPersonaNameBannedUntil")]
    public long PersonaNameBannedUntil { get; set; }
    /// <summary>
    /// 
    /// </summary>

    [JsonProperty("rtProfileSummaryBannedUntil")]
    public long ProfileSummaryBannedUntil { get; set; }
    /// <summary>
    /// 
    /// </summary>

    [JsonProperty("rtAvatarBannedUntil")]
    public long AvatarBannedUntil { get; set; }
    /// <summary>
    /// 
    /// </summary>

    [JsonProperty("LocationData")]
    public LocationData? Location { get; set; }
    /// <summary>
    /// 
    /// </summary>

    [JsonProperty("ProfilePreferences")]
    public ProfilePreferencesData? ProfilePreferences { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public class LocationData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("locCountry")]
        public string? Country { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("locCountryCode")]
        public string? CountryCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("locState")]
        public string? State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("locStateCode")]
        public string? StateCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("locCity")]
        public string? City { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("locCityCode")]
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
        [JsonProperty("hide_profile_awards")]
        public int HideProfileAwards { get; set; }
    }

}
