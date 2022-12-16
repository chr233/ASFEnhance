namespace ASFEnhance.IPC.Responses
{
    public sealed class PurchaseResultResponse
    {
        public AddCartResult AddCartResult { get; set; } = new();
        public PurchaseResult PurchaseResult { get; set; } = new();
    }

    public sealed class AddCartResult
    {
        public Dictionary<string, bool> SubIds { get; set; } = new();
        public Dictionary<string, bool> BundleIds { get; set; } = new();
    }

    public sealed class PurchaseResult
    {
        public HashSet<CartItem> CartItems { get; set; } = new();
        public bool Success { get; set; }
        public long Cost { get; set; }
        public string Currency { get; set; } = "";
        public long BalancePrev { get; set; }
        public long BalanceNow { get; set; }
    }

    public sealed class CartItem
    {
        public string Type { get; set; } = "";
        public uint Id { get; set; }
        public string Name { get; set; } = "";
    }
}
