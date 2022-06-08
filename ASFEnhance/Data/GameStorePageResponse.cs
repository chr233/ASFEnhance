namespace ASFEnhance.Data
{
    /// <summary>
    /// 商店信息
    /// </summary>
    internal class GameStorePageResponse
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
            public uint SubID;
            public string Name;
            public uint Price;

            public SingleSubData(bool bundle = false, uint subID = 0, string name = "", uint price = 0)
            {
                this.IsBundle = bundle;
                this.SubID = subID;
                this.Name = name;
                this.Price = price;
            }
        }
    }
}
