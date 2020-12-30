using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2015
{
    internal sealed class Puzzle_2015_17 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(1638, 17);
        }

        private int[] Containers { get; }

        public Puzzle_2015_17(string input)
        {
            Containers = input.ParseLines(int.Parse);
        }

        protected override object Part1()
        {
            var validCombinations = 0;
            for (var k = 1; k <= Containers.Length; k++)
            {
                validCombinations += Containers.Combinations(k).Count(c => c.Sum() == 150);
            }

            return validCombinations;
        }

        protected override object Part2()
        {
            for (var k = 1; k <= Containers.Length; k++)
            {
                var count = Containers.Combinations(k).Count(c => c.Sum() == 150);

                if(count > 0)
                {
                    return count;
                }
            }

            return -1;
        }
    }
}