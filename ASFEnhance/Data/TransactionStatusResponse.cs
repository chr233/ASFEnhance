using ArchiSteamFarm.Steam.Data;
using ASFEnhance.Data.Common;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed record TransactionStatusResponse : BaseResultResponse
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
