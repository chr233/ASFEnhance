#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using Newtonsoft.Json;

namespace ASFEnhance.Data
{
    internal sealed class AccountHistoryResponse
    {
        [JsonProperty(PropertyName = "html", Required = Required.Always)]
        public string HtmlContent { get; set; }

        [JsonProperty(PropertyName = "cursor", Required = Required.DisallowNull)]
        public CursorData? Cursor { get; set; }

        internal sealed class CursorData
        {
            [JsonProperty("wallet_txnid", Required = Required.Always)]
            public string WalletTxnid { get; set; }

            [JsonProperty("timestamp_newest", Required = Required.Always)]
            public long TimestampNewest { get; set; }

            [JsonProperty("balance", Required = Required.Always)]
            public string Balance { get; set; }

            [JsonProperty("currency", Required = Required.Always)]
            public int Currency { get; set; }
        }
    }

}
