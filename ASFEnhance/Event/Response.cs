using Newtonsoft.Json;

namespace ASFEnhance.Event
{
    internal class Response
    {

    }

    internal sealed class UserInfoResponse
    {
        [JsonProperty(PropertyName = "authwgtoken", Required = Required.Always)]
        public string AuthwgToken { get; set; }
    }
    internal sealed class RaceFestivalResponse
    {
        [JsonProperty(PropertyName = "success", Required = Required.Always)]
        public int Success { get; set; }
    }
}
