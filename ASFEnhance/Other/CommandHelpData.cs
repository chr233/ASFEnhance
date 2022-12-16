using ASFEnhance.Localization;

namespace ASFEnhance.Other
{
    internal static class CommandHelpData
    {
        /// <summary>
        /// 命令参数
        /// </summary>
        internal static Dictionary<string, string> CommandArges { get; } = new() {
            // 更新
            { "ASFENHANCE", "" },
            { "ASFEVERSION", "" },
            { "ASFEUPDATE", "" },
            
            // 账号
            { "PURCHASEHISTORY", "[Bots]" },
            { "FREELICENSES", "[Bots]" },
            { "LICENSES", "[Bots]" },
            { "REMOVEDEMOS", "[Bots]" },
            { "REMOVELICENSES", "[Bots] <SubIDs>" },
            { "EMAILOPTION", "[Bots]" },
            { "SETEMAILOPTION", "[Bots] <Options>" },
            
            // 购物车
            { "ADDCART", "[Bots] <SubIDs|BundleIDs>" },
            { "CART", "[Bots]" },
            { "CARTRESET", "[Bots]" },
            { "CARTCOUNTRY", "[Bots]" },
            { "SETCOUNTRY", "[Bots] <CountryCode>" },
            { "PURCHASE", "[Bots]" },
            { "PURCHASEGIFT", "[BotA] BotB" },
            
            // 鉴赏家
            { "CURATORLIST", "[Bots]" },
            { "FOLLOWCURATOR", "[Bots] <ClanIDs>" },
            { "UNFOLLOWCURATOR", "[Bots] <ClanIDs>" },
            
            // 探索队列
            { "EXPLORER", "[Bots]" },
            
            // 群组
            { "GROUPLIST", "[Bots]" },
            { "JOINGROUP", "[Bots] <GroupName>" },
            { "LEAVEGROUP", "[Bots] <GroupID>" },
            
            // 其他
            { "KEY", "<Text>" },
            { "ASFEHELP", "[Command]" },
            { "EHELP", "[Command]" },
            
            // 个人资料
            { "FRIENDCODE", "[Bots]" },
            { "STEAMID", "[Bots]" },
            { "PROFILE", "[Bots]" },
            { "PROFILELINK", "[Bots]" },
            { "TRADELINK", "[Bots]" },
            
            // 商店
            { "APPDETAIL", "[Bots] <AppIDS>" },
            { "DELETERECOMMENT", "[Bots] <AppIDS>" },
            { "PUBLISHRECOMMENT", "[Bots] <AppIDS> COMMENT" },
            { "SEARCH", "[Bots] Keywords" },
            { "SUBS", "[Bots] <AppIDS|SubIDS|BundleIDS>" },
            
            // 愿望单
            { "ADDWISHLIST", "[Bots] <AppIDs>" },
            { "CHECK", "[Bots] <AppIDs>" },
            { "FOLLOWGAME", "[Bots] <AppIDs>" },
            { "REMOVEWISHLIST", "[Bots] <AppIDs>" },
            { "UNFOLLOWGAME", "[Bots] <AppIDs>" },
        };

        /// <summary>
        /// 命令说明
        /// </summary>
        internal static Dictionary<string, string> CommandUsage { get; } = new() {
            // 更新
            { "ASFENHANCE", Langs.UsageASFENHANCE},
            { "ASFEVERSION", Langs.UsageASFEVERSION },
            { "ASFEUPDATE", Langs.UsageASFEUPDATE },
            
            // 账号
            { "PURCHASEHISTORY", Langs.UsagePURCHASEHISTORY },
            { "FREELICENSES", Langs.UsageFREELICENSES },
            { "LICENSES", Langs.UsageLICENSES },
            { "REMOVEDEMOS", Langs.UsageREMOVEDEMOS },
            { "REMOVELICENSES", Langs.UsageREMOVELICENSES },
            { "EMAILOPTION", Langs.UsageEMAILOPTION },
            { "SETEMAILOPTION", Langs.UsageSETEMAILOPTION },
            
            // 购物车
            { "ADDCART", Langs.UsageADDCART },
            { "CART", Langs.UsageCART },
            { "CARTCOUNTRY", Langs.UsageCARTCOUNTRY },
            { "CARTRESET", Langs.UsageCARTRESET },
            { "SETCOUNTRY", Langs.UsageSETCOUNTRY },
            { "PURCHASE", Langs.UsagePURCHASE },
            { "PURCHASEGIFT", Langs.UsagePURCHASEGIFT },
            
            // 鉴赏家
            { "CURATORLIST", Langs.UsageCURATORLIST },
            { "FOLLOWCURATOR", Langs.UsageFOLLOWCURATOR },
            { "UNFOLLOWCURATOR", Langs.UsageUNFOLLOWCURATOR },
            
            // 探索队列
            { "EXPLORER", Langs.UsageEXPLORER },
            
            // 群组
            { "GROUPLIST", Langs.UsageGROUPLIST },
            { "JOINGROUP", Langs.UsageJOINGROUP },
            { "LEAVEGROUP", Langs.UsageLEAVEGROUP },
            
            // 其他
            { "KEY", Langs.UsageKEY },
            { "ASFEHELP", Langs.UsageASFEHELP },
            { "EHELP", Langs.UsageASFEHELP },

            // 个人资料
            { "FRIENDCODE", Langs.UsageFRIENDCODE },
            { "STEAMID", Langs.UsageSTEAMID },
            { "PROFILE", Langs.UsagePROFILE },
            { "PROFILELINK", Langs.UsagePROFILELINK },
            { "TRADELINK", Langs.UsageTRADELINK },
            
            // 商店
            { "APPDETAIL", Langs.UsageAPPDETAIL },
            { "DELETERECOMMENT", Langs.UsageDELETERECOMMENT },
            { "PUBLISHRECOMMENT", Langs.UsagePUBLISHRECOMMENT },
            { "REQUESTACCESS", Langs.UsageREQUESTACCESS },
            { "SEARCH", Langs.UsageSEARCH },
            { "SUBS", Langs.UsageSUBS },
            
            // 愿望单
            { "ADDWISHLIST", Langs.UsageADDWISHLIST },
            { "CHECK", Langs.UsageCHECK },
            { "FOLLOWGAME", Langs.UsageFOLLOWGAME },
            { "REMOVEWISHLIST", Langs.UsageREMOVEWISHLIST },
            { "UNFOLLOWGAME", Langs.UsageUNFOLLOWGAME },
        };

        /// <summary>
        /// 命令缩写
        /// </summary>
        internal static Dictionary<string, string> ShortCmd2FullCmd { get; } = new() {
            // 更新
            { "ASFE", "ASFENHANCE" },
            { "AV", "ASFEVERSION" },
            { "AU", "ASFEUPDATE" },
            
            // 账号
            { "PH", "PURCHASEHISTORY" },
            { "FL", "FREELICENSES" },
            { "L", "LICENSES" },
            { "RD", "REMOVEDEMOS" },
            { "RL", "REMOVELICENSES" },
            { "EO", "EMAILOPTION" },
            { "SEO", "SETEMAILOPTION" },
            
            // 购物车
            { "C", "CART" },
            { "AC", "ADDCART" },
            { "CR", "CARTRESET" },
            { "CC", "CARTCOUNTRY" },
            { "SC", "SETCOUNTRY" },
            { "PC", "PURCHASE" },
            { "PCG", "PURCHASEGIFT" },
            
            // 鉴赏家
            { "CL", "CURATORLIST" },
            { "FCU", "FOLLOWCURATOR" },
            { "UFCU", "UNFOLLOWCURATOR" },
            
            // 探索队列
            { "EX", "EXPLORER" },
            
            // 群组
            { "GL", "GROUPLIST" },
            { "JG", "JOINGROUP" },
            { "LG", "LEAVEGROUP" },
            
            // 其他
            { "K", "KEY" },
            { "EHELP", "ASFEHELP" },
            { "HELP", "EHELP" },
            
            // 个人资料
            { "FC", "FRIENDCODE" },
            { "SID", "STEAMID" },
            { "PF", "PROFILE" },
            { "PFL", "PROFILELINK" },
            { "TL", "TRADELINK" },
            
            // 商店
            { "AD", "APPDETAIL" },
            { "DREC", "DELETERECOMMENT" },
            { "PREC", "PUBLISHRECOMMENT" },
            { "SS", "SEARCH" },
            { "S", "SUBS" },
            
            // 愿望单
            { "AW", "ADDWISHLIST" },
            { "CK", "CHECK" },
            { "FG", "FOLLOWGAME" },
            { "RW", "REMOVEWISHLIST" },
            { "UFG", "UNFOLLOWGAME" },
        };

        /// <summary>
        /// 命令缩写转全称
        /// </summary>
        internal static readonly Dictionary<string, string> FullCmd2ShortCmd = ShortCmd2FullCmd.ToDictionary(x => x.Value, x => x.Key);
    }
}
