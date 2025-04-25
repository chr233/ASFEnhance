using SteamKit2;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data.WebApi;

/// <summary>
/// 
/// </summary>
public sealed record AjaxCreateWalletAndCheckFundsResponse
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("success")]
    public EResult Success { get; set; }
}
