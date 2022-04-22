using System;
using System.Collections.Generic;

namespace Chrxw.ASFEnhance.Cart
{
    internal class Response
    {
        //购物车信息
        internal sealed class CartResponse
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
        internal sealed class CartData : IEquatable<CartData>
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
            public bool Equals(CartData other)
            {
                return this.path.ToLowerInvariant() == other.path.ToLowerInvariant();
            }
        }
    }
}
