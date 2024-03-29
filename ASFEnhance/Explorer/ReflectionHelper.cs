using System.Reflection;

namespace ASFEnhance.Explorer;

internal static class ReflectionHelper
{
    //1、得到私有字段的值：
    internal static T? GetPrivateField<T>(this object instance, string fieldname) where T : notnull
    {
        var flag = BindingFlags.Instance | BindingFlags.NonPublic;
        var type = instance.GetType();
        var field = type.GetField(fieldname, flag);
        return (T?)field?.GetValue(instance);
    }

    internal static object? GetPrivateField(this object instance, string fieldname, Type type)
    {
        var flag = BindingFlags.Instance | BindingFlags.NonPublic;
        var typeInfo = instance.GetType();
        var field = typeInfo.GetField(fieldname, flag);

        return Convert.ChangeType(field?.GetValue(instance), type, null);
    }

    internal static T? GetStaticPrivateProperty<T>(this Type type, string fieldname) where T : notnull
    {
        var flag = BindingFlags.Static | BindingFlags.NonPublic;
        var field = type.GetProperty(fieldname, flag);
        return (T?)field?.GetValue(null);
    }
}
