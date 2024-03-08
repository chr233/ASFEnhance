namespace ASFEnhance.Data;

internal sealed record SteamGameId
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

[Flags]
internal enum ESteamGameIdType : byte
{
    Error = 0,
    App = 1,
    Sub = 2,
    Bundle = 4,
    All = App | Sub | Bundle,
}
