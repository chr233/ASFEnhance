using System.Reflection;

namespace ASFEnhance.Explorer;

internal static class ReflectionHelper
{
    internal static T? GetStaticPrivateProperty<T>(this Type type, string fieldname) where T : notnull
    {
        var flag = BindingFlags.Static | BindingFlags.NonPublic;
        var field = type.GetProperty(fieldname, flag);
        return (T?)field?.GetValue(null);
    }
}
