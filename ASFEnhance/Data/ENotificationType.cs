namespace ASFEnhance.Data;

/// <summary>
/// 通知类型
/// </summary>
internal enum ENotificationType : byte
{
    /// <summary>
    /// 收到礼物
    /// </summary>
    ReceivedGift = 2,
    /// <summary>
    /// 订阅的讨论回复
    /// </summary>
    SubscribedDissionReplyed,
    /// <summary>
    /// 有新物品
    /// </summary>
    ReceivedNewItem,
    /// <summary>
    /// 收到好友邀请
    /// </summary>
    ReceivedFriendInvitation,
    /// <summary>
    /// 大促开始
    /// </summary>
    MajorSaleStart,
    /// <summary>
    /// 愿望单物品打折
    /// </summary>
    ItemInWishlistOnSale = 8,
    /// <summary>
    /// 收到交易报价
    /// </summary>
    ReceivedTradeOffer,
    /// <summary>
    /// 收到客服回复
    /// </summary>
    ReceivedSteamSupportReply = 11,
    /// <summary>
    /// Steam回合通知
    /// </summary>
    SteamTurnNotification,
    /// <summary>
    /// Steam消息
    /// </summary>
    SteamCommunityMessage = 14,
}
