namespace ASFEnhance.Data.WebApi;

/// <summary>
/// 
/// </summary>
/// <param name="Name"></param>
/// <param name="Price"></param>
/// <param name="Amount"></param>
/// <param name="OrderId"></param>
public sealed record OrderInfoData(string? Name, string? Price, string? Amount, string? OrderId);

