using System;
using System.Collections.Generic;
using System.Linq;

namespace RedMoonGames.Basics
{
    public static class DictionaryExtensions
    {
        public static TValue Get<TKey, TValue>(
            this IReadOnlyDictionary<TKey, TValue> dictionary,
            Func<KeyValuePair<TKey, TValue>, bool> check,
            TValue def
        )
        {
            foreach (var pair in dictionary)
            {
                if (check(pair))
                {
                    return pair.Value;
                }
            }
            return def;
        }

        public static bool Get<TKey, TValue>(
            this IReadOnlyDictionary<TKey, TValue> dictionary,
            Func<KeyValuePair<TKey, TValue>, bool> check,
            out KeyValuePair<TKey, TValue> returnValue)
        {
            returnValue = new KeyValuePair<TKey, TValue>();
            foreach (var pair in dictionary)
            {
                if (check(pair))
                {
                    returnValue = pair;
                    return true;
                }
            }

            return false;
        }

        public static bool Check<TKey, TValue>(
            this IReadOnlyDictionary<TKey, TValue> dictionary,
            Func<KeyValuePair<TKey, TValue>, bool> check
        )
        {
            var res = false;
            foreach (var pair in dictionary)
            {
                if (check(pair))
                {
                    res = true;
                    break;
                }
            }
            return res;
        }

        public static string ToDebugString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return "{" + string.Join(",", dictionary.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";
        }

        public static TKey SearchKeyByValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value)
        {
            if (!dictionary.TrySearchKeyByValue(value, out var key))
            {
                return default(TKey);
            }

            return key;
        }

        public static bool TrySearchKeyByValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value, out TKey key)
        {
            foreach (var item in dictionary)
            {
                if (item.Value.Equals(value))
                {
                    key = item.Key;
                    return true;
                }
            }

            key = default(TKey);
            return false;

        }

        public static TReturn Check<TKey, TValue, TReturn>(
            this IReadOnlyDictionary<TKey, TValue> dictionary,
            Func<TReturn, KeyValuePair<TKey, TValue>, TReturn> check,
            TReturn def
        )
        {
            TReturn res = def;
            foreach (var pair in dictionary)
            {
                res = check(res, pair);
            }
            return res;
        }

        public static void Set<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
                return;
            }

            dictionary.Add(key, value);
        }
    }
}
