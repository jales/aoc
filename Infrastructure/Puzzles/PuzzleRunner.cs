using System;
using System.Collections.Generic;
using System.IO;

namespace AoC.Infrastructure.Puzzles
{
    internal static class PuzzleRunner
    {
        public static IEnumerable<PuzzleResult> Solve(PuzzleRegistration registration, Func<TextWriter>? logOverride = null)
        {
            registration.ConfigurePuzzle();

            var isSequentialPuzzle = PuzzleRegistrar.IsSequential(registration);

            var runs = PuzzleRegistrar.GetRunsForPuzzle(registration);
            foreach (var run in runs)
            {
                var puzzle = InitializePuzzleInstance(registration, run);

                var puzzleResult = puzzle.SolvePart1(run);

                if(!run.IsTestRun || run.Part1Solution is not null)
                {
                    yield return puzzleResult;
                }

                if (run.IsTestRun && run.Part2Solution is null) continue;

                puzzle = isSequentialPuzzle ? puzzle : InitializePuzzleInstance(registration, run);

                yield return puzzle.SolvePart2(run);
            }
        }

        private static Puzzle InitializePuzzleInstance(PuzzleRegistration registration, PuzzleRun run)
        {
            var puzzle = registration.Factory(run.Input);

            if (run.IsTestRun)
                puzzle.ChangePreconditionsForTests();

            return puzzle;
        }
    }
}