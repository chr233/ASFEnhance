#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Data;
using ArchiSteamFarm.Web.Responses;

using SteamKit2;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using static Chrxw.ASFEnhance.Utils;

namespace Chrxw.ASFEnhance.Wishlist
{
    internal static class WebRequest
    {
        /// <summary>
        /// 添加愿望单
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="gameID"></param>
        /// <returns></returns>
        internal static async Task<bool> AddWishlist(Bot bot, uint gameID)
        {
            if (gameID == 0)
            {
                throw new ArgumentException(null, nameof(gameID));
            }

            Uri request = new(SteamStoreURL, "/api/addtowishlist");
            Uri referer = new(SteamStoreURL, "/app/" + gameID);

            Dictionary<string, string> data = new(2, StringComparer.Ordinal)
            {
                { "appid", gameID.ToString() },
                //{ "sessionid", "" }
            };

            ObjectResponse<ResultResponse>? response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<ResultResponse>(request, data: data, referer: referer).ConfigureAwait(false);

            if (response == null)
            {
                return false;
            }

            if (response.Content.Result != EResult.OK)
            {
                bot.ArchiLogger.LogGenericWarning(Strings.WarningFailed);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 删除愿望单
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="gameID"></param>
        /// <returns></returns>
        internal static async Task<bool> RemoveWishlist(Bot bot, uint gameID)
        {
            if (gameID == 0)
            {
                throw new ArgumentException(null, nameof(gameID));
            }

            Uri request = new(SteamStoreURL, "/api/removefromwishlist");
            Uri referer = new(SteamStoreURL, "/app/" + gameID);

            Dictionary<string, string> data = new(2, StringComparer.Ordinal)
            {
                { "appid", gameID.ToString() },
                //{ "sessionid", "" }
            };

            ObjectResponse<ResultResponse>? response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<ResultResponse>(request, data: data, referer: referer).ConfigureAwait(false);

            if (response == null)
            {
                return false;
            }

            if (response.Content.Result != EResult.OK)
            {
                bot.ArchiLogger.LogGenericWarning(Strings.WarningFailed);
                return false;
            }
            return true;
        }
    }
}
