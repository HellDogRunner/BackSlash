using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedMoonGames.Basics
{
    public static class ListExtensions
    {
        public static T GetRandom<T>(this List<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        public static T GetBy<T>(this List<T> list, Func<T, bool> condition, T defaultValue = default)
        {
            foreach (var item in list)
            {
                if (condition(item))
                    return item;
            }

            return defaultValue;
        }

        public static List<T> GetListBy<T>(this List<T> list, Func<T, bool> condition, T defaultValue = default)
        {
            var result = new List<T>();

            foreach (var item in list)
            {
                if (condition(item)) result.Add(item);
            }

            return result;
        }

        public static bool TryWeightRandom<T>(this List<T> list, Func<T, float> getWeight, out T result)
        {
            if (list.Count == 0) throw new Exception("The number of items in the list is zero");

            var res = false;
            result = list[0];

            var totalWeight = 0f;
            foreach (var item in list)
                totalWeight += getWeight(item);

            var random = UnityEngine.Random.Range(0f, totalWeight);
            var accumulatedValue = 0f;

            foreach (var item in list)
            {
                var weight = getWeight(item);
                accumulatedValue += weight;

                if (accumulatedValue >= random)
                {
                    result = item;
                    res = true;
                    break;
                }
            }

            return res;
        }

        public static IEnumerable<TSource> IntersectBy<TSource, TKey>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer)
        {
            if (first == null ||
                second == null ||
                keySelector == null ||
                comparer == null)
            {
                Debug.LogError("ArgumentNullException");
                return null;
            }

            var result = new List<TSource>();

            foreach (var f in first)
            {
                foreach (var s in second)
                {
                    if (comparer.Equals(keySelector(f), keySelector(s)))
                    {
                        result.Add(f);
                    }
                }
            }

            return result;
        }

        public static IEnumerable<TSource> IntersectBy<TSource, TKey>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            Func<TSource, TKey> keySelector,
            Func<TKey, TKey, bool> comparer)
        {
            if (first == null ||
                second == null ||
                keySelector == null ||
                comparer == null)
            {
                Debug.LogError("ArgumentNullException");
                return null;
            }

            var result = new List<TSource>();

            foreach (var f in first)
            {
                foreach (var s in second)
                {
                    if (comparer(keySelector(f), keySelector(s)))
                    {
                        result.Add(f);
                    }
                }
            }

            return result;
        }

        public static IEnumerable<TFirst> IntersectBy<TFirst, TSecond, TKey>(
            this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TKey> firstKeySelector,
            Func<TSecond, TKey> secondKeySelector,
            Func<TKey, TKey, bool> comparer)
        {
            if (first == null ||
                second == null ||
                firstKeySelector == null ||
                comparer == null)
            {
                Debug.LogError("ArgumentNullException");
                return null;
            }

            var result = new List<TFirst>();

            foreach (var f in first)
            {
                foreach (var s in second)
                {
                    if (comparer(firstKeySelector(f), secondKeySelector(s)))
                    {
                        result.Add(f);
                    }
                }
            }

            return result;
        }
    }
}
