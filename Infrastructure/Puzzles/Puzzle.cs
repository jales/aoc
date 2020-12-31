using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using static AoC.Infrastructure.Puzzles.PuzzleResult;

namespace AoC.Infrastructure.Puzzles
{
    public abstract partial class Puzzle
    {
        public const string MerryChristmas = "Merry Christmas!";

        protected StringBuilder Log { get; set; } = null!;

        public PuzzleResult SolvePart1(PuzzleRun run)
        {
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                Log = new StringBuilder();
                var result = Part1();
                return Part1Result(run, result, sw.Elapsed, Log);
            }
            catch (Exception e)
            {
                return Part1Exception(run, e, sw.Elapsed, Log);
            }
        }

        public object BenchmarkPart1() => Part1();

        protected abstract object Part1();

        public PuzzleResult SolvePart2(PuzzleRun run)
        {
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                Log = new StringBuilder();
                var result = Part2();
                return Part2Result(run, result, sw.Elapsed, Log);
            }
            catch (Exception e)
            {
                return Part2Exception(run, e, sw.Elapsed, Log);
            }
        }

        public object BenchmarkPart2() => Part2();

        protected abstract object Part2();

        public virtual void ChangePreconditionsForTests()
        {
        }

        protected static void SetSolution(object part1Solution)
        {
            PuzzleRegistrar.SetOfficialSolution(GetPuzzleType(), part1Solution, null);
        }

        protected static void SetSolution(object part1Solution, object part2Solution)
        {
            PuzzleRegistrar.SetOfficialSolution(GetPuzzleType(), part1Solution, part2Solution);
        }

        protected static void AddPart1Test(string input, object part1Solution)
        {
            PuzzleRegistrar.AddTestRun(GetPuzzleType(), input, part1Solution, null);
        }

        protected static void AddPart2Test(string input, object part2Solution)
        {
            PuzzleRegistrar.AddTestRun(GetPuzzleType(), input, null, part2Solution);
        }

        protected static void AddTest(string input, object part1Solution, object? part2Solution = null)
        {
            PuzzleRegistrar.AddTestRun(GetPuzzleType(), input, part1Solution, part2Solution);
        }

        protected static void IsSequential()
        {
            PuzzleRegistrar.SetSequential(GetPuzzleType());
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Type GetPuzzleType() => new StackTrace().GetFrame(2)!.GetMethod()!.DeclaringType!;
    }
}