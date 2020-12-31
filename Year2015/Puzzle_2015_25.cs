using System;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2015
{
    public sealed class Puzzle_2015_25 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(9132360, MerryChristmas);
        }

        private int Row { get; }
        private int Column { get; }

        public Puzzle_2015_25(string input)
        {
            var numbers = input.Int32Matches();

            Row = numbers[0];
            Column = numbers[1];
        }

        protected override object Part1()
        {
            var col = 1;
            var row = 1;
            var value = 20151125L;

            while (true)
            {
                if (row == 1)
                {
                    row = col + 1;
                    col = 1;
                }
                else
                {
                    row -= 1;
                    col += 1;
                }

                value *= 252533;
                value %= 33554393;

                if (col == Column && row == Row)
                {
                    return value;
                }
            }
        }

        protected override object Part2()
        {
            return MerryChristmas;
        }
    }
}