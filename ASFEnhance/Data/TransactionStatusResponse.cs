using ArchiSteamFarm.Steam.Data;
using Newtonsoft.Json;

namespace ASFEnhance.Data;

internal sealed class TransactionStatusResponse : ResultResponse
{
    [JsonProperty(PropertyName = "purchasereceipt", Required = Required.Default)]
    public PurchaseReceiptResponse? PurchaseReceipt { get; private set; }

    internal sealed class PurchaseReceiptResponse
    {
        [JsonProperty(PropertyName = "baseprice", Required = Required.DisallowNull)]
        public int BasePrice { get; private set; }

        [JsonProperty(PropertyName = "formattedTotal", Required = Required.DisallowNull)]
        public string FormattedTotal { get; private set; } = "";
    }
}
