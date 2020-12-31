using System;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2016
{
    public sealed class Puzzle_2016_02 : Puzzle
    {
        public static void Configure()
        {
            SetSolution("48584", "563B6");
        }

        private string[] DigitDirections { get; }

        public Puzzle_2016_02(string input)
        {
            DigitDirections = input.Lines();
        }

        protected override object Part1()
        {
            var code = "";
            var digit = 5;

            foreach (var digitDirection in DigitDirections)
            {
                foreach (var direction in digitDirection)
                {
                    digit = (direction, digit) switch
                    {
                        ('U', 1 or 2 or 3) => digit,
                        ('U', _) => digit - 3,

                        ('R', 3 or 6 or 9) => digit,
                        ('R', _) => digit + 1,

                        ('D', 7 or 8 or 9) => digit,
                        ('D', _) => digit + 3,

                        ('L', 1 or 4 or 7) => digit,
                        ('L', _) => digit - 1,

                        _        => digit
                    };
                }

                code += digit;
            }

            return code;
        }

        protected override object Part2()
        {
            var code = "";
            var digit = 5;

            foreach (var digitDirection in DigitDirections)
            {
                foreach (var direction in digitDirection)
                {
                    digit = (direction, digit) switch
                    {
                        ('U', 5 or 2 or 1 or 4 or 9)    => digit,
                        ('U', 3 or 13)                  => digit - 2,
                        ('U', _)                        => digit - 4,

                        ('R', 1 or 4 or 9 or 12 or 13)  => digit,
                        ('R', _)                        => digit + 1,

                        ('D', 5 or 10 or 13 or 12 or 9) => digit,
                        ('D', 1 or 11)                  => digit + 2,
                        ('D', _)                        => digit + 4,

                        ('L', 1 or 2 or 5 or 10 or 13)  => digit,
                        ('L', _)                        => digit - 1,

                        _        => digit
                    };
                }

                if (digit < 10)
                    code += digit;
                else
                    code += (char)('A' + (digit - 10));
            }

            return code;
        }
    }
}
