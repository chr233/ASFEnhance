using ArchiSteamFarm.Core;
using ArchiSteamFarm.Helpers.Json;
using ArchiSteamFarm.Plugins.Interfaces;
using ArchiSteamFarm.Steam;
using ASFEnhance.Data.Plugin;
using System.ComponentModel;
using System.Composition;
using System.Text;
using System.Text.Json;

namespace ASFEnhance;

[Export(typeof(IPlugin))]
internal sealed class ASFEnhance : IASF, IBotCommand2, IBotFriendRequest, IBotModules, IGitHubPluginUpdates
{
    public string Name => nameof(ASFEnhance);

    public Version Version => MyVersion;

    public bool CanUpdate => true;
    public string RepositoryName => "chr233/ASFEnhance";

    private Timer? StatisticTimer;

    private Timer? ClaimItemTimer;

    /// <summary>
    /// ASF启动事件
    /// </summary>
    /// <param name="additionalConfigProperties"></param>
    /// <returns></returns>
    public Task OnASFInit(IReadOnlyDictionary<string, JsonElement>? additionalConfigProperties = null)
    {
        var message = new StringBuilder("\n");
        message.AppendLine(Static.Line);
        message.AppendLine(Static.Logo);
        message.AppendLine(Static.Line);
        message.AppendLineFormat(Langs.PluginVer, nameof(ASFEnhance), MyVersion);
        message.AppendLine(Langs.PluginContact);
        message.AppendLine(Langs.PluginInfo);
        message.AppendLine(Static.Line);

        if (_Adapter_.ExtensionCore.HasSubModule)
        {
            message.AppendLineFormat(Langs.SubModuleLoadedModules, _Adapter_.ExtensionCore.SubModules.Count);
            message.AppendLine();
            int index = 1;
            foreach (var (_, subModule) in _Adapter_.ExtensionCore.SubModules)
            {
                message.AppendLineFormat(Langs.SubModuleListItem, index++, subModule.CmdPrefix ?? "---", subModule.PluginName, subModule.PluginVersion);
            }
        }
        else
        {
            message.AppendLine(Langs.SubModuleNoModule);
        }
        message.AppendLine(Static.Line);

        ASFLogger.LogGenericInfo(message.ToString());

        PluginConfig? config = null;

        if (additionalConfigProperties != null)
        {
            foreach (var (configProperty, configValue) in additionalConfigProperties)
            {
                if (configProperty == "ASFEnhance" && configValue.ValueKind == JsonValueKind.Object)
                {
                    try
                    {
                        config = configValue.ToJsonObject<PluginConfig>();
                        if (config != null)
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        ASFLogger.LogGenericException(ex);
                    }
                }
            }
        }

        var warnings = new StringBuilder("\n");

        Utils.Config = config ?? new();

        //开发者特性
        if (Config.DevFeature)
        {
            warnings.AppendLine(Static.Line);
            warnings.AppendLine(Langs.DevFeatureEnabledWarning);
            warnings.AppendLine(Static.Line);
        }

        //使用协议
        if (!Config.EULA)
        {
            warnings.AppendLine(Static.Line);
            warnings.AppendLineFormat(Langs.EulaWarning, Name);
            warnings.AppendLine(Static.Line);
        }

        if (warnings.Length > 1)
        {
            ASFLogger.LogGenericWarning(warnings.ToString());
        }

        //地址信息
        if (Config.Addresses == null)
        {
            Config.Addresses = [];
        }
        if (Config.Address != null)
        {
            Config.Addresses.Add(Config.Address);
            Config.Address = null;
        }

        //统计
        if (Config.Statistic)
        {
            StatisticTimer = new Timer(StatisticCallback, null, TimeSpan.FromSeconds(30), TimeSpan.FromHours(24));
        }

        //禁用命令
        if (Config.DisabledCmds != null)
        {
            var disabledCommands = new HashSet<string>();
            foreach (var cmd in Config.DisabledCmds)
            {
                disabledCommands.Add(cmd.ToUpperInvariant());
            }
            Config.DisabledCmds = disabledCommands;
        }

        //领取道具
        if (!string.IsNullOrEmpty(Config.AutoClaimItemBotNames))
        {
            var period = Math.Max(Config.AutoClaimItemPeriod, 8);
            ClaimItemTimer = new Timer(ClaimItemCallback, null, TimeSpan.FromHours(1), TimeSpan.FromHours(period));
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// 插件加载事件
    /// </summary>
    /// <returns></returns>
    public Task OnLoaded()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// 获取插件信息
    /// </summary>
    private static string? PluginInfo => string.Format("{0} {1}", nameof(ASFEnhance), MyVersion);

    /// <summary>
    /// 处理命令
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="access"></param>
    /// <param name="cmd"></param>
    /// <param name="message"></param>
    /// <param name="args"></param>
    /// <param name="steamId"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static Task<string?>? ResponseCommand(Bot bot, EAccess access, string cmd, string message, string[] args, ulong steamId)
    {
        var argLength = args.Length;

        if (access >= EAccess.Operator && argLength >= 2 && cmd.EndsWith('X'))
        {
            var match = RegexUtils.MatchRunTimes().Match(cmd);
            if (match.Success && uint.TryParse(match.Groups[1].Value, out var runs))
            {
                return Other.Command.ResponseRepeatCommands(bot, access, Utilities.GetArgsAsText(message, 1), steamId, runs);
            }
        }

        return argLength switch
        {
            0 => throw new InvalidOperationException(nameof(args)),
            1 => cmd switch //不带参数
            {
#if DEBUG
                "TEST" when access >= EAccess.Master =>
                    Cart.Command.ResponseTest(bot),
#endif

                //Plugin Info
                "ASFENHANCE" or
                "ASFE" when access >= EAccess.FamilySharing =>
                    Task.FromResult(PluginInfo),

                //Update
                "PLUGINLIST" or
                "PL" when access >= EAccess.Operator =>
                    Task.FromResult(Update.Command.ResponsePluginList()),

                //Event
                "DL2" when access >= EAccess.Operator =>
                    Event.Command.ResponseDL2(bot, null),

                "CLAIMITEM" or
                "CI" when access >= EAccess.Operator =>
                    Event.Command.ResponseClaimItem(bot),

                "CLAIMPOINTITEM" or
                "CLAIMPOINTSITEM" or
                "CPI" when access >= EAccess.Operator =>
                    Event.Command.ResponseClaimPointStoreItem(bot),

                "CLAIM20TH" or
                "C20" when access >= EAccess.Operator =>
                    Event.Command.ResponseClaim20Th(bot),

                "AV" or
                "AUTUMNVOTE" when access >= EAccess.Operator =>
                    Event.Command.ResponseAutumnSteamAwardVote(bot, ""),

                "CAV" or
                "CHECKAUTUMNVOTE" when access >= EAccess.Operator =>
                    Event.Command.ResponseCheckAutumnSteamAwardVote(bot),

                "V" or
                "VOTE" or
                "WV" or
                "WINTERVOTE" when access >= EAccess.Operator =>
                    Event.Command.ResponseWinterSteamAwardVote(bot, ""),

                "CV" or
                "CHECKVOTE" or
                "CWV" or
                "CHECKWINTERVOTE" when access >= EAccess.Operator =>
                    Event.Command.ResponseCheckWinterSteamAwardVote(bot),

                //Shortcut
                "P" =>
                    bot.Commands.Response(access, "POINTS", steamId),
                "PA" =>
                    bot.Commands.Response(access, "POINTS ASF", steamId),
                "LA" =>
                    bot.Commands.Response(access, "LEVEL ASF", steamId),
                "BA" =>
                    bot.Commands.Response(access, "BALANCE ASF", steamId),
                "CA" =>
                    bot.Commands.Response(access, "CART ASF", steamId),

                //Account
                "CHECKAPIKEY" when access >= EAccess.Operator =>
                    Account.Command.ResponseCheckApiKey(bot),
                "REVOKEAPIKEY" when access >= EAccess.Master =>
                    Account.Command.ResponseRevokeApiKey(bot),

                "PURCHASEHISTORY" or
                "PH" when access >= EAccess.Operator =>
                    Account.Command.ResponseAccountHistory(bot),

                "FREELICENSES" or
                "FREELICENSE" or
                "FL" when access >= EAccess.Operator =>
                    Account.Command.ResponseGetAccountLicenses(bot, true),

                "LICENSES" or
                "LICENSE" or
                "L" when access >= EAccess.Operator =>
                    Account.Command.ResponseGetAccountLicenses(bot, false),

                "EMAILOPTIONS" or
                "EMAILOPTION" or
                "EO" when access >= EAccess.Operator =>
                    Account.Command.ResponseGetEmailOptions(bot),

                "NOTIFICATIONOPTIONS" or
                "NOTIFICATIONOPTION" or
                "NOO" when access >= EAccess.Operator =>
                    Account.Command.ResponseGetNotificationOptions(bot),

                "GETBOTBANNED" or
                "GETBOTBAN" or
                "GBB" when access >= EAccess.Operator =>
                    Account.Command.ResponseGetAccountBanned(bot, null),

                "RECEIVEGIFT" or
                "RG" when access >= EAccess.Operator =>
                    Account.Command.ResponseReceiveGift(bot),

                "PLAYTIME" or
                "PT" when access >= EAccess.Operator =>
                    Account.Command.ResponseGetPlayTime(bot, null),

                "EMAIL" or
                "EM" when access >= EAccess.Operator =>
                    Account.Command.ResponseGetEmail(bot),

                "GETPRIVACYAPP" or
                "GPA" when access >= EAccess.Operator =>
                    Account.Command.ResponseGetPrivacyAppList(bot),

                "CHECKMARKETLIMIT" or
                "CML" when access >= EAccess.Operator =>
                    Account.Command.ResponseCheckMarketLimit(bot),

                "PHONESUFFIX" when access >= EAccess.Operator =>
                    Account.Command.ResponseGetPhoneSuffix(bot),

                "REGISTEDATE" when access >= EAccess.Operator =>
                    Account.Command.ResponseGetRegisterDate(bot),

                "MYBAN" when access >= EAccess.Operator =>
                    Account.Command.ResponseGetMyBans(bot),

                "REMOVEALLDEMOS" or
                "REMOVEALLDEMO" or
                "RAD" when access >= EAccess.Master =>
                    Account.Command.ResponseRemoveFreeLicenses(bot, null, true),

                //Cart
                "CART" or
                "C" when access >= EAccess.Operator =>
                    Cart.Command.ResponseGetCartGames(bot),

                "CARTRESET" or
                "CR" when access >= EAccess.Operator =>
                    Cart.Command.ResponseClearCartGames(bot),

                "FAKEPURCHASE" or
                "FPC" when access >= EAccess.Master =>
                    Cart.Command.ResponseFakePurchaseSelf(bot),

                "PURCHASE" or
                "PC" when access >= EAccess.Master =>
                    Cart.Command.ResponsePurchaseSelf(bot),

                "GETREGION" when access >= EAccess.Operator =>
                    Cart.Command.ResponseGetRegion(bot),

                "CHANGEREGION" when access >= EAccess.Master =>
                    Cart.Command.ResponseChangeRegion(bot, null),

                //Community
                "NOTIFICATION" or
                "N" when access >= EAccess.Operator =>
                    Community.Command.ResponseGetNotifications(bot),

                "CLEARNOTIFICATION" or
                "CN" when access >= EAccess.Operator =>
                    Community.Command.ResponseClearNotification(bot),

                //Curator
                "CURATORLIST" or
                "CL" when Config.EULA && access >= EAccess.Master =>
                    Curator.Command.ResponseGetFollowingCurators(bot),

                "UNFOLLOWALLCURASOR" or
                "UNFOLLOWALLCURASORS" or
                "UFACU" when Config.EULA && access >= EAccess.Master =>
                    Curator.Command.ResponseUnFollowAllCurators(bot),

                //Explorer
                "EXPLORER" or
                "EX" when access >= EAccess.Master =>
                    Explorer.Command.ResponseExploreDiscoveryQueue(bot),

                //Family
                "FAMILYGROUP" when access >= EAccess.Master =>
                    Family.Command.ResponseFamilyGroup(bot),

                //Friend
                "DELETEALLFRIEND" when access >= EAccess.Master =>
                    Friend.Command.ResponseDeleteAllFriend(bot),

                "INVITELINK" or
                "IL" when access >= EAccess.Operator =>
                    Friend.Command.ResponseGetInviteLink(bot),

                //Group
                "GROUPLIST" or
                "GL" when Config.EULA && access >= EAccess.FamilySharing =>
                    Group.Command.ResponseGroupList(bot),

                //Other
                "ASFEHELP" or
                "EHELP" =>
                    Task.FromResult(Other.Command.ResponseAllCommands()),

                //Profile
                "CLEARALIAS" when access >= EAccess.Operator =>
                    Profile.Command.ResponseClearAliasHistory(bot),

                "CRAFTBADGE" or
                "CRAFTBADGES" or
                "CB" when access >= EAccess.Master =>
                    Profile.Command.ResponseCraftBadge(bot),

                "DELETEAVATAR" when access >= EAccess.Master =>
                    Profile.Command.ResponseDelProfileAvatar(bot),

                "FRIENDCODE" or
                "FC" when access >= EAccess.FamilySharing =>
                    Task.FromResult(Profile.Command.ResponseGetFriendCode(bot)),

                "STEAMID" or
                "SID" when access >= EAccess.FamilySharing =>
                    Task.FromResult(Profile.Command.ResponseGetSteamId(bot)),

                "PROFILE" or
                "PF" when access >= EAccess.FamilySharing =>
                    Profile.Command.ResponseGetProfileSummary(bot),

                "PROFILELINK" or
                "PFL" when access >= EAccess.FamilySharing =>
                    Profile.Command.ResponseGetProfileLink(bot),

                "RANDOMGAMEAVATAR" or
                "RGA" when access >= EAccess.Master =>
                    Profile.Command.ResponseSetProfileGameAvatar(bot, null, null),

                "REPLAY" or
                "RP" when access >= EAccess.Operator =>
                    Profile.Command.ResponseGetReplay(bot, "2025"),

                "TRADELINK" or
                "TL" when access >= EAccess.Operator =>
                    Profile.Command.ResponseGetTradeLink(bot),

                "DELETECUSTOMURL" or
                "DCU" when access >= EAccess.Master =>
                    Profile.Command.ResponseEditCustomUrl(bot, null),

                "DELETEREALNAME" or
                "DRN" when access >= EAccess.Master =>
                    Profile.Command.ResponseEditRealName(bot, null),

                "BALANCEINFO" or
                "BI" when access >= EAccess.Operator =>
                    Profile.Command.ResponseBalanceInfo(bot),

                "CLEARPROFILETHEME" or
                "CPT" when access >= EAccess.Master =>
                    Profile.Command.ResponseSetProfileTheme(bot, null),

                "CLEARPROFILEMODIFIER" or
                "CPM" when argLength == 2 && access >= EAccess.Master =>
                    Profile.Command.ResponseClearProfileModifier(bot),

                "GETPROFILEMODIFIER" or
                "GPM" when access >= EAccess.Master =>
                    Profile.Command.ResponseGetProfileItems(bot),

                //Wishlist
                "WISHLIST" or
                "WL" when access >= EAccess.Operator =>
                    WishList.Command.ResponseGetWishlist(bot),

                "CLEARWISHLIST" or
                "CWL" when access >= EAccess.Operator =>
                    WishList.Command.ResponseRemoveAllWishlist(bot),

                //Inventory
                "PENDINGGIFT" or
                "PG" when access >= EAccess.Operator =>
                    Inventory.Command.ResponseGetPendingGifts(bot),

                "TRADEOFFERS" or
                "TRADEOFFER" or
                "TO" when access >= EAccess.Operator =>
                    Inventory.Command.ResponseGetTradeOffers(bot),

                //DevFuture
                "COOKIES" when Config.DevFeature && access >= EAccess.Owner =>
                    Task.FromResult(DevFeature.Command.ResponseGetCookies(bot)),
                "ACCESSTOKEN" when Config.DevFeature && access >= EAccess.Owner =>
                    DevFeature.Command.ResponseGetAccessToken(bot),

                //Limited Tips
                "CURATORLIST" or
                "CL" or
                "GROUPLIST" or
                "GL" when access >= EAccess.Master =>
                    Task.FromResult(Other.Command.ResponseEulaCmdUnavilable()),

                "COOKIES" or
                "ACCESSTOKEN" when access >= EAccess.Owner =>
                    Task.FromResult(Other.Command.ResponseDevFeatureUnavilable()),

                _ => null,
            },
            _ => cmd switch //带参数
            {
#if DEBUG
                "TEST" when access >= EAccess.Master =>
                    Cart.Command.ResponseTest(Utilities.GetArgsAsText(args, 1, ",")),
#endif

                //Event
                "DL2" when argLength > 2 && access >= EAccess.Operator =>
                    Event.Command.ResponseDL2(SkipBotNames(args, 1, 1), args.Last()),
                "DL2" when access >= EAccess.Operator =>
                    Event.Command.ResponseDL2(args[1], null),

                "CLAIMITEM" or
                "CI" when access >= EAccess.Operator =>
                    Event.Command.ResponseClaimItem(Utilities.GetArgsAsText(args, 1, ",")),

                "CLAIMPOINTITEM" or
                "CLAIMPOINTSITEM" or
                "CPI" when access >= EAccess.Operator =>
                    Event.Command.ResponseClaimPointStoreItem(Utilities.GetArgsAsText(args, 1, ",")),

                "CLAIM20TH" or
                "C20" when access >= EAccess.Operator =>
                    Event.Command.ResponseClaim20Th(Utilities.GetArgsAsText(args, 1, ",")),

                "AV" or
                "AUTUMNVOTE" when argLength > 2 && access >= EAccess.Operator =>
                    Event.Command.ResponseAutumnSteamAwardVote(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "AUTUMNV" or
                "AUTUMNVOTE" when access >= EAccess.Operator =>
                    Event.Command.ResponseAutumnSteamAwardVote(args[1], ""),

                "CAV" or
                "CHECKAUTUMNVOTE" when access >= EAccess.Operator =>
                    Event.Command.ResponseCheckAutumnSteamAwardVote(Utilities.GetArgsAsText(args, 1, ",")),

                "V" or
                "VOTE" or
                "WV" or
                "WINTERVOTE" when argLength > 2 && access >= EAccess.Operator =>
                    Event.Command.ResponseWinterSteamAwardVote(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "V" or
                "VOTE" or
                "WV" or
                "WINTERVOTE" when access >= EAccess.Operator =>
                    Event.Command.ResponseWinterSteamAwardVote(args[1], ""),

                "CV" or
                "CHECKVOTE" or
                "CWV" or
                "CHECKWINTERVOTE" when access >= EAccess.Operator =>
                    Event.Command.ResponseCheckWinterSteamAwardVote(Utilities.GetArgsAsText(args, 1, ",")),

                //Shortcut
                "AL" =>
                    bot.Commands.Response(access, "ADDLICENSE " + Utilities.GetArgsAsText(message, 1), steamId),
                "P" =>
                    bot.Commands.Response(access, "POINTS " + Utilities.GetArgsAsText(message, 1), steamId),
                "TR" =>
                    bot.Commands.Response(access, "TRANSFER " + Utilities.GetArgsAsText(message, 1), steamId),

                //Account
                "CHECKAPIKEY" when access >= EAccess.Operator =>
                    Account.Command.ResponseCheckApiKey(Utilities.GetArgsAsText(args, 1, ",")),
                "REVOKEAPIKEY" when access >= EAccess.Master =>
                    Account.Command.ResponseRevokeApiKey(Utilities.GetArgsAsText(args, 1, ",")),

                "PURCHASEHISTORY" or
                "PH" when access > EAccess.Operator =>
                    Account.Command.ResponseAccountHistory(Utilities.GetArgsAsText(args, 1, ",")),

                "FREELICENSES" or
                "FREELICENSE" or
                "FL" when access >= EAccess.Operator =>
                    Account.Command.ResponseGetAccountLicenses(Utilities.GetArgsAsText(args, 1, ","), true),

                "LICENSES" or
                "LICENSE" or
                "L" when access >= EAccess.Operator =>
                    Account.Command.ResponseGetAccountLicenses(Utilities.GetArgsAsText(args, 1, ","), false),

                "EMAILOPTIONS" or
                "EMAILOPTION" or
                "EO" when access >= EAccess.Operator =>
                    Account.Command.ResponseGetEmailOptions(Utilities.GetArgsAsText(args, 1, ",")),

                "NOTIFICATIONOPTIONS" or
                "NOTIFICATIONOPTION" or
                "NOO" when access >= EAccess.Operator =>
                    Account.Command.ResponseGetNotificationOptions(Utilities.GetArgsAsText(args, 1, ",")),

                "SETEMAILOPTIONS" or
                "SETEMAILOPTION" or
                "SEO" when argLength > 2 && access >= EAccess.Master =>
                    Account.Command.ResponseSetEmailOptions(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "SETEMAILOPTIONS" or
                "SETEMAILOPTION" or
                "SEO" when access >= EAccess.Master =>
                    Account.Command.ResponseSetEmailOptions(bot, args[1]),

                "SETNOTIFICATIONOPTIONS" or
                "SETNOTIFICATIONOPTION" or
                "SNO" when argLength > 2 && access >= EAccess.Master =>
                    Account.Command.ResponseSetNotificationOptions(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "SETNOTIFICATIONOPTIONS" or
                "SETNOTIFICATIONOPTION" or
                "SNO" when access >= EAccess.Master =>
                    Account.Command.ResponseSetNotificationOptions(bot, args[1]),

                "GETBOTBANNED" or
                "GETBOTBAN" or
                "GBB" when access >= EAccess.Operator =>
                    Account.Command.ResponseGetAccountBanned(Utilities.GetArgsAsText(args, 1, ",")),

                "GETACCOUNTBANNED" or
                "GETACCOUNTBAN" or
                "GAB" when access >= EAccess.Operator =>
                    Account.Command.ResponseSteamIdAccountBanned(Utilities.GetArgsAsText(args, 1, ",")),

                "RECEIVEGIFT" or
                "RG" when access >= EAccess.Operator =>
                    Account.Command.ResponseReceiveGift(Utilities.GetArgsAsText(args, 1, ",")),

                "PLAYTIME" or
                "PT" when argLength > 2 && access >= EAccess.Operator =>
                    Account.Command.ResponseGetPlayTime(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "PLAYTIME" or
                "PT" when access >= EAccess.Operator =>
                    Account.Command.ResponseGetPlayTime(bot, args[1]),

                "EMAIL" or
                "EM" when access >= EAccess.Operator =>
                    Account.Command.ResponseGetEmail(Utilities.GetArgsAsText(args, 1, ",")),

                "GETPRIVACYAPP" or
                "GPA" when access >= EAccess.Operator =>
                    Account.Command.ResponseGetPrivacyAppList(Utilities.GetArgsAsText(args, 1, ",")),

                "SETAPPPRIVATE" or
                "SAPRI" when argLength > 2 && access >= EAccess.Master =>
                    Account.Command.ResponseSetAppListPrivacy(args[1], Utilities.GetArgsAsText(args, 2, ","), true),
                "SETAPPPRIVATE" or
                "SAPRI" when access >= EAccess.Master =>
                    Account.Command.ResponseSetAppListPrivacy(bot, args[1], true),

                "SETAPPPUBLIC" or
                "SAPUB" when argLength > 2 && access >= EAccess.Master =>
                    Account.Command.ResponseSetAppListPrivacy(args[1], Utilities.GetArgsAsText(args, 2, ","), false),
                "SETAPPPUBLIC" or
                "SAPUB" when access >= EAccess.Master =>
                    Account.Command.ResponseSetAppListPrivacy(bot, args[1], false),

                "CHECKMARKETLIMIT" or
                "CML" when access >= EAccess.Operator =>
                    Account.Command.ResponseCheckMarketLimit(Utilities.GetArgsAsText(args, 1, ",")),

                "PHONESUFFIX" when access >= EAccess.Operator =>
                    Account.Command.ResponseGetPhoneSuffix(Utilities.GetArgsAsText(args, 1, ",")),

                "REGISTEDATE" when access >= EAccess.Operator =>
                    Account.Command.ResponseGetRegisterDate(Utilities.GetArgsAsText(args, 1, ",")),

                "REMOVELICENSES" or
                "REMOVELICENSE" or
                "RL" when argLength > 2 && access >= EAccess.Master =>
                    Account.Command.ResponseRemoveFreeLicenses(args[1], Utilities.GetArgsAsText(args, 2, ","), false),

                "REMOVELICENSES" or
                "REMOVELICENSE" or
                "RL" when access >= EAccess.Master =>
                    Account.Command.ResponseRemoveFreeLicenses(bot, args[1], false),

                "REMOVEALLDEMOS" or
                "REMOVEALLDEMO" or
                "RAD" when argLength > 2 && access >= EAccess.Master =>
                    Account.Command.ResponseRemoveFreeLicenses(Utilities.GetArgsAsText(args, 1, ","), null, true),

                "MYBAN" when access >= EAccess.Operator =>
                    Account.Command.ResponseGetMyBans(Utilities.GetArgsAsText(args, 1, ",")),

                //Cart
                "CART" or
                "C" when access >= EAccess.Operator =>
                    Cart.Command.ResponseGetCartGames(Utilities.GetArgsAsText(args, 1, ",")),

                "ADDCART" or
                "AC" when argLength > 2 && access >= EAccess.Operator =>
                    Cart.Command.ResponseAddCartGames(args[1], Utilities.GetArgsAsText(args, 2, ","), false),
                "ADDCART" or
                "AC" when access >= EAccess.Operator =>
                    Cart.Command.ResponseAddCartGames(bot, args[1], false),

                "ADDCARTPRIVATE" or
                "ACP" when argLength > 2 && access >= EAccess.Operator =>
                    Cart.Command.ResponseAddCartGames(args[1], Utilities.GetArgsAsText(args, 2, ","), false),
                "ADDCARTPRIVATE" or
                "ACP" when access >= EAccess.Operator =>
                    Cart.Command.ResponseAddCartGames(bot, args[1], true),

                "ADDCARTGIFT" or
                "ACG" when argLength > 3 && access >= EAccess.Operator =>
                    Cart.Command.ResponseAddGiftCartGames(args[1], SkipBotNames(args, 1, 1), args.Last()),
                "ADDCARTGIFT" or
                "ACG" when argLength == 3 && access >= EAccess.Operator =>
                    Cart.Command.ResponseAddGiftCartGames(bot, args[1], args[2]),

                "EDITCART" or
                "EC" when argLength > 2 && access >= EAccess.Operator =>
                    Cart.Command.ResponseEditCartGame(args[1], Utilities.GetArgsAsText(args, 2, ","), false, args.Last()),
                "EDITCART" or
                "EC" when argLength == 2 && access >= EAccess.Operator =>
                    Cart.Command.ResponseEditCartGame(bot, args[1], false, null),

                "DELETECART" or
                "DC" when argLength > 2 && access >= EAccess.Operator =>
                    Cart.Command.ResponseRemoveCartGame(args[1], Utilities.GetArgsAsText(args, 1, ",")),
                "DELETECART" or
                "DC" when argLength == 2 && access >= EAccess.Operator =>
                    Cart.Command.ResponseRemoveCartGame(bot, args[1]),

                "EDITCARTPRIVATE" or
                "ECP" when argLength > 2 && access >= EAccess.Operator =>
                    Cart.Command.ResponseEditCartGame(args[1], Utilities.GetArgsAsText(args, 2, ","), true, null),
                "EDITCARTPRIVATE" or
                "ECP" when argLength == 2 && access >= EAccess.Operator =>
                    Cart.Command.ResponseEditCartGame(bot, args[1], true, null),

                "EDITCARTGIFT" or
                "ECG" when argLength > 3 && access >= EAccess.Operator =>
                    Cart.Command.ResponseEditCartGame(args[1], SkipBotNames(args, 1, 1), false, args.Last()),
                "EDITCARTGIFT" or
                "ECG" when argLength == 3 && access >= EAccess.Operator =>
                    Cart.Command.ResponseEditCartGame(bot, args[1], false, args[2]),

                "CARTRESET" or
                "CR" when access >= EAccess.Operator =>
                    Cart.Command.ResponseClearCartGames(Utilities.GetArgsAsText(args, 1, ",")),

                "FAKEPURCHASE" or
                "FPC" when access >= EAccess.Master =>
                    Cart.Command.ResponseFakePurchaseSelf(Utilities.GetArgsAsText(args, 1, ",")),

                "PURCHASE" or
                "PC" when access >= EAccess.Master =>
                    Cart.Command.ResponsePurchaseSelf(Utilities.GetArgsAsText(args, 1, ",")),

                "ADDFUNDS" when argLength > 2 && access >= EAccess.Operator =>
                    Cart.Command.ResponseAddFunds(SkipBotNames(args, 0, 1), args.Last()),
                "ADDFUNDS" when argLength == 2 && access >= EAccess.Operator =>
                    Cart.Command.ResponseAddFunds(bot, args[1]),

                "CHANGEREGION" when argLength > 2 && access >= EAccess.Operator =>
                    Cart.Command.ResponseChangeRegion(SkipBotNames(args, 0, 1), args.Last()),
                "CHANGEREGION" when argLength == 2 && access >= EAccess.Operator =>
                    Cart.Command.ResponseChangeRegion(SkipBotNames(args, 0, 1), null),

                "GETREGION" when access >= EAccess.Operator =>
                    Cart.Command.ResponseGetRegion(Utilities.GetArgsAsText(args, 1, ",")),

                "PURCHASEEXTERNAL" or
                "PCE" when argLength > 2 && access >= EAccess.Master =>
                   Cart.Command.ResponsePurchaseSelfExternal(SkipBotNames(args, 0, 1), args.Last()),

                "PURCHASEEXTERNAL" or
                "PCE" when argLength == 2 && access >= EAccess.Master =>
                   Cart.Command.ResponsePurchaseSelfExternal(bot, args[1]),

                //Community
                "NOTIFICATION" or
                "N" when access >= EAccess.Operator =>
                    Community.Command.ResponseGetNotifications(Utilities.GetArgsAsText(args, 1, ",")),

                "CLEARNOTIFICATION" or
                "CN" when access >= EAccess.Operator =>
                    Community.Command.ResponseClearNotification(Utilities.GetArgsAsText(args, 1, ",")),

                //Curator
                "CURATORLIST" or
                "CL" when Config.EULA && access >= EAccess.Master =>
                    Curator.Command.ResponseGetFollowingCurators(Utilities.GetArgsAsText(args, 1, ",")),

                "FOLLOWCURATOR" or
                "FCU" when Config.EULA && argLength > 2 && access >= EAccess.Master =>
                    Curator.Command.ResponseFollowCurator(args[1], Utilities.GetArgsAsText(args, 2, ","), true),
                "FOLLOWCURATOR" or
                "FCU" when Config.EULA && access >= EAccess.Master =>
                    Curator.Command.ResponseFollowCurator(bot, args[1], true),

                "UNFOLLOWALLCURASOR" or
                "UNFOLLOWALLCURASORS" or
                "UFACU" when Config.EULA && access >= EAccess.Master =>
                    Curator.Command.ResponseUnFollowAllCurators(Utilities.GetArgsAsText(args, 1, ",")),

                "UNFOLLOWCURATOR" or
                "UFCU" when Config.EULA && argLength > 2 && access >= EAccess.Master =>
                    Curator.Command.ResponseFollowCurator(args[1], Utilities.GetArgsAsText(args, 2, ","), false),
                "UNFOLLOWCURATOR" or
                "UFCU" when Config.EULA && access >= EAccess.Master =>
                    Curator.Command.ResponseFollowCurator(bot, args[1], false),

                //Explorer
                "EXPLORER" or
                "EX" when access >= EAccess.Master =>
                    Explorer.Command.ResponseExploreDiscoveryQueue(Utilities.GetArgsAsText(args, 1, ",")),

                //Family
                "FAMILYGROUP" when access >= EAccess.Master =>
                    Family.Command.ResponseFamilyGroup(Utilities.GetArgsAsText(args, 1, ",")),

                "EDITFAMILYGROUP" or
                "EFG" when argLength > 2 && access >= EAccess.Master =>
                    Family.Command.ResponseFamilyGroupName(args[1], Utilities.GetArgsAsText(message, 2)),
                "EDITFAMILYGROUP" or
                "EFG" when access >= EAccess.Master =>
                    Family.Command.ResponseFamilyGroupName(bot, args[1]),

                //Friend
                "ADDBOTFRIEND" or
                "ABF" when argLength > 2 && access >= EAccess.Master =>
                    Friend.Command.ResponseAddBotFriend(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "ADDBOTFRIEND" or
                "ABF" when access >= EAccess.Master =>
                    Friend.Command.ResponseAddBotFriend(bot, args[1]),

                "ADDBOTFRIENDMULI" or
                "ABFM" when access >= EAccess.Master =>
                    Friend.Command.ResponseAddBotFriendMuli(Utilities.GetArgsAsText(message, 1)),

                "ADDFRIEND" or
                "AF" when argLength > 2 && access >= EAccess.Master =>
                    Friend.Command.ResponseAddFriend(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "ADDFRIEND" or
                "AF" when access >= EAccess.Master =>
                    Friend.Command.ResponseAddFriend(bot, args[1]),

                "DELETEFRIEND" or
                "DF" when argLength > 2 && access >= EAccess.Master =>
                    Friend.Command.ResponseDeleteFriend(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "DELETEFRIEND" or
                "DF" when access >= EAccess.Master =>
                    Friend.Command.ResponseDeleteFriend(bot, args[1]),

                "DELETEALLFRIEND" when access >= EAccess.Master =>
                    Friend.Command.ResponseDeleteAllFriend(Utilities.GetArgsAsText(args, 1, ",")),

                "INVITELINK" or
                "IL" when access >= EAccess.Operator =>
                    Friend.Command.ResponseGetInviteLink(Utilities.GetArgsAsText(args, 1, ",")),

                //Group
                "GROUPLIST" or
                "GL" when Config.EULA && access >= EAccess.FamilySharing =>
                    Group.Command.ResponseGroupList(Utilities.GetArgsAsText(args, 1, ",")),

                "JOINGROUP" or
                "JG" when Config.EULA && argLength > 2 && access >= EAccess.Master =>
                    Group.Command.ResponseJoinGroup(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "JOINGROUP" or
                "JG" when Config.EULA && access >= EAccess.Master =>
                    Group.Command.ResponseJoinGroup(bot, args[1]),

                "LEAVEGROUP" or
                "LG" when Config.EULA && argLength > 2 && access >= EAccess.Master =>
                    Group.Command.ResponseLeaveGroup(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "LEAVEGROUP" or
                "LG" when Config.EULA && access >= EAccess.Master =>
                    Group.Command.ResponseLeaveGroup(bot, args[1]),

                //Other
                "DUMP" when access >= EAccess.Operator =>
                    Task.FromResult(Other.Command.ResponseDumpToFile(bot, access, Utilities.GetArgsAsText(message, 1), steamId)),

                "KEY" or
                "K" when access >= EAccess.FamilySharing =>
                    Task.FromResult(Other.Command.ResponseExtractKeys(Utilities.GetArgsAsText(args, 1, ","))),

                "EHELP" or
                "HELP" when access >= EAccess.FamilySharing =>
                    Task.FromResult(Other.Command.ResponseCommandHelp(args)),

                //Profile
                "CLEARALIAS" when access >= EAccess.Operator =>
                    Profile.Command.ResponseClearAliasHistory(Utilities.GetArgsAsText(args, 1, ",")),

                "CRAFTBADGE" or
                "CRAFTBADGES" or
                "CB" when access >= EAccess.Master =>
                    Profile.Command.ResponseCraftBadge(Utilities.GetArgsAsText(args, 1, ",")),

                "CRAFTSPECIFYBADGE" or
                "CRAFTSPECIFYBADGES" or
                "CSB" when access >= EAccess.Master && argLength > 2 =>
                    Profile.Command.ResponseCraftSpecifyBadge(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "CRAFTSPECIFYBADGE" or
                "CRAFTSPECIFYBADGES" or
                "CSB" when access >= EAccess.Master =>
                    Profile.Command.ResponseCraftSpecifyBadge(bot, args[1]),

                "DELETEAVATAR" when access >= EAccess.Master =>
                    Profile.Command.ResponseDelProfileAvatar(Utilities.GetArgsAsText(args, 1, ",")),

                "FRIENDCODE" or
                "FC" when access >= EAccess.FamilySharing =>
                    Profile.Command.ResponseGetFriendCode(Utilities.GetArgsAsText(args, 1, ",")),

                "GAMEAVATAR" or
                "GA" when argLength > 3 && access >= EAccess.Master =>
                    Profile.Command.ResponseSetProfileGameAvatar(SkipBotNames(args, 1, 2), args[argLength - 2], args.Last()),
                "GAMEAVATAR" or
                "GA" when argLength == 3 && access >= EAccess.Master =>
                    Profile.Command.ResponseSetProfileGameAvatar(args[1], args[2], null),
                "GAMEAVATAR" or
                "GA" when access >= EAccess.Master =>
                    Profile.Command.ResponseSetProfileGameAvatar(bot, args[1], null),

                "SETAVATAR" or
                "SEA" when argLength >= 3 && access >= EAccess.Master =>
                    Profile.Command.ResponseSetProfileAvatar(SkipBotNames(args, 1, 1), args.Last()),
                "SETAVATAR" or
                "SEA" when access >= EAccess.Master =>
                    Profile.Command.ResponseSetProfileAvatar(bot, args[1]),

                "STEAMID" or
                "SID" when access >= EAccess.FamilySharing =>
                    Profile.Command.ResponseGetSteamId(Utilities.GetArgsAsText(args, 1, ",")),

                "PROFILE" or
                "PF" when access >= EAccess.FamilySharing =>
                    Profile.Command.ResponseGetProfileSummary(Utilities.GetArgsAsText(args, 1, ",")),

                "PROFILELINK" or
                "PFL" when access >= EAccess.FamilySharing =>
                    Profile.Command.ResponseGetProfileLink(Utilities.GetArgsAsText(args, 1, ",")),

                "RANDOMGAMEAVATAR" or
                "RGA" when access >= EAccess.Master =>
                    Profile.Command.ResponseSetProfileGameAvatar(Utilities.GetArgsAsText(args, 1, ","), null, null),

                "ADVNICKNAME" or
                "ANN" when argLength > 2 && access >= EAccess.Master =>
                    Profile.Command.ResponseAdvNickName(args[1], Utilities.GetArgsAsText(message, 2)),
                "ADVNICKNAME" or
                "ANN" when access >= EAccess.Master =>
                    Profile.Command.ResponseAdvNickName(bot, args[1]),

                "REPLAY" or
                "RP" when argLength > 2 && access >= EAccess.Master =>
                    Profile.Command.ResponseGetReplay(Utilities.GetArgsAsText(message, 2), args[1]),
                "REPLAY" or
                "RP" when access >= EAccess.Operator =>
                    Profile.Command.ResponseGetReplay(args[1], "2025"),

                "REPLAYPRIVACY" or
                "RPP" when argLength > 3 && access >= EAccess.Operator =>
                    Profile.Command.ResponseSetReplayPrivacy(args[2], Utilities.GetArgsAsText(args, 3, ","), args[1]),
                "REPLAYPRIVACY" or
                "RPP" when argLength == 3 && access >= EAccess.Operator =>
                    Profile.Command.ResponseSetReplayPrivacy(args[1], Utilities.GetArgsAsText(args, 2, ","), "2025"),
                "REPLAYPRIVACY" or
                "RPP" when access >= EAccess.Operator =>
                    Profile.Command.ResponseSetReplayPrivacy(bot, args[1], "2025"),

                "TRADELINK" or
                "TL" when access >= EAccess.Operator =>
                    Profile.Command.ResponseGetTradeLink(Utilities.GetArgsAsText(args, 1, ",")),

                "EDITCUSTOMURL" or
                "ECU" when argLength == 3 && access >= EAccess.Master =>
                    Profile.Command.ResponseEditCustomUrl(args[1], args[2]),
                "EDITCUSTOMURL" or
                "ECU" when argLength == 2 && access >= EAccess.Master =>
                    Profile.Command.ResponseEditCustomUrl(bot, args[1]),

                "DELETECUSTOMURL" or
                "DCU" when access >= EAccess.Master =>
                    Profile.Command.ResponseEditCustomUrl(Utilities.GetArgsAsText(args, 1, ","), null),

                "EDITREALNAME" or
                "ERN" when argLength == 3 && access >= EAccess.Master =>
                    Profile.Command.ResponseEditRealName(args[1], args[2]),
                "EDITREALNAME" or
                "ERN" when argLength == 2 && access >= EAccess.Master =>
                    Profile.Command.ResponseEditRealName(bot, args[1]),

                "DELETEREALNAME" or
                "DRN" when access >= EAccess.Master =>
                    Profile.Command.ResponseEditRealName(Utilities.GetArgsAsText(args, 1, ","), null),

                "BALANCEINFO" or
                "BI" when access >= EAccess.Operator =>
                    Profile.Command.ResponseBalanceInfo(Utilities.GetArgsAsText(args, 1, ",")),

                "SETPROFILETHEME" or
                "SPT" when argLength == 3 && access >= EAccess.Master =>
                    Profile.Command.ResponseSetProfileTheme(args[1], args[2]),
                "SETPROFILETHEME" or
                "SPT" when argLength == 2 && access >= EAccess.Master =>
                    Profile.Command.ResponseSetProfileTheme(bot, args[1]),

                "CLEARPROFILETHEME" or
                "CPT" when access >= EAccess.Master =>
                    Profile.Command.ResponseSetProfileTheme(Utilities.GetArgsAsText(args, 1, ","), null),

                "SETPROFILEMODIFIER" or
                "SPM" when argLength == 3 && access >= EAccess.Master =>
                    Profile.Command.ResponseSetProfileModifier(args[1], args[2]),
                "SETPROFILEMODIFIER" or
                "SPM" when argLength == 2 && access >= EAccess.Master =>
                    Profile.Command.ResponseSetProfileModifier(bot, args[1]),

                "CLEARPROFILEMODIFIER" or
                "CPM" when access >= EAccess.Master =>
                    Profile.Command.ResponseClearProfileModifier(Utilities.GetArgsAsText(args, 1, ",")),

                "GETPROFILEMODIFIER" or
                "GPM" when access >= EAccess.Master =>
                    Profile.Command.ResponseGetProfileItems(Utilities.GetArgsAsText(args, 1, ",")),

                //Store
                "SUBS" or
                "S" or
                "APPDETAIL" or
                "AD" when argLength > 2 && access >= EAccess.Operator =>
                    Store.Command.ResponseGetAppsDetail(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "SUBS" or
                "S" or
                "APPDETAIL" or
                "AD" when access >= EAccess.Operator =>
                    Store.Command.ResponseGetAppsDetail(bot, args[1]),

                "RECOMMENT" or
                "REC" when Config.EULA && argLength > 2 && access >= EAccess.Master =>
                    Store.Command.ResponseGetReview(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "RECOMMENT" or
                "REC" when Config.EULA && access >= EAccess.Master =>
                    Store.Command.ResponseGetReview(bot, args[1]),

                "DELETERECOMMENT" or
                "DREC" when Config.EULA && argLength > 2 && access >= EAccess.Master =>
                    Store.Command.ResponseDeleteReview(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "DELETERECOMMENT" or
                "DREC" when Config.EULA && access >= EAccess.Master =>
                    Store.Command.ResponseDeleteReview(bot, args[1]),

                "PUBLISHRECOMMENT" or
                "PREC" when Config.EULA && argLength > 3 && access >= EAccess.Master =>
                    Store.Command.ResponsePublishReview(args[1], args[2], Utilities.GetArgsAsText(message, 3)),
                "PUBLISHRECOMMENT" or
                "PREC" when Config.EULA && argLength == 3 && access >= EAccess.Master =>
                    Store.Command.ResponsePublishReview(bot, args[1], args[2]),

                "REQUESTACCESS" or
                "RA" when argLength > 2 && access >= EAccess.Operator =>
                    Store.Command.ResponseRequestAccess(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "REQUESTACCESS" or
                "RA" when access >= EAccess.Operator =>
                    Store.Command.ResponseRequestAccess(bot, args[1]),

                "SEARCH" or
                "SS" when argLength > 2 && access >= EAccess.Operator =>
                    Store.Command.ResponseSearchGame(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "SEARCH" or
                "SS" when access >= EAccess.Operator =>
                    Store.Command.ResponseSearchGame(bot, args[1]),

                "VIEWPAGE" or
                "VP" when argLength > 2 && access >= EAccess.Operator =>
                    Store.Command.ResponseViewPage(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "VIEWPAGE" or
                "VP" when access >= EAccess.Operator =>
                    Store.Command.ResponseViewPage(bot, args[1]),

                "REDEEMPOINTSITEM" or
                "REDEEMPOINTITEM" or
                "RPI" when argLength > 2 && access >= EAccess.Master =>
                    Store.Command.ResponseUnlockPointItem(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "REDEEMPOINTSITEM" or
                "REDEEMPOINTITEM" or
                "RPI" when access >= EAccess.Master =>
                    Store.Command.ResponseUnlockPointItem(bot, args[1]),

                "REDEEMPOINTSBADGE" or
                "REDEEMPOINTBADGE" or
                "RPB" when argLength > 3 && access >= EAccess.Master =>
                    Store.Command.ResponseUnlockPointBadge(args[1], args[2], Utilities.GetArgsAsText(args, 3, ",")),
                "REDEEMPOINTSBADGE" or
                "REDEEMPOINTBADGE" or
                "RPB" when argLength > 2 && access >= EAccess.Master =>
                    Store.Command.ResponseUnlockPointBadge(bot, args[1], args[2]),

                //Wallet
                "REDEEMWALLET" or
                "RWA" when args.Length > 2 && access >= EAccess.Master =>
                    Wallet.Command.ResponseRedeemWallet(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "REDEEMWALLET" or
                "RWA" when access >= EAccess.Master =>
                    Wallet.Command.ResponseRedeemWallet(bot, args[1]),

                "REDEEMWALLETMULT" or
                "RWAM" when access >= EAccess.Master =>
                    Wallet.Command.ResponseRedeemWalletMuli(Utilities.GetArgsAsText(args, 1, ",")),

                //WishList
                "ADDWISHLIST" or
                "AW" when argLength > 2 && access >= EAccess.Master =>
                    WishList.Command.ResponseAddWishlist(args[1], Utilities.GetArgsAsText(args, 2, ","), true),
                "ADDWISHLIST" or
                "AW" when access >= EAccess.Master =>
                    WishList.Command.ResponseAddWishlist(bot, args[1], true),

                "REMOVEWISHLIST" or
                "RW" when argLength > 2 && access >= EAccess.Master =>
                    WishList.Command.ResponseAddWishlist(args[1], Utilities.GetArgsAsText(args, 2, ","), false),
                "REMOVEWISHLIST" or
                "RW" when access >= EAccess.Master =>
                    WishList.Command.ResponseAddWishlist(bot, args[1], false),

                "CHECK" or
                "CK" when argLength > 2 && access >= EAccess.Master =>
                    WishList.Command.ResponseCheckGame(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "CHECK" or
                "CK" when access >= EAccess.Master =>
                    WishList.Command.ResponseCheckGame(bot, args[1]),

                "FOLLOWGAME" or
                "FG" when argLength > 2 && access >= EAccess.Master =>
                    WishList.Command.ResponseFollowGame(args[1], Utilities.GetArgsAsText(args, 2, ","), true),
                "FOLLOWGAME" or
                "FG" when access >= EAccess.Master =>
                    WishList.Command.ResponseFollowGame(bot, args[1], true),

                "UNFOLLOWGAME" or
                "UFG" when argLength > 2 && access >= EAccess.Master =>
                    WishList.Command.ResponseFollowGame(args[1], Utilities.GetArgsAsText(args, 2, ","), false),
                "UNFOLLOWGAME" or
                "UFG" when access >= EAccess.Master =>
                    WishList.Command.ResponseFollowGame(bot, args[1], false),

                "IGNOREGAME" or
                "IG" when argLength > 2 && access >= EAccess.Master =>
                    WishList.Command.ResponseIgnoreGame(args[1], Utilities.GetArgsAsText(args, 2, ","), true),
                "IGNOREGAME" or
                "IG" when access >= EAccess.Master =>
                    WishList.Command.ResponseIgnoreGame(bot, args[1], true),

                "REMOVEIGNOREGAME" or
                "RIG" when argLength > 2 && access >= EAccess.Master =>
                    WishList.Command.ResponseIgnoreGame(args[1], Utilities.GetArgsAsText(args, 2, ","), false),
                "REMOVEIGNOREGAME" or
                "RIG" when access >= EAccess.Master =>
                    WishList.Command.ResponseIgnoreGame(bot, args[1], false),

                "WISHLIST" or
                "WL" when access >= EAccess.Operator =>
                    WishList.Command.ResponseGetWishlist(Utilities.GetArgsAsText(args, 1, ",")),

                "CLEARWISHLIST" or
                "CWL" when access >= EAccess.Operator =>
                    WishList.Command.ResponseRemoveAllWishlist(Utilities.GetArgsAsText(args, 1, ",")),

                //Inventory
                "STACKINVENTORY" or
                "STACKINV" or
                "STI" when argLength > 3 && access >= EAccess.Operator =>
                    Inventory.Command.ResponseStackInventory(SkipBotNames(args, 1, 2), args[argLength - 2], args.Last()),
                "STACKINVENTORY" or
                "STACKINV" or
                "STI" when argLength == 3 && access >= EAccess.Operator =>
                    Inventory.Command.ResponseStackInventory(bot, args[1], args[2]),

                "UNSTACKINVENTORY" or
                "UNSTACKINV" or
                "USTI" when argLength > 3 && access >= EAccess.Operator =>
                    Inventory.Command.ResponseUnStackInventory(SkipBotNames(args, 1, 2), args[argLength - 2], args.Last()),
                "UNSTACKINVENTORY" or
                "UNSTACKINV" or
                "USTI" when argLength == 3 && access >= EAccess.Operator =>
                    Inventory.Command.ResponseUnStackInventory(bot, args[1], args[2]),

                "PENDINGGIFT" or
                "PG" when access >= EAccess.Operator =>
                    Inventory.Command.ResponseGetPendingGifts(Utilities.GetArgsAsText(args, 1, ",")),

                "ACCEPTGIFT" or
                "AG" when argLength > 2 && access >= EAccess.Master =>
                    Inventory.Command.ResponseAcceptGift(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "ACCEPTGIFT" or
                "AG" when argLength == 2 && access >= EAccess.Master =>
                    Inventory.Command.ResponseAcceptGift(bot, args[1]),

                "DECLINEGIFT" or
                "DG" when argLength > 2 && access >= EAccess.Master =>
                    Inventory.Command.ResponseDeclinetGift(args[1], Utilities.GetArgsAsText(args, 2, ","), null),
                "DECLINEGIFT" or
                "DG" when argLength == 2 && access >= EAccess.Master =>
                    Inventory.Command.ResponseDeclinetGift(bot, args[1], null),

                "TRADEOFFERS" or
                "TRADEOFFER" or
                "TO" when access >= EAccess.Operator =>
                    Inventory.Command.ResponseGetTradeOffers(Utilities.GetArgsAsText(args, 1, ",")),

                "ACCEPTOFFER" or
                "AO" when argLength > 2 && access >= EAccess.Master =>
                    Inventory.Command.ResponseDoTradeOffers(args[1], Utilities.GetArgsAsText(args, 2, ","), true),
                "ACCEPTOFFER" or
                "AO" when argLength == 2 && access >= EAccess.Master =>
                    Inventory.Command.ResponseDoTradeOffers(bot, args[1], true),

                "CANCELOFFER" or
                "CO" when argLength > 2 && access >= EAccess.Master =>
                    Inventory.Command.ResponseDoTradeOffers(args[1], Utilities.GetArgsAsText(args, 2, ","), false),
                "CANCELOFFER" or
                "CO" when argLength == 2 && access >= EAccess.Master =>
                    Inventory.Command.ResponseDoTradeOffers(bot, args[1], false),


                //DevFuture
                "COOKIES" when Config.DevFeature && access >= EAccess.Owner =>
                    DevFeature.Command.ResponseGetCookies(Utilities.GetArgsAsText(args, 1, ",")),
                "ACCESSTOKEN" when Config.DevFeature && access >= EAccess.Owner =>
                    DevFeature.Command.ResponseGetAccessToken(Utilities.GetArgsAsText(args, 1, ",")),

                //Limited Tips
                "FOLLOWCURATOR" or
                "FCU" or
                "UNFOLLOWCURATOR" or
                "UFCU" or
                "CURATORLIST" or
                "CL" or
                "JOINGROUP" or
                "JG" or
                "LEAVEGROUP" or
                "LG" or
                "GROUPLIST" or
                "GL" or
                "DELETERECOMMENT" or
                "DREC" or
                "PUBLISHRECOMMEND" or
                "PREC" when !Config.EULA && argLength >= 1 && access >= EAccess.Master =>
                    Task.FromResult(Other.Command.ResponseEulaCmdUnavilable()),

                "COOKIES" or
                "ACCESSTOKEN" when Config.DevFeature && access >= EAccess.Owner =>
                    Task.FromResult(Other.Command.ResponseDevFeatureUnavilable()),

                _ => null,
            }
        };
    }

    /// <summary>
    /// 处理命令事件
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="access"></param>
    /// <param name="message"></param>
    /// <param name="args"></param>
    /// <param name="steamId"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<string?> OnBotCommand(Bot bot, EAccess access, string message, string[] args, ulong steamId = 0)
    {
        if (!Enum.IsDefined(access))
        {
            throw new InvalidEnumArgumentException(nameof(access), (int)access, typeof(EAccess));
        }

        string? moduleName = null;

        try
        {
            var cmd = args[0].ToUpperInvariant();

            //跳过禁用命令
            if (IsCmdDisabled(cmd) == true)
            {
                ASFLogger.LogGenericInfo(string.Format(Langs.CommandDisabled, cmd));
                return null;
            }

            var splits = cmd.Split('.', 2, StringSplitOptions.RemoveEmptyEntries);

            Task<string?>? task = null;

            if (splits.Length > 1) //指定插件名称
            {
                var pluginName = splits[0];
                cmd = splits[1];

                if (pluginName == "ASFE" || pluginName == "ASFENHANCE") //调用插件命令
                {
                    moduleName = Name;
                    task = ResponseCommand(bot, access, cmd, message, args, steamId);
                }
                else if (_Adapter_.ExtensionCore.HasSubModule) //调用外部模块命令
                {
                    (moduleName, task) = _Adapter_.ExtensionCore.ExecuteCommand(pluginName, cmd, bot, access, message, args, steamId);
                }
            }
            else //未指定插件名称
            {
                moduleName = Name;
                task = ResponseCommand(bot, access, cmd, message, args, steamId);

                if (task == null && _Adapter_.ExtensionCore.HasSubModule)
                {
                    //如果本插件未调用则调用外部插件命令
                    (moduleName, task) = _Adapter_.ExtensionCore.ExecuteCommand(cmd, bot, access, message, args, steamId);
                }
            }

            if (task != null) //如果有匹配的执行器则执行
            {
                return await task.ConfigureAwait(false);
            }
            else //显示命令帮助
            {
                return Other.Command.ShowUsageIfAvilable(cmd);
            }
        }
        catch (AccessTokenNullException ex)
        {
            return bot.FormatBotResponse(Langs.AccessTokenIsNullWarnMessage, ex.Message);
        }
        catch (MissingMethodException ex)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Detected [Missing Method Exception] error , Use ASF-generic version may help");
            sb.AppendLine("检测到 [Missing Method Exception] 错误, 换成 ASF-generic 版本可能可以修正");
            sb.AppendLine(Static.Line);
            sb.AppendLine(ex.StackTrace);
            return FormatStaticResponse(sb.ToString());
        }
        catch (Exception ex) //错误日志
        {
            var cfg = Config.ToJsonText();

            if (!string.IsNullOrEmpty(Config.ApiKey))
            {
                cfg = cfg.Replace(Config.ApiKey, "**hidden**");
            }

            var sb = new StringBuilder();
            sb.AppendLineFormat(Langs.ErrorLogTitle, moduleName);
            sb.AppendLine(Static.Line);
            sb.AppendLineFormat(Langs.ErrorLogOriginMessage, message);
            sb.AppendLineFormat(Langs.ErrorLogAccess, access);
            sb.AppendLineFormat(Langs.ErrorLogASFVersion, ASFVersion);
            sb.AppendLineFormat(Langs.ErrorLogPluginVersion, MyVersion);
            sb.AppendLine(Static.Line);
            sb.AppendLine(cfg);
            sb.AppendLine(Static.Line);
            sb.AppendLineFormat(Langs.ErrorLogErrorName, ex.GetType());
            sb.AppendLineFormat(Langs.ErrorLogErrorMessage, ex.Message);
            sb.AppendLine(ex.StackTrace);

            _ = Task.Run(async () =>
            {
                await Task.Delay(500).ConfigureAwait(false);
                sb.Insert(0, '\n');
                ASFLogger.LogGenericError(sb.ToString());
            }).ConfigureAwait(false);

            return sb.ToString();
        }
    }

    /// <summary>
    /// 响应添加好友请求
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="steamId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<bool> OnBotFriendRequest(Bot bot, ulong steamId)
    {
        if (Bot.BotsReadOnly != null)
        {
            foreach (var (_, b) in Bot.BotsReadOnly)
            {
                if (b.SteamID == steamId)
                {
                    ASFLogger.LogGenericInfo(string.Format(Langs.AcceptFriendRequest, bot.BotName, steamId));
                    return Task.FromResult(true);
                }
            }
        }
        return Task.FromResult(false);
    }

    /// <inheritdoc/>
    public Task OnBotInitModules(Bot bot, IReadOnlyDictionary<string, JsonElement>? additionalConfigProperties)
    {
        if (additionalConfigProperties != null)
        {
            foreach (var (configProperty, configValue) in additionalConfigProperties)
            {
                if (configProperty == "UserCountry" && configValue.ValueKind == JsonValueKind.String)
                {
                    var countryCode = configValue.GetString();
                    if (!string.IsNullOrEmpty(countryCode))
                    {
                        CustomUserCountry.TryAdd(bot, countryCode.ToUpperInvariant());
                    }
                    break;
                }
            }
        }
        return Task.CompletedTask;
    }
}
