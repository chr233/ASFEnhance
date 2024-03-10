using System.Text.Json.Serialization;

namespace ASFEnhance.Data;

internal sealed record AccountHistoryResponse
{
    [JsonPropertyName("html")]
    public string HtmlContent { get; set; } = "";

    [JsonPropertyName("cursor")]
    public CursorData Cursor { get; set; } = new();

    internal sealed record CursorData
    {
        [JsonPropertyName("wallet_txnid")]
        public string WalletTxnid { get; set; } = "";

        [JsonPropertyName("timestamp_newest")]
        public long TimestampNewest { get; set; }

        [JsonPropertyName("balance")]
        public string Balance { get; set; } = "";

        [JsonPropertyName("currency")]
        public int Currency { get; set; }
    }
}
