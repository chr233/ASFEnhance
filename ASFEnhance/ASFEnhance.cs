#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Core;
using ArchiSteamFarm.Plugins.Interfaces;
using ArchiSteamFarm.Steam;
using ASFEnhance.Localization;
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
        public bool DeveloperFeature { get; private set; }

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

            foreach ((string configProperty, JToken configValue) in additionalConfigProperties)
            {
                if (configProperty == "ASFEnhanceDevFuture" && configValue.Type == JTokenType.Boolean)
                {
                    DeveloperFeature = configValue.Value<bool>();
                    break;
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

            if (DeveloperFeature)
            {
                message.AppendLine(Langs.DevFeatureEnabledWarning);
                message.AppendLine(Static.Line);
            }

            ASFLogger.LogGenericInfo(message.ToString());

            return Task.CompletedTask;
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

            switch (args.Length)
            {
                case 0:
                    throw new InvalidOperationException(nameof(args.Length));
                case 1: //不带参数
                    switch (args[0].ToUpperInvariant())
                    {
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
                        case "PURCHASEHISTORY" when access > EAccess.Operator:
                        case "PH" when access > EAccess.Operator:
                            return await Account.Command.ResponseAccountHistory(bot).ConfigureAwait(false);

                        //Event
                        case "EVENT" when access >= EAccess.Operator:
                        case "E" when access >= EAccess.Operator:
                            return await Event.Command.ResponseEvent(bot, false).ConfigureAwait(false);
                        case "EVENTENDLESS" when access >= EAccess.Operator:
                        case "EE" when access >= EAccess.Operator:
                            return await Event.Command.ResponseEvent(bot, true).ConfigureAwait(false);

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

                        //Group
                        case "GROUPLIST" when access >= EAccess.FamilySharing:
                        case "GL" when access >= EAccess.FamilySharing:
                            return await Group.Command.ResponseGroupList(bot).ConfigureAwait(false);

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

                        //Other
                        case "ASFENHANCE" when access >= EAccess.FamilySharing:
                        case "ASFE" when access >= EAccess.FamilySharing:
                            return Other.Command.ResponseASFEnhanceVersion();

                        case "ASFEVERSION" when access >= EAccess.Owner:
                        case "AV" when access >= EAccess.Owner:
                            return await Other.Command.ResponseCheckLatestVersion().ConfigureAwait(false);

                        case "ASFEUPDATE" when access >= EAccess.Owner:
                        case "AU" when access >= EAccess.Owner:
                            return await Other.Command.ResponseUpdatePlugin().ConfigureAwait(false);

                        case "ASFEHELP":
                        case "EHELP":
                            return Other.Command.ResponseAllCommands();

                        //DevFuture
                        case "COOKIES" when DeveloperFeature && access >= EAccess.Owner:
                            return DevFeature.Command.ResponseGetCookies(bot);
                        case "APIKEY" when DeveloperFeature && access >= EAccess.Owner:
                            return await DevFeature.Command.ResponseGetAPIKey(bot).ConfigureAwait(false);
                        case "ACCESSTOKEN" when DeveloperFeature && access >= EAccess.Owner:
                            return await DevFeature.Command.ResponseGetAccessToken(bot).ConfigureAwait(false);

                        default:
                            return null;
                    }
                default: //带参数
                    switch (args[0].ToUpperInvariant())
                    {
                        //SHORTCUT
                        case "AL":
                            return await bot.Commands.Response(access, "ADDLICENSE " + Utilities.GetArgsAsText(message, 1), steamID).ConfigureAwait(false);
                        case "P":
                            return await bot.Commands.Response(access, "POINTS " + Utilities.GetArgsAsText(message, 1), steamID).ConfigureAwait(false);

                        //Account
                        case "PURCHASEHISTORY" when access > EAccess.Operator:
                        case "PH" when access > EAccess.Operator:
                            return await Account.Command.ResponseAccountHistory(Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        //Event
                        case "EVENT" when access >= EAccess.Operator:
                        case "E" when access >= EAccess.Operator:
                            return await Event.Command.ResponseEvent(Utilities.GetArgsAsText(message, 1), false).ConfigureAwait(false);
                        case "EVENTENDLESS" when access >= EAccess.Operator:
                        case "EE" when access >= EAccess.Operator:
                            return await Event.Command.ResponseEvent(Utilities.GetArgsAsText(message, 1), true).ConfigureAwait(false);

                        //Group
                        case "JOINGROUP" when args.Length > 2 && access >= EAccess.Master && access >= EAccess.Master:
                        case "JG" when args.Length > 2 && access >= EAccess.Master:
                            return await Group.Command.ResponseJoinGroup(args[1], Utilities.GetArgsAsText(message, 2)).ConfigureAwait(false);
                        case "JOINGROUP" when access >= EAccess.Master:
                        case "JG" when access >= EAccess.Master:
                            return await Group.Command.ResponseJoinGroup(bot, args[1]).ConfigureAwait(false);

                        case "LEAVEGROUP" when args.Length > 2 && access >= EAccess.Master && access >= EAccess.Master:
                        case "LG" when args.Length > 2 && access >= EAccess.Master:
                            return await Group.Command.ResponseLeaveGroup(args[1], Utilities.GetArgsAsText(message, 2)).ConfigureAwait(false);
                        case "LEAVEGROUP" when access >= EAccess.Master:
                        case "LG" when access >= EAccess.Master:
                            return await Group.Command.ResponseLeaveGroup(bot, args[1]).ConfigureAwait(false);

                        case "GROUPLIST" when access >= EAccess.FamilySharing:
                        case "GL" when access >= EAccess.FamilySharing:
                            return await Group.Command.ResponseGroupList(Utilities.GetArgsAsText(message, 1)).ConfigureAwait(false);

                        //WishList
                        case "ADDWISHLIST" when args.Length > 2 && access >= EAccess.Master:
                        case "AW" when args.Length > 2 && access >= EAccess.Master:
                            return await Wishlist.Command.ResponseAddWishlist(args[1], Utilities.GetArgsAsText(message, 2)).ConfigureAwait(false);
                        case "ADDWISHLIST" when access >= EAccess.Master:
                        case "AW" when access >= EAccess.Master:
                            return await Wishlist.Command.ResponseAddWishlist(bot, args[1]).ConfigureAwait(false);

                        case "REMOVEWISHLIST" when args.Length > 2 && access >= EAccess.Master:
                        case "RW" when args.Length > 2 && access >= EAccess.Master:
                            return await Wishlist.Command.ResponseRemoveWishlist(args[1], Utilities.GetArgsAsText(message, 2)).ConfigureAwait(false);
                        case "REMOVEWISHLIST" when access >= EAccess.Master:
                        case "RW" when access >= EAccess.Master:
                            return await Wishlist.Command.ResponseRemoveWishlist(bot, args[1]).ConfigureAwait(false);

                        //Cart
                        case "CART" when access >= EAccess.Operator:
                        case "C" when access >= EAccess.Operator:
                            return await Cart.Command.ResponseGetCartGames(Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "ADDCART" when args.Length > 2 && access >= EAccess.Operator:
                        case "AC" when args.Length > 2 && access >= EAccess.Operator:
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

                        case "SETCOUNTRY" when args.Length > 2 && access >= EAccess.Master:
                        case "SC" when args.Length > 2 && access >= EAccess.Master:
                            return await Cart.Command.ResponseSetCountry(args[1], Utilities.GetArgsAsText(args, 2, ",")).ConfigureAwait(false);
                        case "SETCOUNTRY" when access >= EAccess.Master:
                        case "SC" when access >= EAccess.Master:
                            return await Cart.Command.ResponseSetCountry(bot, args[1]).ConfigureAwait(false);

                        case "PURCHASE" when access >= EAccess.Master:
                        case "PC" when access >= EAccess.Master:
                            return await Cart.Command.ResponsePurchaseSelf(Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "PURCHASEGIFT" when args.Length == 3 && access >= EAccess.Master:
                        case "PCG" when args.Length == 3 && access >= EAccess.Master:
                            return await Cart.Command.ResponsePurchaseGift(args[1], args[2]).ConfigureAwait(false);
                        case "PURCHASEGIFT" when args.Length == 2 && access >= EAccess.Master:
                        case "PCG" when args.Length == 2 && access >= EAccess.Master:
                            return await Cart.Command.ResponsePurchaseGift(bot, args[1]).ConfigureAwait(false);

                        //Store
                        case "APPDETAIL" when args.Length > 2 && access >= EAccess.Operator:
                        case "AD" when args.Length > 2 && access >= EAccess.Operator:
                            return await Store.Command.ResponseGetAppsDetail(args[1], Utilities.GetArgsAsText(args, 2, ",")).ConfigureAwait(false);
                        case "APPDETAIL" when access >= EAccess.Operator:
                        case "AD" when access >= EAccess.Operator:
                            return await Store.Command.ResponseGetAppsDetail(bot, args[1]).ConfigureAwait(false);

                        case "DELETERECOMMENT" when args.Length > 2 && access >= EAccess.Master:
                        case "DREC" when args.Length > 2 && access >= EAccess.Master:
                            return await Store.Command.ResponseDeleteReview(args[1], Utilities.GetArgsAsText(args, 2, ",")).ConfigureAwait(false);
                        case "DELETERECOMMENT" when access >= EAccess.Master:
                        case "DREC" when access >= EAccess.Master:
                            return await Store.Command.ResponseDeleteReview(bot, args[1]).ConfigureAwait(false);

                        case "PUBLISHRECOMMEND" when args.Length > 3 && access >= EAccess.Master:
                        case "PREC" when args.Length > 3 && access >= EAccess.Master:
                            return await Store.Command.ResponsePublishReview(args[1], args[2], Utilities.GetArgsAsText(args, 3, ",")).ConfigureAwait(false);
                        case "PUBLISHRECOMMEND" when args.Length == 3 && access >= EAccess.Master:
                        case "PREC" when args.Length == 3 && access >= EAccess.Master:
                            return await Store.Command.ResponsePublishReview(bot, args[1], args[2]).ConfigureAwait(false);

                        case "SEARCH" when args.Length > 2 && access >= EAccess.Operator:
                        case "SS" when args.Length > 2 && access >= EAccess.Operator:
                            return await Store.Command.ResponseSearchGame(args[1], Utilities.GetArgsAsText(args, 2, ",")).ConfigureAwait(false);
                        case "SEARCH" when access >= EAccess.Operator:
                        case "SS" when access >= EAccess.Operator:
                            return await Store.Command.ResponseSearchGame(bot, args[1]).ConfigureAwait(false);

                        case "SUBS" when args.Length > 2 && access >= EAccess.Operator:
                        case "S" when args.Length > 2 && access >= EAccess.Operator:
                            return await Store.Command.ResponseGetGameSubes(args[1], Utilities.GetArgsAsText(args, 2, ",")).ConfigureAwait(false);
                        case "SUBS" when access >= EAccess.Operator:
                        case "S" when access >= EAccess.Operator:
                            return await Store.Command.ResponseGetGameSubes(bot, args[1]).ConfigureAwait(false);

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

                        //Other
                        case "KEY" when access >= EAccess.FamilySharing:
                        case "K" when access >= EAccess.FamilySharing:
                            return Other.Command.ResponseExtractKeys(Utilities.GetArgsAsText(args, 1, ","));

                        case "EHELP" when access >= EAccess.FamilySharing:
                        case "HELP" when access >= EAccess.FamilySharing:
                            return Other.Command.ResponseCommandHelp(args);

                        //DevFuture
                        case "COOKIES" when DeveloperFeature && access >= EAccess.Owner:
                            return await DevFeature.Command.ResponseGetCookies(Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);
                        case "APIKEY" when DeveloperFeature && access >= EAccess.Owner:
                            return await DevFeature.Command.ResponseGetAPIKey(Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);
                        case "ACCESSTOKEN" when DeveloperFeature && access >= EAccess.Owner:
                            return await DevFeature.Command.ResponseGetAccessToken(Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        default:
                            return null;
                    }
            }
        }
    }
}
