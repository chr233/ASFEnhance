using ArchiSteamFarm.Steam;

namespace ASFEnhance;
/// <summary>
/// AccessToken 为NULL
/// </summary>
public class AccessTokenNullException(Bot bot) : Exception(bot.BotName)
{
}