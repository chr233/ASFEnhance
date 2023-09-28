using ArchiSteamFarm.Core;
using ArchiSteamFarm.Plugins.Interfaces;
using ArchiSteamFarm.Steam;
using ASFEnhance.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Composition;
using System.Text;

namespace ASFEnhance;

[Export(typeof(IPlugin))]
internal sealed class ASFEnhance : IASF, IBotCommand2, IBotFriendRequest, IWebInterface
{
    public string Name => nameof(ASFEnhance);
    public Version Version => MyVersion;

    [JsonProperty]
    public static PluginConfig Config => Utils.Config;

    private Timer? StatisticTimer { get; set; }

    public string PhysicalPath => "www";

    public string WebPath => "/";

    /// <summary>
    /// ASF启动事件
    /// </summary>
    /// <param name="additionalConfigProperties"></param>
    /// <returns></returns>
    public Task OnASFInit(IReadOnlyDictionary<string, JToken>? additionalConfigProperties = null)
    {
        var sb = new StringBuilder();

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

        Utils.Config = config ?? new();

        //开发者特性
        if (Config.DevFeature)
        {
            sb.AppendLine();
            sb.AppendLine(Static.Line);
            sb.AppendLine(Langs.DevFeatureEnabledWarning);
            sb.AppendLine(Static.Line);
        }
        //使用协议
        if (!Config.EULA)
        {
            sb.AppendLine();
            sb.AppendLine(Static.Line);
            sb.AppendLine(Langs.EulaWarning);
            sb.AppendLine(Static.Line);
        }
        //地址信息
        if (Config.Addresses == null)
        {
            Config.Addresses = new();
        }
        if (Config.Address != null)
        {
            Config.Addresses.Add(Config.Address);
            Config.Address = null;
        }

        if (sb.Length > 0)
        {
            ASFLogger.LogGenericWarning(sb.ToString());
        }
        //统计
        if (Config.Statistic)
        {
            var request = new Uri("https://asfe.chrxw.com/asfenhace");
            StatisticTimer = new Timer(
                async (_) => await ASF.WebBrowser!.UrlGetToHtmlDocument(request).ConfigureAwait(false),
                null,
                TimeSpan.FromSeconds(30),
                TimeSpan.FromHours(24)
            );
        }
        //禁用命令
        if (Config.DisabledCmds == null)
        {
            Config.DisabledCmds = new();
        }
        else
        {
            for (int i = 0; i < Config.DisabledCmds.Count; i++)
            {
                Config.DisabledCmds[i] = Config.DisabledCmds[i].ToUpperInvariant();
            }
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// 插件加载事件
    /// </summary>
    /// <returns></returns>
    public Task OnLoaded()
    {
        var message = new StringBuilder("\n");
        message.AppendLine(Static.Line);
        message.AppendLine(Static.Logo);
        message.AppendLine(Static.Line);
        message.AppendLine(string.Format(Langs.PluginVer, nameof(ASFEnhance), MyVersion.ToString()));
        message.AppendLine(Langs.PluginContact);
        message.AppendLine(Langs.PluginInfo);
        message.AppendLine(Static.Line);

        string pluginFolder = Path.GetDirectoryName(MyLocation) ?? ".";
        string backupPath = Path.Combine(pluginFolder, $"{nameof(ASFEnhance)}.bak");
        bool existsBackup = File.Exists(backupPath);
        if (existsBackup)
        {
            try
            {
                File.Delete(backupPath);
                message.AppendLine(Langs.CleanUpOldBackup);
            }
            catch (Exception e)
            {
                ASFLogger.LogGenericException(e);
                message.AppendLine(Langs.CleanUpOldBackupFailed);
            }
        }
        else
        {
            message.AppendLine(Langs.ASFEVersionTips);
            message.AppendLine(Langs.ASFEUpdateTips);
        }

        message.AppendLine(Static.Line);

        ASFLogger.LogGenericInfo(message.ToString());

        return Task.CompletedTask;
    }

    /// <summary>
    /// 处理命令
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="access"></param>
    /// <param name="message"></param>
    /// <param name="args"></param>
    /// <param name="steamId"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static Task<string?>? ResponseCommand(Bot bot, EAccess access, string message, string[] args, ulong steamId)
    {
        var cmd = args[0].ToUpperInvariant();

        if (cmd.StartsWith("ASFE."))
        {
            cmd = cmd[5..];
        }
        else
        {
            //跳过禁用命令
            if (Config.DisabledCmds?.Contains(cmd) == true)
            {
                ASFLogger.LogGenericInfo("Command {0} is disabled!");
                return null;
            }
        }

        var argLength = args.Length;
        switch (argLength)
        {
            case 0:
                throw new InvalidOperationException(nameof(args));
            case 1: //不带参数
                switch (cmd)
                {
                    //Event
                    case "SIM4" when access >= EAccess.Operator:
                        return Event.Command.ResponseSim4(bot);

                    case "DL2" when access >= EAccess.Operator:
                        return Event.Command.ResponseDL2(bot);

                    case "DL22" when access >= EAccess.Operator:
                        return Event.Command.ResponseDL22(bot, null);

                    case "RLE" when access >= EAccess.Operator:
                        return Event.Command.ResponseRle(bot, null);

                    case "CLAIMITEM" when access >= EAccess.Operator:
                    case "CI" when access >= EAccess.Operator:
                        return Event.Command.ResponseClaimItem(bot);

                    case "CLAIM20TH" when access >= EAccess.Operator:
                    case "C20" when access >= EAccess.Operator:
                        return Event.Command.ResponseClaim20Th(bot);

                    //Shortcut
                    case "P":
                        return bot.Commands.Response(access, "POINTS", steamId);
                    case "PA":
                        return bot.Commands.Response(access, "POINTS ASF", steamId);
                    case "LA":
                        return bot.Commands.Response(access, "LEVEL ASF", steamId);
                    case "BA":
                        return bot.Commands.Response(access, "BALANCE ASF", steamId);
                    case "CA":
                        return bot.Commands.Response(access, "CART ASF", steamId);

                    //Account
                    case "PURCHASEHISTORY" when access >= EAccess.Operator:
                    case "PH" when access >= EAccess.Operator:
                        return Account.Command.ResponseAccountHistory(bot);

                    case "FREELICENSES" when access >= EAccess.Operator:
                    case "FREELICENSE" when access >= EAccess.Operator:
                    case "FL" when access >= EAccess.Operator:
                        return Account.Command.ResponseGetAccountLicenses(bot, true);

                    case "LICENSES" when access >= EAccess.Operator:
                    case "LICENSE" when access >= EAccess.Operator:
                    case "L" when access >= EAccess.Operator:
                        return Account.Command.ResponseGetAccountLicenses(bot, false);

                    case "REMOVEDEMOS" when access >= EAccess.Master:
                    case "REMOVEDEMO" when access >= EAccess.Master:
                    case "RD" when access >= EAccess.Master:
                        return Account.Command.ResponseRemoveAllDemos(bot);

                    case "EMAILOPTIONS" when access >= EAccess.Operator:
                    case "EMAILOPTION" when access >= EAccess.Operator:
                    case "EO" when access >= EAccess.Operator:
                        return Account.Command.ResponseGetEmailOptions(bot);

                    case "NOTIFICATIONOPTIONS" when access >= EAccess.Operator:
                    case "NOTIFICATIONOPTION" when access >= EAccess.Operator:
                    case "NOO" when access >= EAccess.Operator:
                        return Account.Command.ResponseGetNotificationOptions(bot);

                    case "GETBOTBANNED" when access >= EAccess.Operator:
                    case "GETBOTBAN" when access >= EAccess.Operator:
                    case "GBB" when access >= EAccess.Operator:
                        return Account.Command.ResponseGetAccountBanned(bot, null);

                    case "RECEIVEGIFT" when access >= EAccess.Operator:
                    case "RG" when access >= EAccess.Operator:
                        return Account.Command.ResponseReceiveGift(bot);

                    //Cart
                    case "CART" when access >= EAccess.Operator:
                    case "C" when access >= EAccess.Operator:
                        return Cart.Command.ResponseGetCartGames(bot);

                    case "CARTCOUNTRY" when access >= EAccess.Operator:
                    case "CC" when access >= EAccess.Operator:
                        return Cart.Command.ResponseGetCartCountries(bot);

                    case "CARTRESET" when access >= EAccess.Operator:
                    case "CR" when access >= EAccess.Operator:
                        return Cart.Command.ResponseClearCartGames(bot);

                    case "DIGITALGIFTCARDOPTION" when access >= EAccess.Operator:
                    case "DGCO" when access >= EAccess.Operator:
                        return Cart.Command.ResponseGetDigitalGiftCcardOptions(bot);

                    case "FAKEPURCHASE" when access >= EAccess.Master:
                    case "FPC" when access >= EAccess.Master:
                        return Cart.Command.ResponseFakePurchaseSelf(bot);

                    case "PURCHASE" when access >= EAccess.Master:
                    case "PC" when access >= EAccess.Master:
                        return Cart.Command.ResponsePurchaseSelf(bot);

                    //Community
                    case "CLEARNOTIFICATION" when access >= EAccess.Operator:
                    case "CN" when access >= EAccess.Operator:
                        return Community.Command.ResponseClearNotification(bot);

                    //Curasor
                    case "CURATORLIST" when Config.EULA && access >= EAccess.Master:
                    case "CL" when Config.EULA && access >= EAccess.Master:
                        return Curator.Command.ResponseGetFollowingCurators(bot);

                    case "UNFOLLOWALLCURASOR" when Config.EULA && access >= EAccess.Master:
                    case "UNFOLLOWALLCURASORS" when Config.EULA && access >= EAccess.Master:
                    case "UFACU" when Config.EULA && access >= EAccess.Master:
                        return Curator.Command.ResponseUnFollowAllCurators(bot);

                    //Explorer
                    case "EXPLORER" when access >= EAccess.Master:
                    case "EX" when access >= EAccess.Master:
                        return Task.FromResult(Explorer.Command.ResponseExploreDiscoveryQueue(bot));

                    //Friend
                    case "DELETEALLFRIEND" when access >= EAccess.Master:
                        return Friend.Command.ResponseDeleteAllFriend(bot);

                    case "INVITELINK" when access >= EAccess.Operator:
                    case "IL" when access >= EAccess.Operator:
                        return Friend.Command.ResponseGetInviteLink(bot);

                    //Group
                    case "GROUPLIST" when Config.EULA && access >= EAccess.FamilySharing:
                    case "GL" when Config.EULA && access >= EAccess.FamilySharing:
                        return Group.Command.ResponseGroupList(bot);

                    //Other
                    case "ASFEHELP":
                    case "EHELP":
                        return Task.FromResult(Other.Command.ResponseAllCommands());

                    //Profile
                    case "CLEARALIAS" when access >= EAccess.Operator:
                        return Profile.Command.ResponseClearAliasHistory(bot);

                    case "CRAFTBADGE" when access >= EAccess.Master:
                    case "CRAFTBADGES" when access >= EAccess.Master:
                    case "CB" when access >= EAccess.Master:
                        return Profile.Command.ResponseCraftBadge(bot);

                    case "DELETEAVATAR" when access >= EAccess.Master:
                        return Profile.Command.ResponseDelProfileAvatar(bot);

                    case "FRIENDCODE" when access >= EAccess.FamilySharing:
                    case "FC" when access >= EAccess.FamilySharing:
                        return Task.FromResult(Profile.Command.ResponseGetFriendCode(bot));

                    case "STEAMID" when access >= EAccess.FamilySharing:
                    case "SID" when access >= EAccess.FamilySharing:
                        return Task.FromResult(Profile.Command.ResponseGetSteamId(bot));

                    case "PROFILE" when access >= EAccess.FamilySharing:
                    case "PF" when access >= EAccess.FamilySharing:
                        return Profile.Command.ResponseGetProfileSummary(bot);

                    case "PROFILELINK" when access >= EAccess.FamilySharing:
                    case "PFL" when access >= EAccess.FamilySharing:
                        return Task.FromResult(Profile.Command.ResponseGetProfileLink(bot));

                    case "RANDOMGAMEAVATAR" when access >= EAccess.Master:
                    case "RGA" when access >= EAccess.Master:
                        return Profile.Command.ResponseSetProfileGameAvatar(bot, null, null);

                    case "REPLAY" when access >= EAccess.Operator:
                    case "RP" when access >= EAccess.Operator:
                        return Profile.Command.ResponseGetReplay(bot);

                    case "TRADELINK" when access >= EAccess.Operator:
                    case "TL" when access >= EAccess.Operator:
                        return Profile.Command.ResponseGetTradeLink(bot);

                    //Update
                    case "ASFENHANCE" when access >= EAccess.FamilySharing:
                    case "ASFE" when access >= EAccess.FamilySharing:
                        return Task.FromResult(Update.Command.ResponseASFEnhanceVersion());

                    case "ASFEVERSION" when access >= EAccess.Operator:
                    case "AV" when access >= EAccess.Operator:
                        return Update.Command.ResponseCheckLatestVersion();

                    case "ASFEUPDATE" when access >= EAccess.Owner:
                    case "AU" when access >= EAccess.Owner:
                        return Update.Command.ResponseUpdatePlugin();

                    //DevFuture
                    case "COOKIES" when Config.DevFeature && access >= EAccess.Owner:
                        return Task.FromResult(DevFeature.Command.ResponseGetCookies(bot));
                    case "APIKEY" when Config.DevFeature && access >= EAccess.Owner:
                        return DevFeature.Command.ResponseGetAPIKey(bot);
                    case "ACCESSTOKEN" when Config.DevFeature && access >= EAccess.Owner:
                        return DevFeature.Command.ResponseGetAccessToken(bot);

                    //Limited Tips
                    case "CURATORLIST" when access >= EAccess.Master:
                    case "CL" when access >= EAccess.Master:
                    case "GROUPLIST" when access >= EAccess.Master:
                    case "GL" when access >= EAccess.Master:
                        return Task.FromResult(Other.Command.ResponseEulaCmdUnavilable());

                    case "COOKIES" when access >= EAccess.Owner:
                    case "APIKEY" when access >= EAccess.Owner:
                    case "ACCESSTOKEN" when access >= EAccess.Owner:
                        return Task.FromResult(Other.Command.ResponseDevFeatureUnavilable());

                    default:
                        return Task.FromResult(Other.Command.ShowUsageIfAvilable(args[0].ToUpperInvariant()));
                }
            default: //带参数
                switch (cmd)
                {
                    //Event
                    case "SIM4" when access >= EAccess.Operator:
                        return Event.Command.ResponseSim4(Utilities.GetArgsAsText(args, 1, ","));

                    case "DL2" when access >= EAccess.Operator:
                        return Event.Command.ResponseDL2(Utilities.GetArgsAsText(args, 1, ","));

                    case "DL22" when argLength > 2 && access >= EAccess.Operator:
                        {
                            string botNames = string.Join(',', args[1..(argLength - 1)]);
                            return Event.Command.ResponseDL22(botNames, args.Last());
                        }
                    case "DL22" when access >= EAccess.Operator:
                        return Event.Command.ResponseDL22(args[1], null);

                    case "RLE" when argLength > 2 && access >= EAccess.Operator:
                        {
                            string botNames = string.Join(',', args[1..(argLength - 1)]);
                            return Event.Command.ResponseRle(botNames, args.Last());
                        }
                    case "RLE" when access >= EAccess.Operator:
                        return Event.Command.ResponseRle(args[1], null);

                    case "CLAIMITEM" when access >= EAccess.Operator:
                    case "CI" when access >= EAccess.Operator:
                        return Event.Command.ResponseClaimItem(Utilities.GetArgsAsText(args, 1, ","));

                    case "CLAIM20TH" when access >= EAccess.Operator:
                    case "C20" when access >= EAccess.Operator:
                        return Event.Command.ResponseClaim20Th(Utilities.GetArgsAsText(args, 1, ","));

                    //Shortcut
                    case "AL":
                        return bot.Commands.Response(access, "ADDLICENSE " + Utilities.GetArgsAsText(message, 1), steamId);
                    case "P":
                        return bot.Commands.Response(access, "POINTS " + Utilities.GetArgsAsText(message, 1), steamId);
                    case "TR":
                        return bot.Commands.Response(access, "TRANSFER " + Utilities.GetArgsAsText(message, 1), steamId);

                    //Account
                    case "PURCHASEHISTORY" when access > EAccess.Operator:
                    case "PH" when access > EAccess.Operator:
                        return Account.Command.ResponseAccountHistory(Utilities.GetArgsAsText(args, 1, ","));

                    case "FREELICENSES" when access >= EAccess.Operator:
                    case "FREELICENSE" when access >= EAccess.Operator:
                    case "FL" when access >= EAccess.Operator:
                        return Account.Command.ResponseGetAccountLicenses(Utilities.GetArgsAsText(args, 1, ","), true);

                    case "LICENSES" when access >= EAccess.Operator:
                    case "LICENSE" when access >= EAccess.Operator:
                    case "L" when access >= EAccess.Operator:
                        return Account.Command.ResponseGetAccountLicenses(Utilities.GetArgsAsText(args, 1, ","), false);

                    case "REMOVEDEMOS" when access >= EAccess.Master:
                    case "REMOVEDEMO" when access >= EAccess.Master:
                    case "RD" when access >= EAccess.Master:
                        return Account.Command.ResponseRemoveAllDemos(Utilities.GetArgsAsText(args, 1, ","));

                    case "REMOVELICENSES" when argLength > 2 && access >= EAccess.Master:
                    case "REMOVELICENSE" when argLength > 2 && access >= EAccess.Master:
                    case "RL" when argLength > 2 && access >= EAccess.Master:
                        return Account.Command.ResponseRemoveFreeLicenses(args[1], Utilities.GetArgsAsText(args, 2, ","));

                    case "REMOVELICENSES" when access >= EAccess.Master:
                    case "REMOVELICENSE" when access >= EAccess.Master:
                    case "RL" when access >= EAccess.Master:
                        return Account.Command.ResponseRemoveFreeLicenses(bot, args[1]);

                    case "EMAILOPTIONS" when access >= EAccess.Operator:
                    case "EMAILOPTION" when access >= EAccess.Operator:
                    case "EO" when access >= EAccess.Operator:
                        return Account.Command.ResponseGetEmailOptions(Utilities.GetArgsAsText(args, 1, ","));

                    case "NOTIFICATIONOPTIONS" when access >= EAccess.Operator:
                    case "NOTIFICATIONOPTION" when access >= EAccess.Operator:
                    case "NOO" when access >= EAccess.Operator:
                        return Account.Command.ResponseGetNotificationOptions(Utilities.GetArgsAsText(args, 1, ","));

                    case "SETEMAILOPTIONS" when argLength > 2 && access >= EAccess.Master:
                    case "SETEMAILOPTION" when argLength > 2 && access >= EAccess.Master:
                    case "SEO" when argLength > 2 && access >= EAccess.Master:
                        return Account.Command.ResponseSetEmailOptions(args[1], Utilities.GetArgsAsText(args, 2, ","));
                    case "SETEMAILOPTIONS" when access >= EAccess.Master:
                    case "SETEMAILOPTION" when access >= EAccess.Master:
                    case "SEO" when access >= EAccess.Master:
                        return Account.Command.ResponseSetEmailOptions(bot, args[1]);

                    case "SETNOTIFICATIONOPTIONS" when argLength > 2 && access >= EAccess.Master:
                    case "SETNOTIFICATIONOPTION" when argLength > 2 && access >= EAccess.Master:
                    case "SNO" when argLength > 2 && access >= EAccess.Master:
                        return Account.Command.ResponseSetNotificationOptions(args[1], Utilities.GetArgsAsText(args, 2, ","));
                    case "SETNOTIFICATIONOPTIONS" when access >= EAccess.Master:
                    case "SETNOTIFICATIONOPTION" when access >= EAccess.Master:
                    case "SNO" when access >= EAccess.Master:
                        return Account.Command.ResponseSetNotificationOptions(bot, args[1]);

                    case "GETBOTBANNED" when access >= EAccess.Operator:
                    case "GETBOTBAN" when access >= EAccess.Operator:
                    case "GBB" when access >= EAccess.Operator:
                        return Account.Command.ResponseGetAccountBanned(Utilities.GetArgsAsText(args, 1, ","));

                    case "GETACCOUNTBANNED" when access >= EAccess.Operator:
                    case "GETACCOUNTBAN" when access >= EAccess.Operator:
                    case "GAB" when access >= EAccess.Operator:
                        return Account.Command.ResponseSteamidAccountBanned(Utilities.GetArgsAsText(args, 1, ","));

                    case "RECEIVEGIFT" when access >= EAccess.Operator:
                    case "RG" when access >= EAccess.Operator:
                        return Account.Command.ResponseReceiveGift(Utilities.GetArgsAsText(args, 1, ","));

                    //Cart
                    case "CART" when access >= EAccess.Operator:
                    case "C" when access >= EAccess.Operator:
                        return Cart.Command.ResponseGetCartGames(Utilities.GetArgsAsText(args, 1, ","));

                    case "ADDCART" when argLength > 2 && access >= EAccess.Operator:
                    case "AC" when argLength > 2 && access >= EAccess.Operator:
                        return Cart.Command.ResponseAddCartGames(args[1], Utilities.GetArgsAsText(args, 2, ","));
                    case "ADDCART" when access >= EAccess.Operator:
                    case "AC" when access >= EAccess.Operator:
                        return Cart.Command.ResponseAddCartGames(bot, args[1]);

                    case "CARTCOUNTRY" when access >= EAccess.Operator:
                    case "CC" when access >= EAccess.Operator:
                        return Cart.Command.ResponseGetCartCountries(Utilities.GetArgsAsText(args, 1, ","));

                    case "CARTRESET" when access >= EAccess.Operator:
                    case "CR" when access >= EAccess.Operator:
                        return Cart.Command.ResponseClearCartGames(Utilities.GetArgsAsText(args, 1, ","));

                    case "DIGITALGIFTCARDOPTION" when access >= EAccess.Operator:
                    case "DGCO" when access >= EAccess.Operator:
                        return Cart.Command.ResponseGetDigitalGiftCcardOptions(Utilities.GetArgsAsText(args, 1, ","));

                    case "SENDDIGITALGIFTCARD" when argLength >= 4 && access >= EAccess.Operator:
                    case "SDGC" when argLength >= 4 && access >= EAccess.Operator:
                        {
                            string botNames = string.Join(',', args[2..(argLength - 1)]);
                            return Cart.Command.ResponseSendDigitalGiftCardBot(args[1], botNames, args.Last());
                        }
                    case "SENDDIGITALGIFTCARD" when argLength >= 3 && access >= EAccess.Operator:
                    case "SDGC" when argLength >= 3 && access >= EAccess.Operator:
                        return Cart.Command.ResponseSendDigitalGiftCardBot(bot, args[1], args[2]);

                    case "FAKEPURCHASE" when access >= EAccess.Master:
                    case "FPC" when access >= EAccess.Master:
                        return Cart.Command.ResponseFakePurchaseSelf(Utilities.GetArgsAsText(args, 1, ","));

                    case "PURCHASE" when access >= EAccess.Master:
                    case "PC" when access >= EAccess.Master:
                        return Cart.Command.ResponsePurchaseSelf(Utilities.GetArgsAsText(args, 1, ","));

                    case "PURCHASEGIFT" when argLength == 3 && access >= EAccess.Master:
                    case "PCG" when argLength == 3 && access >= EAccess.Master:
                        return Cart.Command.ResponsePurchaseGift(args[1], args[2]);
                    case "PURCHASEGIFT" when argLength == 2 && access >= EAccess.Master:
                    case "PCG" when argLength == 2 && access >= EAccess.Master:
                        return Cart.Command.ResponsePurchaseGift(bot, args[1]);

                    //Community
                    case "CLEARNOTIFICATION" when access >= EAccess.Operator:
                    case "CN" when access >= EAccess.Operator:
                        return Community.Command.ResponseClearNotification(Utilities.GetArgsAsText(args, 1, ","));

                    //Curasor
                    case "CURATORLIST" when Config.EULA && access >= EAccess.Master:
                    case "CL" when Config.EULA && access >= EAccess.Master:
                        return Curator.Command.ResponseGetFollowingCurators(Utilities.GetArgsAsText(args, 1, ","));

                    case "FOLLOWCURATOR" when Config.EULA && argLength > 2 && access >= EAccess.Master:
                    case "FCU" when Config.EULA && argLength > 2 && access >= EAccess.Master:
                        return Curator.Command.ResponseFollowCurator(args[1], Utilities.GetArgsAsText(args, 2, ","), true);
                    case "FOLLOWCURATOR" when Config.EULA && access >= EAccess.Master:
                    case "FCU" when Config.EULA && access >= EAccess.Master:
                        return Curator.Command.ResponseFollowCurator(bot, args[1], true);

                    case "UNFOLLOWALLCURASOR" when Config.EULA && access >= EAccess.Master:
                    case "UNFOLLOWALLCURASORS" when Config.EULA && access >= EAccess.Master:
                    case "UFACU" when Config.EULA && access >= EAccess.Master:
                        return Curator.Command.ResponseUnFollowAllCurators(Utilities.GetArgsAsText(args, 1, ","));

                    case "UNFOLLOWCURATOR" when Config.EULA && argLength > 2 && access >= EAccess.Master:
                    case "UFCU" when Config.EULA && argLength > 2 && access >= EAccess.Master:
                        return Curator.Command.ResponseFollowCurator(args[1], Utilities.GetArgsAsText(args, 2, ","), false);
                    case "UNFOLLOWCURATOR" when Config.EULA && access >= EAccess.Master:
                    case "UFCU" when Config.EULA && access >= EAccess.Master:
                        return Curator.Command.ResponseFollowCurator(bot, args[1], false);

                    //Explorer
                    case "EXPLORER" when access >= EAccess.Master:
                    case "EX" when access >= EAccess.Master:
                        return Explorer.Command.ResponseExploreDiscoveryQueue(Utilities.GetArgsAsText(args, 1, ","));

                    //Friend            
                    case "ADDBOTFRIEND" when argLength > 2 && access >= EAccess.Master:
                    case "ABF" when argLength > 2 && access >= EAccess.Master:
                        return Friend.Command.ResponseAddBotFriend(args[1], Utilities.GetArgsAsText(args, 2, ","));
                    case "ADDBOTFRIEND" when access >= EAccess.Master:
                    case "ABF" when access >= EAccess.Master:
                        return Friend.Command.ResponseAddBotFriend(bot, args[1]);

                    case "ADDBOTFRIENDMULI" when access >= EAccess.Master:
                    case "ABFM" when access >= EAccess.Master:
                        return Friend.Command.ResponseAddBotFriendMuli(Utilities.GetArgsAsText(message, 1));

                    case "ADDFRIEND" when argLength > 2 && access >= EAccess.Master:
                    case "AF" when argLength > 2 && access >= EAccess.Master:
                        return Friend.Command.ResponseAddFriend(args[1], Utilities.GetArgsAsText(args, 2, ","));
                    case "ADDFRIEND" when access >= EAccess.Master:
                    case "AF" when access >= EAccess.Master:
                        return Friend.Command.ResponseAddFriend(bot, args[1]);

                    case "DELETEFRIEND" when argLength > 2 && access >= EAccess.Master:
                    case "DF" when argLength > 2 && access >= EAccess.Master:
                        return Friend.Command.ResponseDeleteFriend(args[1], Utilities.GetArgsAsText(args, 2, ","));
                    case "DELETEFRIEND" when access >= EAccess.Master:
                    case "DF" when access >= EAccess.Master:
                        return Friend.Command.ResponseDeleteFriend(bot, args[1]);

                    case "DELETEALLFRIEND" when access >= EAccess.Master:
                        return Friend.Command.ResponseDeleteAllFriend(Utilities.GetArgsAsText(args, 1, ","));

                    case "INVITELINK" when access >= EAccess.Operator:
                    case "IL" when access >= EAccess.Operator:
                        return Friend.Command.ResponseGetInviteLink(Utilities.GetArgsAsText(args, 1, ","));

                    //Group
                    case "GROUPLIST" when Config.EULA && access >= EAccess.FamilySharing:
                    case "GL" when Config.EULA && access >= EAccess.FamilySharing:
                        return Group.Command.ResponseGroupList(Utilities.GetArgsAsText(args, 1, ","));

                    case "JOINGROUP" when Config.EULA && argLength > 2 && access >= EAccess.Master && access >= EAccess.Master:
                    case "JG" when Config.EULA && argLength > 2 && access >= EAccess.Master:
                        return Group.Command.ResponseJoinGroup(args[1], Utilities.GetArgsAsText(args, 2, ","));
                    case "JOINGROUP" when Config.EULA && access >= EAccess.Master:
                    case "JG" when Config.EULA && access >= EAccess.Master:
                        return Group.Command.ResponseJoinGroup(bot, args[1]);

                    case "LEAVEGROUP" when Config.EULA && argLength > 2 && access >= EAccess.Master && access >= EAccess.Master:
                    case "LG" when Config.EULA && argLength > 2 && access >= EAccess.Master:
                        return Group.Command.ResponseLeaveGroup(args[1], Utilities.GetArgsAsText(args, 2, ","));
                    case "LEAVEGROUP" when Config.EULA && access >= EAccess.Master:
                    case "LG" when Config.EULA && access >= EAccess.Master:
                        return Group.Command.ResponseLeaveGroup(bot, args[1]);

                    //Other
                    case "DUMP" when access >= EAccess.Operator:
                        return Task.FromResult(Other.Command.ResponseDumpToFile(bot, access, Utilities.GetArgsAsText(args, 1, ","), steamId));

                    case "KEY" when access >= EAccess.FamilySharing:
                    case "K" when access >= EAccess.FamilySharing:
                        return Task.FromResult(Other.Command.ResponseExtractKeys(Utilities.GetArgsAsText(args, 1, ",")));

                    case "EHELP" when access >= EAccess.FamilySharing:
                    case "HELP" when access >= EAccess.FamilySharing:
                        return Task.FromResult(Other.Command.ResponseCommandHelp(args));

                    //Profile
                    case "CLEARALIAS" when access >= EAccess.Operator:
                        return Profile.Command.ResponseClearAliasHistory(Utilities.GetArgsAsText(args, 1, ","));

                    case "CRAFTBADGE" when access >= EAccess.Master:
                    case "CRAFTBADGES" when access >= EAccess.Master:
                    case "CB" when access >= EAccess.Master:
                        return Profile.Command.ResponseCraftBadge(Utilities.GetArgsAsText(args, 1, ","));

                    case "DELETEAVATAR" when access >= EAccess.Master:
                        return Profile.Command.ResponseDelProfileAvatar(Utilities.GetArgsAsText(args, 1, ","));

                    case "FRIENDCODE" when access >= EAccess.FamilySharing:
                    case "FC" when access >= EAccess.FamilySharing:
                        return Profile.Command.ResponseGetFriendCode(Utilities.GetArgsAsText(args, 1, ","));

                    case "GAMEAVATAR" when argLength > 3 && access >= EAccess.Master:
                    case "GA" when argLength > 3 && access >= EAccess.Master:
                        {
                            string botNames = string.Join(',', args[1..(argLength - 2)]);
                            return Profile.Command.ResponseSetProfileGameAvatar(botNames, args[argLength - 2], args.Last());
                        }
                    case "GAMEAVATAR" when argLength == 3 && access >= EAccess.Master:
                    case "GA" when argLength == 3 && access >= EAccess.Master:
                        return Profile.Command.ResponseSetProfileGameAvatar(args[1], args[2], null);
                    case "GAMEAVATAR" when access >= EAccess.Master:
                    case "GA" when access >= EAccess.Master:
                        return Profile.Command.ResponseSetProfileGameAvatar(bot, args[1], null);

                    case "SETAVATAR" when argLength >= 3 && access >= EAccess.Master:
                    case "SEA" when argLength >= 3 && access >= EAccess.Master:
                        {
                            string botNames = string.Join(',', args[1..(argLength - 1)]);
                            return Profile.Command.ResponseSetProfileAvatar(botNames, args.Last());
                        }
                    case "SETAVATAR" when access >= EAccess.Master:
                    case "SEA" when access >= EAccess.Master:
                        return Profile.Command.ResponseSetProfileAvatar(bot, args[1]);

                    case "STEAMID" when access >= EAccess.FamilySharing:
                    case "SID" when access >= EAccess.FamilySharing:
                        return Profile.Command.ResponseGetSteamId(Utilities.GetArgsAsText(args, 1, ","));

                    case "PROFILE" when access >= EAccess.FamilySharing:
                    case "PF" when access >= EAccess.FamilySharing:
                        return Profile.Command.ResponseGetProfileSummary(Utilities.GetArgsAsText(args, 1, ","));

                    case "PROFILELINK" when access >= EAccess.FamilySharing:
                    case "PFL" when access >= EAccess.FamilySharing:
                        return Profile.Command.ResponseGetProfileLink(Utilities.GetArgsAsText(args, 1, ","));

                    case "RANDOMGAMEAVATAR" when access >= EAccess.Master:
                    case "RGA" when access >= EAccess.Master:
                        return Profile.Command.ResponseSetProfileGameAvatar(Utilities.GetArgsAsText(args, 1, ","), null, null);

                    case "ADVNICKNAME" when argLength > 2 && access >= EAccess.Master:
                    case "ANN" when argLength > 2 && access >= EAccess.Master:
                        return Profile.Command.ResponseAdvNickName(args[1], Utilities.GetArgsAsText(message, 2));
                    case "ADVNICKNAME" when access >= EAccess.Master:
                    case "ANN" when access >= EAccess.Master:
                        return Profile.Command.ResponseAdvNickName(bot, args[1]);

                    case "REPLAY" when access >= EAccess.Operator:
                    case "RP" when access >= EAccess.Operator:
                        return Profile.Command.ResponseGetReplay(Utilities.GetArgsAsText(args, 1, ","));

                    case "REPLAYPRIVACY" when argLength > 2 && access >= EAccess.Operator:
                    case "RPP" when argLength > 2 && access >= EAccess.Operator:
                        return Profile.Command.ResponseSetReplayPrivacy(args[1], Utilities.GetArgsAsText(args, 2, ","));
                    case "REPLAYPRIVACY" when access >= EAccess.Operator:
                    case "RPP" when access >= EAccess.Operator:
                        return Profile.Command.ResponseSetReplayPrivacy(bot, args[1]);

                    case "TRADELINK" when access >= EAccess.Operator:
                    case "TL" when access >= EAccess.Operator:
                        return Profile.Command.ResponseGetTradeLink(Utilities.GetArgsAsText(args, 1, ","));

                    //Store
                    case "APPDETAIL" when argLength > 2 && access >= EAccess.Operator:
                    case "AD" when argLength > 2 && access >= EAccess.Operator:
                        return Store.Command.ResponseGetAppsDetail(args[1], Utilities.GetArgsAsText(args, 2, ","));
                    case "APPDETAIL" when access >= EAccess.Operator:
                    case "AD" when access >= EAccess.Operator:
                        return Store.Command.ResponseGetAppsDetail(bot, args[1]);

                    case "DELETERECOMMENT" when Config.EULA && argLength > 2 && access >= EAccess.Master:
                    case "DREC" when Config.EULA && argLength > 2 && access >= EAccess.Master:
                        return Store.Command.ResponseDeleteReview(args[1], Utilities.GetArgsAsText(args, 2, ","));
                    case "DELETERECOMMENT" when Config.EULA && access >= EAccess.Master:
                    case "DREC" when Config.EULA && access >= EAccess.Master:
                        return Store.Command.ResponseDeleteReview(bot, args[1]);

                    case "PUBLISHRECOMMENT" when Config.EULA && argLength > 3 && access >= EAccess.Master:
                    case "PREC" when Config.EULA && argLength > 3 && access >= EAccess.Master:
                        return Store.Command.ResponsePublishReview(args[1], args[2], Utilities.GetArgsAsText(message, 3));
                    case "PUBLISHRECOMMENT" when Config.EULA && argLength == 3 && access >= EAccess.Master:
                    case "PREC" when Config.EULA && argLength == 3 && access >= EAccess.Master:
                        return Store.Command.ResponsePublishReview(bot, args[1], args[2]);

                    case "REQUESTACCESS" when argLength > 2 && access >= EAccess.Operator:
                    case "RA" when argLength > 2 && access >= EAccess.Operator:
                        return Store.Command.ResponseRequestAccess(args[1], Utilities.GetArgsAsText(args, 2, ","));
                    case "REQUESTACCESS" when access >= EAccess.Operator:
                    case "RA" when access >= EAccess.Operator:
                        return Store.Command.ResponseRequestAccess(bot, args[1]);

                    case "SEARCH" when argLength > 2 && access >= EAccess.Operator:
                    case "SS" when argLength > 2 && access >= EAccess.Operator:
                        return Store.Command.ResponseSearchGame(args[1], Utilities.GetArgsAsText(args, 2, ","));
                    case "SEARCH" when access >= EAccess.Operator:
                    case "SS" when access >= EAccess.Operator:
                        return Store.Command.ResponseSearchGame(bot, args[1]);

                    case "SUBS" when argLength > 2 && access >= EAccess.Operator:
                    case "S" when argLength > 2 && access >= EAccess.Operator:
                        return Store.Command.ResponseGetGameSubes(args[1], Utilities.GetArgsAsText(args, 2, ","));
                    case "SUBS" when access >= EAccess.Operator:
                    case "S" when access >= EAccess.Operator:
                        return Store.Command.ResponseGetGameSubes(bot, args[1]);

                    case "VIEWPAGE" when argLength > 2 && access >= EAccess.Operator:
                    case "VP" when argLength > 2 && access >= EAccess.Operator:
                        return Store.Command.ResponseViewPage(args[1], Utilities.GetArgsAsText(args, 2, ","));
                    case "VIEWPAGE" when access >= EAccess.Operator:
                    case "VP" when access >= EAccess.Operator:
                        return Store.Command.ResponseViewPage(bot, args[1]);

                    //Wallet
                    case "REDEEMWALLET" when args.Length > 2 && access >= EAccess.Master:
                    case "RWA" when args.Length > 2 && access >= EAccess.Master:
                        return Wallet.Command.ResponseRedeemWallet(args[1], Utilities.GetArgsAsText(args, 2, ","));
                    case "REDEEMWALLET" when access >= EAccess.Master:
                    case "RWA" when access >= EAccess.Master:
                        return Wallet.Command.ResponseRedeemWallet(bot, args[1]);

                    case "REDEEMWALLETMULT" when access >= EAccess.Master:
                    case "RWAM" when access >= EAccess.Master:
                        return Wallet.Command.ResponseRedeemWalletMult(Utilities.GetArgsAsText(args, 1, ","));

                    //WishList
                    case "ADDWISHLIST" when argLength > 2 && access >= EAccess.Master:
                    case "AW" when argLength > 2 && access >= EAccess.Master:
                        return Wishlist.Command.ResponseAddWishlist(args[1], Utilities.GetArgsAsText(args, 2, ","));
                    case "ADDWISHLIST" when access >= EAccess.Master:
                    case "AW" when access >= EAccess.Master:
                        return Wishlist.Command.ResponseAddWishlist(bot, args[1]);

                    case "CHECK" when argLength > 2 && access >= EAccess.Master:
                    case "CK" when argLength > 2 && access >= EAccess.Master:
                        return Wishlist.Command.ResponseCheckGame(args[1], Utilities.GetArgsAsText(args, 2, ","));
                    case "CHECK" when access >= EAccess.Master:
                    case "CK" when access >= EAccess.Master:
                        return Wishlist.Command.ResponseCheckGame(bot, args[1]);

                    case "FOLLOWGAME" when argLength > 2 && access >= EAccess.Master:
                    case "FG" when argLength > 2 && access >= EAccess.Master:
                        return Wishlist.Command.ResponseFollowGame(args[1], Utilities.GetArgsAsText(args, 2, ","), true);
                    case "FOLLOWGAME" when access >= EAccess.Master:
                    case "FG" when access >= EAccess.Master:
                        return Wishlist.Command.ResponseFollowGame(bot, args[1], true);

                    case "REMOVEWISHLIST" when argLength > 2 && access >= EAccess.Master:
                    case "RW" when argLength > 2 && access >= EAccess.Master:
                        return Wishlist.Command.ResponseRemoveWishlist(args[1], Utilities.GetArgsAsText(args, 2, ","));
                    case "REMOVEWISHLIST" when access >= EAccess.Master:
                    case "RW" when access >= EAccess.Master:
                        return Wishlist.Command.ResponseRemoveWishlist(bot, args[1]);

                    case "UNFOLLOWGAME" when argLength > 2 && access >= EAccess.Master:
                    case "UFG" when argLength > 2 && access >= EAccess.Master:
                        return Wishlist.Command.ResponseFollowGame(args[1], Utilities.GetArgsAsText(args, 2, ","), false);
                    case "UNFOLLOWGAME" when access >= EAccess.Master:
                    case "UFG" when access >= EAccess.Master:
                        return Wishlist.Command.ResponseFollowGame(bot, args[1], false);

                    //DevFuture
                    case "COOKIES" when Config.DevFeature && access >= EAccess.Owner:
                        return DevFeature.Command.ResponseGetCookies(Utilities.GetArgsAsText(args, 1, ","));
                    case "APIKEY" when Config.DevFeature && access >= EAccess.Owner:
                        return DevFeature.Command.ResponseGetAPIKey(Utilities.GetArgsAsText(args, 1, ","));
                    case "ACCESSTOKEN" when Config.DevFeature && access >= EAccess.Owner:
                        return DevFeature.Command.ResponseGetAccessToken(Utilities.GetArgsAsText(args, 1, ","));

                    //Limited Tips
                    case "FOLLOWCURATOR" when !Config.EULA && argLength >= 1 && access >= EAccess.Master:
                    case "FCU" when !Config.EULA && argLength >= 1 && access >= EAccess.Master:
                    case "UNFOLLOWCURATOR" when !Config.EULA && argLength >= 1 && access >= EAccess.Master:
                    case "UFCU" when !Config.EULA && argLength >= 1 && access >= EAccess.Master:
                    case "CURATORLIST" when !Config.EULA && access >= EAccess.Master:
                    case "CL" when !Config.EULA && access >= EAccess.Master:
                    case "JOINGROUP" when !Config.EULA && argLength >= 1 && access >= EAccess.Master:
                    case "JG" when !Config.EULA && argLength >= 1 && access >= EAccess.Master:
                    case "LEAVEGROUP" when !Config.EULA && argLength >= 1 && access >= EAccess.Master:
                    case "LG" when !Config.EULA && argLength >= 1 && access >= EAccess.Master:
                    case "GROUPLIST" when !Config.EULA && access >= EAccess.Master:
                    case "GL" when !Config.EULA && access >= EAccess.Master:
                    case "DELETERECOMMENT" when !Config.EULA && argLength >= 1 && access >= EAccess.Master:
                    case "DREC" when !Config.EULA && argLength >= 1 && access >= EAccess.Master:
                    case "PUBLISHRECOMMEND" when !Config.EULA && argLength >= 2 && access >= EAccess.Master:
                    case "PREC" when !Config.EULA && argLength >= 2 && access >= EAccess.Master:
                        return Task.FromResult(Other.Command.ResponseEulaCmdUnavilable());

                    case "COOKIES" when Config.DevFeature && access >= EAccess.Owner:
                    case "APIKEY" when Config.DevFeature && access >= EAccess.Owner:
                    case "ACCESSTOKEN" when Config.DevFeature && access >= EAccess.Owner:
                        return Task.FromResult(Other.Command.ResponseDevFeatureUnavilable());

                    default:
                        return Task.FromResult(Other.Command.ShowUsageIfAvilable(args[0].ToUpperInvariant()));
                }
        }
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
            var task = ResponseCommand(bot, access, message, args, steamId);
            if (task != null)
            {
                return await task.ConfigureAwait(false);
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            var version = await bot.Commands.Response(EAccess.Owner, "VERSION").ConfigureAwait(false) ?? Langs.AccountSubUnknown;
            var i = version.LastIndexOf('V');
            if (i >= 0)
            {
                version = version[++i..];
            }
            var cfg = JsonConvert.SerializeObject(Config, Formatting.Indented);

            var sb = new StringBuilder();
            sb.AppendLine(Langs.ErrorLogTitle);
            sb.AppendLine(Static.Line);
            sb.AppendLine(string.Format(Langs.ErrorLogOriginMessage, message));
            sb.AppendLine(string.Format(Langs.ErrorLogAccess, access.ToString()));
            sb.AppendLine(string.Format(Langs.ErrorLogASFVersion, version));
            sb.AppendLine(string.Format(Langs.ErrorLogPluginVersion, MyVersion));
            sb.AppendLine(Static.Line);
            sb.AppendLine(cfg);
            sb.AppendLine(Static.Line);
            sb.AppendLine(string.Format(Langs.ErrorLogErrorName, ex.GetType()));
            sb.AppendLine(string.Format(Langs.ErrorLogErrorMessage, ex.Message));
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
        var bots = Bot.GetBots("ASF")?.Select(b => b.SteamID).ToList();
        bool approve = bots?.Contains(steamId) ?? false;

        if (approve)
        {
            ASFLogger.LogGenericInfo(string.Format(Langs.AcceptFriendRequest, bot.BotName, steamId));
        }

        return Task.FromResult(approve);
    }
}
