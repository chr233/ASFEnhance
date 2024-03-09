namespace ASFEnhance.Data.Plugin;

/// <summary>
/// 
/// </summary>
public sealed record SteamGameId
{
    /// <summary>
    /// 
    /// </summary>
    public string? Input { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public ESteamGameIdType Type { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="gameId"></param>
    public SteamGameId(ESteamGameIdType type, uint gameId)
    {
        Input = "";
        Type = type;
        Id = gameId;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <param name="type"></param>
    /// <param name="gameId"></param>
    public SteamGameId(string input, ESteamGameIdType type, uint gameId)
    {
        Input = input;
        Type = type;
        Id = gameId;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
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
