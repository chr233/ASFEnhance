namespace ASFEnhance.Data;

internal sealed record SteamGameId
{
    public string Input { get; set; }
    public SteamGameIdType Type { get; set; }
    public uint GameId { get; set; }

    public SteamGameId(SteamGameIdType type, uint gameId)
    {
        Input = "";
        Type = type;
        GameId = gameId;
    }
    public SteamGameId(string input, SteamGameIdType type, uint gameId)
    {
        Input = input;
        Type = type;
        GameId = gameId;
    }

    public override string ToString()
    {
        return $"{Type}/{GameId}";
    }
}

[Flags]
internal enum SteamGameIdType : byte
{
    Error = 0,
    App = 1,
    Sub = 2,
    Bundle = 4,
    All = App | Sub | Bundle,
}
