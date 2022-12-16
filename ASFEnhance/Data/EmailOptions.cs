namespace ASFEnhance.Data
{
    internal sealed record EmailOptions
    {
        /// <summary>
        /// 允许发送电子邮件 (总开关)
        /// </summary>
        public bool EnableEmailNotification { get; set; }
        /// <summary>
        /// 愿望单物品打折时
        /// </summary>
        public bool WhenWishlistDiscount { get; set; }
        /// <summary>
        /// 愿望单物品发行或脱离抢先体验时
        /// </summary>
        public bool WhenWishlistRelease { get; set; }
        /// <summary>
        /// 关注的青睐之光物品发行或脱离抢先体验时
        /// </summary>
        public bool WhenGreenLightRelease { get; set; }
        /// <summary>
        /// 关注的发行商发行或者脱离抢险体验时
        /// </summary>
        public bool WhenFollowPublisherRelease { get; set; }
        /// <summary>
        /// 当季节促销开始时
        /// </summary>
        public bool WhenSaleEvent { get; set; }
        /// <summary>
        /// 收到鉴赏家评测副本
        /// </summary>
        public bool WhenReceiveCuratorReview { get; set; }
        /// <summary>
        /// 收到社区奖励
        /// </summary>
        public bool WhenReceiveCommunityReward { get; set; }
        /// <summary>
        /// 收到游戏活动通知
        /// </summary>
        public bool WhenGameEventNotification { get; set; }
    }
}
