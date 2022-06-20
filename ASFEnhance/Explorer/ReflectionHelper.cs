using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

        //2、得到私有属性的值：
        internal static T GetPrivateProperty<T>(this object instance, string propertyname)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            PropertyInfo field = type.GetProperty(propertyname, flag);
            return (T)field.GetValue(instance, null);
        }

        //3、设置私有成员的值：
        internal static void SetPrivateField(this object instance, string fieldname, object value)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            FieldInfo field = type.GetField(fieldname, flag);
            field.SetValue(instance, value);
        }
        //4、设置私有属性的值： 
        internal static void SetPrivateProperty(this object instance, string propertyname, object value)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            PropertyInfo field = type.GetProperty(propertyname, flag);
            field.SetValue(instance, value, null);
        }

        //5、调用私有方法：
        internal static void CallPrivateMethod(this object instance, string name, params object[] param)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            MethodInfo method = type.GetMethod(name, flag);
            method.Invoke(instance, param);
        }

        internal static T CallPrivateMethod<T>(this object instance, string name, params object[] param)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            MethodInfo method = type.GetMethod(name, flag);
            return (T)method.Invoke(instance, param);
        }

        internal static async Task CallPrivateMethodAsync(this object instance, string name, params object[] param)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            MethodInfo method = type.GetMethod(name, flag);
            Task task = method.Invoke(instance, param) as Task;
            await task.ConfigureAwait(false);
        }

        internal static async Task<T> CallPrivateMethodAsync<T>(this object instance, string name, params object[] param)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            MethodInfo method = type.GetMethod(name, flag);
            Task task = method.Invoke(instance, param) as Task;
            await task.ConfigureAwait(false);
            return (T)task.GetType().GetProperty("Result").GetValue(task, null);
        }
    }
}
