using ArchiSteamFarm.Steam;
using ASFEnhance.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASFEnhance._Adapter_;
public static class Core
{
    internal static Dictionary<string, SubModuleInfo> SubModules { get; } = new();

    internal static Task<string?>? OnBotCommand(Bot bot, EAccess access, string message, string[] args)
    {
        if (SubModules.Count == 0)
        {
            return null;
        }

        var cmd = args[0].ToUpperInvariant();

        foreach (var (name, subModule) in SubModules)
        {
            if (cmd.Contains('.'))
            {
                string prefix;
                if (!string.IsNullOrEmpty(subModule.CmdPrefix))
                {

                }
                else
                {

                }
            }

            var response = subModule.CommandHandler?.Invoke(null, null);
        }
        return Task.FromResult<string?>("");
    }
}
