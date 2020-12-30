using System.Diagnostics.CodeAnalysis;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
    public static class CollectionExtensions
    {
        public static void Add<TKey, TValue>(this IDictionary<TKey, TValue> source, IDictionary<TKey, TValue> other)
        {
            foreach (var (key, value) in other)
            {
                source.Add(key, value);
            }
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue newValue)
        {
            if (dictionary.TryGetValue(key, out var current))
            {
                return current;
            }

            dictionary[key] = newValue;
            return newValue;
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> factory) where TKey : notnull
        {
            if (dictionary.TryGetValue(key, out var current))
            {
                return current;
            }

            var newValue = factory();
            dictionary[key] = newValue;
            return newValue;
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> factory) where TKey : notnull
        {
            if (dictionary.TryGetValue(key, out var current))
            {
                return current;
            }

            var newValue = factory(key);
            dictionary[key] = newValue;
            return newValue;
        }

        public static TValue? GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TKey : notnull
        {
            dictionary.TryGetValue(key, out var current);
            return current;
        }

        public static TValue? GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue @default) where TKey : notnull
        {
            if(dictionary.TryGetValue(key, out var current))
                return current;

            return @default;
        }

        public static TResult? GetOrDefault<TKey, TValue, TResult>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue, TResult> selector) where TKey : notnull
        {
            if(dictionary.TryGetValue(key, out var current))
                return selector(current);

            return default;
        }

        public static TResult? GetOrDefault<TKey, TValue, TResult>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue @default, Func<TValue, TResult> selector) where TKey : notnull
        {
            if(dictionary.TryGetValue(key, out var current))
                return selector(current);

            return selector(@default);
        }

        public static bool TryGetValue<TKey, TValue, TResult>(this IDictionary<TKey, TValue> source,  TKey key, Func<TValue, TResult> selector, [MaybeNullWhen(false)] out TResult value) where TKey : notnull
        {
            if(!source.TryGetValue(key, out var val))
            {
                value = default;
                return false;
            }
            else
            {
                value = selector(val);
                return true;
            }
        }

        public static bool TryFirst<TKey, TValue>(this Dictionary<TKey, TValue> source, Func<TKey, TValue, bool> predicate, out KeyValuePair<TKey, TValue> first) where TKey: notnull
        {
            var items = source.Where(kvp => predicate(kvp.Key, kvp.Value)).Take(1).ToArray();

            if (items.Length == 0)
            {
                first = default;
                return false;
            }
            else
            {
                first = items[0];
                return true;
            }
        }

        public static void Swap<T>(this T[] array, int i1, int i2)
        {
            T temp = array[i1];
            array[i1] = array[i2];
            array[i2] = temp;
        }

        public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                source.Add(item);
            }
        }
    }
}