#pragma warning disable CS8600 
#pragma warning disable CS8602 
#pragma warning disable CS8603 

using System.Reflection;

namespace ASFEnhance.Explorer
{
    internal static class ReflectionHelper
    {
        //1、得到私有字段的值：
        internal static T GetPrivateField<T>(this object instance, string fieldname)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            FieldInfo field = type.GetField(fieldname, flag);
            return (T)field.GetValue(instance);
        }

        internal static object GetPrivateField(this object instance, string fieldname, Type type)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type typeInfo = instance.GetType();
            FieldInfo field = typeInfo.GetField(fieldname, flag);

            return Convert.ChangeType(field.GetValue(instance), type);
        }
    }
}
