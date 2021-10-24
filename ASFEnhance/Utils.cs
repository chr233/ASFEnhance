using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;
using ArchiSteamFarm.Steam.Interaction;
using Chrxw.ASFEnhance.Localization;
using System;
using System.Globalization;

namespace Chrxw.ASFEnhance
{
    internal class Utils
    {
        internal static string FormatStaticResponse(string response)
        {
            return Commands.FormatStaticResponse(response);
        }

        internal static string FormatBotResponse(Bot bot, string response)
        {
            return bot.Commands.FormatBotResponse(response);
        }

        internal static string FormatBoolen(bool result)
        {
            return result ? Langs.Success : Langs.Failure;
        }

        internal static CultureInfo CurrentCulture => CultureInfo.CurrentCulture;

        internal static Uri SteamStoreURL => ArchiWebHandler.SteamStoreURL;
        internal static Uri SteamCommunityURL => ArchiWebHandler.SteamCommunityURL;

    }
}
