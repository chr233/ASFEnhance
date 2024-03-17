using ASFEnhance.Data.Common;
using SteamKit2;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed record InitTransactionResponse : BaseResultResponse
{
    [JsonPropertyName("purchaseresultdetail")]
    public int PurchaseResultDetail { get; set; }
    [JsonPropertyName("paymentmethod")]
    public int PaymentMethod { get; set; }
    [JsonPropertyName("transid")]
    public string? TransId { get; set; }
    [JsonPropertyName("transactionid")]
    public string? TransActionId { get; set; }
    [JsonPropertyName("transactionprovider")]
    public int TransActionProvider { get; set; }
    [JsonPropertyName("paymentmethodcountrycode")]
    public string? PaymentMethodCountryCode { get; set; }
    [JsonPropertyName("paypaltoken")]
    public string? PaypalToken { get; set; }
    [JsonPropertyName("paypalacct")]
    public int PaypalAcct { get; set; }
    [JsonPropertyName("packagewitherror")]
    public int PackageWithError { get; set; }
    [JsonPropertyName("appcausingerror")]
    public int AppCausingError { get; set; }
    [JsonPropertyName("pendingpurchasepaymentmethod")]
    public int PendingPurchasePaymenTmethod { get; set; }
    [JsonPropertyName("authorizationurl")]
    public string? AuthorizationUrl { get; set; }
}
