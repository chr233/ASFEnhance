namespace ASFEnhance.Data
{
    internal sealed class SteamGameID
    {
        internal SteamGameIDType Type { get; set; }
        internal uint GameID { get; set; }



        internal SteamGameID(SteamGameIDType type, uint gameID)
        {
            Type = type;
            GameID = gameID;
        }
    }


    internal enum SteamGameIDType
    {
        Error = 0,
        App = 1,
        Sub = 2,
        Bundle = 3,
    }
}
