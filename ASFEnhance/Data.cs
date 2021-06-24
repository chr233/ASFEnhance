using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chrxw.ASFEnhance
{
    internal class Data
    {
        //购物车信息
        internal struct CartResponse
        {
            //购物车列表
            public List<CartData>? cartData;
            //购物车总价
            public uint totalPrice;
            //是否能为自己购买
            public bool purchaseSelf;
            //是否能作为礼物购买
            public bool purchaseGift;
            public CartResponse(List<CartData> cartData = null, uint totalPrice = 0, bool purchaseSelf = false, bool purchaseGift = false)
            {
                this.cartData = cartData;
                this.totalPrice = totalPrice;
                this.purchaseSelf = purchaseSelf;
                this.purchaseGift = purchaseGift;
            }
        }

        //单个购物车项目
        internal struct CartData
        {
            //游戏路径(sub/xxx,app/xxx)
            public string path;
            public string name;
            public uint price;
            public CartData(string path = "", string name = "", uint price = 0)
            {
                this.name = name;
                this.path = path;
                this.price = price;
            }
        }

        //商店信息
        internal struct StoreResponse
        {
            //Sub列表
            public List<SubData>? subData;
            //游戏名称
            public string gameName;
            public StoreResponse(List<SubData> subData = null, string gameName = "")
            {
                this.subData = subData;
                this.gameName = gameName;
            }
        }

        //Sub信息
        internal struct SubData
        {
            public uint subID;
            public string name;
            public uint price;
            public SubData(uint subID = 0, string name = "", uint price = 0)
            {
                this.subID = subID;
                this.name = name;
                this.price = price;
            }
        }
    }
}
