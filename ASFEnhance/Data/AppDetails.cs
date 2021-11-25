using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SteamKit2;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Chrxw.ASFEnhance.Data
{
    [SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]
    internal sealed class AppDetailsResponse
    {
        [JsonExtensionData]
        private IDictionary<string, JToken> _additionalData;
    }
    internal sealed class AppDetailsPayload
    {
        [JsonProperty(PropertyName = "success", Required = Required.Always)]
        public EResult Result { get; private set; }

        [JsonProperty(PropertyName = "data", Required = Required.DisallowNull)]
        public AppDetailsData Data { get; private set; }

        [JsonProperty(PropertyName = "dlc", Required = Required.DisallowNull)]
        public AppDetailsDlcs Dlc { get; private set; }

        [JsonProperty(PropertyName = "discount", Required = Required.DisallowNull)]
        public int Discount { get; private set; }
    }

    internal sealed class AppDetailsData
    {
        [JsonProperty(PropertyName = "type", Required = Required.Always)]
        public EResult Result { get; private set; }

        [JsonProperty(PropertyName = "name", Required = Required.DisallowNull)]
        public int BasePrice { get; private set; }

        [JsonProperty(PropertyName = "steam_appid", Required = Required.DisallowNull)]
        public int Tax { get; private set; }

        [JsonProperty(PropertyName = "required_age", Required = Required.DisallowNull)]
        public int Discount { get; private set; }

        [JsonProperty(PropertyName = "is_free", Required = Required.DisallowNull)]
        public bool IsFree { get; private set; }
    }
    internal sealed class AppDetailsDlcs
    {
        [JsonExtensionData]
        private IDictionary<int, int> Dlcs;
    }

}
