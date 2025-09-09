using ArchiSteamFarm.Steam;
using ASFEnhance.Data;
using ASFEnhance.Data.IFamilyGroupsService;

namespace ASFEnhance.Family;

internal static class WebRequest
{
    /// <summary>
    /// 获取账号家庭组信息
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    /// <exception cref="AccessTokenNullException"></exception>
    public static async Task<GetFamilyGroupForUserResponse?> GetFamilyGroupForUser(Bot bot)
    {
        var token = bot.AccessToken ?? throw new AccessTokenNullException(bot);
        var request = new Uri(SteamApiURL, $"/IFamilyGroupsService/GetFamilyGroupForUser/v1?access_token={token}&include_family_group_response=1");
        var response = await bot.ArchiWebHandler.UrlGetToJsonObjectWithSession<AbstractResponse<GetFamilyGroupForUserResponse>>(request).ConfigureAwait(false);

        return response?.Content?.Response;
    }

    /// <summary>
    /// 修改家庭组名称
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="groupId"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="AccessTokenNullException"></exception>
    public static async Task<JoinFamilyGroupResponse?> ModifyFamilyGroupDetails(Bot bot, int groupId, string name)
    {
        var token = bot.AccessToken ?? throw new AccessTokenNullException(bot);
        var request = new Uri(SteamApiURL, $"/IFamilyGroupsService/ModifyFamilyGroupDetails/v1/?access_token={token}&family_groupid={groupId}&name={name}");

        var response = await bot.ArchiWebHandler.UrlPostToJsonObject<AbstractResponse<JoinFamilyGroupResponse>>(request).ConfigureAwait(false);

        return response?.Content?.Response;
    }
}