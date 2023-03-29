using ArchiSteamFarm.Steam;
using ASFEnhance.Data;

namespace ASFEnhance.Wallet
{
    internal static class WebRequest
    {
        /// <summary>
        /// 激活钱包代码
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        internal static async Task<AjaxRedeemWalletCodeResponse?> RedeemWalletCode(Bot bot, string code)
        {
            Uri request = new(SteamStoreURL, "/account/ajaxredeemwalletcode");
            Uri referer = new(SteamStoreURL, "/account/redeemwalletcode");

            Dictionary<string, string> data = new(2, StringComparer.Ordinal) {
                { "wallet_code", code }
            };

            var response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<AjaxRedeemWalletCodeResponse>(request, data: data, referer: referer).ConfigureAwait(false);

            return response?.Content;
        }

        /// <summary>
        /// 激活钱包代码
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="code"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        internal static async Task<AjaxCreateWalletAndCheckFundsResponse?> RedeemWalletCode(Bot bot, string code, PluginConfig.AddressConfig address)
        {
            Uri request = new(SteamStoreURL, "/account/ajaxcreatewalletandcheckfunds/");
            Uri referer = new(SteamStoreURL, "/account/redeemwalletcode");

            Dictionary<string, string> data = new(8, StringComparer.Ordinal) {
                { "wallet_code", code },
                { "CreateFromAddress", "1" },
                { "Address", address.Address },
                { "City", address.City },
                { "Country", address.Country },
                { "State", address.State },
                { "PostCode", address.PostCode },
            };

            var response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<AjaxCreateWalletAndCheckFundsResponse>(request, data: data, referer: referer).ConfigureAwait(false);

            return response?.Content;
        }
    }
}
