namespace ASFEnhance.Data
{
    internal sealed class SteamGameID
    {
        internal SteamGameIDType GameType { get; set; }
        internal uint GameID { get; set; }



        internal SteamGameID(SteamGameIDType type, uint gameID)
        {
            GameType = type;
            GameID = gameID;
        }
    }


    internal enum SteamGameIDType
    {
        Error = 0,
        App = 1,
        Sub = 2,
        Bundle = 3,
        Item = 4,
    }
}
