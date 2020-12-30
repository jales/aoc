using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2020
{
    internal sealed class Puzzle_2020_05 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(818, 559);

            AddPart1Test(@"FBFBBFFRLR
BFFFBBFRRR
FFFBBBFRRR
BBFFBBFRLL", 820);
        }

        private int[] SeatsIds { get; }

        public Puzzle_2020_05(string input)
        {
            SeatsIds = input
                .ParseLines(GetSeatId)
                .Sorted()
                .ToArray();
        }

        protected override object Part1()
        {
            return SeatsIds.Last();
        }

        protected override object Part2()
        {
            var ids = SeatsIds.ToArray();

            for (int i = 1; i < ids.Length; i++)
            {
                if (ids[i] != ids[i - 1] + 1)
                    return ids[i - 1] + 1;
            }

            return -1;
        }

        private int GetSeatId(string seat)
        {
            var id = 0;

            foreach (var c in seat)
            {
                id <<= 1;
                id += (c == 'B' || c == 'R') ? 1 : 0;
            }

            return id;
        }
    }
}