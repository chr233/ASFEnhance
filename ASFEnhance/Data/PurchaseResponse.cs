using ASFEnhance.Data.WebApi;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed record PurchaseResponse : BaseResultResponse
{
    [JsonPropertyName("transid")]
    public string? TransId { get; set; }
    
    [JsonPropertyName("paymentmethod")]
    public byte PaymentMethod { get; set; }
    
    [JsonPropertyName("transactionid")]
    public string? TransActionId { get; set; }
    
    [JsonPropertyName("transactionprovider")]
    public byte TransSctionProvider { get; set; }
    
    [JsonPropertyName("paymentmethodcountrycode")]
    public string? PaymentMethodCountryCode { get; set; }
}
