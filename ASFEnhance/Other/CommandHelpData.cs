namespace ASFEnhance.Other
{
    internal sealed class CommandHelpData
    {
        internal static readonly Dictionary<string, string> CommandArges = new() {
            // 其他
            { "ASFENHANCE", ""},
            { "KEY", "<Text>"},
            { "ASFEHELP", "ASFEHELP" },
            { "HELP", "<Command>" },
            { "ASFEVERSION", "" },
            { "ASFEUPDATE", "" },
            // 社区
            { "PROFILE", "[Bots]"},
            { "STEAMID", "[Bots]"},
            { "FRIENDCODE", "[Bots]"},
            { "GROUPLIST", "[Bots]"},
            { "JOINGROUP", "[Bots] <GroupName>"},
            { "LEAVEGROUP", "[Bots] <GroupID>"},
            // 愿望单
            { "ADDWISHLIST", "[Bots] <AppIDs>"},
            { "REMOVEWISHLIST", "[Bots] <AppIDs>"},
            // 商店
            { "APPDETAIL", "[Bots] <AppIDS>"},
            { "SEARCH", "[Bots] Keywords"},
            { "SUBS", "[Bots] <AppIDS|SubIDS|BundleIDS>"},
            { "PURCHASEHISTORY", ""},
            { "PUBLISHRECOMMENT", "[Bots] <AppIDS> COMMENT"},
            { "DELETERECOMMENT", "[Bots] <AppIDS>"},
            // 购物车
            { "CART", "[Bots]"},
            { "ADDCART", "[Bots] <SubIDs|BundleIDs>"},
            { "CARTRESET", "[Bots]"},
            { "CARTCOUNTRY", "[Bots]"},
            { "SETCOUNTRY", "[Bots] <CountryCode>"},
            { "PURCHASE", "[Bots]"},
            { "PURCHASEGIFT", "[BotA] BotB"},
        };

        internal static readonly Dictionary<string, string> CommandUsage = new() {
            // 其他
            { "ASFENHANCE", "查看 ASFEnhance 的版本"},
            { "KEY", "从文本提取 key"},
            { "ASFEHELP", "查看全部指令说明" },
            { "HELP", "查看指令说明" },
            { "ASFEVERSION", "检查 ASFEnhance 的最新版本" },
            { "ASFEUPDATE", "更新 ASFEnhance 到最新版本" },
            // 社区
            { "PROFILE", "查看个人资料"},
            { "STEAMID", "查看 steamID"},
            { "FRIENDCODE", "查看好友代码"},
            { "GROUPLIST", "查看机器人的群组列表"},
            { "JOINGROUP", "加入指定群组"},
            { "LEAVEGROUP", "离开指定群组"},
            // 愿望单
            { "ADDWISHLIST", "添加愿望单"},
            { "REMOVEWISHLIST", "移除愿望单"},
            // 商店
            { "APPDETAIL", "获取 APP 信息, 无法获取锁区游戏信息, 仅支持APP"},
            { "SEARCH", "搜索商店"},
            { "SUBS", "查询商店 SUB, 支持APP/SUB/BUNDLE"},
            { "PURCHASEHISTORY", "读取商店消费历史记录"},
            { "PUBLISHRECOMMENT", "发布评测, APPID > 0 给好评, AppID < 0 给差评"},
            { "DELETERECOMMENT", "删除评测"},
            // 购物车
            { "CART", "查看机器人购物车"},
            { "ADDCART", "添加购物车, 仅能使用SubID和BundleID"},
            { "CARTRESET", "清空购物车"},
            { "CARTCOUNTRY", "获取购物车可用结算区域(跟账号钱包和当前 IP 所在地有关)"},
            { "SETCOUNTRY", "购物车改区,可以用CARTCOUNTRY命令获取当前可选的CountryCode(仍然有 Bug)"},
            { "PURCHASE", "结算机器人的购物车, 只能为机器人自己购买 (使用 Steam 钱包余额结算)"},
            { "PURCHASEGIFT", "结算机器人 A 的购物车, 发送礼物给机器人 B (使用 Steam 钱包余额结算)"},
        };

        internal static readonly Dictionary<string, string> ShortCmd2FullCmd = new() {
            { "K", "KEY" },
            { "ASFE", "ASFENHANCE" },
            { "EHELP", "ASFEHELP" },
            { "AV", "ASFEVERSION" },
            { "AU", "ASFEUPDATE" },
            { "PF", "PROFILE" },
            { "SID", "STEAMID" },
            { "FC", "FRIENDCODE" },
            { "GL", "GROUPLIST" },
            { "JG", "JOINGROUP" },
            { "LG", "LEAVEGROUP" },
            { "AW", "ADDWISHLIST" },
            { "RW", "REMOVEWISHLIST" },
            { "AD", "APPDETAIL" },
            { "SS", "SEARCH" },
            { "S", "SUBS" },
            { "PH", "PURCHASEHISTORY" },
            { "PREC", "PUBLISHRECOMMENT" },
            { "DREC", "DELETERECOMMENT" },
            { "C", "CART" },
            { "AC", "ADDCART" },
            { "CR", "CARTRESET" },
            { "CC", "CARTCOUNTRY" },
            { "SC", "SETCOUNTRY" },
            { "PC", "PURCHASE" },
            { "PCG", "PURCHASEGIFT" },
        };

        internal static readonly Dictionary<string, string> FullCmd2ShortCmd = ShortCmd2FullCmd.ToDictionary(x => x.Value, x => x.Key);
    }
}
