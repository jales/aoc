using AoC.Support;

// ReSharper disable once CheckNamespace
namespace System
{
    public static class StringExtensions
    {
        public static SparseGrid<T> ToSparseGrid<T>(this string source, Func<int, int, char, (bool exists, T value)> cellParser) where T: notnull
        {
            return SparseGrid.Parse(source, cellParser);
        }

        public static SparseGrid<T> ToSparseGrid<T>(this string source, Func<char, (bool exists, T value)> cellParser) where T: notnull
        {
            return SparseGrid.Parse(source, cellParser);
        }
    }
}