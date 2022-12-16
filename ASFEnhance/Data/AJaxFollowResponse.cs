using ArchiSteamFarm.Steam.Data;
using Newtonsoft.Json;

namespace ASFEnhance.Data
{
    internal sealed record AJaxFollowResponse
    {
        [JsonProperty("success", Required = Required.DisallowNull)]
        public AjaxFlollowSuccess Success { get; set; }

        internal sealed class AjaxFlollowSuccess : ResultResponse
        {
        }
    }
}
