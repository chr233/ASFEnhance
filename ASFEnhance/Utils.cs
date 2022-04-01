using ArchiSteamFarm.Core;
using ArchiSteamFarm.NLog;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;
using ArchiSteamFarm.Steam.Interaction;
using System;
using System.Globalization;

namespace Chrxw.ASFEnhance
{
    internal static class Utils
    {
        /// <summary>
        /// 格式化返回文本
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static string FormatStaticResponse(string response)
        {
            return Commands.FormatStaticResponse(response);
        }

        /// <summary>
        /// 格式化返回文本
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static string FormatBotResponse(Bot bot, string response)
        {
            return bot.Commands.FormatBotResponse(response);
        }

        internal static Version MyVersion => typeof(ASFEnhance).Assembly.GetName().Version;

        /// <summary>
        /// 当前语言代码
        /// </summary>
        internal static CultureInfo CurrentCulture => CultureInfo.CurrentCulture;

        /// <summary>
        /// Steam商店链接
        /// </summary>
        internal static Uri SteamStoreURL => ArchiWebHandler.SteamStoreURL;
        /// <summary>
        /// Steam社区链接
        /// </summary>
        internal static Uri SteamCommunityURL => ArchiWebHandler.SteamCommunityURL;

        internal static ArchiLogger ASFLogger => ASF.ArchiLogger;
    }
}
