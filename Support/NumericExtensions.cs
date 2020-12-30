using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace System
{
    public static class NumericExtensions
    {
        public static IEnumerable<int> GetDivisors(this int number)
        {
            var sqrt = (int) Math.Sqrt(number) + 1;
            for (var i = 1; i < sqrt; i++)
            {
                if (number % i == 0)
                {
                    yield return i;

                    if (i * i != number)
                    {
                        yield return number / i;
                    }
                }
            }
        }

        public static bool IsBetween(this int number, int lowerBoundary, int upperBoundary)
        {
            return lowerBoundary < number && number < upperBoundary;
        }

        public static bool IsBetweenOrEqual(this int number, int lowerBoundary, int upperBoundary)
        {
            return lowerBoundary <= number && number <= upperBoundary;
        }
    }
}