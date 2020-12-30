using System;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2015
{
    internal class Puzzle_2015_02 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(1598415, 3812909);
        }

        private (int l, int w, int h)[] BoxDimensions { get; }

        public Puzzle_2015_02(string input)
        {
             BoxDimensions = input
                .ParseLines(line => line.Int32Matches().Parse(matches => (matches[0], matches[1], matches[2])));
        }

        protected override object Part1()
        {
            var area = 0;
            foreach (var (l, w, h) in BoxDimensions)
            {
                var side1 = l * w;
                var side2 = w * h;
                var side3 = h * l;

                var padding = Math.Min(side1, Math.Min(side2, side3));

                area += 2 * side1 + 2 * side2 + 2 * side3 + padding;
            }

            return area;
        }

        protected override object Part2()
        {
            var length = 0;
            foreach (var (l, w, h) in BoxDimensions)
            {
                var maxDimension = Math.Max(l, Math.Max(w, h));

                length += 2 * (l + w + h - maxDimension);

                length += l*w*h;
            }

            return length;
        }
    }
}
