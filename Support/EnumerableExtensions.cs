using System.Linq;

// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IReadOnlyList<T>> Permutations<T>(this IEnumerable<T> source)
        {
            var elements = source.ToArray();

            yield return elements.ToArray();

            var indexes = new int[elements.Length];

            var i = 0;
            while (i < elements.Length)
            {
                if (indexes[i] < i)
                {
                    if ((i % 2) == 0)
                    {
                        var tmp = elements[0];
                        elements[0] = elements[i];
                        elements[i] = tmp;
                    }
                    else
                    {
                        var tmp = elements[indexes[i]];
                        elements[indexes[i]] = elements[i];
                        elements[i] = tmp;
                    }
                    yield return elements.ToArray();
                    indexes[i] += 1;
                    i = 0;
                }
                else
                {
                    indexes[i] = 0;
                    i += 1;
                }
            }
        }

        public static IEnumerable<IReadOnlyCollection<T>> Combinations<T>(this IEnumerable<T> source, int minK, int maxK)
        {
            var cachedSource = source.ToList();

            for (; minK <= maxK; minK++)
            {
                foreach (var combination in cachedSource.Combinations(minK))
                {
                    yield return combination;
                }
            }
        }

        public static IEnumerable<IReadOnlyCollection<T>> Combinations<T>(this IEnumerable<T> source, int k)
        {
            if (k <= 0)
            {
                yield break;
            }

            var elements = source.ToArray();

            if (k > elements.Length)
            {
                yield break;
            }

            var indexes = new int[k];

            for (var i = 0; i < k; i++)
            {
                indexes[i] = i;
            }

            var combination = new T[k];

            do
            {
                for (var i = 0; i < k; i++)
                {
                    combination[i] = elements[indexes[i]];
                }
                yield return combination.ToArray();
            } while (NextCombination(indexes, k, elements.Length));
        }

        private static bool NextCombination(int[] indexes, int k, int n)
        {
            var finished = false;
            var changed = false;

            for (var i = k - 1; !finished && !changed; i--)
            {
                if (indexes[i] < n - 1 - (k - 1) + i)
                {
                    indexes[i]++;

                    if (i < k - 1)
                    {
                        for (var j = i + 1; j < k; j++)
                        {
                            indexes[j] = indexes[j - 1] + 1;
                        }
                    }
                    changed = true;
                }
                finished = i == 0;
            }

            return changed;
        }

        public static IEnumerable<T> Sorted<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => x);
        }

        public static IEnumerable<T> SortedDescending<T>(this IEnumerable<T> source)
        {
            return source.OrderByDescending(x => x);
        }

        public static bool None<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return !source.Any(predicate);
        }

        public static int Product(this IEnumerable<int> source)
        {
            var product = 1;
            checked
            {
                foreach (var item in source)
                {
                    product *= item;
                }
            }

            return product;
        }

        public static long Product(this IEnumerable<long> source)
        {
            var product = 1L;
            checked
            {
                foreach (var item in source)
                {
                    product *= item;
                }
            }

            return product;
        }
    }
}