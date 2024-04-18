using ArchiSteamFarm.Core;
using ArchiSteamFarm.Helpers.Json;
using ArchiSteamFarm.NLog;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;
using ArchiSteamFarm.Web;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data.Plugin;
using ProtoBuf;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using System.Text.Json;
using static ArchiSteamFarm.Steam.Integration.ArchiWebHandler;

namespace ASFEnhance;

internal static class Utils
{
    /// <summary>
    /// 插件配置
    /// </summary>
    internal static PluginConfig Config { get; set; } = new();

    /// <summary>
    /// 格式化返回文本
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    internal static string FormatStaticResponse(string message)
    {
        return $"<ASFE> {message}";
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
        return $"<{bot.BotName}> {message}";
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

    [Obsolete("请使用 AppendLine")]
    internal static StringBuilder AppendLineFormat(this StringBuilder sb, string format)
    {
        return sb.AppendLine(format);
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
    internal static ulong SteamId2Steam32(ulong steamId) => steamId - 0x110000100000000;

    /// <summary>
    /// 转换SteamId
    /// </summary>
    /// <param name="steamId"></param>
    /// <returns></returns>
    internal static ulong Steam322SteamId(ulong steamId) => steamId + 0x110000100000000;

    internal static bool IsSteam32ID(ulong id) => id <= 0xFFFFFFFF;

    /// <summary>
    /// 匹配Steam商店Id
    /// </summary>
    /// <param name="query"></param>
    /// <param name="validType"></param>
    /// <param name="defaultType"></param>
    /// <returns></returns>
    internal static List<SteamGameId> FetchGameIds(string query, ESteamGameIdType validType, ESteamGameIdType defaultType)
    {
        var result = new List<SteamGameId>();
        var entries = query.Split(',', StringSplitOptions.RemoveEmptyEntries);

        foreach (string entry in entries)
        {
            uint gameId;
            string strType;
            int index = entry.IndexOf('/', StringComparison.Ordinal);

            if ((index > 0) && (entry.Length > index + 1))
            {
                if (!uint.TryParse(entry[(index + 1)..], out gameId) || (gameId == 0))
                {
                    result.Add(new(entry, ESteamGameIdType.Error, 0));
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
                result.Add(new(entry, ESteamGameIdType.Error, 0));
                continue;
            }

            ESteamGameIdType type = strType.ToUpperInvariant() switch
            {
                "A" or "APP" => ESteamGameIdType.App,
                "S" or "SUB" => ESteamGameIdType.Sub,
                "B" or "BUNDLE" => ESteamGameIdType.Bundle,
                _ => ESteamGameIdType.Error,
            };

            if (validType.HasFlag(type))
            {
                result.Add(new(entry, type, gameId));
            }
            else
            {
                result.Add(new(entry, ESteamGameIdType.Error, 0));
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
    internal static Version MyVersion => Assembly.GetExecutingAssembly().GetName().Version ?? new Version("0.0.0.0");

    /// <summary>
    /// 获取ASF版本
    /// </summary>
    internal static Version ASFVersion => typeof(ASF).Assembly.GetName().Version ?? new Version("0.0.0.0");

    /// <summary>
    /// 获取插件所在路径
    /// </summary>
    internal static string MyLocation => Assembly.GetExecutingAssembly().Location;

    /// <summary>
    /// 获取插件所在文件夹路径
    /// </summary>
    internal static string MyDirectory => Path.GetDirectoryName(MyLocation) ?? ".";

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
    internal static char ToStr(this bool b) => Bool2Str(b);

    /// <summary>
    /// 跳过参数获取Bot名称
    /// </summary>
    /// <param name="args"></param>
    /// <param name="skipStart"></param>
    /// <param name="skipEnd"></param>
    /// <returns></returns>
    internal static string SkipBotNames(string[] args, int skipStart, int skipEnd)
    {
        return string.Join(',', args[skipStart..(args.Length - skipEnd)]);
    }

    /// <summary>
    /// 命令是否被禁用
    /// </summary>
    /// <param name="cmd"></param>
    /// <returns></returns>
    internal static bool IsCmdDisabled(string cmd)
    {
        return Config.DisabledCmds?.Contains(cmd) == true;
    }

    /// <summary>
    /// Protobuf编码
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="payload"></param>
    /// <returns></returns>
    internal static string ProtoBufEncode<T>(T payload)
    {
        var ms = new MemoryStream();
        Serializer.Serialize(ms, payload);
        var enc = Convert.ToBase64String(ms.ToArray());
        return enc;
    }

    /// <summary>
    /// 逗号分隔符
    /// </summary>
    internal static readonly char[] SeparatorDot = [','];

    /// <summary>
    /// 加号分隔符
    /// </summary>
    internal static readonly char[] SeparatorPlus = ['+'];

    /// <summary>
    /// 逗号空格分隔符
    /// </summary>
    internal static readonly char[] SeparatorDotSpace = [',', ' '];

    /// <summary>
    /// 需要编码的URL字符串
    /// </summary>
    private static readonly ReadOnlyDictionary<char, string> CharacterEncodings = new Dictionary<char, string>() {
        {' ', "%20"}, {'!', "%21"}, {'\"', "%22"}, {'#', "%23"},  {'$', "%24"},
        {'%', "%25"}, {'&', "%26"}, {'\'', "%27"}, {'(', "%28"},  {')', "%29"},
        {'*', "%2A"}, {'+', "%2B"}, {',', "%2C"},  {'-', "%2D"},  {'.', "%2E"},
        {'/', "%2F"}, {':', "%3A"}, {';', "%3B"},  {'<', "%3C"},  {'=', "%3D"},
        {'>', "%3E"}, {'@', "%40"}, {'[', "%5B"},  {'\\', "%5C"}, {']', "%5D"},
        {'^', "%5E"}, {'_', "%5F"}, {'`', "%60"},  {'{', "%7B"},  {'|', "%7C"},
        {'}', "%7D"}, {'~', "%7E"},
    }.AsReadOnly();

    /// <summary>
    /// URL编码
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    internal static string UrlEncode(string url)
    {
        var encodedUrl = new StringBuilder();
        foreach (var c in url)
        {
            if (CharacterEncodings.TryGetValue(c, out var str))
            {
                encodedUrl.Append(str);
            }
            else
            {
                encodedUrl.Append(c);
            }
        }
        return encodedUrl.ToString();
    }

    internal static Task<ObjectResponse<T>?> UrlPostToJsonObject<T>(this ArchiWebHandler handler,
        Uri request,
        IDictionary<string, string>? data = null,
        Uri? referer = null,
        WebBrowser.ERequestOptions requestOptions = WebBrowser.ERequestOptions.None,
        bool checkSessionPreemptively = true,
        byte maxTries = WebBrowser.MaxTries,
        int rateLimitingDelay = 0,
        bool allowSessionRefresh = true,
        CancellationToken cancellationToken = default) => handler.UrlPostToJsonObjectWithSession<T>(request, null, data, referer, requestOptions, ESession.None, checkSessionPreemptively, maxTries, rateLimitingDelay, allowSessionRefresh, cancellationToken);

    public static Task<bool> UrlPost(this ArchiWebHandler handler,
        Uri request,
        IDictionary<string, string>? data = null,
        Uri? referer = null,
        WebBrowser.ERequestOptions requestOptions = WebBrowser.ERequestOptions.None,
        bool checkSessionPreemptively = true,
        byte maxTries = WebBrowser.MaxTries,
        int rateLimitingDelay = 0,
        bool allowSessionRefresh = true,
        CancellationToken cancellationToken = default) => handler.UrlPostWithSession(request, null, data, referer, requestOptions, ESession.None, checkSessionPreemptively, maxTries, rateLimitingDelay, allowSessionRefresh, cancellationToken);

    /// <summary>
    /// Json序列化设置
    /// </summary>
    public static JsonSerializerOptions JsonOptions => JsonUtilities.DefaultJsonSerialierOptions;

    public static JsonSerializerOptions DebugJsonOptions => JsonUtilities.IndentedJsonSerialierOptions;


    internal static string GetGifteeProfile(ulong accountId)
    {
        ulong steam32;
        ulong steam64;

        if (IsSteam32ID(accountId))
        {
            steam32 = accountId;
            steam64 = Steam322SteamId(accountId);
        }
        else
        {
            steam32 = SteamId2Steam32(accountId);
            steam64 = accountId;
        }

        if (Bot.BotsReadOnly != null)
        {
            foreach (var bot in Bot.BotsReadOnly.Values)
            {
                if (bot.SteamID == steam64)
                {
                    return string.Format("{0} ({1})", bot.BotName, steam64);
                }
            }
        }

        return string.Format("{0} ({1})", steam32, steam64);
    }

    internal static string DefaultOrCurrentLanguage => Config.DefaultLanguage ?? Langs.Language;
}
