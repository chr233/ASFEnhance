namespace ASFEnhance.Data;

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

    internal class SingleSubData(bool bundle = false, uint subId = 0, string name = "", uint price = 0)
    {
        public bool IsBundle = bundle;
        public uint SubId = subId;
        public string Name = name;
        public uint Price = price;
    }
}
