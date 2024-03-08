namespace ASFEnhance.IPC.Requests;

/// <summary>
/// 下单请求
/// </summary>
public sealed record AddCartRequest
{
    /// <summary>
    /// SubID列表
    /// </summary>
    public List<ItemData>? Items { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public sealed record ItemData
    {
        /// <summary>
        /// 
        /// </summary>
        public uint PackageId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public uint BundleId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public GiftInfoData? GiftInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsPrivate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsGift { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public sealed record GiftInfoData
        {
            /// <summary>
            /// 
            /// </summary>
            public ulong AccountIdGiftee { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string? GifteeName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string? Message { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string? Sentiment { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string? Signature { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public ulong TimeScheduledSend { get; set; }
        }
    }
}
