using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Infrastructure.Puzzles
{
    public static partial class PuzzleRegistrar
    {
        private static Dictionary<(int year, int day), PuzzleRegistration> Registrations { get; } = new();
        private static readonly Dictionary<Type, PuzzleRun> OfficialRun = new();
        private static readonly Dictionary<Type, List<PuzzleRun>> TestRuns = new();
        private static readonly HashSet<Type> SequentialPuzzles = new();

        public static PuzzleRegistration? GetPuzzle(int year, int day) => Registrations
           .GetValueOrDefault((year, day));

        public static PuzzleRegistration? GetLatestPuzzle() => Registrations
           .OrderByDescending(kvp => kvp.Key.year)
           .ThenByDescending(kvp => kvp.Key.day)
           .Select(kvp => kvp.Value)
           .FirstOrDefault();

        public static PuzzleRegistration? GetLatestPuzzleForYear(int year) => Registrations
           .Where(kvp => kvp.Key.year == year)
           .OrderByDescending(kvp => kvp.Key.day)
           .Select(kvp => kvp.Value)
           .FirstOrDefault();

        public static PuzzleRegistration? GetLatestPuzzleForDay(int day) => Registrations
           .Where(kvp => kvp.Key.day == day)
           .OrderByDescending(kvp => kvp.Key.year)
           .Select(kvp => kvp.Value)
           .FirstOrDefault();

        public static List<PuzzleRegistration> GetAllPuzzles() => Registrations
           .OrderBy(kvp => kvp.Key.year)
           .ThenBy(kvp => kvp.Key.day)
           .Select(kvp => kvp.Value)
           .ToList();

        public static List<PuzzleRegistration> GetAllPuzzlesForYear(int year) => Registrations
           .Where(kvp => kvp.Key.year == year)
           .OrderBy(kvp => kvp.Key.day)
           .Select(kvp => kvp.Value)
           .ToList();

        public static IEnumerable<PuzzleRun> GetRunsForPuzzle(PuzzleRegistration registration)
        {
            foreach (var testRun in GetTestRunsForPuzzle(registration.PuzzleType))
            {
                yield return testRun;
            }

            yield return GetOfficialRunForPuzzle(registration.PuzzleType);
        }

        public static void SetOfficialSolution(Type puzzleType, object? part1Solution, object? part2Solution)
        {
            OfficialRun[puzzleType] = OfficialRun.TryGetValue(puzzleType, out var run)
                ? run with {Part1Solution = part1Solution, Part2Solution = part2Solution}
                : new PuzzleRun(false, 0, string.Empty, part1Solution, part2Solution);
        }

        public static void SetOfficialInput(Type puzzleType, string input)
        {
            OfficialRun[puzzleType] = OfficialRun.TryGetValue(puzzleType, out var run)
                ? run with {Input = input}
                : new PuzzleRun(false, 0, input, null, null);
        }

        public static PuzzleRun GetOfficialRunForPuzzle(Type puzzleType)
        {
            return OfficialRun[puzzleType];
        }

        public static void AddTestRun(Type puzzleType, string input, object? part1Solution, object? part2Solution)
        {
            TestRuns.TryAdd(puzzleType, new List<PuzzleRun>());

            TestRuns[puzzleType].Add(new PuzzleRun(true, TestRuns[puzzleType].Count + 1, input, part1Solution, part2Solution));
        }

        private static IEnumerable<PuzzleRun> GetTestRunsForPuzzle(Type puzzleType)
        {
            return TestRuns.TryGetValue(puzzleType, out var runs)
                ? runs
                : Enumerable.Empty<PuzzleRun>();
        }


        public static void SetSequential(Type puzzleType)
        {
            SequentialPuzzles.Add(puzzleType);
        }

        public static bool IsSequential(PuzzleRegistration registration)
        {
            return SequentialPuzzles.Contains(registration.PuzzleType);
        }

        public static Func<string, Puzzle> GetPuzzleFactory(Type puzzleType)
        {
            return Registrations.First(kvp => kvp.Value.PuzzleType == puzzleType).Value.Factory;
        }
    }
}