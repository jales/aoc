using System;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2019
{
    internal sealed class Puzzle_2019_01 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(3331849, 4994898);
        }

        private int[] Masses { get; }

        public Puzzle_2019_01(string input)
        {
            Masses = input
                .ParseLines(int.Parse);
        }

        protected override object Part1()
        {
            return Masses
                .Select(m => (m/3) -2)
                .Sum();
        }

        protected override object Part2()
        {
            return Masses
                .Select(CalculateFuel)
                .Sum();
        }

        private static int CalculateFuel(int mass)
        {
            var fuel = (mass / 3) - 2;

            return fuel <= 0
                ? 0
                : fuel + CalculateFuel(fuel);
        }
    }
}