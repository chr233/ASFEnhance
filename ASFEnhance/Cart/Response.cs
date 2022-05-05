namespace ASFEnhance.Cart
{
    internal class Response
    {
        //购物车信息
        internal sealed class CartResponse
        {
            //购物车列表
            public HashSet<CartData> CardDatas;
            //购物车总价
            public int TotalPrice;
            //是否能为自己购买
            public bool PurchaseForSelf;
            //是否能作为礼物购买
            public bool PurchaseAsGift;

            public CartResponse()
            {
                this.CardDatas = new();
            }

            public CartResponse(HashSet<CartData> cartData, int totalPrice, bool purchaseSelf, bool purchaseGift)
            {
                this.CardDatas = cartData;
                this.TotalPrice = totalPrice;
                this.PurchaseForSelf = purchaseSelf;
                this.PurchaseAsGift = purchaseGift;
            }
        }

        //单个购物车项目
        internal sealed class CartData
        {
            //游戏路径(sub/xxx,app/xxx)
            public string Path;
            public string Name;
            public int Price;
            public CartData()
            {
            }
            public CartData(string path, string name, int price)
            {
                this.Name = name;
                this.Path = path;
                this.Price = price;
            }
        }
    }
}
