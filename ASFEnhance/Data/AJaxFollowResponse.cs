using ArchiSteamFarm.Steam.Data;
using Newtonsoft.Json;

namespace ASFEnhance.Data
{
    internal sealed class AJaxFollowResponse
    {
        [JsonProperty("success", Required = Required.DisallowNull)]
        public AjaxFlollowSuccess Success { get; set; }

        [JsonConstructor]
        public AJaxFollowResponse() { }

        internal sealed class AjaxFlollowSuccess : ResultResponse
        {
            [JsonConstructor]
            public AjaxFlollowSuccess() { }
        }
    }
}
