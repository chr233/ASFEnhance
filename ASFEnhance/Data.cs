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
        internal class CartResponse
        {
            //购物车列表
            public List<CartData> cartData;
            //购物车总价
            public int totalPrice;
            //是否能为自己购买
            public bool purchaseSelf;
            //是否能作为礼物购买
            public bool purchaseGift;
            public CartResponse(List<CartData> cartData = null, int totalPrice = 0, bool purchaseSelf = false, bool purchaseGift = false)
            {
                this.cartData = cartData ?? new();
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
            public int price;
            public CartData(string path = "", string name = "", int price = 0)
            {
                this.name = name;
                this.path = path;
                this.price = price;
            }
        }

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
