namespace ASFEnhance.Other
{
    internal static class Response
    {
        internal static readonly Dictionary<string, string> CommandUsage = new() {
            // 其他
            { "ASFENHANCE", "ASFENHANCE"},
            { "KEY", "KEY <Text>"},
            // 社区
            { "PROFILE", "PROFILE [Bots]"},
            { "STEAMID", "STEAMID [Bots]"},
            { "FRIENDCODE", "FRIENDCODE [Bots]"},
            { "GROUPLIST", "GROUPLIST [Bots]"},
            { "JOINGROUP", "JOINGROUP [Bots] <GroupName>"},
            { "LEAVEGROUP [Bots] <GroupID>", "LEAVEGROUP [Bots] <GroupID>"},
            // 愿望单
            { "ADDWISHLIST", "ADDWISHLIST [Bots] <AppIDs>"},
            { "REMOVEWISHLIST", "REMOVEWISHLIST [Bots] <AppIDs>"},
            // 商店
            { "APPDETAIL", "APPDETAIL [Bots] <AppIDS>"},
            { "SEARCH", "SEARCH [Bots] Keywords"},
            { "SUBS", "SUBS [Bots] <AppIDS|SubIDS|BundleIDS>"},
            { "PURCHASEHISTORY", "PURCHASEHISTORY"},
            { "PUBLISHRECOMMENT", "PUBLISHRECOMMENT [Bots] <AppIDS> COMMENT"},
            { "DELETERECOMMENT", "DELETERECOMMENT [Bots] <AppIDS>"},
            // 购物车
            { "CART", "CART [Bots]"},
            { "ADDCART", "ADDCART [Bots] <SubIDs|BundleIDs>"},
            { "CARTRESET", "CARTRESET [Bots]"},
            { "CARTCOUNTRY", "CARTCOUNTRY [Bots]"},
            { "SETCOUNTRY", "SETCOUNTRY [Bots] <CountryCode>"},
            { "PURCHASE", "PURCHASE [Bots]"},
            { "PURCHASEGIFT", "PURCHASEGIFT [BotA] BotB"},
            // 调试
            { "COOKIES", "COOKIES [Bots]"},
            { "APIKEY", "APIKEY [Bots]"},
            { "ACCESSTOKEN", "ACCESSTOKEN [Bots]"},
        };

        internal static readonly Dictionary<string, string> CommandShortcut = new() {
            { "AV", "ASFEVERSION" },
        };
    }
}
