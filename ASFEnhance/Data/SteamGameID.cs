namespace ASFEnhance.Data
{
    internal sealed class SteamGameID
    {
        public SteamGameIDType GameType { get; set; }
        public uint GameID { get; set; }

        public SteamGameID(SteamGameIDType type, uint gameID)
        {
            GameType = type;
            GameID = gameID;
        }

        public override string ToString()
        {
            return $"{GameType}/{GameID}";
        }
    }

    internal enum SteamGameIDType : byte
    {
        Error = 0,
        App = 1,
        Sub = 2,
        Bundle = 3,
        Item = 4,
    }
}
