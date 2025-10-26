using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using System.Text;

namespace ASFEnhance.Family;
internal static class Command
{
    /// <summary>
    /// 获取家庭组信息
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseFamilyGroup(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var info = await WebRequest.GetFamilyGroupForUser(bot).ConfigureAwait(false);

        var group = info?.FamilyGroup;
        if (info == null || group == null)
        {
            return bot.FormatBotResponse(Langs.FamilyGroupNotJoined);
        }
        else
        {
            var sb = new StringBuilder();
            sb.AppendLineFormat(Langs.FamilyGroupNameItem, group.Name);
            sb.AppendLineFormat(Langs.FamilyGroupIdItem, info.FamilyGroupId);
            if (group.Members != null)
            {
                sb.AppendLineFormat(Langs.FamilyGroupMemberItem, group.Members.Count);
                foreach (var member in group.Members)
                {
                    string? nickname = null;
                    if (ulong.TryParse(member.SteamId, out var steamId))
                    {
                        nickname = bot.SteamFriends.GetFriendPersonaName(steamId);
                    }

                    var role = member.Role switch
                    {
                        1 => Langs.Adult,
                        2 => Langs.Children,
                        _ => Langs.Unknown
                    };
                    sb.AppendLineFormat(" - {0} {1} ({2})", nickname, member.SteamId, role);
                }
            }

            return bot.FormatBotResponse(sb.ToString());
        }

    }

    /// <summary>
    /// 获取家庭组信息 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseFamilyGroup(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(ResponseFamilyGroup)).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }

    /// <summary>
    /// 修改家庭组名称
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseFamilyGroupName(Bot bot, string name)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var info = await WebRequest.GetFamilyGroupForUser(bot).ConfigureAwait(false);

        var id = info?.FamilyGroupId;
        if (string.IsNullOrEmpty(id))
        {
            return bot.FormatBotResponse(Langs.FamilyGroupNameEditFailedNotJoined);
        }

        if (!int.TryParse(id, out var groupId))
        {
            return bot.FormatBotResponse(Langs.FamilyGroupNameEditFailedIdInvalid);
        }

        await WebRequest.ModifyFamilyGroupDetails(bot, groupId, name).ConfigureAwait(false);

        return bot.FormatBotResponse(Langs.FamilyGroupNameEditSuccess);
    }

    /// <summary>
    /// 修改家庭组名称 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseFamilyGroupName(string botNames, string name)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if (bots == null || bots.Count == 0)
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(x => ResponseFamilyGroupName(x, name))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }
}
