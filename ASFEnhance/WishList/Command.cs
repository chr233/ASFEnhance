using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using System.Text;


namespace ASFEnhance.Wishlist;

internal static class Command
{
    /// <summary>
    /// 添加愿望单
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="targetGameIds"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseAddWishlist(Bot bot, string targetGameIds)
    {
        if (string.IsNullOrEmpty(targetGameIds))
        {
            throw new ArgumentNullException(nameof(targetGameIds));
        }

        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        StringBuilder response = new();

        string[] games = targetGameIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string game in games)
        {
            if (!uint.TryParse(game, out uint gameId) || (gameId == 0))
            {
                response.AppendLine(bot.FormatBotResponse(string.Format(Strings.ErrorIsInvalid, nameof(gameId))));
                continue;
            }

            bool result = await bot.AddWishlist(gameId).ConfigureAwait(false);

            response.AppendLine(bot.FormatBotResponse(string.Format(Strings.BotAddLicense, gameId, result ? Langs.Success : Langs.Failure)));
        }

        return response.Length > 0 ? response.ToString() : null;
    }

    /// <summary>
    /// 添加愿望单 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="targetGameIds"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseAddWishlist(string botNames, string targetGameIds)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        if (string.IsNullOrEmpty(targetGameIds))
        {
            throw new ArgumentNullException(nameof(targetGameIds));
        }

        HashSet<Bot>? bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
        }

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseAddWishlist(bot, targetGameIds))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 删除愿望单
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="targetGameIds"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseRemoveWishlist(Bot bot, string targetGameIds)
    {
        if (string.IsNullOrEmpty(targetGameIds))
        {
            throw new ArgumentNullException(nameof(targetGameIds));
        }

        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        StringBuilder response = new();

        string[] games = targetGameIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string game in games)
        {
            if (!uint.TryParse(game, out uint gameId) || (gameId == 0))
            {
                response.AppendLine(bot.FormatBotResponse(string.Format(Strings.ErrorIsInvalid, nameof(gameId))));
                continue;
            }

            bool result = await bot.RemoveWishlist(gameId).ConfigureAwait(false);

            response.AppendLine(bot.FormatBotResponse(string.Format(Strings.BotAddLicense, gameId, result ? Langs.Success : Langs.Failure)));
        }

        return response.Length > 0 ? response.ToString() : null;
    }

    /// <summary>
    /// 删除愿望单 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="targetGameIds"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseRemoveWishlist(string botNames, string targetGameIds)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        if (string.IsNullOrEmpty(targetGameIds))
        {
            throw new ArgumentNullException(nameof(targetGameIds));
        }

        HashSet<Bot>? bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
        }

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseRemoveWishlist(bot, targetGameIds))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 关注游戏
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="targetGameIds"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseFollowGame(Bot bot, string targetGameIds, bool isFollow)
    {
        if (string.IsNullOrEmpty(targetGameIds))
        {
            throw new ArgumentNullException(nameof(targetGameIds));
        }

        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        StringBuilder response = new();

        string[] games = targetGameIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string game in games)
        {
            if (!uint.TryParse(game, out uint gameId) || (gameId == 0))
            {
                response.AppendLine(bot.FormatBotResponse(string.Format(Strings.ErrorIsInvalid, nameof(gameId))));
                continue;
            }

            bool result = await bot.FollowGame(gameId, isFollow).ConfigureAwait(false);

            response.AppendLine(bot.FormatBotResponse(string.Format(Strings.BotAddLicense, gameId, result ? Langs.Success : Langs.Failure)));
        }

        return response.Length > 0 ? response.ToString() : null;
    }

    /// <summary>
    /// 关注游戏 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="targetGameIds"></param>
    /// <param name="isFollow"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseFollowGame(string botNames, string targetGameIds, bool isFollow)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        if (string.IsNullOrEmpty(targetGameIds))
        {
            throw new ArgumentNullException(nameof(targetGameIds));
        }

        HashSet<Bot>? bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
        }

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseFollowGame(bot, targetGameIds, isFollow))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }


    /// <summary>
    /// 检查游戏拥有/愿望单/关注
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="targetGameIds"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseCheckGame(Bot bot, string targetGameIds)
    {
        if (string.IsNullOrEmpty(targetGameIds))
        {
            throw new ArgumentNullException(nameof(targetGameIds));
        }

        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var sb = new StringBuilder();
        sb.AppendLine(bot.FormatBotResponse(Langs.MultipleLineResult));
        sb.AppendLine(Langs.CheckGameListTitle);

        string[] games = targetGameIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string game in games)
        {
            if (!uint.TryParse(game, out uint gameId) || (gameId == 0))
            {
                sb.AppendLine(string.Format(Langs.CheckGameItemError, game));
                continue;
            }

            var result = await bot.CheckGame(gameId).ConfigureAwait(false);

            if (result != null && result.Success)
            {
                sb.AppendLine(string.Format(Langs.CheckGameItemSuccess, gameId, result.Name, result.Owned ? "√" : "×", result.InWishlist ? "√" : "×", result.IsFollow ? "√" : "×"));
            }
            else
            {
                sb.AppendLine(string.Format(Langs.CheckGameItemFailed, gameId, result?.Name ?? gameId.ToString()));
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// 检查游戏拥有/愿望单/关注 （多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="targetGameIds"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseCheckGame(string botNames, string targetGameIds)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        if (string.IsNullOrEmpty(targetGameIds))
        {
            throw new ArgumentNullException(nameof(targetGameIds));
        }

        HashSet<Bot>? bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
        }

        IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseCheckGame(bot, targetGameIds))).ConfigureAwait(false);

        List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

}
