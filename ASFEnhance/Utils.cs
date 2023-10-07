using ArchiSteamFarm.Core;
using ArchiSteamFarm.NLog;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;
using ASFEnhance.Data;
using System.Reflection;
using System.Text;

namespace ASFEnhance;

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
    private static string UpdateFlag => UpdatePadding ? "*" : "";

    /// <summary>
    /// 格式化返回文本
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    internal static string FormatStaticResponse(string message)
    {
        return $"<ASFE{UpdateFlag}> {message}";
    }

    /// <summary>
    /// 格式化返回文本
    /// </summary>
    /// <param name="message"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    internal static string FormatStaticResponse(string message, params object?[] args)
    {
        return FormatStaticResponse(string.Format(message, args));
    }

    /// <summary>
    /// 格式化返回文本
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    internal static string FormatBotResponse(this Bot bot, string message)
    {
        return $"<{bot.BotName}{UpdateFlag}> {message}";
    }

    /// <summary>
    /// 格式化返回文本
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="message"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    internal static string FormatBotResponse(this Bot bot, string message, params object?[] args)
    {
        return bot.FormatBotResponse(string.Format(message, args));
    }

    internal static StringBuilder AppendLineFormat(this StringBuilder sb, string format, params object?[] args)
    {
        return sb.AppendLine(string.Format(format, args));
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
        return steamId - 0x110000100000000;
    }

    /// <summary>
    /// 转换SteamId
    /// </summary>
    /// <param name="steamId"></param>
    /// <returns></returns>
    internal static ulong Steam322SteamId(ulong steamId)
    {
        return steamId + 0x110000100000000;
    }

    /// <summary>
    /// 匹配Steam商店Id
    /// </summary>
    /// <param name="query"></param>
    /// <param name="validType"></param>
    /// <param name="defaultType"></param>
    /// <returns></returns>
    internal static List<SteamGameId> FetchGameIds(string query, SteamGameIdType validType, SteamGameIdType defaultType)
    {
        var result = new List<SteamGameId>();

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

            SteamGameIdType type = strType.ToUpperInvariant() switch
            {
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
    /// 获取SessionId
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static string? FetchSessionId(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return null;
        }
        var cc = bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookies(SteamCommunityURL);
        var sessionId = cc["sessionid"];
        return sessionId?.Value;
    }

    /// <summary>
    /// 绕过年龄检查
    /// </summary>
    /// <param name="webHandler"></param>
    internal static void BypassAgeCheck(this ArchiWebHandler webHandler)
    {
        var cookieContainer = webHandler.WebBrowser.CookieContainer;
        if (string.IsNullOrEmpty(cookieContainer.GetCookieValue(SteamStoreURL, "birthtime")))
        {
            cookieContainer.Add(new System.Net.Cookie("birthtime", "0", "/", $".{SteamStoreURL.Host}"));
        }
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
    internal static Uri SteamCommunityURL => ArchiWebHandler.SteamCommunityURL;

    /// <summary>
    /// SteamAPI链接
    /// </summary>
    internal static Uri SteamApiURL => new("https://api.steampowered.com");

    /// <summary>
    /// Steam结算链接
    /// </summary>
    internal static Uri SteamCheckoutURL => ArchiWebHandler.SteamCheckoutURL;

    /// <summary>
    /// 日志
    /// </summary>
    internal static ArchiLogger ASFLogger => ASF.ArchiLogger;

    /// <summary>
    /// 布尔转换为char
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
    internal static char Bool2Str(bool b) => b ? '√' : '×';
}
