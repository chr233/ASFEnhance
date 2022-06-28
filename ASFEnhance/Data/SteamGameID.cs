namespace ASFEnhance.Data
{
    internal sealed class SteamGameID
    {
        public string Input { get; set; }
        public SteamGameIDType Type { get; set; }
        public uint GameID { get; set; }

        public SteamGameID(SteamGameIDType type, uint gameID)
        {
            Input = "";
            Type = type;
            GameID = gameID;
        }
        public SteamGameID(string input, SteamGameIDType type, uint gameID)
        {
            Input = input;
            Type = type;
            GameID = gameID;
        }

        public override string ToString()
        {
            return $"{Type}/{GameID}";
        }
    }

    [Flags]
    internal enum SteamGameIDType : byte
    {
        Error = 0,
        App = 1,
        Sub = 2,
        Bundle = 4,
        All = App | Sub | Bundle,
    }
}
