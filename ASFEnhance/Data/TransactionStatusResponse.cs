using ArchiSteamFarm.Steam.Data;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed class TransactionStatusResponse : ResultResponse
{
    [JsonPropertyName("purchasereceipt")]
    public PurchaseReceiptResponse? PurchaseReceipt { get; private set; }

    internal sealed class PurchaseReceiptResponse
    {
        [JsonPropertyName("baseprice")]
        public int BasePrice { get; private set; }

        [JsonPropertyName("formattedTotal")]
        public string FormattedTotal { get; private set; } = "";
    }
}
