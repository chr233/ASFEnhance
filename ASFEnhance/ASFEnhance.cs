#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Core;
using ArchiSteamFarm.Plugins.Interfaces;
using ArchiSteamFarm.Steam;

using Chrxw.ASFEnhance.Localization;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Composition;
using System.Threading.Tasks;

using static Chrxw.ASFEnhance.Utils;

namespace Chrxw.ASFEnhance
{
    [Export(typeof(IPlugin))]
    internal sealed class ASFEnhance : IASF, IBotCommand2
    {
        public string Name => nameof(ASFEnhance);
        public Version Version => typeof(ASFEnhance).Assembly.GetName().Version ?? throw new ArgumentNullException(nameof(Version));

        [JsonProperty]
        public bool DeveloperFeature { get; private set; } = false;

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
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            ASFLogger.LogGenericInfo(string.Format(CurrentCulture, Langs.PluginVer, nameof(ASFEnhance), version.Major, version.Minor, version.Build, version.Revision));
            ASFLogger.LogGenericInfo(string.Format(CurrentCulture, Langs.PluginContact));

            if (DeveloperFeature)
            {
                ASFLogger.LogGenericWarning(string.Format(CurrentCulture, Langs.DevFeatureEnabledWarning));
            }

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
        /// <exception cref="InvalidOperationException"></exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0066:将 switch 语句转换为表达式", Justification = "<挂起>")]
        public async Task<string?> OnBotCommand(Bot bot, EAccess access, string message, string[] args, ulong steamID = 0)
        {
            switch (args.Length)
            {
                case 0:
                    throw new InvalidOperationException(nameof(args.Length));
                case 1: //不带参数
                    switch (args[0].ToUpperInvariant())
                    {
                        case "P":
                            return await bot.Commands.Response(access, "POINTS", steamID).ConfigureAwait(false);
                        case "PA":
                            return await bot.Commands.Response(access, "POINTS ASF", steamID).ConfigureAwait(false);
                        case "LA":
                            return await bot.Commands.Response(access, "LEVEL ASF", steamID).ConfigureAwait(false);
                        case "BA":
                            return await bot.Commands.Response(access, "BALANCE ASF", steamID).ConfigureAwait(false);
                        case "CA":
                            return await Cart.Command.ResponseGetCartGames(access, "ASF").ConfigureAwait(false);

                        //EVENT

                        //Community
                        //case "GROUPLIST":
                        //case "GL":
                        //    return await Community.Command.ResponseGroupList(bot, steamID).ConfigureAwait(false);

                        //Cart
                        case "CART":
                        case "C":
                            return await Cart.Command.ResponseGetCartGames(bot, access).ConfigureAwait(false);

                        case "CARTCOUNTRY":
                        case "CC":
                            return await Cart.Command.ResponseGetCartCountries(bot, access).ConfigureAwait(false);

                        case "CARTRESET":
                        case "CR":
                            return await Cart.Command.ResponseClearCartGames(bot, access).ConfigureAwait(false);
                        case "PC":
                        case "PURCHASE":
                            return await Cart.Command.ResponsePurchase(bot, access).ConfigureAwait(false);

                        //Profile
                        case "FRIENDCODE":
                        case "FC":
                            return Profile.Command.ResponseGetFriendCode(bot, access);

                        case "STEAMID":
                        case "SID":
                            return Profile.Command.ResponseGetSteamID(bot, access);

                        case "PROFILE":
                        case "PF":
                            return await Profile.Command.ResponseGetProfileSummary(bot, access).ConfigureAwait(false);

                        //Other
                        case "ASFENHANCE":
                        case "ASFE":
                            return Other.Command.ResponseASFEnhanceVersion(access);

                        //DevFuture
                        case "COOKIES" when this.DeveloperFeature:
                            return DevFeature.Command.ResponseGetCookies(bot, access);
                        case "APIKEY" when this.DeveloperFeature:
                            return await DevFeature.Command.ResponseGetAPIKey(bot, access).ConfigureAwait(false);
                        case "ACCESSTOKEN" when this.DeveloperFeature:
                            return await DevFeature.Command.ResponseGetAccessToken(bot, access).ConfigureAwait(false);

                        default:
                            return null;
                    }
                default: //带参数
                    switch (args[0].ToUpperInvariant())
                    {
                        case "AL":
                            return await bot.Commands.Response(access, "ADDLICENSE " + Utilities.GetArgsAsText(message, 1), steamID).ConfigureAwait(false);
                        case "P":
                            return await bot.Commands.Response(access, "POINTS " + Utilities.GetArgsAsText(message, 1), steamID).ConfigureAwait(false);

                        //Event

                        //Community
                        case "JOINGROUP" when args.Length > 2:
                        case "JG" when args.Length > 2:
                            return await Community.Command.ResponseJoinGroup(access, args[1], Utilities.GetArgsAsText(message, 2)).ConfigureAwait(false);
                        case "JOINGROUP":
                        case "JG":
                            return await Community.Command.ResponseJoinGroup(bot, access, args[1]).ConfigureAwait(false);

                        //case "GROUPLIST":
                        //case "GL":
                        //    return await Community.Command.ResponseGroupList(steamID, Utilities.GetArgsAsText(message, 1)).ConfigureAwait(false);

                        //WishList
                        case "ADDWISHLIST" when args.Length > 2:
                        case "AW" when args.Length > 2:
                            return await Wishlist.Command.ResponseAddWishlist(access, args[1], Utilities.GetArgsAsText(message, 2)).ConfigureAwait(false);
                        case "ADDWISHLIST":
                        case "AW":
                            return await Wishlist.Command.ResponseAddWishlist(bot, access, args[1]).ConfigureAwait(false);

                        case "REMOVEWISHLIST" when args.Length > 2:
                        case "RW" when args.Length > 2:
                            return await Wishlist.Command.ResponseRemoveWishlist(access, args[1], Utilities.GetArgsAsText(message, 2)).ConfigureAwait(false);
                        case "REMOVEWISHLIST":
                        case "RW":
                            return await Wishlist.Command.ResponseRemoveWishlist(bot, access, args[1]).ConfigureAwait(false);

                        //Cart
                        case "CART":
                        case "C":
                            return await Cart.Command.ResponseGetCartGames(access, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "ADDCART" when args.Length > 2:
                        case "AC" when args.Length > 2:
                            return await Cart.Command.ResponseAddCartGames(access, args[1], Utilities.GetArgsAsText(args, 2, ",")).ConfigureAwait(false);
                        case "ADDCART":
                        case "AC":
                            return await Cart.Command.ResponseAddCartGames(bot, access, args[1]).ConfigureAwait(false);

                        case "CC":
                        case "CARTCOUNTRY":
                            return await Cart.Command.ResponseGetCartCountries(access, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "CARTRESET":
                        case "CR":
                            return await Cart.Command.ResponseClearCartGames(access, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "SETCOUNTRY" when args.Length > 2:
                        case "SC" when args.Length > 2:
                            return await Cart.Command.ResponseSetCountry(access, args[1], Utilities.GetArgsAsText(args, 2, ",")).ConfigureAwait(false);
                        case "SETCOUNTRY":
                        case "SC":
                            return await Cart.Command.ResponseSetCountry(bot, access, args[1]).ConfigureAwait(false);

                        case "PC":
                        case "PURCHASE":
                            return await Cart.Command.ResponsePurchase(access, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        //Store
                        case "SUBS" when args.Length > 2:
                        case "S" when args.Length > 2:
                            return await Store.Command.ResponseGetGameSubes(access, args[1], Utilities.GetArgsAsText(args, 2, ",")).ConfigureAwait(false);
                        case "SUBS":
                        case "S":
                            return await Store.Command.ResponseGetGameSubes(bot, access, args[1]).ConfigureAwait(false);

                        case "PUBLISHRECOMMEND" when args.Length > 3:
                        case "PREC" when args.Length > 3:
                            return await Store.Command.ResponsePublishReview(access, args[1], args[2], Utilities.GetArgsAsText(args, 3, ",")).ConfigureAwait(false);
                        case "PUBLISHRECOMMEND" when args.Length == 3:
                        case "PREC" when args.Length == 3:
                            return await Store.Command.ResponsePublishReview(bot, access, args[1], args[2]).ConfigureAwait(false);

                        case "DELETERECOMMENT" when args.Length > 2:
                        case "DREC" when args.Length > 2:
                            return await Store.Command.ResponseDeleteReview(access, args[1], Utilities.GetArgsAsText(args, 2, ",")).ConfigureAwait(false);
                        case "DELETERECOMMENT":
                        case "DREC":
                            return await Store.Command.ResponseDeleteReview(bot, access, args[1]).ConfigureAwait(false);

                        //Profile
                        case "FRIENDCODE":
                        case "FC":
                            return await Profile.Command.ResponseGetFriendCode(access, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "STEAMID":
                        case "SID":
                            return await Profile.Command.ResponseGetSteamID(access, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        case "PROFILE":
                        case "PF":
                            return await Profile.Command.ResponseGetProfileSummary(access, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        //Other
                        case "K":
                        case "KEY":
                            return Other.Command.ResponseExtractKeys(access, message);

                        //DevFuture
                        case "COOKIES" when this.DeveloperFeature:
                            return await DevFeature.Command.ResponseGetCookies(access, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);
                        case "APIKEY" when this.DeveloperFeature:
                            return await DevFeature.Command.ResponseGetAPIKey(access, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);
                        case "ACCESSTOKEN" when this.DeveloperFeature:
                            return await DevFeature.Command.ResponseGetAccessToken(access, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        default:
                            return null;
                    }
            }
        }
    }
}
