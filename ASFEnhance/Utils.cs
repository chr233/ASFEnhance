using ArchiSteamFarm.Core;
using ArchiSteamFarm.NLog;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;
using ASFEnhance.Data;
using System.Reflection;

namespace ASFEnhance
{
    internal static class Utils
    {
        /// <summary>
        /// 插件配置
        /// </summary>
        internal static PluginConfig Config { get; set; } = new();

        /// <summary>
        /// 更新已就绪
        /// </summary>
        internal static bool UpdatePadding { get; set; }

        /// <summary>
        /// 更新标记
        /// </summary>
        /// <returns></returns>
        private static string UpdateFlag()
        {
            if (UpdatePadding)
            {
                return "*";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 格式化返回文本
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        internal static string FormatStaticResponse(string message)
        {
            string flag = UpdateFlag();

            return $"<ASFE{flag}> {message}";
        }

        /// <summary>
        /// 格式化返回文本
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        internal static string FormatBotResponse(this Bot bot, string message)
        {
            string flag = UpdateFlag();

            return $"<{bot.BotName}{flag}> {message}";
        }

        /// <summary>
        /// 获取个人资料链接
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> GetProfileLink(this Bot bot)
        {
            return await bot.ArchiWebHandler.GetAbsoluteProfileURL(true).ConfigureAwait(false);
        }

        /// <summary>
        /// 转换SteamId
        /// </summary>
        /// <param name="steamId"></param>
        /// <returns></returns>
        internal static ulong SteamId2Steam32(ulong steamId)
        {
            return steamId & 0x001111011111111;
        }

        /// <summary>
        /// 转换SteamId
        /// </summary>
        /// <param name="steamId"></param>
        /// <returns></returns>
        internal static ulong Steam322SteamId(ulong steamId)
        {
            return steamId | 0x110000100000000;
        }

        /// <summary>
        /// 匹配Steam商店Id
        /// </summary>
        /// <param name="query"></param>
        /// <param name="defaultType"></param>
        /// <returns></returns>
        internal static List<SteamGameId> FetchGameIds(string query, SteamGameIdType validType, SteamGameIdType defaultType)
        {
            List<SteamGameId> result = new();

            string[] entries = query.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (string entry in entries)
            {
                uint gameId;
                string strType;
                int index = entry.IndexOf('/', StringComparison.Ordinal);

                if ((index > 0) && (entry.Length > index + 1))
                {
                    if (!uint.TryParse(entry[(index + 1)..], out gameId) || (gameId == 0))
                    {
                        result.Add(new(entry, SteamGameIdType.Error, 0));
                        continue;
                    }

                    strType = entry[..index];
                }
                else if (uint.TryParse(entry, out gameId) && (gameId > 0))
                {
                    result.Add(new(entry, defaultType, gameId));
                    continue;
                }
                else
                {
                    result.Add(new(entry, SteamGameIdType.Error, 0));
                    continue;
                }

                SteamGameIdType type = strType.ToUpperInvariant() switch {
                    "A" or "APP" => SteamGameIdType.App,
                    "S" or "SUB" => SteamGameIdType.Sub,
                    "B" or "BUNDLE" => SteamGameIdType.Bundle,
                    _ => SteamGameIdType.Error,
                };

                if (validType.HasFlag(type))
                {
                    result.Add(new(entry, type, gameId));
                }
                else
                {
                    result.Add(new(entry, SteamGameIdType.Error, 0));
                }
            }
            return result;
        }

        /// <summary>
        /// 获取版本号
        /// </summary>
        internal static Version MyVersion => Assembly.GetExecutingAssembly().GetName().Version ?? new Version("0");

        /// <summary>
        /// 获取插件所在路径
        /// </summary>
        internal static string MyLocation => Assembly.GetExecutingAssembly().Location;

        /// <summary>
        /// Steam商店链接
        /// </summary>
        internal static Uri SteamStoreURL => ArchiWebHandler.SteamStoreURL;

        /// <summary>
        /// Steam社区链接
        /// </summary>
        internal static Uri SteamCommunityURL = ArchiWebHandler.SteamCommunityURL;

        /// <summary>
        /// Steam API链接
        /// </summary>
        internal static Uri SteamApiURL => new("https://api.steampowered.com");

        /// <summary>
        /// 日志
        /// </summary>
        internal static ArchiLogger ASFLogger => ASF.ArchiLogger;
    }
}
