#pragma warning disable CS8600
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Helpers;
using ArchiSteamFarm.Plugins;
using ArchiSteamFarm.Steam.Integration;
using System.Collections.Immutable;
using System.Reflection;

namespace ASFEnhance;

internal static class ReflectionHelper
{
    internal static async Task AddService()
    {
        var filedInfo = typeof(ASF).GetProperty("WebLimitingSemaphores", BindingFlags.NonPublic | BindingFlags.Static);

        if (filedInfo != null)
        {
            var newValue = new Dictionary<Uri, (ICrossProcessSemaphore RateLimitingSemaphore, SemaphoreSlim OpenConnectionsSemaphore)>(5);
            var oldValue = (ImmutableDictionary<Uri, (ICrossProcessSemaphore RateLimitingSemaphore, SemaphoreSlim OpenConnectionsSemaphore)>)filedInfo.GetValue(null);

            if (oldValue != null)
            {
                foreach (var (k, v) in oldValue)
                {
                    newValue.Add(k, v);
                }
            }

            newValue.Add(SteamCheckoutURL, (await PluginsCore.GetCrossProcessSemaphore($"{nameof(ArchiWebHandler)}-{nameof(SteamCheckoutURL)}").ConfigureAwait(false), new SemaphoreSlim(5, 5)));

            filedInfo.SetValue(null, newValue.ToImmutableDictionary());
        }
        else
        {
            ASFLogger.LogGenericWarning("Fieldinfo is null");
        }
    }
}
