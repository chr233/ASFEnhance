namespace ASFEnhance.IPC.Data.Responses;

/// <summary>
/// 购物结果响应
/// </summary>
public sealed record ExternalPurchaseResponse(string? TransId, string? TotalCoast, string PaymentUrl);
