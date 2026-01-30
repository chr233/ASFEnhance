using ASFEnhance.Data.WebApi;
using SteamKit2;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

/// <summary>
/// 
/// </summary>
public sealed record InitTransactionResponse : BaseResultResponse
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("purchaseresultdetail")]
    public EPurchaseResultDetail PurchaseResultDetail { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("paymentmethod")]
    public int PaymentMethod { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("transid")]
    public string? TransId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("transactionid")]
    public string? TransActionId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("transactionprovider")]
    public int TransActionProvider { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("paymentmethodcountrycode")]
    public string? PaymentMethodCountryCode { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("paypaltoken")]
    public string? PaypalToken { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("paypalacct")]
    public int PaypalAcct { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("packagewitherror")]
    public int PackageWithError { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("appcausingerror")]
    public int AppCausingError { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("pendingpurchasepaymentmethod")]
    public int PendingPurchasePaymenTmethod { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("authorizationurl")]
    public string? AuthorizationUrl { get; set; }
}
