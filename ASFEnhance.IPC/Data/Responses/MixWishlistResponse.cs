using ASFEnhance.Data.IWishlistService;

namespace ASFEnhance.IPC.Data.Responses;

/// <summary>
/// 愿望单响应
/// </summary>
public sealed record MixWishlistResponse
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="count"></param>
    /// <param name="data"></param>
    public MixWishlistResponse(int count, GetWishlistResponse? data)
    {
        Count = count;
        Items = data?.Items;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    public MixWishlistResponse(GetWishlistResponse? data)
    {
        Count = data?.Items?.Count ?? -1;
        Items = data?.Items;
    }

    /// <summary>
    /// 
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<GetWishlistResponse.ItemData>? Items { get; set; }
}
