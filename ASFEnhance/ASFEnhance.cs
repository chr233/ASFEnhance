#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Core;
using ArchiSteamFarm.Plugins.Interfaces;
using ArchiSteamFarm.Steam;
using Chrxw.ASFEnhance.Localization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Threading.Tasks;
using static Chrxw.ASFEnhance.Utils;

namespace Chrxw.ASFEnhance
{
    [Export(typeof(IPlugin))]
    internal sealed class ASFEnhance : IASF, IBotCommand
    {
        public string Name => nameof(ASFEnhance);
        public Version Version => typeof(ASFEnhance).Assembly.GetName().Version ?? throw new ArgumentNullException(nameof(Version));

        private bool DeveloperFeature = false;

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
                switch (configProperty)
                {
                    case "ASFEnhanceDevFuture" when configValue.Type == JTokenType.Boolean:
                        this.DeveloperFeature = configValue.Value<bool>();
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
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            ASF.ArchiLogger.LogGenericInfo(string.Format(CurrentCulture, Langs.PluginVer, version.Major, version.Minor, version.Build, version.Revision));
            ASF.ArchiLogger.LogGenericInfo(string.Format(CurrentCulture, Langs.PluginContact));
            return Task.CompletedTask;
        }

        /// <summary>
        /// 处理命令事件
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="steamID"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0066:将 switch 语句转换为表达式", Justification = "<挂起>")]
        public async Task<string?> OnBotCommand(Bot bot, ulong steamID, string message, string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    throw new InvalidOperationException(nameof(args.Length));
                case 1: //不带参数
                    switch (args[0].ToUpperInvariant())
                    {
                        case "P":
                            return await bot.Commands.Response(steamID, "POINTS").ConfigureAwait(false);
                        case "PA":
                            return await bot.Commands.Response(steamID, "POINTS ASF").ConfigureAwait(false);
                        case "LA":
                            return await bot.Commands.Response(steamID, "LEVEL ASF").ConfigureAwait(false);
                        case "BA":
                            return await bot.Commands.Response(steamID, "BALANCE ASF").ConfigureAwait(false);
                        case "CA":
                            return await Cart.Command.ResponseGetCartGames(steamID, "ASF").ConfigureAwait(false);

                        //EVENT

                        //Community
                        //case "GROUPLIST":
                        //case "GL":
                        //    return await Community.Command.ResponseGroupList(bot, steamID).ConfigureAwait(false);

                        //Cart
                        case "CART":
                        case "C":
                            return await Cart.Command.ResponseGetCartGames(bot, steamID).ConfigureAwait(false);

                        case "CARTCOUNTRY":
                        case "CC":
                            return await Cart.Command.ResponseGetCartCountries(bot, steamID).ConfigureAwait(false);

                        case "CARTRESET":
                        case "CR":
                            return await Cart.Command.ResponseClearCartGames(bot, steamID).ConfigureAwait(false);
                        case "PC":
                        case "PURCHASE":
                            return await Cart.Command.ResponsePurchase(bot, steamID).ConfigureAwait(false);

                        //Profile
                        case "FRIENDCODE":
                        case "FC":
                            return Profile.Command.ResponseGetFriendCode(bot, steamID);

                        case "STEAMID":
                        case "SID":
                            return Profile.Command.ResponseGetSteamID(bot, steamID);

                        case "PROFILE":
                        case "PF":
                            return await Profile.Command.ResponseGetProfileSummary(bot, steamID).ConfigureAwait(false);

                        //Other
                        case "ASFENHANCE":
                        case "ASFE":
                            return Other.Command.ResponseASFEnhanceVersion();

                        case "KEY":
                        case "K":
                            return Other.Command.ResponseExtractKeys(Utilities.GetArgsAsText(message, 1));

                        //DevFuture
                        case "COOKIES" when this.DeveloperFeature:
                            return DevFeature.Command.ResponseGetCookies(bot, steamID);
                        case "APIKEY" when this.DeveloperFeature:
                            return await DevFeature.Command.ResponseGetAPIKey(bot, steamID).ConfigureAwait(false);
                        case "ACCESSTOKEN" when this.DeveloperFeature:
                            return await DevFeature.Command.ResponseGetAccessToken(bot, steamID).ConfigureAwait(false);

                        default:
                            return null;
                    }
                default: //带参数
                    switch (args[0].ToUpperInvariant())
                    {
                        case "AL":
                            return await bot.Commands.Response(steamID, "ADDLICENSE " + Utilities.GetArgsAsText(message, 1)).ConfigureAwait(false);
                        case "P":
                            return await bot.Commands.Response(steamID, "POINTS " + Utilities.GetArgsAsText(message, 1)).ConfigureAwait(false);

                        //Event

                        //Community
                        case "JOINGROUP" when args.Length > 2:
                        case "JG" when args.Length > 2:
                            return await Community.Command.ResponseJoinGroup(steamID, args[1], Utilities.GetArgsAsText(message, 2)).ConfigureAwait(false);
                        case "JOINGROUP":
                        case "JG":
                            return await Community.Command.ResponseJoinGroup(bot, steamID, args[1]).ConfigureAwait(false);

                        //case "GROUPLIST":
                        //case "GL":
                        //    return await Community.Command.ResponseGroupList(steamID, Utilities.GetArgsAsText(message, 1)).ConfigureAwait(false);

                        //WishList
                        case "ADDWISHLIST" when args.Length > 2:
                        case "AW" when args.Length > 2:
                            return await Wishlist.Command.ResponseAddWishlist(steamID, args[1], Utilities.GetArgsAsText(message, 2)).ConfigureAwait(false);
                        case "ADDWISHLIST":
                        case "AW":
                            return await Wishlist.Command.ResponseAddWishlist(bot, steamID, args[1]).ConfigureAwait(false);

                        case "REMOVEWISHLIST" when args.Length > 2:
                        case "RW" when args.Length > 2:
                            return await Wishlist.Command.ResponseRemoveWishlist(steamID, args[1], Utilities.GetArgsAsText(message, 2)).ConfigureAwait(false);
                        case "REMOVEWISHLIST":
                        case "RW":
                            return await Wishlist.Command.ResponseRemoveWishlist(bot, steamID, args[1]).ConfigureAwait(false);

                        //Cart
                        case "CART":
                        case "C":
                            return await Cart.Command.ResponseGetCartGames(steamID, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "ADDCART" when args.Length > 2:
                        case "AC" when args.Length > 2:
                            return await Cart.Command.ResponseAddCartGames(steamID, args[1], Utilities.GetArgsAsText(args, 2, ",")).ConfigureAwait(false);
                        case "ADDCART":
                        case "AC":
                            return await Cart.Command.ResponseAddCartGames(bot, steamID, args[1]).ConfigureAwait(false);

                        case "CC":
                        case "CARTCOUNTRY":
                            return await Cart.Command.ResponseGetCartCountries(steamID, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "CARTRESET":
                        case "CR":
                            return await Cart.Command.ResponseClearCartGames(steamID, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "SETCOUNTRY" when args.Length > 2:
                        case "SC" when args.Length > 2:
                            return await Cart.Command.ResponseSetCountry(steamID, args[1], Utilities.GetArgsAsText(args, 2, ",")).ConfigureAwait(false);
                        case "SETCOUNTRY":
                        case "SC":
                            return await Cart.Command.ResponseSetCountry(bot, steamID, args[1]).ConfigureAwait(false);

                        case "PC":
                        case "PURCHASE":
                            return await Cart.Command.ResponsePurchase(steamID, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        //Store
                        case "SUBS" when args.Length > 2:
                        case "S" when args.Length > 2:
                            return await Store.Command.ResponseGetGameSubes(steamID, args[1], Utilities.GetArgsAsText(args, 2, ",")).ConfigureAwait(false);
                        case "SUBS":
                        case "S":
                            return await Store.Command.ResponseGetGameSubes(bot, steamID, args[1]).ConfigureAwait(false);

                        case "PUBLISHRECOMMEND" when args.Length > 3:
                        case "PREC" when args.Length > 3:
                            return await Store.Command.ResponsePublishReview(steamID, args[1], args[2], Utilities.GetArgsAsText(args, 3, ",")).ConfigureAwait(false);
                        case "PUBLISHRECOMMEND" when args.Length == 3:
                        case "PREC" when args.Length == 3:
                            return await Store.Command.ResponsePublishReview(bot, steamID, args[1], args[2]).ConfigureAwait(false);

                        case "DELETERECOMMENT" when args.Length > 2:
                        case "DREC" when args.Length > 2:
                            return await Store.Command.ResponseDeleteReview(steamID, args[1], Utilities.GetArgsAsText(args, 2, ",")).ConfigureAwait(false);
                        case "DELETERECOMMENT":
                        case "DREC":
                            return await Store.Command.ResponseDeleteReview(bot, steamID, args[1]).ConfigureAwait(false);

                        //Profile
                        case "FRIENDCODE":
                        case "FC":
                            return await Profile.Command.ResponseGetFriendCode(steamID, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "STEAMID":
                        case "SID":
                            return await Profile.Command.ResponseGetSteamID(steamID, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "PROFILE":
                        case "PF":
                            return await Profile.Command.ResponseGetProfileSummary(steamID, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        //Other
                        case "K":
                        case "KEY":
                            return Other.Command.ResponseExtractKeys(message);

                        //DevFuture
                        case "COOKIES" when this.DeveloperFeature:
                            return await DevFeature.Command.ResponseGetCookies(steamID, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);
                        case "APIKEY" when this.DeveloperFeature:
                            return await DevFeature.Command.ResponseGetAPIKey(steamID, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);
                        case "ACCESSTOKEN" when this.DeveloperFeature:
                            return await DevFeature.Command.ResponseGetAccessToken(steamID, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        default:
                            return null;
                    }
            }
        }
    }
}
