using System.Collections.Generic;

namespace Chrxw.ASFEnhance.Store
{
    internal class Response
    {
        //商店信息
        internal sealed class StoreResponse
        {
            internal List<SubData1> SubData;

            internal string GameName;
            public StoreResponse(List<SubData1> subData, string gameName = "")
            {
                this.SubData = subData;
                this.GameName = gameName;
            }
        }

        //单个Sub信息
        internal sealed class SubData1
        {
            public bool Bundle;
            public uint SubID;
            public string Name;
            public uint Price;
            public SubData1(bool bundle = false, uint subID = 0, string name = "", uint price = 0)
            {
                this.Bundle = bundle;
                this.SubID = subID;
                this.Name = name;
                this.Price = price;
            }
        }
    }
}
