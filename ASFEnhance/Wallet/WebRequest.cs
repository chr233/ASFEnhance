#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;

namespace ASFEnhance.Wallet
{
    internal static class WebRequest
    {
        internal static async Task<AjaxRedeemWalletCodeResponse?> RedeemWalletCode(Bot bot, string code)
        {
            Uri request = new(SteamStoreURL, "/account/ajaxredeemwalletcode");
            Uri referer = new(SteamStoreURL, "/account/redeemwalletcode");

            Dictionary<string, string> data = new(2, StringComparer.Ordinal) {
                { "wallet_code", code }
            };

            ObjectResponse<AjaxRedeemWalletCodeResponse>? response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<AjaxRedeemWalletCodeResponse>(request, data: data, referer: referer).ConfigureAwait(false);

            return response?.Content;
        }
    }
}
