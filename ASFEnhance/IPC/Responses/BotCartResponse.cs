using ASFEnhance.Data.Common;
using ASFEnhance.Data.IAccountCartService;
using SteamKit2;

namespace ASFEnhance.IPC.Responses;
public sealed record BotCartResponse
{
    public List<CartItemData>? Items { get; set; }
    public bool IsValid { get; set; }

    public sealed record CartItemData
    {
        public uint? PackageId { get; set; }
        public uint? BundleId { get; set; }
        public string? LineItemId { get; set; }
        public bool IsValid { get; set; }
        public ulong TimeAdded { get; set; }
        public string? PriceCents { get; set; }
        public ECurrencyCode CurrencyCode { get; set; }
        public string? PriceFormatted { get; set; }
        public bool IsGift { get; set; }
        public bool IsPrivate { get; set; }
        public GiftInfoData? GiftInfo { get; set; }
    }
}
