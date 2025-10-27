using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using SteamKit2;
using System.Text;

namespace ASFEnhance.Wallet;

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

        var sb = new StringBuilder();

        string[] codes = targetCode.Split(SeparatorDot, StringSplitOptions.RemoveEmptyEntries);

        if (codes.Length > 1)
        {
            sb.AppendLine(Langs.MultipleLineResult);
        }

        foreach (string code in codes)
        {
            var result = await WebRequest.RedeemWalletCode(bot, code).ConfigureAwait(false);

            if (result != null)
            {
                if (result.Success == EResult.OK)
                {
                    sb.AppendLineFormat(Langs.CookieItem, code, Langs.Success);
                }
                else if (result.Success == EResult.InvalidState)
                {
                    if (Config.Addresses?.Count > 0)
                    {
                        var address = Config.Addresses[Random.Shared.Next(0, Config.Addresses.Count)];
                        var result2 = await WebRequest.RedeemWalletCode(bot, code, address).ConfigureAwait(false);

                        if (result2 != null)
                        {
                            sb.AppendLineFormat(Langs.RedeemWalletError, code, result2.Success == EResult.OK ? Langs.Success : Langs.Failure, result.Detail.ToString());
                        }
                        else
                        {
                            sb.AppendLineFormat(Langs.CookieItem, code, Langs.NetworkError);
                        }
                    }
                    else
                    {
                        sb.AppendLineFormat(Langs.CookieItem, code, Langs.NoAvilableAddressError);
                    }
                }
                else
                {
                    sb.AppendLineFormat(Langs.RedeemWalletError, code, Langs.Failure, result.Detail);
                }
            }
            else
            {
                sb.AppendLineFormat(Langs.CookieItem, code, Langs.NetworkError);
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

        var bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseRedeemWallet(bot, targetCode))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 激活钱包充值码 (多个Bot)
    /// </summary>
    /// <param name="targetCode"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseRedeemWalletMuli(string targetCode)
    {
        var bots = Bot.GetBots("ASF")?.Where(x => x.IsConnectedAndLoggedOn).ToList();

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(Strings.BotNotFound, "ASF");
        }

        var codes = targetCode.Split(',', StringSplitOptions.RemoveEmptyEntries);

        var tasks = new List<Task<string?>>();
        int index = 0;
        foreach (string code in codes)
        {
            if (index >= bots.Count)
            {
                index = 0;
            }
            var bot = bots[index++];
            tasks.Add(ResponseRedeemWallet(bot, code));
        }

        var results = await Utilities.InParallel(tasks).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }
}
