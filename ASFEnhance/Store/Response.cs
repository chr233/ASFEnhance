using System.Collections.Generic;

namespace Chrxw.ASFEnhance.Store
{
    internal class Response
    {
        //商店信息
        internal class StoreResponse
        {
            //Sub列表
            public List<SubData> subData;
            //游戏名称
            public string gameName;
            public StoreResponse(List<SubData> subData = null, string gameName = "")
            {
                this.subData = subData ?? new();
                this.gameName = gameName;
            }
        }

        //单个Sub信息
        internal struct SubData
        {
            public bool bundle;
            public uint subID;
            public string name;
            public uint price;
            public SubData(bool bundle = false, uint subID = 0, string name = "", uint price = 0)
            {
                this.bundle = bundle;
                this.subID = subID;
                this.name = name;
                this.price = price;
            }
        }
    }
}
