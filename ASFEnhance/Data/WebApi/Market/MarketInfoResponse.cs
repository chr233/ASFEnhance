namespace ASFEnhance.Data.WebApi.Market;

internal sealed record MarketInfoResponse(string Name, string BucketId, string AppId, string HashName, string ItemId, bool IsNewMarket);
