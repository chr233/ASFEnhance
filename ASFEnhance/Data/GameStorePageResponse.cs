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
        this.SubDatas = subDatas;
        this.GameName = gameName;
    }

    internal class SingleSubData
    {
        public bool IsBundle;
        public uint SubId;
        public string Name;
        public uint Price;

        public SingleSubData(bool bundle = false, uint subId = 0, string name = "", uint price = 0)
        {
            this.IsBundle = bundle;
            this.SubId = subId;
            this.Name = name;
            this.Price = price;
        }
    }
}
