//Code is not mine, is Suceru's https://github.com/Suceru/Survivalcraft2CreatorWandMod

using System.Collections.Generic;
using System.Reflection;

namespace CreatorWandModAPI
{
    public static class ProbeFunctions
    {
        public static T GetPrivateField<T>(this object instance, string fieldname)
        {
            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic;
            return (T)instance.GetType().GetField(fieldname, bindingAttr).GetValue(instance);
        }

        public static T GetPrivateProperty<T>(this object instance, string propertyname)
        {
            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic;
            return (T)instance.GetType().GetProperty(propertyname, bindingAttr).GetValue(instance, null);
        }

        public static void SetPrivateField(this object instance, string fieldname, object value)
        {
            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic;
            instance.GetType().GetField(fieldname, bindingAttr).SetValue(instance, value);
        }

        public static void SetPrivateProperty(this object instance, string propertyname, object value)
        {
            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic;
            instance.GetType().GetProperty(propertyname, bindingAttr).SetValue(instance, value, null);
        }

        public static T CallPrivateMethod<T>(this object instance, string name, params object[] param)
        {
            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic;
            return (T)instance.GetType().GetMethod(name, bindingAttr).Invoke(instance, param);
        }

        public static List<string> GetProperties<T>(T t)
        {
            List<string> list = new List<string>();
            if (t == null)
            {
                return list;
            }

            PropertyInfo[] properties = t.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties.Length == 0)
            {
                return list;
            }

            PropertyInfo[] array = properties;
            foreach (PropertyInfo propertyInfo in array)
            {
                string name = propertyInfo.Name;
                object value = propertyInfo.GetValue(t, null);
                if (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType.Name.StartsWith("String"))
                {
                    list.Add(name);
                }
                else
                {
                    GetProperties(value);
                }
            }

            return list;
        }

        public static List<string> GetFields<T>(T t)
        {
            List<string> list = new List<string>();
            if (t == null)
            {
                return list;
            }

            FieldInfo[] fields = t.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (fields.Length == 0)
            {
                return list;
            }

            FieldInfo[] array = fields;
            foreach (FieldInfo fieldInfo in array)
            {
                string name = fieldInfo.Name;
                object value = fieldInfo.GetValue(t);
                if (fieldInfo.FieldType.IsValueType || fieldInfo.FieldType.Name.StartsWith("String"))
                {
                    list.Add(name);
                }
                else
                {
                    GetFields(value);
                }
            }

            return list;
        }
    }
}
