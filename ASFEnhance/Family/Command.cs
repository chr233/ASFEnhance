using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using System.Text;

namespace ASFEnhance.Family;
internal static class Command
{
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
            return bot.FormatBotResponse("未加入家庭组");
        }
        else
        {
            var sb = new StringBuilder();
            sb.AppendLineFormat("名称: {0}", group.Name);
            sb.AppendLineFormat("Id: {0}", info.FamilyGroupId);
            if (group.Members != null)
            {
                sb.AppendLineFormat("成员 ({0}/6):", group.Members.Count);
                foreach (var member in group.Members)
                {
                    string? nickname = null;
                    if (ulong.TryParse(member.SteamId, out var steamId))
                    {
                        nickname = bot.SteamFriends.GetFriendPersonaName(steamId);
                    }

                    var role = member.Role switch
                    {
                        1 => "成人",
                        2 => "儿童",
                        _ => "未知"
                    };
                    sb.AppendLineFormat(" - {0} {1} ({2})", nickname, member.SteamId, role);
                }
            }

            return bot.FormatBotResponse(sb.ToString());
        }

    }

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
            return bot.FormatBotResponse("修改失败, 未加入家庭组");
        }

        if (!int.TryParse(id, out var groupId))
        {
            return bot.FormatBotResponse("修改失败, 家庭组 Id 无效");
        }

        await WebRequest.ModifyFamilyGroupDetails(bot, int.Parse(id), name).ConfigureAwait(false);

        return bot.FormatBotResponse("修改家庭组名称成功");
    }

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
