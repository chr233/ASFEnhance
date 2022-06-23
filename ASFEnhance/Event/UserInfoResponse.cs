#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using Newtonsoft.Json;

namespace ASFEnhance.Event
{
    internal sealed class UserInfoResponse
    {
        [JsonProperty("logged_in")]
        public bool LoggedIn { get; set; }

        [JsonProperty("steamid")]
        public string SteamId { get; set; } = "";

        [JsonProperty("accountid")]
        public int Accountid { get; set; }

        [JsonProperty("account_name")]
        public string AccountName { get; set; } = "";

        [JsonProperty("authwgtoken")]
        public string AuthwgToken { get; set; } = "";

        [JsonProperty("is_support")]
        public bool IsSupport { get; set; }

        [JsonProperty("is_limited")]
        public bool IsLimited { get; set; }

        [JsonProperty("is_partner_member")]
        public bool IsPartnerMember { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; } = "";
    }
}
