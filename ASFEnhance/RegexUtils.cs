using System.Text.RegularExpressions;

namespace ASFEnhance;
internal static partial class RegexUtils
{
    [GeneratedRegex(@"\d+([.,]\d+)?")]
    public static partial Regex MatchTotalPrice();

    [GeneratedRegex("[0-9,.]+")]
    public static partial Regex MatchPrice();

    [GeneratedRegex(@"([.,])\d\d?$")]
    public static partial Regex MatchPriceValue();

    [GeneratedRegex(@"(\w+)\/(\d+)")]
    public static partial Regex MatchGameLink();

    [GeneratedRegex(@"[,.\d]+")]
    public static partial Regex MatchStrPrice();

    [GeneratedRegex("g_historyCursor = ([^;]+)")]
    public static partial Regex MatchHistortyCursor();

    [GeneratedRegex(@"^([-+])?([^\d,.]*)([\d,.]+)\s*([^\d,.]*|[руб6.]*)$")]
    public static partial Regex MatchHistoryItem();

    [GeneratedRegex(@"\( (\d+),")]
    public static partial Regex MatchSubId();

    [GeneratedRegex("g_rgTopCurators = ([^;]+);")]
    public static partial Regex MatchCuratorPayload();

    [GeneratedRegex("\"CLANACCOUNTID\":(\\d+),")]
    public static partial Regex MatchClanaCCountId();

    [GeneratedRegex("\"steamid\":\"(\\d+)\"")]
    public static partial Regex MatchSteamId();

    [GeneratedRegex(@"\( '(\d+)',")]
    public static partial Regex MatchStrOnClick();

    [GeneratedRegex("[A-Z0-9]{5}-?[A-Z0-9]{5}-?[A-Z0-9]{5}", RegexOptions.IgnoreCase, "zh-CN")]
    public static partial Regex MatchGameKey();

    [GeneratedRegex(@"%(?:(l|u|d|bot)(\d*))%")]
    public static partial Regex MatchVariables();

    [GeneratedRegex(@"ogg\/(\d+)")]
    public static partial Regex MatchGameId();

    [GeneratedRegex(@"(\d)[^,]*,")]
    public static partial Regex MatchLevel();

    [GeneratedRegex(@"gamecards\/(\d+)")]
    public static partial Regex MatchBadgeAppId();

    [GeneratedRegex("\"short_url\":\"([^\"]+)\"")]
    public static partial Regex MatchShortLink();

    [GeneratedRegex(@"((app|sub|bundle)\/\d+)")]
    public static partial Regex MatchGameIds();

    [GeneratedRegex(@"\s+|\(\?\)")]
    public static partial Regex MatchSubNames();

    [GeneratedRegex(@"\d+$")]
    public static partial Regex MatchSubPrice();

    [GeneratedRegex(@"\( (\d+) \)")]
    public static partial Regex MatchCardBalance();

    [GeneratedRegex(@"(?:(?:s.team\/p\/)|(?:steamcommunity.com\/user\/))([^-]+-[^/]+)\/([A-Z]+)")]
    public static partial Regex MatchFriendInviteLink();

    [GeneratedRegex(@"gift(\d+)_step_init")]
    public static partial Regex MatchGiftId();

    [GeneratedRegex(@"""webapi_token"":""([^""]+)""")]
    public static partial Regex MatchWebApiToken();

    [GeneratedRegex(@"(?:\(|（)([^)）]+)(?:\)|）)")]
    public static partial Regex MatchWalletTooltips();

    [GeneratedRegex(@"pending_gift_(\d+)")]
    public static partial Regex MatchPendingGift();

    [GeneratedRegex(@"ShowDeclineGiftOptions\( '\d+', '(\d+)' \);")]
    public static partial Regex MatchPendingGiftAndSender();
    
    [GeneratedRegex(@"cart=(\d+)")]
    public static partial Regex MatchCartIdFromUri();
}
