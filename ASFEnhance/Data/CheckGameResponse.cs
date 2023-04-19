namespace ASFEnhance.Data;

public sealed record CheckGameResponse
{
    public bool Success { get; set; }
    public string Name { get; set; }
    public bool Owned { get; set; }
    public bool InWishlist { get; set; }
    public bool IsFollow { get; set; }

    public CheckGameResponse(bool success, string name)
    {
        Success = success;
        Name = name;
    }
    public CheckGameResponse(bool success, string name, bool owned, bool inWishlist, bool isFollow)
    {
        Success = success;
        Name = name;
        Owned = owned;
        InWishlist = inWishlist;
        IsFollow = isFollow;
    }
}
