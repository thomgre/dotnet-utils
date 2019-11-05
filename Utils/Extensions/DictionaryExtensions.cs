using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils.Extensions
{
    public static class DictionaryExtensions
    {
        public static Dictionary<TKey, TValue> RemoveNullValues<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            return dictionary.Where(kv => kv.Value != null).ToDictionary(r => r.Key, r => r.Value);
        }

        public static bool TryGetTypedValue<TKey, TValue, TActual>(this IDictionary<TKey, TValue> data, TKey key, out TActual value) where TActual : TValue
        {
            TValue tmp;
            if (data.TryGetValue(key, out tmp))
            {
                value = (TActual)tmp;
                return true;
            }
            value = default(TActual);
            return false;
        }

        public static void RemoveAll<TKey, TValue>(this Dictionary<TKey, TValue> data, Func<KeyValuePair<TKey, TValue>, bool> condition)
        {
            foreach (var cur in data.Where(condition).ToList())
            {
                data.Remove(cur.Key);
            }
        }

        public static void RemoveAllByKey<TKey, TValue>(this IDictionary<TKey, TValue> data, List<TKey> keysToRemove)
        {
            foreach (var key in keysToRemove)
            {
                data.Remove(key);
            }
        }

        public static Dictionary<TKey, TValue> GetKeysByPrefix<TKey, TValue>(this IDictionary<TKey, TValue> data, string keyPrefix)
        {
            // Remove all keys not matching given prefix
            var keysToRemove = data.Keys.Where(k => !k.ToString().StartsWith(keyPrefix)).ToList();

            foreach (var key in keysToRemove)
            {
                data.Remove(key);
            }

            return data.ToDictionary(r => r.Key, r => r.Value);
        }
    }
}
