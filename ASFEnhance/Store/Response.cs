namespace ASFEnhance.Store
{
    internal class Response
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
        }

        /// <summary>
        /// 单个Sub信息
        /// </summary>
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

        internal class TotalSpendResponse
        {
            /// <summary>单位USD</summary>
            public int TotalSpend;
            /// <summary>单位USD</summary>
            public int OldSpend;
            /// <summary>单位USD</summary>
            public int PWSpend;
            /// <summary>单位RMB</summary>
            public int ChinaSpend;
        }

        /// <summary>
        /// 账户历史数据返回值
        /// </summary>
        internal class HistoryParseResponse
        {
            /// <summary>未知</summary>
            public int Unknown;
            /// <summary>商店购买</summary>
            public int StorePurchase;
            /// <summary>商店购买[余额]</summary>
            public int StorePurchaseWallet;
            /// <summary>礼物购买</summary>
            public int GiftPurchase;
            /// <summary>礼物购买[余额]</summary>
            public int GiftPurchaseWallet;
            /// <summary>市场购买</summary>
            public int MarketPurchase;
            /// <summary>市场出售</summary>
            public int MarketSelling;
            /// <summary>游戏内购</summary>
            public int InGamePurchase;
            /// <summary>退款</summary>
            public int RefundPurchase;
            /// <summary>退款[余额]</summary>
            public int RefundPurchaseWallet;
            /// <summary>其他</summary>
            public int Other;

            public static HistoryParseResponse operator +(HistoryParseResponse a, HistoryParseResponse b)
            {
                HistoryParseResponse result = new() {
                    Unknown = a.Unknown + b.Unknown,
                    StorePurchase = a.StorePurchase + b.StorePurchase,
                    StorePurchaseWallet = a.StorePurchaseWallet + b.StorePurchaseWallet,
                    GiftPurchase = a.GiftPurchase + b.GiftPurchase,
                    GiftPurchaseWallet = a.GiftPurchaseWallet + b.GiftPurchaseWallet,
                    MarketPurchase = a.MarketPurchase + b.MarketPurchase,
                    MarketSelling = a.MarketSelling + b.MarketSelling,
                    InGamePurchase = a.InGamePurchase + b.InGamePurchase,
                    RefundPurchase = a.RefundPurchase + b.RefundPurchase,
                    RefundPurchaseWallet = a.RefundPurchaseWallet + b.RefundPurchaseWallet,
                    Other = a.Other + b.Other,
                };
                return result;
            }
        }
    }
}
