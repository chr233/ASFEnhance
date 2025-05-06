using ASFEnhance.Data.WebApi;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

/// <summary>
/// 
/// </summary>
public sealed record TransactionStatusResponse : BaseResultResponse
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("purchaseresultdetail")]
    public int PurchaseResultDetail { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("purchasereceipt")]
    public PurchaseReceiptResponse? PurchaseReceipt { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public sealed class PurchaseReceiptResponse
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("paymentmethod")]
        public int PaymentMethod { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("purchasestatus")]
        public int PurchaseStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("resultdetail")]
        public int ResultDetail { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("baseprice")]
        public string? BasePrice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("formattedTotal")]
        public string FormattedTotal { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("tax")]
        public string? Tax { get; set; }
    }
}
