using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace System
{
    public static class ParsingExtensions
    {
        public static TTarget Parse<TTarget>(this string source, Func<string, TTarget> parser) => parser(source);

        public static TTarget Parse<TSource, TTarget>(this TSource[] source, Func<TSource[], TTarget> parser)
            => parser(source);

        public static TTarget[] ParseArray<TSource, TTarget>(this IEnumerable<TSource> source, Func<TSource, TTarget> parser)
            => source.Select(parser).ToArray();


        public static int ToInt32(this string source) => source.Parse(int.Parse);

        public static long ToInt64(this string source) => source.Parse(long.Parse);


        public static bool IsMatch(this string source, [RegexPattern] string regex)
        {
            var match = Regex.Match(source, regex);

            return match.Success && match.Index == 0 && match.Length == source.Length;
        }

        public static bool HasMatch(this string source, [RegexPattern] string regex)
        {
            var match = Regex.Match(source, regex);

            return match.Success;
        }

        public static string[] Matches(this string source, [RegexPattern] string regex)
            => Regex.Matches(source, regex).Select(m => m.Value).ToArray();

        public static IDictionary<string, string> MatchesByName(this string source, [RegexPattern] string regex)
        {
            var matches = Regex.Matches(source, regex);

            return matches
               .SelectMany(m => m.Groups.OfType<Group>())
               .Where(g => g.Success)
               .ToDictionary(g => g.Name, g=> g.Value);
        }

        public static T[] ParseMatches<T>(this string source, [RegexPattern] string regex, Func<string, T> parser)
            => source.Matches(regex).Select(parser).ToArray();


        public static int[] Int32Matches(this string source) =>
            source.ParseMatches("[-+]?[0-9]+", int.Parse);

        public static long[] ToInt64Array(this string source) =>
            source.ParseMatches("[-+]?[0-9]+", long.Parse);


        public static IEnumerable<string> SplitByMatches(this string source, [RegexPattern] string regex)
            => Regex.Split(source, regex).ToArray();


        public static string[] Lines(this string source, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            using var reader = new StringReader(source);

            var items = new List<string>();
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if(options == StringSplitOptions.RemoveEmptyEntries && string.IsNullOrWhiteSpace(line)) continue;
                items.Add(line);
            }

            return items.ToArray();
        }

        public static T[] ParseLines<T>(this string source, Func<string, T> parser, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            using var reader = new StringReader(source);

            var items = new List<T>();
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (options == StringSplitOptions.RemoveEmptyEntries && string.IsNullOrWhiteSpace(line)) continue;
                items.Add(parser(line));
            }

            return items.ToArray();
        }

        public static T[] ParseLines<T>(this string source, Func<string, int, T> parser, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            using var reader = new StringReader(source);

            var items = new List<T>();
            var index = 0;
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (options == StringSplitOptions.RemoveEmptyEntries && string.IsNullOrWhiteSpace(line)) continue;
                items.Add(parser(line, index++));
            }

            return items.ToArray();
        }


        public static string[][] LineGroups(this string source, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            using var reader = new StringReader(source);

            var items = new List<string[]>();
            var lines = new List<string>();
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    if(lines.Count > 0 || options == StringSplitOptions.None)
                    {
                        items.Add(lines.ToArray());
                        lines = new List<string>();
                    }
                }
                else
                {
                    lines.Add(line);
                }
            }

            if (lines.Count > 0 || options == StringSplitOptions.None)
            {
                items.Add(lines.ToArray());
            }

            return items.ToArray();
        }

        public static T[] ParseLineGroups<T>(this string source, Func<string[], T> parser, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            using var reader = new StringReader(source);

            var items = new List<T>();
            var lines = new List<string>();
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    if(lines.Count > 0 || options == StringSplitOptions.None)
                    {
                        items.Add(parser(lines.ToArray()));
                        lines = new List<string>();
                    }
                }
                else
                {
                    lines.Add(line);
                }
            }

            if (lines.Count > 0 || options == StringSplitOptions.None)
            {
                items.Add(parser(lines.ToArray()));
            }

            return items.ToArray();
        }

        public static T[] ParseLineGroups<T>(this string source, Func<string[], int, T> parser, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            using var reader = new StringReader(source);

            var items = new List<T>();
            var lines = new List<string>();
            var index = 0;
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    if(lines.Count > 0 || options == StringSplitOptions.None)
                    {
                        items.Add(parser(lines.ToArray(), index++));
                        lines = new List<string>();
                    }
                }
                else
                {
                    lines.Add(line);
                }
            }

            if (lines.Count > 0 || options == StringSplitOptions.None)
            {
                items.Add(parser(lines.ToArray(), index));
            }

            return items.ToArray();
        }
    }
}