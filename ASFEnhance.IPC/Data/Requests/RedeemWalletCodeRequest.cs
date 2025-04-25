using ASFEnhance.Data.Plugin;

namespace ASFEnhance.IPC.Data.Requests;
/// <summary>
/// 
/// </summary>
public sealed record RedeemWalletCodeRequest
{
    /// <summary>
    /// 
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public AddressConfig? Address { get; set; }
}
