using ArchiSteamFarm.Steam.Data;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed class TransactionStatusResponse : ResultResponse
{
    [JsonPropertyName("purchasereceipt")]
    public PurchaseReceiptResponse? PurchaseReceipt { get; set; }

    internal sealed class PurchaseReceiptResponse
    {
        [JsonPropertyName("baseprice")]
        public int BasePrice { get; set; }

        [JsonPropertyName("formattedTotal")]
        public string FormattedTotal { get; set; } = "";
    }
}
