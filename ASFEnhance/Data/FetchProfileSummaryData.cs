namespace ASFEnhance.Data;
/// <summary>
/// 个人资料数据
/// </summary>
internal sealed record FetchProfileSummaryData
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public ulong SteamId { get; set; }

    /// <summary>
    /// 昵称名
    /// </summary>
    public string? NickName { get; set; }

    /// <summary>
    /// 真名
    /// </summary>
    public string? RealName { get; set; }

    /// <summary>
    /// 国家/地区
    /// </summary>
    public string? Region { get; set; }

    public string? Description { get; set; }

    public bool IsOnline { get; set; }

    /// <summary>
    /// 等级
    /// </summary>
    public int Level { get; set; } = -1;
    /// <summary>
    /// 徽章数量
    /// </summary>
    public long BadgeCount { get; set; } = -1;
    /// <summary>
    /// 游戏数量
    /// </summary>
    public long GameCount { get; set; } = -1;
    /// <summary>
    /// 愿望单数量
    /// </summary>
    public long WishlistCount { get; set; } = -1;
    /// <summary>
    /// 截图数量
    /// </summary>
    public long ScreenshotCount { get; set; } = -1;
    /// <summary>
    /// 视频数量
    /// </summary>
    public long VideoCount { get; set; } = -1;
    /// <summary>
    /// 创意工坊物品数量
    /// </summary>
    public long WorkshopCount { get; set; } = -1;
    /// <summary>
    /// 评测数量
    /// </summary>
    public long ReviewCount { get; set; } = -1;
    /// <summary>
    /// 指南数量
    /// </summary>
    public long GuideCount { get; set; } = -1;
    /// <summary>
    /// 艺术作品数量
    /// </summary>
    public long ArtworkCount { get; set; } = -1;
    /// <summary>
    /// 好友数量
    /// </summary>
    public long FriendCount { get; set; } = -1;
    /// <summary>
    /// 组数量
    /// </summary>
    public long GroupCount { get; set; } = -1;

    /// <summary>
    /// 个人资料留言数量
    /// </summary>
    public long CommentCount { get; set; } = -1;
}