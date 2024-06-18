using ArchiSteamFarm.Steam;
using ASFEnhance.Data;
using ASFEnhance.Data.Common;


namespace ASFEnhance.Inventory;


internal static class WebRequest
{
    /// <summary>
    /// 物品堆叠
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="appId"></param>
    /// <param name="fromAssetId"></param>
    /// <param name="destAssetId"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    internal static async Task<CombineItemStacksResponse?> CombineItemStacks(Bot bot, uint appId, ulong fromAssetId, ulong destAssetId, uint amount)
    {
        var token = bot.AccessToken ?? throw new AccessTokenNullException();
        var request = new Uri(SteamApiURL, "/IInventoryService/CombineItemStacks/v1/");

        var data = new Dictionary<string, string>(3) {
            { "access_token", token },
            { "appid", appId.ToString() },
            { "fromitemid", fromAssetId.ToString() },
            { "destitemid", destAssetId.ToString() },
            { "quantity", amount.ToString() },
            { "steamid", bot.SteamID.ToString() },
        };

        var response = await bot.ArchiWebHandler.UrlPostToJsonObject<CombineItemStacksResponse>(request, data: data, referer: SteamCommunityURL).ConfigureAwait(false);
        return response?.Content;
    }

    /// <summary>
    /// 堆叠物品拆分
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="appId"></param>
    /// <param name="itemAssetId"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    internal static async Task<CombineItemStacksResponse?> SplitItemStack(Bot bot, uint appId, ulong itemAssetId, uint amount)
    {
        var token = bot.AccessToken ?? throw new AccessTokenNullException();
        var request = new Uri(SteamApiURL, "/IInventoryService/SplitItemStack/v1/");

        var data = new Dictionary<string, string>(3) {
            { "access_token", token },
            { "appid", appId.ToString() },
            { "itemid", itemAssetId.ToString() },
            { "quantity", amount.ToString() },
            { "steamid", bot.SteamID.ToString() },
        };

        var response = await bot.ArchiWebHandler.UrlPostToJsonObject<CombineItemStacksResponse>(request, data: data, referer: SteamCommunityURL).ConfigureAwait(false);
        return response?.Content;
    }
}
