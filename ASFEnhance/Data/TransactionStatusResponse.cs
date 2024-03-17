using ASFEnhance.Data.Common;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed record TransactionStatusResponse : BaseResultResponse
{
    [JsonPropertyName("purchaseresultdetail")]
    public int PurchaseResultDetail { get; set; }
    [JsonPropertyName("purchasereceipt")]
    public PurchaseReceiptResponse? PurchaseReceipt { get; set; }

    internal sealed class PurchaseReceiptResponse
    {
        [JsonPropertyName("paymentmethod")]
        public int PaymentMethod { get; set; }
        [JsonPropertyName("purchasestatus")]
        public int PurchaseStatus { get; set; }
        [JsonPropertyName("resultdetail")]
        public int ResultDetail { get; set; }
        [JsonPropertyName("baseprice")]
        public string? BasePrice { get; set; }

        [JsonPropertyName("formattedTotal")]
        public string FormattedTotal { get; set; } = "";
        [JsonPropertyName("tax")]
        public string? Tax { get; set; }
    }
}
