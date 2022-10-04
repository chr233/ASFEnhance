using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ASFEnhance.Event
{
    internal sealed class APIResponse
    {
        [JsonProperty("succ", Required = Required.DisallowNull)]
        public bool Success { get; set; }

        [JsonProperty("msg", Required = Required.DisallowNull)]
        public string Message { get; set; }

        [JsonProperty("data", Required = Required.DisallowNull)]
        public List<string>? Data { get; set; }
    }
}
