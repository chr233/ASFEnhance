using SteamKit2;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data.WebApi;

/// <summary>
/// 
/// </summary>
public sealed record AjaxRedeemWalletCodeResponse
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("success")]
    public EResult Success { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("detail")]
    internal int Detail { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("formattednewwalletbalance")]
    internal string WalletBalance { get; set; } = "";
}
