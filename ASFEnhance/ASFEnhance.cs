#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Core;
using ArchiSteamFarm.Plugins.Interfaces;
using ArchiSteamFarm.Steam;
using Chrxw.ASFEnhance.Localization;
using System;
using System.Composition;
using System.Threading.Tasks;
using static Chrxw.ASFEnhance.Utils;

namespace Chrxw.ASFEnhance
{
    [Export(typeof(IPlugin))]
    internal sealed class ASFEnhance : IBotCommand
    {
        public string Name => nameof(ASFEnhance);
        public Version Version => typeof(ASFEnhance).Assembly.GetName().Version ?? throw new ArgumentNullException(nameof(Version));

        public void OnLoaded()
        {
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            ASF.ArchiLogger.LogGenericInfo(string.Format(CurrentCulture, Langs.PluginVer, version.Major, version.Minor, version.Build, version.Revision));
            ASF.ArchiLogger.LogGenericInfo(string.Format(CurrentCulture, Langs.PluginContact));
        }

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
                        case "EVENT":
                        case "E":
                            return await Event.Command.ResponseSteamEvents(bot, steamID, "").ConfigureAwait(false);

                        case "EVENTCHECK":
                        case "EC":
                            return await Event.Command.ResponseCheckSummerBadge(bot, steamID).ConfigureAwait(false);

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

                        case "COOKIES":
                            return Other.Command.ResponseGetCookies(bot, steamID);

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
                        case "EVENT" when args.Length > 2:
                        case "E" when args.Length > 2:
                            return await Event.Command.ResponseSteamEvents(steamID, args[1], Utilities.GetArgsAsText(message, 2)).ConfigureAwait(false);
                        case "EVENT":
                        case "E":
                            return await Event.Command.ResponseSteamEvents(bot, steamID, args[1]).ConfigureAwait(false);

                        case "EVENTCHECK":
                        case "EC":
                            return await Event.Command.ResponseCheckSummerBadge(steamID, Utilities.GetArgsAsText(message, 1)).ConfigureAwait(false);

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
                        case "DREC" :
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

                        case "COOKIES":
                            return await Other.Command.ResponseGetCookies(steamID, Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);

                        default:
                            return null;
                    }
            }
        }
    }
}
