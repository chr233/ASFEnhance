namespace ASFEnhance.Data.Plugin;

/// <summary>
/// 商店信息
/// </summary>
internal sealed record GameStorePageResponse
{
    internal HashSet<SingleSubData> SubDatas;
    internal string GameName;

    public GameStorePageResponse(HashSet<SingleSubData> subDatas, string gameName)
    {
        SubDatas = subDatas;
        GameName = gameName;
    }

    internal class SingleSubData
    {
        public bool IsBundle;
        public uint SubId;
        public string Name;
        public uint Price;

        public SingleSubData(bool bundle = false, uint subId = 0, string name = "", uint price = 0)
        {
            IsBundle = bundle;
            SubId = subId;
            Name = name;
            Price = price;
        }
    }
}
