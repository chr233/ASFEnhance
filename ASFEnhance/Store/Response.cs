namespace ASFEnhance.Store
{
    internal class Response
    {
        //商店信息
        internal sealed class GameStorePageResponse
        {
            internal HashSet<SingleSubData> SubDatas;

            internal string GameName;

            public GameStorePageResponse()
            {
                this.SubDatas = new();
            }

            public GameStorePageResponse(HashSet<SingleSubData> subDatas, string gameName)
            {
                this.SubDatas = subDatas;
                this.GameName = gameName;
            }
        }

        //单个Sub信息
        internal sealed class SingleSubData
        {
            public bool IsBundle;
            public uint SubID;
            public string Name;
            public uint Price;

            public SingleSubData()
            {
                this.Name = "";
            }
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
