using System.IO;
using System.Threading.Tasks;
using AoC.Infrastructure.Puzzles;
using LiteDB;

namespace AoC.Infrastructure.Services
{
    internal static partial class AdventOfCode
    {
        public static async Task<string> GetPuzzleInput(PuzzleRegistration registration)
        {
            using var db = GetDatabase();

            var collection = db.GetCollection<Input>("inputs");

            var input = collection.FindOne(i => i.Year == registration.Year && i.Day == registration.Day);

            input ??= new Input
            {
                Year = registration.Year,
                Day = registration.Day,
                Value = $"|{(await DownloadPuzzleInput(registration)).TrimEnd()}", // LiteDB at the start by default! We just want to trim the end
            };

            collection.Upsert(input);

            //We need to remove trimming protection
            return input.Value?.Substring(1) ?? string.Empty;
        }

        public static Task<int> CleanCache()
        {
            return Task.Run(() =>
            {
                using var db = GetDatabase();

                var collection = db.GetCollection<Input>("inputs");

                return collection.DeleteAll();
            });
        }

        private static LiteDatabase GetDatabase() => new(Path.Combine(Path.GetTempPath(), "aoc.db"));

        private class Input
        {
            public int Id { get; init; }
            public int Year { get; init; }
            public int Day { get; init; }
            public string? Value { get; init; }
        }
    }
}