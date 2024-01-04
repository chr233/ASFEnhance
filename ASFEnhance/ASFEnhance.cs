using ArchiSteamFarm.Core;
using ArchiSteamFarm.Plugins.Interfaces;
using ArchiSteamFarm.Steam;
using ASFEnhance.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Composition;
using System.Text;

namespace ASFEnhance;

[Export(typeof(IPlugin))]
internal sealed class ASFEnhance : IASF, IBotCommand2, IBotFriendRequest
{
    public string Name => nameof(ASFEnhance);
    public Version Version => MyVersion;

    [JsonProperty]
    public static PluginConfig Config => Utils.Config;

    private Timer? StatisticTimer { get; set; }

    private Timer? ClaimItemTimer { get; set; }

    /// <summary>
    /// ASF启动事件
    /// </summary>
    /// <param name="additionalConfigProperties"></param>
    /// <returns></returns>
    public Task OnASFInit(IReadOnlyDictionary<string, JToken>? additionalConfigProperties = null)
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
        message.AppendLine(Langs.SubModulePluginVersion);
        message.AppendLine(Langs.SubModulePluginUpdate);
        message.AppendLine(Langs.SubModuleCmdTips);
        message.AppendLine(Static.Line);

        ASFLogger.LogGenericInfo(message.ToString());

        PluginConfig? config = null;

        if (additionalConfigProperties != null)
        {
            foreach ((string configProperty, JToken configValue) in additionalConfigProperties)
            {
                if (configProperty == "ASFEnhance" && configValue.Type == JTokenType.Object)
                {
                    try
                    {
                        config = configValue.ToObject<PluginConfig>();
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
            var request = new Uri("https://asfe.chrxw.com/asfenhace");
            if (_Adapter_.ExtensionCore.HasSubModule)
            {
                List<string>? names = ["asfenhance"];
                foreach (var subModules in _Adapter_.ExtensionCore.SubModules.Keys)
                {
                    names.Add(subModules.ToLowerInvariant());
                }
                request = new Uri(request, string.Join('+', names));
            }

            StatisticTimer = new Timer(
                async (_) => await ASF.WebBrowser!.UrlGetToHtmlDocument(request).ConfigureAwait(false),
                null,
                TimeSpan.FromSeconds(30),
                TimeSpan.FromHours(24)
            );
        }

        //禁用命令
        if (Config.DisabledCmds != null)
        {
            var disabledCmds = new HashSet<string>();
            foreach (var cmd in Config.DisabledCmds)
            {
                disabledCmds.Add(cmd.ToUpperInvariant());
            }
            Config.DisabledCmds = disabledCmds;
        }

        if (!string.IsNullOrEmpty(Config.AutoClaimItemBotNames))
        {
            ClaimItemTimer = new Timer(
                async (_) =>
                {
                    var bots = Bot.GetBots(Config.AutoClaimItemBotNames);
                    if (bots == null || bots.Count == 0)
                    {
                        return;
                    }
                    foreach (var bot in bots)
                    {
                        var result = await Event.Command.ResponseClaimItem(bot).ConfigureAwait(false);
                        ASFLogger.LogGenericInfo(result ?? "Null");
                        await Task.Delay(TimeSpan.FromSeconds(10)).ConfigureAwait(false);
                    }
                }
                , null,
                TimeSpan.FromHours(1),
                TimeSpan.FromHours(Math.Max(Config.AutoClaimItemPeriod, 8))
            );
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// 插件加载事件
    /// </summary>
    /// <returns></returns>
    public Task OnLoaded()
    {
        foreach (var backupPath in Directory.GetFiles(MyDirectory, "*.autobak"))
        {
            try
            {
                File.Delete(backupPath);
                ASFLogger.LogGenericInfo(string.Format(Langs.SubModuleDeleteOldPluginSuccess, backupPath));
            }
            catch (Exception)
            {
                ASFLogger.LogGenericWarning(string.Format(Langs.SubModuleDeleteOldPluginFailed, backupPath));
            }
        }

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

        return argLength switch
        {
            0 => throw new InvalidOperationException(nameof(args)),
            1 => cmd switch //不带参数
            {
                //Plugin Info
                "ASFENHANCE" or
                "ASFE" when access >= EAccess.FamilySharing =>
                    Task.FromResult(PluginInfo),

                //Update
                "ASFEUPDATE" or
                "AU" or
                "ASFEVERSION" or
                "AV" when access >= EAccess.Operator =>
                    Task.FromResult(Update.Command.ResponseOldCmdTips()),

                "PLUGINSLIST" or
                "PLUGINLIST" or
                "PL" when access >= EAccess.Operator =>
                    Task.FromResult(Update.Command.ResponsePluginList()),

                "PLUGINSVERSION" or
                "PLUGINVERSION" or
                "PV" when access >= EAccess.Master =>
                    Update.Command.ResponseGetPluginLatestVersion(null),

                "PLUGINSUPDATE" or
                "PLUGINUPDATE" or
                "PU" when access >= EAccess.Master =>
                    Update.Command.ResponsePluginUpdate(null),

                //Event
                "SIM4" when access >= EAccess.Operator =>
                    Event.Command.ResponseSim4(bot),

                "DL2" when access >= EAccess.Operator =>
                    Event.Command.ResponseDL2(bot),

                "DL22" when access >= EAccess.Operator =>
                    Event.Command.ResponseDL22(bot, null),

                "RLE" when access >= EAccess.Operator =>
                    Event.Command.ResponseRle(bot, null),

                "CLAIMITEM" or
                "CI" when access >= EAccess.Operator =>
                    Event.Command.ResponseClaimItem(bot),

                "CLAIM20TH" or
                "C20" when access >= EAccess.Operator =>
                    Event.Command.ResponseClaim20Th(bot),

                //"V" or
                //"VOTE" when access >= EAccess.Operator =>
                //    Event.Command.ResponseWinterSteamAwardVote(bot, ""),

                "CV" or
                "CHECKVOTE" when access >= EAccess.Operator =>
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

                "REMOVEDEMOS" or
                "REMOVEDEMO" or
                "RD" when access >= EAccess.Master =>
                    Account.Command.ResponseRemoveAllDemos(bot),

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

                //Cart
                "CART" or
                "C" when access >= EAccess.Operator =>
                    Cart.Command.ResponseGetCartGames(bot),

                "CARTCOUNTRY" or
                "CC" when access >= EAccess.Operator =>
                    Cart.Command.ResponseGetCartCountries(bot),

                "CARTRESET" or
                "CR" when access >= EAccess.Operator =>
                    Cart.Command.ResponseClearCartGames(bot),

                "DIGITALGIFTCARDOPTION" or
                "DGCO" when access >= EAccess.Operator =>
                    Cart.Command.ResponseGetDigitalGiftCcardOptions(bot),

                "FAKEPURCHASE" or
                "FPC" when access >= EAccess.Master =>
                    Cart.Command.ResponseFakePurchaseSelf(bot),

                "PURCHASE" or
                "PC" when access >= EAccess.Master =>
                    Cart.Command.ResponsePurchaseSelf(bot),

                //Community
                "CLEARNOTIFICATION" or
                "CN" when access >= EAccess.Operator =>
                    Community.Command.ResponseClearNotification(bot),

                //Curasor
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
                    Task.FromResult(Explorer.Command.ResponseExploreDiscoveryQueue(bot)),

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
                    Task.FromResult(Profile.Command.ResponseGetProfileLink(bot)),

                "RANDOMGAMEAVATAR" or
                "RGA" when access >= EAccess.Master =>
                    Profile.Command.ResponseSetProfileGameAvatar(bot, null, null),

                "REPLAY" or
                "RP" when access >= EAccess.Operator =>
                    Profile.Command.ResponseGetReplay(bot, "2023"),

                "TRADELINK" or
                "TL" when access >= EAccess.Operator =>
                    Profile.Command.ResponseGetTradeLink(bot),

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
                //Update
                "PLUGINSVERSION" or
                "PLUGINVERSION" or
                "PV" when access >= EAccess.Master =>
                    Update.Command.ResponseGetPluginLatestVersion(Utilities.GetArgsAsText(args, 1, ",")),

                "PLUGINSUPDATE" or
                "PLUGINUPDATE" or
                "PU" when access >= EAccess.Master =>
                    Update.Command.ResponsePluginUpdate(Utilities.GetArgsAsText(args, 1, ",")),

                //Event
                "SIM4" when access >= EAccess.Operator =>
                    Event.Command.ResponseSim4(Utilities.GetArgsAsText(args, 1, ",")),

                "DL2" when access >= EAccess.Operator =>
                    Event.Command.ResponseDL2(Utilities.GetArgsAsText(args, 1, ",")),

                "DL22" when argLength > 2 && access >= EAccess.Operator =>
                    Event.Command.ResponseDL22(SkipBotNames(args, 1, 1), args.Last()),
                "DL22" when access >= EAccess.Operator =>
                    Event.Command.ResponseDL22(args[1], null),

                "RLE" when argLength > 2 && access >= EAccess.Operator =>
                    Event.Command.ResponseRle(SkipBotNames(args, 1, 1), args.Last()),
                "RLE" when access >= EAccess.Operator =>
                    Event.Command.ResponseRle(args[1], null),

                "CLAIMITEM" or
                "CI" when access >= EAccess.Operator =>
                    Event.Command.ResponseClaimItem(Utilities.GetArgsAsText(args, 1, ",")),

                "CLAIM20TH" or
                "C20" when access >= EAccess.Operator =>
                    Event.Command.ResponseClaim20Th(Utilities.GetArgsAsText(args, 1, ",")),

                //"V" or
                //"VOTE" when argLength > 2 && access >= EAccess.Operator =>
                //     Event.Command.ResponseWinterSteamAwardVote(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                //"V" or
                //"VOTE" when access >= EAccess.Operator =>
                //    Event.Command.ResponseWinterSteamAwardVote(args[1], ""),

                "CV" or
                "CHECKVOTE" when access >= EAccess.Operator =>
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

                "REMOVEDEMOS" or
                "REMOVEDEMO" or
                "RD" when access >= EAccess.Master =>
                    Account.Command.ResponseRemoveAllDemos(Utilities.GetArgsAsText(args, 1, ",")),

                "REMOVELICENSES" or
                "REMOVELICENSE" or
                "RL" when argLength > 2 && access >= EAccess.Master =>
                    Account.Command.ResponseRemoveFreeLicenses(args[1], Utilities.GetArgsAsText(args, 2, ",")),

                "REMOVELICENSES" or
                "REMOVELICENSE" or
                "RL" when access >= EAccess.Master =>
                    Account.Command.ResponseRemoveFreeLicenses(bot, args[1]),

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
                    Account.Command.ResponseSteamidAccountBanned(Utilities.GetArgsAsText(args, 1, ",")),

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

                //Cart
                "CART" or
                "C" when access >= EAccess.Operator =>
                    Cart.Command.ResponseGetCartGames(Utilities.GetArgsAsText(args, 1, ",")),

                "ADDCART" or
                "AC" when argLength > 2 && access >= EAccess.Operator =>
                    Cart.Command.ResponseAddCartGames(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "ADDCART" or
                "AC" when access >= EAccess.Operator =>
                    Cart.Command.ResponseAddCartGames(bot, args[1]),

                "CARTCOUNTRY" or
                "CC" when access >= EAccess.Operator =>
                    Cart.Command.ResponseGetCartCountries(Utilities.GetArgsAsText(args, 1, ",")),

                "CARTRESET" or
                "CR" when access >= EAccess.Operator =>
                    Cart.Command.ResponseClearCartGames(Utilities.GetArgsAsText(args, 1, ",")),

                "DIGITALGIFTCARDOPTION" or
                "DGCO" when access >= EAccess.Operator =>
                    Cart.Command.ResponseGetDigitalGiftCcardOptions(Utilities.GetArgsAsText(args, 1, ",")),

                "SENDDIGITALGIFTCARD" or
                "SDGC" when argLength >= 4 && access >= EAccess.Operator =>
                    Cart.Command.ResponseSendDigitalGiftCardBot(args[1], SkipBotNames(args, 2, 1), args.Last()),
                "SENDDIGITALGIFTCARD" or
                "SDGC" when argLength >= 3 && access >= EAccess.Operator =>
                    Cart.Command.ResponseSendDigitalGiftCardBot(bot, args[1], args[2]),

                "FAKEPURCHASE" or
                "FPC" when access >= EAccess.Master =>
                    Cart.Command.ResponseFakePurchaseSelf(Utilities.GetArgsAsText(args, 1, ",")),

                "PURCHASE" or
                "PC" when access >= EAccess.Master =>
                    Cart.Command.ResponsePurchaseSelf(Utilities.GetArgsAsText(args, 1, ",")),

                "PURCHASEGIFT" or
                "PCG" when argLength == 3 && access >= EAccess.Master =>
                    Cart.Command.ResponsePurchaseGift(args[1], args[2]),
                "PURCHASEGIFT" or
                "PCG" when argLength == 2 && access >= EAccess.Master =>
                    Cart.Command.ResponsePurchaseGift(bot, args[1]),

                //Community
                "CLEARNOTIFICATION" or
                "CN" when access >= EAccess.Operator =>
                    Community.Command.ResponseClearNotification(Utilities.GetArgsAsText(args, 1, ",")),

                //Curasor
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
                    Task.FromResult(Other.Command.ResponseDumpToFile(bot, access, Utilities.GetArgsAsText(args, 1, ","), steamId)),

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
                    Profile.Command.ResponseGetReplay(args[1], "2023"),

                "REPLAYPRIVACY" or
                "RPP" when argLength > 3 && access >= EAccess.Operator =>
                    Profile.Command.ResponseSetReplayPrivacy(args[2], Utilities.GetArgsAsText(args, 3, ","), args[1]),
                "REPLAYPRIVACY" or
                "RPP" when argLength == 3 && access >= EAccess.Operator =>
                    Profile.Command.ResponseSetReplayPrivacy(args[1], Utilities.GetArgsAsText(args, 2, ","), "2023"),
                "REPLAYPRIVACY" or
                "RPP" when access >= EAccess.Operator =>
                    Profile.Command.ResponseSetReplayPrivacy(bot, args[1], "2023"),

                "TRADELINK" or
                "TL" when access >= EAccess.Operator =>
                    Profile.Command.ResponseGetTradeLink(Utilities.GetArgsAsText(args, 1, ",")),

                //Store
                "APPDETAIL" or
                "AD" when argLength > 2 && access >= EAccess.Operator =>
                    Store.Command.ResponseGetAppsDetail(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "APPDETAIL" or
                "AD" when access >= EAccess.Operator =>
                    Store.Command.ResponseGetAppsDetail(bot, args[1]),

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

                "SUBS" or
                "S" when argLength > 2 && access >= EAccess.Operator =>
                    Store.Command.ResponseGetGameSubes(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "SUBS" or
                "S" when access >= EAccess.Operator =>
                    Store.Command.ResponseGetGameSubes(bot, args[1]),

                "VIEWPAGE" or
                "VP" when argLength > 2 && access >= EAccess.Operator =>
                    Store.Command.ResponseViewPage(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "VIEWPAGE" or
                "VP" when access >= EAccess.Operator =>
                    Store.Command.ResponseViewPage(bot, args[1]),

                //Wallet
                "REDEEMWALLET" or
                "RWA" when args.Length > 2 && access >= EAccess.Master =>
                    Wallet.Command.ResponseRedeemWallet(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "REDEEMWALLET" or
                "RWA" when access >= EAccess.Master =>
                    Wallet.Command.ResponseRedeemWallet(bot, args[1]),

                "REDEEMWALLETMULT" or
                "RWAM" when access >= EAccess.Master =>
                    Wallet.Command.ResponseRedeemWalletMult(Utilities.GetArgsAsText(args, 1, ",")),

                //WishList
                "ADDWISHLIST" or
                "AW" when argLength > 2 && access >= EAccess.Master =>
                    Wishlist.Command.ResponseAddWishlist(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "ADDWISHLIST" or
                "AW" when access >= EAccess.Master =>
                    Wishlist.Command.ResponseAddWishlist(bot, args[1]),

                "CHECK" or
                "CK" when argLength > 2 && access >= EAccess.Master =>
                    Wishlist.Command.ResponseCheckGame(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "CHECK" or
                "CK" when access >= EAccess.Master =>
                    Wishlist.Command.ResponseCheckGame(bot, args[1]),

                "FOLLOWGAME" or
                "FG" when argLength > 2 && access >= EAccess.Master =>
                    Wishlist.Command.ResponseFollowGame(args[1], Utilities.GetArgsAsText(args, 2, ","), true),
                "FOLLOWGAME" or
                "FG" when access >= EAccess.Master =>
                    Wishlist.Command.ResponseFollowGame(bot, args[1], true),

                "REMOVEWISHLIST" or
                "RW" when argLength > 2 && access >= EAccess.Master =>
                    Wishlist.Command.ResponseRemoveWishlist(args[1], Utilities.GetArgsAsText(args, 2, ",")),
                "REMOVEWISHLIST" or
                "RW" when access >= EAccess.Master =>
                    Wishlist.Command.ResponseRemoveWishlist(bot, args[1]),

                "UNFOLLOWGAME" or
                "UFG" when argLength > 2 && access >= EAccess.Master =>
                    Wishlist.Command.ResponseFollowGame(args[1], Utilities.GetArgsAsText(args, 2, ","), false),
                "UNFOLLOWGAME" or
                "UFG" when access >= EAccess.Master =>
                    Wishlist.Command.ResponseFollowGame(bot, args[1], false),

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
                    task = ResponseCommand(bot, access, cmd, message, args, steamId);
                }
                else if (_Adapter_.ExtensionCore.HasSubModule) //调用外部模块命令
                {
                    task = _Adapter_.ExtensionCore.ExecuteCommand(pluginName, cmd, bot, access, message, args, steamId);
                }
            }
            else //未指定插件名称
            {
                task = ResponseCommand(bot, access, cmd, message, args, steamId);

                if (task == null && _Adapter_.ExtensionCore.HasSubModule)
                {
                    //如果本插件未调用则调用外部插件命令
                    task = _Adapter_.ExtensionCore.ExecuteCommand(cmd, bot, access, message, args, steamId);
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
        catch (Exception ex) //错误日志
        {
            var cfg = JsonConvert.SerializeObject(Config, Formatting.Indented);

            var sb = new StringBuilder();
            sb.AppendLine(Langs.ErrorLogTitle);
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
        var bots = Bot.GetBots("ASF")?.Select(static b => b.SteamID).ToList();
        bool approve = bots?.Contains(steamId) ?? false;

        if (approve)
        {
            ASFLogger.LogGenericInfo(string.Format(Langs.AcceptFriendRequest, bot.BotName, steamId));
        }

        return Task.FromResult(approve);
    }
}
