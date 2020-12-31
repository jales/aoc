using System;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2020
{
    public sealed class Puzzle_2020_15 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(1280, 651639);

            AddTest(@"0,3,6", 436, 175594);
        }

        private int[] StartingNumbers { get; }

        public Puzzle_2020_15(string input)
        {
            StartingNumbers = input.Int32Matches();
        }

        protected override object Part1()
        {
            return SolveForTurn(2020);
        }

        protected override object Part2()
        {
            return SolveForTurn(30000000);
        }

        private int SolveForTurn(int finalTurn)
        {
            // Unreadable code. But I was having fun optimizing for speed :)
            var spokenNumbers = new (int last, int prev)[finalTurn+1];

            for (var i = 0; i < StartingNumbers.Length; i++)
                spokenNumbers[StartingNumbers[i]] = (i + 1, i + 1);

            var lastSpokenNumber = StartingNumbers[^1];
            for (var turn = StartingNumbers.Length + 1; turn <= finalTurn; turn++)
            {
                var (last, prev) = spokenNumbers[lastSpokenNumber];
                lastSpokenNumber = last - prev;

                (last, _) = spokenNumbers[lastSpokenNumber];
                spokenNumbers[lastSpokenNumber] = last == 0 ? (turn, turn) : (turn, last);
            }

            return lastSpokenNumber;
        }
    }
}