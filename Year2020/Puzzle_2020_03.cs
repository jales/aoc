using System;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2020
{
    internal sealed class Puzzle_2020_03 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(207, 2655892800);

            AddTest(@"
..##.......
#...#...#..
.#....#..#.
..#.#...#.#
.#...##..#.
..#.##.....
.#.#.#....#
.#........#
#.##...#...
#...##....#
.#..#...#.#", 7, 336);
        }

        private char[][] Grid{ get; }
        private int Height { get; }
        private int Width { get; }

        public Puzzle_2020_03(string input)
        {
            Grid = input
                .ParseLines(line => line.ToArray());

            Height = Grid.Length;
            Width = Grid[0].Length;
        }

        protected override object Part1()
        {
            return GetTrees(3,1);
        }

        protected override object Part2()
        {
            return GetTrees(1, 1) * GetTrees(3, 1) * GetTrees(5, 1) * GetTrees(7, 1) * GetTrees(1, 2);
        }

        private long GetTrees(int right, int down)
        {
            var trees = 0;

            for (int x = 0, y = 0; y < Height; x = (x + right) % Width, y += down)
            {
                if (Grid[y][x] == '#') trees++;
            }

            return trees;
        }
    }
}