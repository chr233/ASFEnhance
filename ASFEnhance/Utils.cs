#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Core;
using ArchiSteamFarm.NLog;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;
using ArchiSteamFarm.Steam.Interaction;
using ASFEnhance.Data;
using ASFEnhance.Localization;

namespace ASFEnhance
{
    internal static class Utils
    {
        internal static bool UpdateTips { get; set; }

        /// <summary>
        /// 格式化返回文本
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        internal static string FormatStaticResponse(string message)
        {
            if (!UpdateTips)
            {
                return Commands.FormatStaticResponse(message);
            }
            else
            {
                return Commands.FormatStaticResponse($"{Langs.PluginUpdateReady}\n{message}");
            }
        }

        /// <summary>
        /// 格式化返回文本
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        internal static string FormatBotResponse(this Bot bot, string message)
        {
            if (!UpdateTips)
            {
                return bot.Commands.FormatBotResponse(message);
            }
            else
            {
                return bot.Commands.FormatBotResponse($"{Langs.PluginUpdateReady}\n{message}");
            }
        }

        /// <summary>
        /// 获取Bot SessionID
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static string? GetBotSessionID(this Bot bot)
        {
            string? sessionID = bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookieValue(SteamStoreURL, "sessionid");

            if (string.IsNullOrEmpty(sessionID))
            {
                bot.ArchiLogger.LogNullError(nameof(sessionID));
                return null;
            }

            return sessionID;
        }

        /// <summary>
        /// 转换SteamID
        /// </summary>
        /// <param name="steamID"></param>
        /// <returns></returns>
        internal static ulong SteamID2Steam32(ulong steamID)
        {
            return steamID - 0x110000100000000;
        }

        /// <summary>
        /// 匹配Steam商店ID
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        internal static Dictionary<string, SteamGameID> FetchGameIDs(string query)
        {
            return FetchGameIDs(query, SteamGameIDType.App);
        }

        /// <summary>
        /// 匹配Steam商店ID
        /// </summary>
        /// <param name="query"></param>
        /// <param name="defaultType"></param>
        /// <returns></returns>
        internal static Dictionary<string, SteamGameID> FetchGameIDs(string query, SteamGameIDType defaultType)
        {
            Dictionary<string, SteamGameID> result = new();

            string[] entries = query.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string entry in entries)
            {
                uint gameID;
                string strType;
                int index = entry.IndexOf('/', StringComparison.Ordinal);

                if ((index > 0) && (entry.Length > index + 1))
                {
                    if (!uint.TryParse(entry[(index + 1)..], out gameID) || (gameID == 0))
                    {
                        result.Add(entry, new(SteamGameIDType.Error, 0));
                        continue;
                    }

                    strType = entry[..index];
                }
                else if (uint.TryParse(entry, out gameID) && (gameID > 0))
                {
                    result.Add(entry, new(defaultType, gameID));
                    continue;
                }
                else
                {
                    result.Add(entry, new(SteamGameIDType.Error, 0));
                    continue;
                }

                SteamGameIDType type = strType.ToUpperInvariant() switch {
                    "A" or "APP" => SteamGameIDType.App,
                    "S" or "SUB" => SteamGameIDType.Sub,
                    "B" or "BUNDLE" => SteamGameIDType.Bundle,
                    _ => SteamGameIDType.Error,
                };
                result.Add(entry, new(type, gameID));
            }
            return result;
        }

        /// <summary>
        /// 获取版本号
        /// </summary>
        internal static Version MyVersion => typeof(ASFEnhance).Assembly.GetName().Version;

        /// <summary>
        /// 获取插件所在路径
        /// </summary>
        internal static string MyLocation => typeof(ASFEnhance).Assembly.Location;

        /// <summary>
        /// Steam商店链接
        /// </summary>
        internal static Uri SteamStoreURL => ArchiWebHandler.SteamStoreURL;

        /// <summary>
        /// Steam社区链接
        /// </summary>
        internal static Uri SteamCommunityURL => ArchiWebHandler.SteamCommunityURL;

        /// <summary>
        /// 日志
        /// </summary>
        internal static ArchiLogger ASFLogger => ASF.ArchiLogger;
    }
}
