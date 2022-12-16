using ArchiSteamFarm.Steam.Data;
using Newtonsoft.Json;

namespace ASFEnhance.Data
{
    internal sealed class AjaxAddFriendResponse : ResultResponse
    {
        [JsonProperty("invited", Required = Required.Always)]
        public List<string>? Invited { get; set; }
    }
}
