using Newtonsoft.Json;

namespace ASFEnhance.Data;

internal sealed record AccountHistoryResponse
{
    [JsonProperty(PropertyName = "html", Required = Required.Always)]
    public string HtmlContent { get; set; } = "";

    [JsonProperty(PropertyName = "cursor", Required = Required.DisallowNull)]
    public CursorData Cursor { get; set; } = new();

    internal sealed record CursorData
    {
        [JsonProperty("wallet_txnid", Required = Required.Always)]
        public string WalletTxnid { get; set; } = "";

        [JsonProperty("timestamp_newest", Required = Required.Always)]
        public long TimestampNewest { get; set; }

        [JsonProperty("balance", Required = Required.Always)]
        public string Balance { get; set; } = "";

        [JsonProperty("currency", Required = Required.Always)]
        public int Currency { get; set; }
    }
}
