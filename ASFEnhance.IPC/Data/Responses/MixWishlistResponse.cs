using ASFEnhance.Data.IWishlistService;

namespace ASFEnhance.IPC.Data.Responses;

public sealed record MixWishlistResponse
{
    public MixWishlistResponse(int count, GetWishlistResponse? data)
    {
        Count = count;
        Items = data?.Items;
    }

    public MixWishlistResponse(GetWishlistResponse? data)
    {
        Count = data?.Items?.Count ?? -1;
        Items = data?.Items;
    }

    public int Count { get; set; }
    public List<GetWishlistResponse.ItemData>? Items { get; set; }
}
