using System;
using System.Numerics;
using System.Text;

namespace AoC.Infrastructure.Puzzles
{
    public sealed record PuzzleResult(PuzzleRun Run, object? Result, bool IsPart1Result, Exception? Error, TimeSpan ElapsedTime, StringBuilder Logs)
    {
        public static PuzzleResult Part1Result(PuzzleRun run, object? result, TimeSpan elapsedTime, StringBuilder logs)
        {
            return new(run, result, true, null, elapsedTime, logs);
        }

        public static PuzzleResult Part2Result(PuzzleRun run, object? result, TimeSpan elapsedTime, StringBuilder logs)
        {
            return new(run, result, false, null, elapsedTime, logs);
        }

        public static PuzzleResult Part1Exception(PuzzleRun run, Exception error, TimeSpan elapsedTime, StringBuilder logs)
        {
            return new(run, null, true, error, elapsedTime, logs);
        }

        public static PuzzleResult Part2Exception(PuzzleRun run, Exception error, TimeSpan elapsedTime, StringBuilder logs)
        {
            return new(run, null, false, error, elapsedTime, logs);
        }

        public string Title => Run.IsTestRun ? $"Test {Run.Index}" : string.Empty;

        public string ErrorDescription => Error?.GetType().Name ?? "Unknown error";

        public bool IsOfficial() => !Run.IsTestRun;

        public bool HasNoResults() => Result is null &&  Error is null;

        public bool HasErrors() => Error is not null;

        public bool ResultIsExpected()
        {
            if (IsPart1Result && Result is not null && Run.Part1Solution is not null)
                return ResultIsExpected(Result, Run.Part1Solution);

            if (!IsPart1Result && Result is not null && Run.Part2Solution is not null)
                return ResultIsExpected(Result, Run.Part2Solution);

            return false;
        }

        public bool ResultIsUnknown()
        {
            if (IsPart1Result && Result is not null && Run.Part1Solution is null)
                return true;

            if (!IsPart1Result && Result is not null && Run.Part2Solution is null)
                return true;

            return false;
        }

        private static bool ResultIsExpected(object expected, object actual) => (expected, actual) switch
        {
            (       int e,        int a) => (e == a),
            (       int e,       uint a) => (e == a),
            (      uint e,        int a) => (e == a),
            (      uint e,       uint a) => (e == a),
            (      long e,       long a) => (e == a),
            (      long e,        int a) => (e == a),
            (       int e,       long a) => (e == a),
            (      long e,       uint a) => (e == a),
            (      uint e,       long a) => (e == a),
            (     ulong e,      ulong a) => (e == a),
            (BigInteger e, BigInteger a) => (e == a),
            (    string e,     string a) => (e == a),
            _ => false
        };
    }
}