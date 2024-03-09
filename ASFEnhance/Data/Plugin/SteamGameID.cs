namespace ASFEnhance.Data.Plugin;

public sealed record SteamGameId
{
    public string? Input { get; set; }
    public ESteamGameIdType Type { get; set; }
    public uint Id { get; set; }

    public SteamGameId(ESteamGameIdType type, uint gameId)
    {
        Input = "";
        Type = type;
        Id = gameId;
    }
    public SteamGameId(string input, ESteamGameIdType type, uint gameId)
    {
        Input = input;
        Type = type;
        Id = gameId;
    }

    public override string ToString()
    {
        return $"{Type}/{Id}";
    }
}

/// <summary>
/// Id类型
/// </summary>
[Flags]
public enum ESteamGameIdType : byte
{
    /// <summary>
    /// 错误
    /// </summary>
    Error = 0,
    /// <summary>
    /// 应用
    /// </summary>
    App = 1,
    /// <summary>
    /// Sub
    /// </summary>
    Sub = 2,
    /// <summary>
    /// 捆绑包
    /// </summary>
    Bundle = 4,
    /// <summary>
    /// 所有
    /// </summary>
    All = App | Sub | Bundle,
}
