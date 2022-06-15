namespace ASFEnhance.Data
{
    /// <summary>
    /// 许可类型
    /// </summary>
    internal enum LicenseType : byte
    {
        Unknown = 0,
        /// <summary>
        /// 零售cdk
        /// </summary>
        Retail,
        /// <summary>
        /// 免费
        /// </summary>
        Complimentary,
        /// <summary>
        /// Steam商店购买
        /// </summary>
        SteamStore,
        /// <summary>
        /// 礼物/玩家通行证
        /// </summary>
        GiftOrGuestPass,
    }

    internal sealed class LicensesData
    {
        public string AA { get; set; }
    }
}
