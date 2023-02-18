using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Data;
using SteamKit2;
using System.Text;

namespace ASFEnhance.Wallet
{
    internal static class Command
    {
        /// <summary>
        /// 激活钱包充值码
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="targetCode"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseRedeemWallet(Bot bot, string targetCode)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            if (string.IsNullOrEmpty(targetCode))
            {
                throw new ArgumentNullException(nameof(targetCode));
            }

            StringBuilder sb = new();

            string[] codes = targetCode.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (codes.Length > 1)
            {
                sb.AppendLine(Langs.MultipleLineResult);
            }

            foreach (string code in codes)
            {
                AjaxRedeemWalletCodeResponse? result = await WebRequest.RedeemWalletCode(bot, code).ConfigureAwait(false);

                if (result?.Success == (int)EResult.OK)
                {
                    sb.AppendLine(string.Format(Langs.CookieItem, code, Langs.Success));
                }
                else
                {
                    sb.AppendLine(string.Format(Langs.RedeemWalletError, code, Langs.Failure, result?.Detail.ToString() ?? Langs.NetworkError));
                }
            }

            return bot.FormatBotResponse(sb.ToString());
        }

        /// <summary>
        /// 激活钱包充值码 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="targetCode"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseRedeemWallet(string botNames, string targetCode)
        {
            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseRedeemWallet(bot, targetCode))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }
    }
}
