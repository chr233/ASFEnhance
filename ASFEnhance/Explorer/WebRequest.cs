using ArchiSteamFarm.Steam;
using ASFEnhance.Data;
using ASFEnhance.Data.IStoreService;

namespace ASFEnhance.Explorer;

internal static class WebRequest
{
    internal static async Task<List<uint>?> GetDiscoveryQueue(Bot bot)
    {
        var token = bot.AccessToken ?? throw new AccessTokenNullException(bot);
        var country = bot.GetUserCountryCode();
        var request = new Uri(SteamApiURL,
            $"/IStoreService/GetDiscoveryQueue/v1/?access_token={token}&country_code={country}&rebuild_queue=1&queue_type=0&ignore_user_preferences=1");
        var response = await bot.ArchiWebHandler
            .UrlGetToJsonObjectWithSession<AbstractResponse<GetDiscoveryQueueResponse>>(request).ConfigureAwait(false);

        return response?.Content?.Response?.AppIds;
    }

    internal static async Task<bool> SkipDiscoveryQueueItem(Bot bot, uint appId)
    {
        var token = bot.AccessToken ?? throw new AccessTokenNullException(bot);
        var request = new Uri(SteamApiURL,
            $"/IStoreService/SkipDiscoveryQueueItem/v1/?access_token={token}&appid={appId}");
        var response = await bot.ArchiWebHandler
            .UrlPostToJsonObject<AbstractResponse>(request).ConfigureAwait(false);

        return response?.Content != null;
    }
}