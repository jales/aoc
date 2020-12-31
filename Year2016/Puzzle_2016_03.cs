using System;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2016
{
    public sealed class Puzzle_2016_03 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(982, 1826);
        }

        private int[][] Triangles { get; }

        public Puzzle_2016_03(string input)
        {
            Triangles = input
                .ParseLines(line => line.Int32Matches());
        }

        protected override object Part1()
        {
            var count = 0;

            foreach (var triangle in Triangles)
            {
                var a = triangle[0];
                var b = triangle[1];
                var c = triangle[2];

                if (a > b) Swap(ref a, ref b);

                if (b > c) Swap(ref b, ref c);

                if (a > b) Swap(ref a, ref b);

                if(a + b > c) count++;
            }

            return count;
        }

        protected override object Part2()
        {
            var count = 0;

            for (var i = 0; i < 3; i ++)
            for (var j = 0; j < Triangles.Length; j += 3)

            {
                var a = Triangles[j    ][i];
                var b = Triangles[j + 1][i];
                var c = Triangles[j + 2][i];

                if (a > b) Swap(ref a, ref b);

                if (b > c) Swap(ref b, ref c);

                if (a > b) Swap(ref a, ref b);

                if (a + b > c) count++;
            }

            return count;
        }
    }
}