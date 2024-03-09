using ASFEnhance.Data.Common;
using SteamKit2;

namespace ASFEnhance.IPC.Responses;
/// <summary>
/// 
/// </summary>
public sealed record BotCartResponse
{
    /// <summary>
    /// 
    /// </summary>
    public List<CartItemData>? Items { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public sealed record CartItemData
    {
        /// <summary>
        /// 
        /// </summary>
        public uint? PackageId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public uint? BundleId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? LineItemId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsValid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ulong TimeAdded { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? PriceCents { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ECurrencyCode CurrencyCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? PriceFormatted { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsGift { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsPrivate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public GiftInfoData? GiftInfo { get; set; }
    }
}
