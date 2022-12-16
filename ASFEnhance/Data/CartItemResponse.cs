namespace ASFEnhance.Data
{
    /// <summary>
    /// 购物车信息
    /// </summary>
    internal sealed record CartItemResponse
    {
        /// <summary>
        /// 购物车列表
        /// </summary>
        public HashSet<CartItem> CartItems { get; set; }
        /// <summary>
        /// 购物车总价
        /// </summary>
        public int TotalPrice { get; set; }
        /// <summary>
        /// 是否能为自己购买
        /// </summary>
        public bool PurchaseForSelf { get; set; }
        /// <summary>
        /// 是否能作为礼物购买
        /// </summary>
        public bool PurchaseAsGift { get; set; }

        public CartItemResponse()
        {
            CartItems = new();
        }

        public CartItemResponse(HashSet<CartItem> cartItems, int totalPrice, bool purchaseSelf, bool purchaseGift)
        {
            CartItems = cartItems;
            TotalPrice = totalPrice;
            PurchaseForSelf = purchaseSelf;
            PurchaseAsGift = purchaseGift;
        }

        /// <summary>
        /// 单个购物车项目
        /// </summary>
        internal sealed record CartItem
        {
            public SteamGameId GameId { get; set; }
            public string Name { get; set; }
            public int Price { get; set; }
            public CartItem(SteamGameId gameId, string name, int price)
            {
                Name = name;
                GameId = gameId;
                Price = price;
            }
        }
    }

}
