namespace ASFEnhance.Data;

/// <summary>
/// 获取游戏详情
/// </summary>
public sealed record CheckGameResponse
{
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Success { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 是否已拥有
    /// </summary>
    public bool Owned { get; set; }
    /// <summary>
    /// 是否添加愿望单
    /// </summary>
    public bool InWishlist { get; set; }
    /// <summary>
    /// 是否已关注
    /// </summary>
    public bool IsFollow { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="success"></param>
    /// <param name="name"></param>
    public CheckGameResponse(bool success, string name)
    {
        Success = success;
        Name = name;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="success"></param>
    /// <param name="name"></param>
    /// <param name="owned"></param>
    /// <param name="inWishlist"></param>
    /// <param name="isFollow"></param>
    public CheckGameResponse(bool success, string name, bool owned, bool inWishlist, bool isFollow)
    {
        Success = success;
        Name = name;
        Owned = owned;
        InWishlist = inWishlist;
        IsFollow = isFollow;
    }
}
