namespace AoC.Infrastructure.Puzzles
{
    public sealed record PuzzleRun(bool IsTestRun, int Index, string Input, object? Part1Solution, object? Part2Solution)
    {
    }
}