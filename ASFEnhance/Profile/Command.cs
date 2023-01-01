#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using AngleSharp.Common;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Localization;
using System.Text.RegularExpressions;
using static ASFEnhance.Utils;


namespace ASFEnhance.Profile
{
    internal static class Command
    {
        /// <summary>
        /// 获取个人资料摘要
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseGetProfileSummary(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            string result = await WebRequest.GetSteamProfile(bot).ConfigureAwait(false) ?? Langs.GetProfileFailed;

            return bot.FormatBotResponse(result);
        }

        /// <summary>
        /// 获取个人资料摘要 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseGetProfileSummary(string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseGetProfileSummary(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 获取Steam64Id
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static string? ResponseGetSteamId(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            return bot.FormatBotResponse(bot.SteamID.ToString());
        }

        /// <summary>
        /// 获取Steam64Id (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseGetSteamId(string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => Task.Run(() => ResponseGetSteamId(bot)))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }


        /// <summary>
        /// 获取个人资料链接
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static string? ResponseGetProfileLink(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            Uri profileLink = new(SteamCommunityURL + $"profiles/{bot.SteamID}");

            return bot.FormatBotResponse(profileLink.ToString());
        }

        /// <summary>
        /// 获取个人资料链接
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseGetProfileLink(string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => Task.Run(() => ResponseGetProfileLink(bot)))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 获取好友代码
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static string? ResponseGetFriendCode(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            ulong friendCode = SteamId2Steam32(bot.SteamID);

            return bot.FormatBotResponse(friendCode.ToString());
        }

        /// <summary>
        /// 获取好友代码 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseGetFriendCode(string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => Task.Run(() => ResponseGetFriendCode(bot)))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 获取交易链接
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseGetTradeLink(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            string tradeLink = await WebRequest.GetTradeofferPrivacyPage(bot).ConfigureAwait(false) ?? Langs.NetworkError;

            return bot.FormatBotResponse(tradeLink);
        }

        /// <summary>
        /// 获取交易链接 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseGetTradeLink(string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseGetTradeLink(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 清除昵称历史
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseClearAliasHistory(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            await WebRequest.ClearAliasHisrory(bot).ConfigureAwait(false);

            return bot.FormatBotResponse(Langs.Done);
        }

        /// <summary>
        /// 清除昵称历史 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseClearAliasHistory(string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseClearAliasHistory(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 获取年度总结
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseGetReplay(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            string? token = await WebRequest.GetReplayToken(bot).ConfigureAwait(false);

            if (string.IsNullOrEmpty(token))
            {
                return bot.FormatBotResponse(Langs.NetworkError);
            }

            string? result = await WebRequest.GetReplayPic(bot, 2022, token).ConfigureAwait(false);

            if (string.IsNullOrEmpty(result))
            {
                return bot.FormatBotResponse(Langs.NetworkError);
            }

            return bot.FormatBotResponse(result);
        }

        /// <summary>
        /// 获取年度总结 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseGetReplay(string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseGetReplay(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }


        /// <summary>
        /// 设置年度总结可见性
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseSetReplayPrivacy(Bot bot, string query)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (!int.TryParse(query, out int privacy))
            {
                return bot.FormatBotResponse(Langs.ReplayPrivacyError);
            }

            string? token = await WebRequest.GetReplayToken(bot).ConfigureAwait(false);

            if (string.IsNullOrEmpty(token))
            {
                return bot.FormatBotResponse(Langs.NetworkError);
            }

            string? result = await WebRequest.SetReplayPermission(bot, 2022, token, privacy).ConfigureAwait(false);

            if (string.IsNullOrEmpty(result))
            {
                return bot.FormatBotResponse(Langs.NetworkError);
            }

            return bot.FormatBotResponse(result);
        }

        /// <summary>
        /// 设置年度总结可见性 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseSetReplayPrivacy(string botNames, string query)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseSetReplayPrivacy(bot, query))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 设置个人资料游戏头像
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="gameId"></param>
        /// <param name="avatarId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseSetProfileGameAvatar(Bot bot, string gameId, string avatarId)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            if (string.IsNullOrEmpty(gameId))
            {
                throw new ArgumentNullException(nameof(gameId));
            }

            if (string.IsNullOrEmpty(avatarId))
            {
                throw new ArgumentNullException(nameof(avatarId));
            }

            if (!int.TryParse(gameId, out int iGameId))
            {
                return bot.FormatBotResponse(Langs.GameAvatarInvalidGameId);
            }

            if (!int.TryParse(avatarId, out int iAvatarId))
            {
                return bot.FormatBotResponse(Langs.GameAvatarInvalidAvatarId);
            }

            string? result = await WebRequest.SetProfileGameAvatar(bot, iGameId, iAvatarId).ConfigureAwait(false);

            return bot.FormatBotResponse(string.IsNullOrEmpty(result) ? Langs.NetworkError : result);
        }

        /// <summary>
        /// 设置个人资料游戏头像 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="gameId"></param>
        /// <param name="avatarId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseSetProfileGameAvatar(string botNames, string gameId, string avatarId)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseSetProfileGameAvatar(bot, gameId, avatarId))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 设置随机配置文件游戏头像 (多个Bot)
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseSetProfileRandomGameAvatar(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            Random rd = new();

            Dictionary<string, List<string>>? avatars = await WebRequest.GetGameAvatars(bot).ConfigureAwait(false);

            if (avatars == null)
            {
                return bot.FormatBotResponse(Langs.NetworkError);
            }

            int keyGameId = rd.Next(avatars.Keys.Count);
            string gameId;

            try
            {
                gameId = avatars.Keys.GetItemByIndex(keyGameId);
            }
            catch (ArgumentOutOfRangeException)
            {
                return bot.FormatBotResponse(Langs.NetworkError);
            }
            string? avatarId = avatars[gameId]?[rd.Next(avatars[gameId]!.Count)];

            if (avatarId == null)
            {
                return bot.FormatBotResponse(Langs.NetworkError);
            }

            if (!int.TryParse(gameId, out int iGameId))
            {
                return bot.FormatBotResponse(Langs.GameAvatarInvalidGameId);
            }

            if (!int.TryParse(avatarId, out int iAvatarId))
            {
                return bot.FormatBotResponse(Langs.GameAvatarInvalidAvatarId);
            }

            string? result = await WebRequest.SetProfileGameAvatar(bot, iGameId, iAvatarId).ConfigureAwait(false);

            return bot.FormatBotResponse(string.IsNullOrEmpty(result) ? Langs.NetworkError : result);
        }

        /// <summary>
        /// 设置随机配置文件游戏头像 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseSetProfileRandomGameAvatar(string botNames)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(ResponseSetProfileRandomGameAvatar)).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }

        /// <summary>
        /// 高级重命名命令 (多个Bot)
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="nickname"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseRename(Bot bot, string nickname)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            Random rd = new();
            // Logic borrowed from old plugin https://github.com/Zignixx/ASF-RenamePlugin/blob/master/RenamePlugin.cs#L28
            Regex regexRandom = new Regex(@"%RANDOM(\d+)%");
            Match match = regexRandom.Match(nickname);
            if (match.Success)
            {
                double maxRangeUserInput = double.Parse(match.Groups[1].Value);
                if (maxRangeUserInput > 9)
                {
                    return bot.FormatBotResponse(Langs.RenameTooBigRandomNumber);
                }
                int randomNumber = rd.Next(0, Convert.ToInt32(Math.Pow(10, maxRangeUserInput) - 1));
                nickname = Regex.Replace(nickname, regexRandom.ToString(), randomNumber.ToString($"D{maxRangeUserInput}"));
            }
            if (new Regex("%BOTNAME%").Match(nickname).Success)
            {
                nickname = Regex.Replace(nickname, @"%BOTNAME%", bot.BotName);
            }
            string? result = await bot.Commands.Response(EAccess.Owner, $"nickname {bot.BotName} {nickname}").ConfigureAwait(false);
            return result ?? bot.FormatBotResponse(Langs.NetworkError);
        }

        /// <summary>
        /// 高级重命名命令
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="nickname"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseRename(string botNames, string nickname)
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

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseRename(bot, nickname))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }
    }
}
