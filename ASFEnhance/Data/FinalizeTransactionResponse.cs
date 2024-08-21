using ASFEnhance.Data.WebApi;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed record FinalizeTransactionResponse : BaseResultResponse
{
    [JsonPropertyName("purchaseresultdetail")]
    public int PurchaseResultDetail { get; set; }
    [JsonPropertyName("bShowBRSpecificCreditCardError")]
    public bool BShowBRSpecificCreditCardError { get; set; }
}
