using Newtonsoft.Json;
using SteamKit2;

namespace Chrxw.ASFEnhance.Data
{
    internal sealed class TransactionStatusResponse
    {
        [JsonProperty(PropertyName = "success", Required = Required.Always)]
        public EResult Result { get; private set; }

        [JsonProperty(PropertyName = "purchasereceipt", Required = Required.Default)]
        public PurchaseReceiptResponse PurchaseReceipt { get; private set; }
    }
    internal sealed class PurchaseReceiptResponse
    {
        [JsonProperty(PropertyName = "baseprice", Required = Required.DisallowNull)]
        public int BasePrice { get; private set; }

        //[JsonProperty(PropertyName = "currencycode", Required = Required.DisallowNull)]
        //public int CurrenCyCode { get; private set; }

        [JsonProperty(PropertyName = "formattedTotal", Required = Required.DisallowNull)]
        public string FormattedTotal { get; private set; }
    }
}
