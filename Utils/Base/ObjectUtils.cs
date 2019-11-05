using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Utils.Base
{
    public static class ObjectUtils
    {
        public static object GetValueFromProperty(object obj, string propertyName)
        {
            return obj.GetType().GetProperties()
               .Single(pi => pi.Name == propertyName)
               .GetValue(obj, null);
        }

        public static Func<TObject, TKey> BuildKeySelector<TObject, TKey>(string propertyName)
        {
            return obj =>
            {
                var prop = typeof(TObject).GetProperty(propertyName, typeof(TKey));
                return (TKey)prop.GetValue(obj);
            };
        }

        public static bool HasProperty(object obj, string name)
        {
            return obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Any(p => p.Name == name);
        }

        public static object GetValueFromField(object obj, string fieldName)
        {
            return obj.GetType().GetField(fieldName).GetValue(obj);
        }

        public static bool IsSimple(Type type)
        {
            return type.IsPrimitive
              || type.Equals(typeof(string));
        }

        public static void TrySetProperty(object obj, string property, object value)
        {
            var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(obj, value, null);
            }
        }

        public static void TrySetField(object obj, string field, object value)
        {
            var f = obj.GetType().GetField(field, BindingFlags.Public | BindingFlags.Instance);
            if (f != null)
            {
                f.SetValue(obj, value);
            }
        }

        public static dynamic Merge(object item1, object item2)
        {
            IDictionary<string, object> result = new ExpandoObject();

            foreach (var property in item1.GetType().GetProperties())
            {
                if (property.CanRead)
                {
                    result[property.Name] = property.GetValue(item1);
                }
            }

            foreach (var property in item2.GetType().GetProperties())
            {
                if (property.CanRead)
                {
                    result[property.Name] = property.GetValue(item2);
                }
            }

            return result;
        }

        public static T Clone<T>(this T obj)
        {
            var inst = obj.GetType().GetMethod("MemberwiseClone", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            return (T)inst?.Invoke(obj, null);
        }

        public static Dictionary<TKey, TValue>
            MergeDictionaries<TKey, TValue>(this IEnumerable<Dictionary<TKey, TValue>> enumerable)
        {
            return enumerable.SelectMany(x => x).ToDictionary(x => x.Key, y => y.Value);
        }

        public static KeyValuePair<T, V> CastFrom<T, V>(object obj)
        {
            return (KeyValuePair<T, V>)obj;
        }
    }
}
