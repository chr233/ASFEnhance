#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Core;
using ArchiSteamFarm.Plugins.Interfaces;
using ArchiSteamFarm.Steam;
using ASFEnhance.Data;
using ASFEnhance.Localization;
using ASFEnhance.Other;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Composition;
using System.Text;
using static ASFEnhance.Utils;

namespace ASFEnhance
{
    [Export(typeof(IPlugin))]
    internal sealed class ASFEnhance : IASF, IBotCommand2
    {
        public string Name => nameof(ASFEnhance);
        public Version Version => MyVersion;

        [JsonProperty]
        public PluginConfig Config { get; private set; }

        /// <summary>
        /// ASF启动事件
        /// </summary>
        /// <param name="additionalConfigProperties"></param>
        /// <returns></returns>
        public Task OnASFInit(IReadOnlyDictionary<string, JToken>? additionalConfigProperties = null)
        {
            if (additionalConfigProperties == null)
            {
                return Task.CompletedTask;
            }

            PluginConfig? config = null;

            StringBuilder message = new();

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
                else if (configProperty == "ASFEnhanceDevFuture" && configValue.Type == JTokenType.Boolean)
                {
                    message.AppendLine();
                    message.AppendLine(Static.Line);
                    message.AppendLine(Langs.ASFEConfigWarning);
                    message.AppendLine(Static.Line);
                    ASFLogger.LogGenericWarning(message.ToString());
                }
            }

            Config = config != null ? config : new();

            if (Config.DevFeature)
            {
                message.AppendLine();
                message.AppendLine(Static.Line);
                message.AppendLine(Langs.DevFeatureEnabledWarning);
                message.AppendLine(Static.Line);
            }

            if (!Config.EULA)
            {
                message.AppendLine();
                message.AppendLine(Static.Line);
                message.AppendLine(Langs.EulaWarning);
                message.AppendLine(Static.Line);
            }

            if (message.Length > 0)
            {
                ASFLogger.LogGenericWarning(message.ToString());
            }

            if (Config.Statistic)
            {
                Uri request = new("https://asfe.chrxw.com/");
                _ = new Timer(
                    async (_) => {
                        await ASF.WebBrowser.UrlGetToHtmlDocument(request).ConfigureAwait(false);
                    },
                    null,
                    TimeSpan.FromSeconds(30),
                    TimeSpan.FromHours(24)
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
            StringBuilder message = new("\n");
            message.AppendLine(Static.Line);
            message.AppendLine(Static.Logo);
            message.AppendLine(string.Format(Langs.PluginVer, nameof(ASFEnhance), MyVersion.ToString()));
            message.AppendLine(Langs.PluginContact);
            message.AppendLine(Langs.PluginInfo);
            message.AppendLine(Static.Line);

            string pluginFolder = Path.GetDirectoryName(MyLocation);
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
        /// <param name="steamID"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private async Task<string> ResponseCommand(Bot bot, EAccess access, string message, string[] args, ulong steamID)
        {
            int argLength = args.Length;
            switch (argLength)
            {
                case 0:
                    throw new InvalidOperationException(nameof(args));
                case 1: //不带参数
                    switch (args[0].ToUpperInvariant())
                    {
                        //Event
                        case "EVENT" when access >= EAccess.Operator:
                        case "E" when access >= EAccess.Operator:
                            return await Event.Command.ResponseEvent(bot).ConfigureAwait(false);

                        //Shortcut
                        case "P":
                            return await bot.Commands.Response(access, "POINTS", steamID).ConfigureAwait(false);
                        case "PA":
                            return await bot.Commands.Response(access, "POINTS ASF", steamID).ConfigureAwait(false);
                        case "LA":
                            return await bot.Commands.Response(access, "LEVEL ASF", steamID).ConfigureAwait(false);
                        case "BA":
                            return await bot.Commands.Response(access, "BALANCE ASF", steamID).ConfigureAwait(false);
                        case "CA":
                            return await bot.Commands.Response(access, "CART ASF", steamID).ConfigureAwait(false);

                        //Account
                        case "PURCHASEHISTORY" when access >= EAccess.Operator:
                        case "PH" when access >= EAccess.Operator:
                            return await Account.Command.ResponseAccountHistory(bot).ConfigureAwait(false);

                        case "FREELICENSES" when access >= EAccess.Operator:
                        case "FREELICENSE" when access >= EAccess.Operator:
                        case "FL" when access >= EAccess.Operator:
                            return await Account.Command.ResponseGetAccountLicenses(bot, true).ConfigureAwait(false);

                        case "LICENSES" when access >= EAccess.Operator:
                        case "LICENSE" when access >= EAccess.Operator:
                        case "L" when access >= EAccess.Operator:
                            return await Account.Command.ResponseGetAccountLicenses(bot, false).ConfigureAwait(false);

                        case "REMOVEDEMOS" when access >= EAccess.Master:
                        case "REMOVEDEMO" when access >= EAccess.Master:
                        case "RD" when access >= EAccess.Master:
                            return await Account.Command.ResponseRemoveAllDemos(bot).ConfigureAwait(false);

                        //Cart
                        case "CART" when access >= EAccess.Operator:
                        case "C" when access >= EAccess.Operator:
                            return await Cart.Command.ResponseGetCartGames(bot).ConfigureAwait(false);

                        case "CARTCOUNTRY" when access >= EAccess.Operator:
                        case "CC" when access >= EAccess.Operator:
                            return await Cart.Command.ResponseGetCartCountries(bot).ConfigureAwait(false);

                        case "CARTRESET" when access >= EAccess.Operator:
                        case "CR" when access >= EAccess.Operator:
                            return await Cart.Command.ResponseClearCartGames(bot).ConfigureAwait(false);

                        case "PURCHASE" when access >= EAccess.Master:
                        case "PC" when access >= EAccess.Master:
                            return await Cart.Command.ResponsePurchaseSelf(bot).ConfigureAwait(false);

                        //Curasor
                        case "CURATORLIST" when Config.EULA && access >= EAccess.Master:
                        case "CL" when Config.EULA && access >= EAccess.Master:
                            return await Curator.Command.ResponseGetFollowingCurators(bot).ConfigureAwait(false);

                        //Explorer
                        case "EXPLORER" when access >= EAccess.Master:
                        case "EX" when access >= EAccess.Master:
                            return await Explorer.Command.ResponseExploreDiscoveryQueue(bot).ConfigureAwait(false);

                        //Group
                        case "GROUPLIST" when Config.EULA && access >= EAccess.FamilySharing:
                        case "GL" when Config.EULA && access >= EAccess.FamilySharing:
                            return await Group.Command.ResponseGroupList(bot).ConfigureAwait(false);

                        //Other
                        case "ASFEHELP":
                        case "EHELP":
                            return Other.Command.ResponseAllCommands();

                        //Profile
                        case "FRIENDCODE" when access >= EAccess.FamilySharing:
                        case "FC" when access >= EAccess.FamilySharing:
                            return Profile.Command.ResponseGetFriendCode(bot);

                        case "STEAMID" when access >= EAccess.FamilySharing:
                        case "SID" when access >= EAccess.FamilySharing:
                            return Profile.Command.ResponseGetSteamID(bot);

                        case "PROFILE" when access >= EAccess.FamilySharing:
                        case "PF" when access >= EAccess.FamilySharing:
                            return await Profile.Command.ResponseGetProfileSummary(bot).ConfigureAwait(false);

                        case "PROFILELINK" when access >= EAccess.FamilySharing:
                        case "PFL" when access >= EAccess.FamilySharing:
                            return Profile.Command.ResponseGetProfileLink(bot);

                        case "TRADELINK" when access >= EAccess.Operator:
                        case "TL" when access >= EAccess.Operator:
                            return await Profile.Command.ResponseGetTradeLink(bot).ConfigureAwait(false);

                        //Update
                        case "ASFENHANCE" when access >= EAccess.FamilySharing:
                        case "ASFE" when access >= EAccess.FamilySharing:
                            return Update.Command.ResponseASFEnhanceVersion();

                        case "ASFEVERSION" when access >= EAccess.Operator:
                        case "AV" when access >= EAccess.Operator:
                            return await Update.Command.ResponseCheckLatestVersion().ConfigureAwait(false);

                        case "ASFEUPDATE" when access >= EAccess.Owner:
                        case "AU" when access >= EAccess.Owner:
                            return await Update.Command.ResponseUpdatePlugin().ConfigureAwait(false);

                        //DevFuture
                        case "COOKIES" when Config.DevFeature && access >= EAccess.Owner:
                            return DevFeature.Command.ResponseGetCookies(bot);
                        case "APIKEY" when Config.DevFeature && access >= EAccess.Owner:
                            return await DevFeature.Command.ResponseGetAPIKey(bot).ConfigureAwait(false);
                        case "ACCESSTOKEN" when Config.DevFeature && access >= EAccess.Owner:
                            return await DevFeature.Command.ResponseGetAccessToken(bot).ConfigureAwait(false);

                        //Limited Tips
                        case "CURATORLIST" when access >= EAccess.Master:
                        case "CL" when access >= EAccess.Master:
                        case "GROUPLIST" when access >= EAccess.Master:
                        case "GL" when access >= EAccess.Master:
                            return Other.Command.ResponseEulaCmdUnavilable();

                        case "COOKIES" when access >= EAccess.Owner:
                        case "APIKEY" when access >= EAccess.Owner:
                        case "ACCESSTOKEN" when access >= EAccess.Owner:
                            return Other.Command.ResponseDevFeatureUnavilable();

                        default:
                            return null;
                    }
                default: //带参数
                    switch (args[0].ToUpperInvariant())
                    {
                        //Event
                        case "EVENT" when access >= EAccess.Operator:
                        case "E" when access >= EAccess.Operator:
                            return await Event.Command.ResponseEvent(Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        //Shortcut
                        case "AL":
                            return await bot.Commands.Response(access, "ADDLICENSE " + Utilities.GetArgsAsText(message, 1), steamID).ConfigureAwait(false);
                        case "P":
                            return await bot.Commands.Response(access, "POINTS " + Utilities.GetArgsAsText(message, 1), steamID).ConfigureAwait(false);
                        case "TR":
                            return await bot.Commands.Response(access, "TRANSFER " + Utilities.GetArgsAsText(message, 1), steamID).ConfigureAwait(false);

                        //Account
                        case "PURCHASEHISTORY" when access > EAccess.Operator:
                        case "PH" when access > EAccess.Operator:
                            return await Account.Command.ResponseAccountHistory(Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "FREELICENSES" when access >= EAccess.Operator:
                        case "FREELICENSE" when access >= EAccess.Operator:
                        case "FL" when access >= EAccess.Operator:
                            return await Account.Command.ResponseGetAccountLicenses(Utilities.GetArgsAsText(args, 1, ","), true).ConfigureAwait(false);

                        case "LICENSES" when access >= EAccess.Operator:
                        case "LICENSE" when access >= EAccess.Operator:
                        case "L" when access >= EAccess.Operator:
                            return await Account.Command.ResponseGetAccountLicenses(Utilities.GetArgsAsText(args, 1, ","), false).ConfigureAwait(false);

                        case "REMOVEDEMOS" when access >= EAccess.Master:
                        case "REMOVEDEMO" when access >= EAccess.Master:
                        case "RD" when access >= EAccess.Master:
                            return await Account.Command.ResponseRemoveAllDemos(Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "REMOVELICENSES" when argLength > 2 && access >= EAccess.Master:
                        case "REMOVELICENSE" when argLength > 2 && access >= EAccess.Master:
                        case "RL" when argLength > 2 && access >= EAccess.Master:
                            return await Account.Command.ResponseRemoveFreeLicenses(args[1], Utilities.GetArgsAsText(args, 2, ",")).ConfigureAwait(false);

                        case "REMOVELICENSES" when access >= EAccess.Master:
                        case "REMOVELICENSE" when access >= EAccess.Master:
                        case "RL" when access >= EAccess.Master:
                            return await Account.Command.ResponseRemoveFreeLicenses(bot, args[1]).ConfigureAwait(false);

                        //Cart
                        case "CART" when access >= EAccess.Operator:
                        case "C" when access >= EAccess.Operator:
                            return await Cart.Command.ResponseGetCartGames(Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "ADDCART" when argLength > 2 && access >= EAccess.Operator:
                        case "AC" when argLength > 2 && access >= EAccess.Operator:
                            return await Cart.Command.ResponseAddCartGames(args[1], Utilities.GetArgsAsText(args, 2, ",")).ConfigureAwait(false);
                        case "ADDCART" when access >= EAccess.Operator:
                        case "AC" when access >= EAccess.Operator:
                            return await Cart.Command.ResponseAddCartGames(bot, args[1]).ConfigureAwait(false);

                        case "CARTCOUNTRY" when access >= EAccess.Operator:
                        case "CC" when access >= EAccess.Operator:
                            return await Cart.Command.ResponseGetCartCountries(Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "CARTRESET" when access >= EAccess.Operator:
                        case "CR" when access >= EAccess.Operator:
                            return await Cart.Command.ResponseClearCartGames(Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "SETCOUNTRY" when argLength > 2 && access >= EAccess.Master:
                        case "SC" when argLength > 2 && access >= EAccess.Master:
                            return await Cart.Command.ResponseSetCountry(args[1], Utilities.GetArgsAsText(args, 2, ",")).ConfigureAwait(false);
                        case "SETCOUNTRY" when access >= EAccess.Master:
                        case "SC" when access >= EAccess.Master:
                            return await Cart.Command.ResponseSetCountry(bot, args[1]).ConfigureAwait(false);

                        case "PURCHASE" when access >= EAccess.Master:
                        case "PC" when access >= EAccess.Master:
                            return await Cart.Command.ResponsePurchaseSelf(Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "PURCHASEGIFT" when argLength == 3 && access >= EAccess.Master:
                        case "PCG" when argLength == 3 && access >= EAccess.Master:
                            return await Cart.Command.ResponsePurchaseGift(args[1], args[2]).ConfigureAwait(false);
                        case "PURCHASEGIFT" when argLength == 2 && access >= EAccess.Master:
                        case "PCG" when argLength == 2 && access >= EAccess.Master:
                            return await Cart.Command.ResponsePurchaseGift(bot, args[1]).ConfigureAwait(false);

                        //Curasor
                        case "FOLLOWCURATOR" when Config.EULA && argLength > 2 && access >= EAccess.Master:
                        case "FCU" when Config.EULA && argLength > 2 && access >= EAccess.Master:
                            return await Curator.Command.ResponseFollowCurator(args[1], Utilities.GetArgsAsText(message, 2), true).ConfigureAwait(false);
                        case "FOLLOWCURATOR" when Config.EULA && access >= EAccess.Master:
                        case "FCU" when Config.EULA && access >= EAccess.Master:
                            return await Curator.Command.ResponseFollowCurator(bot, args[1], true).ConfigureAwait(false);

                        case "UNFOLLOWCURATOR" when Config.EULA && argLength > 2 && access >= EAccess.Master:
                        case "UFCU" when Config.EULA && argLength > 2 && access >= EAccess.Master:
                            return await Curator.Command.ResponseFollowCurator(args[1], Utilities.GetArgsAsText(message, 2), false).ConfigureAwait(false);
                        case "UNFOLLOWCURATOR" when Config.EULA && access >= EAccess.Master:
                        case "UFCU" when Config.EULA && access >= EAccess.Master:
                            return await Curator.Command.ResponseFollowCurator(bot, args[1], false).ConfigureAwait(false);

                        case "CURATORLIST" when Config.EULA && access >= EAccess.Master:
                        case "CL" when Config.EULA && access >= EAccess.Master:
                            return await Curator.Command.ResponseGetFollowingCurators(Utilities.GetArgsAsText(message, 1)).ConfigureAwait(false);

                        //Explorer
                        case "EXPLORER" when access >= EAccess.Master:
                        case "EX" when access >= EAccess.Master:
                            return await Explorer.Command.ResponseExploreDiscoveryQueue(Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        //Group
                        case "JOINGROUP" when Config.EULA && argLength > 2 && access >= EAccess.Master && access >= EAccess.Master:
                        case "JG" when Config.EULA && argLength > 2 && access >= EAccess.Master:
                            return await Group.Command.ResponseJoinGroup(args[1], Utilities.GetArgsAsText(message, 2)).ConfigureAwait(false);
                        case "JOINGROUP" when Config.EULA && access >= EAccess.Master:
                        case "JG" when Config.EULA && access >= EAccess.Master:
                            return await Group.Command.ResponseJoinGroup(bot, args[1]).ConfigureAwait(false);

                        case "LEAVEGROUP" when Config.EULA && argLength > 2 && access >= EAccess.Master && access >= EAccess.Master:
                        case "LG" when Config.EULA && argLength > 2 && access >= EAccess.Master:
                            return await Group.Command.ResponseLeaveGroup(args[1], Utilities.GetArgsAsText(message, 2)).ConfigureAwait(false);
                        case "LEAVEGROUP" when Config.EULA && access >= EAccess.Master:
                        case "LG" when Config.EULA && access >= EAccess.Master:
                            return await Group.Command.ResponseLeaveGroup(bot, args[1]).ConfigureAwait(false);

                        case "GROUPLIST" when Config.EULA && access >= EAccess.FamilySharing:
                        case "GL" when Config.EULA && access >= EAccess.FamilySharing:
                            return await Group.Command.ResponseGroupList(Utilities.GetArgsAsText(message, 1)).ConfigureAwait(false);

                        //Other
                        case "KEY" when access >= EAccess.FamilySharing:
                        case "K" when access >= EAccess.FamilySharing:
                            return Other.Command.ResponseExtractKeys(Utilities.GetArgsAsText(args, 1, ","));

                        case "EHELP" when access >= EAccess.FamilySharing:
                        case "HELP" when access >= EAccess.FamilySharing:
                            return Other.Command.ResponseCommandHelp(args);

                        //Profile
                        case "FRIENDCODE" when access >= EAccess.FamilySharing:
                        case "FC" when access >= EAccess.FamilySharing:
                            return await Profile.Command.ResponseGetFriendCode(Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "STEAMID" when access >= EAccess.FamilySharing:
                        case "SID" when access >= EAccess.FamilySharing:
                            return await Profile.Command.ResponseGetSteamID(Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "PROFILE" when access >= EAccess.FamilySharing:
                        case "PF" when access >= EAccess.FamilySharing:
                            return await Profile.Command.ResponseGetProfileSummary(Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "PROFILELINK" when access >= EAccess.FamilySharing:
                        case "PFL" when access >= EAccess.FamilySharing:
                            return await Profile.Command.ResponseGetProfileLink(Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "TRADELINK" when access >= EAccess.Operator:
                        case "TL" when access >= EAccess.Operator:
                            return await Profile.Command.ResponseGetTradeLink(Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        //Store
                        case "APPDETAIL" when argLength > 2 && access >= EAccess.Operator:
                        case "AD" when argLength > 2 && access >= EAccess.Operator:
                            return await Store.Command.ResponseGetAppsDetail(args[1], Utilities.GetArgsAsText(args, 2, ",")).ConfigureAwait(false);
                        case "APPDETAIL" when access >= EAccess.Operator:
                        case "AD" when access >= EAccess.Operator:
                            return await Store.Command.ResponseGetAppsDetail(bot, args[1]).ConfigureAwait(false);

                        case "DELETERECOMMENT" when Config.EULA && argLength > 2 && access >= EAccess.Master:
                        case "DREC" when Config.EULA && argLength > 2 && access >= EAccess.Master:
                            return await Store.Command.ResponseDeleteReview(args[1], Utilities.GetArgsAsText(args, 2, ",")).ConfigureAwait(false);
                        case "DELETERECOMMENT" when Config.EULA && access >= EAccess.Master:
                        case "DREC" when Config.EULA && access >= EAccess.Master:
                            return await Store.Command.ResponseDeleteReview(bot, args[1]).ConfigureAwait(false);

                        case "PUBLISHRECOMMENT" when Config.EULA && argLength > 3 && access >= EAccess.Master:
                        case "PREC" when Config.EULA && argLength > 3 && access >= EAccess.Master:
                            return await Store.Command.ResponsePublishReview(args[1], args[2], Utilities.GetArgsAsText(args, 3, ",")).ConfigureAwait(false);
                        case "PUBLISHRECOMMENT" when Config.EULA && argLength == 3 && access >= EAccess.Master:
                        case "PREC" when Config.EULA && argLength == 3 && access >= EAccess.Master:
                            return await Store.Command.ResponsePublishReview(bot, args[1], args[2]).ConfigureAwait(false);

                        case "REQUESTACCESS" when argLength > 2 && access >= EAccess.Operator:
                        case "RA" when argLength > 2 && access >= EAccess.Operator:
                            return await Store.Command.ResponseRequestAccess(args[1], Utilities.GetArgsAsText(args, 2, ",")).ConfigureAwait(false);
                        case "REQUESTACCESS" when access >= EAccess.Operator:
                        case "RA" when access >= EAccess.Operator:
                            return await Store.Command.ResponseRequestAccess(bot, args[1]).ConfigureAwait(false);

                        case "SEARCH" when argLength > 2 && access >= EAccess.Operator:
                        case "SS" when argLength > 2 && access >= EAccess.Operator:
                            return await Store.Command.ResponseSearchGame(args[1], Utilities.GetArgsAsText(args, 2, ",")).ConfigureAwait(false);
                        case "SEARCH" when access >= EAccess.Operator:
                        case "SS" when access >= EAccess.Operator:
                            return await Store.Command.ResponseSearchGame(bot, args[1]).ConfigureAwait(false);

                        case "SUBS" when argLength > 2 && access >= EAccess.Operator:
                        case "S" when argLength > 2 && access >= EAccess.Operator:
                            return await Store.Command.ResponseGetGameSubes(args[1], Utilities.GetArgsAsText(args, 2, ",")).ConfigureAwait(false);
                        case "SUBS" when access >= EAccess.Operator:
                        case "S" when access >= EAccess.Operator:
                            return await Store.Command.ResponseGetGameSubes(bot, args[1]).ConfigureAwait(false);

                        //WishList
                        case "ADDWISHLIST" when argLength > 2 && access >= EAccess.Master:
                        case "AW" when argLength > 2 && access >= EAccess.Master:
                            return await Wishlist.Command.ResponseAddWishlist(args[1], Utilities.GetArgsAsText(message, 2)).ConfigureAwait(false);
                        case "ADDWISHLIST" when access >= EAccess.Master:
                        case "AW" when access >= EAccess.Master:
                            return await Wishlist.Command.ResponseAddWishlist(bot, args[1]).ConfigureAwait(false);

                        case "REMOVEWISHLIST" when argLength > 2 && access >= EAccess.Master:
                        case "RW" when argLength > 2 && access >= EAccess.Master:
                            return await Wishlist.Command.ResponseRemoveWishlist(args[1], Utilities.GetArgsAsText(message, 2)).ConfigureAwait(false);
                        case "REMOVEWISHLIST" when access >= EAccess.Master:
                        case "RW" when access >= EAccess.Master:
                            return await Wishlist.Command.ResponseRemoveWishlist(bot, args[1]).ConfigureAwait(false);

                        case "FOLLOWGAME" when argLength > 2 && access >= EAccess.Master:
                        case "FG" when argLength > 2 && access >= EAccess.Master:
                            return await Wishlist.Command.ResponseFollowGame(args[1], Utilities.GetArgsAsText(message, 2), true).ConfigureAwait(false);
                        case "FOLLOWGAME" when access >= EAccess.Master:
                        case "FG" when access >= EAccess.Master:
                            return await Wishlist.Command.ResponseFollowGame(bot, args[1], true).ConfigureAwait(false);

                        case "UNFOLLOWGAME" when argLength > 2 && access >= EAccess.Master:
                        case "UFG" when argLength > 2 && access >= EAccess.Master:
                            return await Wishlist.Command.ResponseFollowGame(args[1], Utilities.GetArgsAsText(message, 2), false).ConfigureAwait(false);
                        case "UNFOLLOWGAME" when access >= EAccess.Master:
                        case "UFG" when access >= EAccess.Master:
                            return await Wishlist.Command.ResponseFollowGame(bot, args[1], false).ConfigureAwait(false);

                        case "CHECK" when argLength > 2 && access >= EAccess.Master:
                        case "CK" when argLength > 2 && access >= EAccess.Master:
                            return await Wishlist.Command.ResponseCheckGame(args[1], Utilities.GetArgsAsText(message, 2)).ConfigureAwait(false);
                        case "CHECK" when access >= EAccess.Master:
                        case "CK" when access >= EAccess.Master:
                            return await Wishlist.Command.ResponseCheckGame(bot, args[1]).ConfigureAwait(false);

                        //DevFuture
                        case "COOKIES" when Config.DevFeature && access >= EAccess.Owner:
                            return await DevFeature.Command.ResponseGetCookies(Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);
                        case "APIKEY" when Config.DevFeature && access >= EAccess.Owner:
                            return await DevFeature.Command.ResponseGetAPIKey(Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);
                        case "ACCESSTOKEN" when Config.DevFeature && access >= EAccess.Owner:
                            return await DevFeature.Command.ResponseGetAccessToken(Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        //Limited Tips
                        case "FOLLOWCURATOR" when argLength >= 1 && access >= EAccess.Master:
                        case "FCU" when argLength >= 1 && access >= EAccess.Master:
                        case "UNFOLLOWCURATOR" when argLength >= 1 && access >= EAccess.Master:
                        case "UFCU" when argLength >= 1 && access >= EAccess.Master:
                        case "CURATORLIST" when access >= EAccess.Master:
                        case "CL" when access >= EAccess.Master:
                        case "JOINGROUP" when argLength >= 1 && access >= EAccess.Master:
                        case "JG" when argLength >= 1 && access >= EAccess.Master:
                        case "LEAVEGROUP" when argLength >= 1 && access >= EAccess.Master:
                        case "LG" when argLength >= 1 && access >= EAccess.Master:
                        case "GROUPLIST" when access >= EAccess.Master:
                        case "GL" when access >= EAccess.Master:
                        case "DELETERECOMMENT" when argLength >= 1 && access >= EAccess.Master:
                        case "DREC" when argLength >= 1 && access >= EAccess.Master:
                        case "PUBLISHRECOMMEND" when argLength >= 2 && access >= EAccess.Master:
                        case "PREC" when argLength >= 2 && access >= EAccess.Master:
                            return Other.Command.ResponseEulaCmdUnavilable();

                        case "COOKIES" when Config.DevFeature && access >= EAccess.Owner:
                        case "APIKEY" when Config.DevFeature && access >= EAccess.Owner:
                        case "ACCESSTOKEN" when Config.DevFeature && access >= EAccess.Owner:
                            return Other.Command.ResponseDevFeatureUnavilable();

                        default:
                            string cmd = args[0].ToUpperInvariant();

                            if (CommandHelpData.ShortCmd2FullCmd.ContainsKey(cmd))
                            {
                                cmd = CommandHelpData.ShortCmd2FullCmd[cmd];
                            }
                            if (CommandHelpData.CommandArges.ContainsKey(cmd))
                            {
                                string cmdArgs = CommandHelpData.CommandArges[cmd];
                                if (string.IsNullOrEmpty(cmdArgs))
                                {
                                    cmdArgs = Langs.NoArgs;
                                }

                                string usage;
                                if (CommandHelpData.CommandUsage.ContainsKey(cmd))
                                {
                                    usage = CommandHelpData.CommandUsage[cmd];
                                }
                                else
                                {
                                    usage = Langs.CommandHelpNoUsage;
                                }

                                if (CommandHelpData.FullCmd2ShortCmd.ContainsKey(cmd))
                                {
                                    string shortCmd = CommandHelpData.FullCmd2ShortCmd[cmd];
                                    return string.Format(Langs.CommandHelpWithShortName, cmd, shortCmd, cmdArgs, usage);
                                }
                                else
                                {
                                    return string.Format(Langs.CommandHelpNoShortName, cmd, cmdArgs, usage);
                                }
                            }

                            return null;
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
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<string?> OnBotCommand(Bot bot, EAccess access, string message, string[] args, ulong steamID = 0)
        {
            if (!Enum.IsDefined(access))
            {
                throw new InvalidEnumArgumentException(nameof(access), (int)access, typeof(EAccess));
            }

            try
            {
                return await ResponseCommand(bot, access, message, args, steamID).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string version = await bot.Commands.Response(EAccess.Owner, "VERSION").ConfigureAwait(false) ?? Langs.AccountSubUnknown;
                var i = version.LastIndexOf('V');
                if (i >= 0)
                {
                    version = version[++i..];
                }
                string cfg = JsonConvert.SerializeObject(Config, Formatting.Indented);

                StringBuilder sb = new();
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

                _ = Task.Run(async () => {
                    await Task.Delay(500).ConfigureAwait(false);
                    sb.Insert(0, '\n');
                    ASFLogger.LogGenericError(sb.ToString());
                }).ConfigureAwait(false);

                return sb.ToString();
            }
        }
    }
}
