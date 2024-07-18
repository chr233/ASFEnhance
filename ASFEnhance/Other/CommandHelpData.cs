using System.Collections.ObjectModel;

namespace ASFEnhance.Other;

internal static class CommandHelpData
{
    /// <summary>
    /// 命令参数
    /// </summary>
    internal static ReadOnlyDictionary<string, string> CommandArges { get; } = new Dictionary<string, string>() {
        // 更新
        { "ASFENHANCE", "" },
        { "PLUGINLIST", "" },
        { "PLUGINVERSION", "[<Plugin Name>]" },
        { "PLUGINUPDATE", "[<Plugin Name>]" },
        
        // 账号
        { "PURCHASEHISTORY", "[Bots]" },
        { "FREELICENSES", "[Bots]" },
        { "LICENSES", "[Bots]" },
        { "REMOVEDEMOS", "[Bots]" },
        { "REMOVELICENSES", "[Bots] <SubIds>" },
        { "EMAILOPTIONS", "[Bots]" },
        { "SETEMAILOPTIONS", "[Bots] <Options>" },
        
        // 其他
        { "KEY", "<Text>" },
        { "ASFEHELP", "[Command]" },
        { "EHELP", "[Command]" },
        
        // 群组
        { "GROUPLIST", "[Bots]" },
        { "JOINGROUP", "[Bots] <GroupName>" },
        { "LEAVEGROUP", "[Bots] <GroupId>" },
        
        // 个人资料
        { "PROFILE", "[Bots]" },
        { "PROFILELINK", "[Bots]" },
        { "STEAMID", "[Bots]" },
        { "FRIENDCODE", "[Bots]" },
        { "TRADELINK", "[Bots]" },
        { "REPLAY", "[Bots]" },
        { "REPLAYPRIVACY", "[Bots] Privacy" },
        { "CLEARALIAS", "[Bots]" },
        { "GAMEAVATAR", "[Bots] AppId [AvatarID]" },
        { "RANDOMGAMEAVATAR", "[Bots]" },
        { "ADVNICKNAME", "[Bots] Query" },
        { "SETAVATAR", "[Bots] ImageUrl" },
        { "DELETEAVATAR", "[Bots]" },
        { "CRAFTBADGE", "[Bots]" },
        
        // 鉴赏家
        { "CURATORLIST", "[Bots]" },
        { "FOLLOWCURATOR", "[Bots] <ClanIds>" },
        { "UNFOLLOWCURATOR", "[Bots] <ClanIds>" },
        { "UNFOLLOWALLCURATORS", "[Bots]" },
  
        // 愿望单
        { "ADDWISHLIST", "[Bots] <AppIds>" },
        { "REMOVEWISHLIST", "[Bots] <AppIds>" },
        { "FOLLOWGAME", "[Bots] <AppIds>" },
        { "UNFOLLOWGAME", "[Bots] <AppIds>" },
        { "CHECK", "[Bots] <AppIds>" },

        // 商店
        { "APPDETAIL", "[Bots] <AppIds>" },
        { "SEARCH", "[Bots] Keywords" },
        { "SUBS", "[Bots] <AppIds|SubIds|BundleIds>" },
        { "PUBLISHRECOMMENT", "[Bots] <AppIds> COMMENT" },
        { "DELETERECOMMENT", "[Bots] <AppIds>" },
        { "REQUESTACCESS", "[Bots] <AppIDs>" },
        { "VIEWPAGE", "[Bots] Url" },
        
        // 购物车
        { "CART", "[Bots]" },
        { "ADDCART", "[Bots] <SubIds|BundleIds>" },
        { "CARTRESET", "[Bots]" },
        { "CARTCOUNTRY", "[Bots]" },
        { "FAKEPURCHASE", "[Bots]" },
        { "PURCHASE", "[Bots]" },
        
        // 社区
        { "CLEARNOTIFICATION", "[Bots]" },
        
        // 探索队列
        { "EXPLORER", "[Bots]" },
        
        // 好友
        { "ADDBOTFRIEND", "[BotAs] <BotBs>" },
        { "ADDFRIEND", "[Bots] <Text>" },
        { "DELETEFRIEND", "[Bots] <Text>" },
        { "DELETEALLFRIEND", "[Bots]" },
        
        // 钱包
        { "REDEEMWALLET", "[Bots] <keys>" },
        { "REDEEMWALLETMULT", "[Bots] <keys>" },
    }.AsReadOnly();

    /// <summary>
    /// 命令说明
    /// </summary>
    internal static ReadOnlyDictionary<string, string> CommandUsage { get; } = new Dictionary<string, string>() {
        // 更新
        { "ASFENHANCE", Langs.UsageASFENHANCE},
        { "PLUGINLIST", "" },
        { "PLUGINVERSION", "" },
        { "PLUGINUPDATE", "" },
        
        // 账号
        { "PURCHASEHISTORY", Langs.UsagePURCHASEHISTORY },
        { "FREELICENSES", Langs.UsageFREELICENSES },
        { "LICENSES", Langs.UsageLICENSES },
        { "REMOVEDEMOS", Langs.UsageREMOVEDEMOS },
        { "REMOVELICENSES", Langs.UsageREMOVELICENSES },
        { "EMAILOPTIONS", Langs.UsageEMAILOPTION },
        { "SETEMAILOPTIONS", Langs.UsageSETEMAILOPTION },
        
        // 其他
        { "KEY", Langs.UsageKEY },
        { "ASFEHELP", Langs.UsageASFEHELP },
        { "EHELP", Langs.UsageASFEHELP },
        
         // 群组
        { "GROUPLIST", Langs.UsageGROUPLIST },
        { "JOINGROUP", Langs.UsageJOINGROUP },
        { "LEAVEGROUP", Langs.UsageLEAVEGROUP },
        
        // 个人资料
        { "PROFILE", Langs.UsagePROFILE },
        { "PROFILELINK", Langs.UsagePROFILELINK },
        { "STEAMID", Langs.UsageSTEAMID },
        { "FRIENDCODE", Langs.UsageFRIENDCODE },
        { "TRADELINK", Langs.UsageTRADELINK },
        { "REPLAY", Langs.UsageREPLAY },
        { "REPLAYPRIVACY", Langs.UsageREPLAYPRIVACY },
        { "CLEARALIAS", Langs.UsageCLEARALIAS },
        { "GAMEAVATAR", Langs.UsageGAMEAVATAR },
        { "RANDOMGAMEAVATAR", Langs.UsageRANDOMGAMEAVATAR },
        { "ADVNICKNAME", Langs.UsageADVNICKNAME },
        { "SETAVATAR", Langs.UsageSETAVATAR },
        { "DELETEAVATAR", Langs.UsageDELETEAVATAR },
        { "CRAFTBADGE", Langs.UsageCRAFTBADGE },
       
        // 鉴赏家
        { "CURATORLIST", Langs.UsageCURATORLIST },
        { "FOLLOWCURATOR", Langs.UsageFOLLOWCURATOR },
        { "UNFOLLOWCURATOR", Langs.UsageUNFOLLOWCURATOR },
        { "UNFOLLOWALLCURATORS", Langs.UsageUNFOLLOWALLCURATORS },
        
        // 愿望单
        { "ADDWISHLIST", Langs.UsageADDWISHLIST },
        { "REMOVEWISHLIST", Langs.UsageREMOVEWISHLIST },
        { "FOLLOWGAME", Langs.UsageFOLLOWGAME },
        { "UNFOLLOWGAME", Langs.UsageUNFOLLOWGAME },
        { "CHECK", Langs.UsageCHECK },
        
        // 商店
        { "APPDETAIL", Langs.UsageAPPDETAIL },
        { "SEARCH", Langs.UsageSEARCH },
        { "SUBS", Langs.UsageSUBS },
        { "PUBLISHRECOMMENT", Langs.UsagePUBLISHRECOMMENT },
        { "DELETERECOMMENT", Langs.UsageDELETERECOMMENT },
        { "REQUESTACCESS", Langs.UsageREQUESTACCESS },
        { "VIEWPAGE", "" },
        
        // 购物车
        { "CART", Langs.UsageCART },
        { "ADDCART", Langs.UsageADDCART },
        { "CARTRESET", Langs.UsageCARTRESET },
        { "CARTCOUNTRY", Langs.UsageCARTCOUNTRY },
        { "FAKEPURCHASE", Langs.UsageFAKEPURCHASE },
        { "PURCHASE", Langs.UsagePURCHASE },
        
        // 社区
        { "CLEARNOTIFICATION", Langs.UsageCLEARNOTIFICATION },
        
        // 探索队列
        { "EXPLORER", Langs.UsageEXPLORER },
        
        // 好友
        { "ADDBOTFRIEND", Langs.UsageADDBOTFRIEND },
        { "ADDFRIEND", Langs.UsageADDFRIEND },
        { "DELETEFRIEND", Langs.UsageDELETEFRIEND },
        { "DELETEALLFRIEND", Langs.UsageDELETEALLFRIEND },

        // 钱包
        { "REDEEMWALLET", Langs.UsageREDEEMWALLET },
        { "REDEEMWALLETMULT", Langs.UsageREDEEMWALLETMULT },
    }.AsReadOnly();

    /// <summary>
    /// 命令缩写
    /// </summary>
    internal static ReadOnlyDictionary<string, string> ShortCmd2FullCmd { get; } = new Dictionary<string, string>() {
        // 更新
        { "ASFE", "ASFENHANCE" },
        { "PL", "PLUGINLIST" },
        { "PV", "PLUGINVERSION" },
        { "PU", "PLUGINUPDATE" },
        
        // 账号
        { "PH", "PURCHASEHISTORY" },
        { "FL", "FREELICENSES" },
        { "L", "LICENSES" },
        { "RD", "REMOVEDEMOS" },
        { "RL", "REMOVELICENSES" },
        { "EO", "EMAILOPTIONS" },
        { "SEO", "SETEMAILOPTIONS" },
        
        // 其他
        { "K", "KEY" },
        { "EHELP", "ASFEHELP" },
        { "HELP", "EHELP" },

        // 群组
        { "GL", "GROUPLIST" },
        { "JG", "JOINGROUP" },
        { "LG", "LEAVEGROUP" },
        
        // 个人资料
        { "PF", "PROFILE" },
        { "PFL", "PROFILELINK" },
        { "SID", "STEAMID" },
        { "FC", "FRIENDCODE" },
        { "TL", "TRADELINK" },
        { "RP", "REPLAY" },
        { "RPP", "REPLAYPRIVACY" },
        { "GA", "GAMEAVATAR"},
        { "RGA", "RANDOMGAMEAVATAR"},
        { "ANN", "ADVNICKNAME"},
        { "SEA", "SETAVATAR" },
        { "CB", "CRAFTBADGE" },
        
        // 鉴赏家
        { "CL", "CURATORLIST" },
        { "FCU", "FOLLOWCURATOR" },
        { "UFCU", "UNFOLLOWCURATOR" },
        { "UFACU", "UNFOLLOWALLCURATORS" },
        
        // 愿望单
        { "AW", "ADDWISHLIST" },
        { "RW", "REMOVEWISHLIST" },
        { "FG", "FOLLOWGAME" },
        { "UFG", "UNFOLLOWGAME" },
        { "CK", "CHECK" },
        
        // 商店
        { "AD", "APPDETAIL" },
        { "SS", "SEARCH" },
        { "S", "SUBS" },
        { "PREC", "PUBLISHRECOMMENT" },
        { "DREC", "DELETERECOMMENT" },
        { "RQ", "REQUESTACCESS" },
        { "VP", "VIEWPAGE" },
        
        // 购物车
        { "C", "CART" },
        { "AC", "ADDCART" },
        { "CR", "CARTRESET" },
        { "CC", "CARTCOUNTRY" },
        { "FPC", "FAKEPURCHASE" },
        { "PC", "PURCHASE" },
        { "PCG", "PURCHASEGIFT" },
        
        // 社区
        { "CN", "CLEARNOTIFICATION" },
        
        // 探索队列
        { "EX", "EXPLORER" },
        
        // 好友
        { "ABF", "ADDBOTFRIEND" },
        { "AF", "ADDFRIEND" },
        { "DF", "DELETEFRIEND" },
        
         // 钱包
        { "RWA", "REDEEMWALLET" },
        { "RWAM", "REDEEMWALLETMULT" },
    }.AsReadOnly();

    /// <summary>
    /// 命令缩写转全称
    /// </summary>
    internal static readonly ReadOnlyDictionary<string, string> FullCmd2ShortCmd = ShortCmd2FullCmd.ToDictionary(x => x.Value, x => x.Key).AsReadOnly();
}
