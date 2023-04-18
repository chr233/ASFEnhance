namespace ASFEnhance.IPC.Responses
{
    /// <summary>
    /// App信息响应
    /// </summary>
    public sealed class AppDetailDictResponse : Dictionary<string, AppDetail>
    {
    }

    /// <summary>
    /// APP信息
    /// </summary>
    public sealed record AppDetail
    {
        public bool Success { get; set; }
        public uint AppId { get; set; }
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public string Desc { get; set; } = "";
        public bool IsFree { get; set; }
        public bool Released { get; set; }
        public HashSet<SubInfo> Subs { get; set; } = new();
    }

    public sealed record SubInfo
    {
        public uint SubId { get; set; }
        public bool IsFree { get; set; }
        public string Name { get; set; } = "";
    }
}
