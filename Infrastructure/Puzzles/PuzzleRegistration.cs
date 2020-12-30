using System;

namespace AoC.Infrastructure.Puzzles
{
    public sealed record PuzzleRegistration(Type PuzzleType, int Year, int Day, Action ConfigurePuzzle, Func<string, Puzzle> Factory)
    {
    }
}