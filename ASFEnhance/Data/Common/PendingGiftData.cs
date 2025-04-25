namespace ASFEnhance.Data.Common;

internal sealed record PendingGiftData
{
    public PendingGiftData(ulong giftId, string? gameName, string? senderName, ulong senderSteamId)
    {
        GiftId = giftId;
        GameName = gameName;
        SenderName = senderName;
        SenderSteamId = senderSteamId;
    }

    public ulong GiftId { get; set; }
    public string? GameName { get; set; }
    public string? SenderName { get; set; }
    public ulong SenderSteamId { get; set; }
}