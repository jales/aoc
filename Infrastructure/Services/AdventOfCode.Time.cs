using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Infrastructure.Services
{
    internal static partial class AdventOfCode
    {
        public static readonly TimeZoneInfo TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

        public static int FirstYear { get; } = 2015;
        public static int CurrentYear { get; } = GetCurrentYear();

        private static int GetCurrentYear()
        {
            var utcNow = DateTimeOffset.UtcNow;
            var estNow = TimeZoneInfo.ConvertTime(utcNow, TimeZone);

            return estNow.Month == 12 ? estNow.Year : estNow.Year - 1;
        }

        public static IEnumerable<(int day, bool isActive)> GetDaysForYear(int year)
        {
            var today = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, TimeZone).Date;

            foreach (var day in Enumerable.Range(1, 25))
                yield return (day, isActive: today >= new DateTime(year, 12, day));
        }

        public static IEnumerable<int> GetYears()
        {
            for (var year = FirstYear; year <= CurrentYear; year++)
            {
                yield return year;
            }
        }

        public static IEnumerable<int> GetDays()
        {
            for (var day = 1; day <= 25; day++)
            {
                yield return day;
            }
        }

        public static DateTimeOffset StartOfDay(int year, int day)
        {
            return new(year, 12, day, 0, 0, 0, TimeZone.BaseUtcOffset);
        }
    }
}
