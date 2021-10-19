using Newtonsoft.Json;
using SteamKit2;
using System.Diagnostics.CodeAnalysis;

namespace Chrxw.ASFEnhance.Data
{
    [SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]
    internal sealed class FinalPriceResponse
    {
        [JsonProperty(PropertyName = "success", Required = Required.Always)]
        public EResult Result { get; private set; }

        [JsonProperty(PropertyName = "base", Required = Required.DisallowNull)]
        public int BasePrice { get; private set; }

        [JsonProperty(PropertyName = "tax", Required = Required.DisallowNull)]
        public int Tax { get; private set; }

        [JsonProperty(PropertyName = "discount", Required = Required.DisallowNull)]
        public int Discount { get; private set; }
    }
}
