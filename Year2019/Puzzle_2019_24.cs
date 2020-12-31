using System;
using System.Collections.Generic;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2019
{
    public sealed class Puzzle_2019_24 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(32511025, 1932);
        }

        private string Input { get; }

        public Puzzle_2019_24(string input)
        {
            Input = input;
        }

        protected override object Part1()
        {
            var map = new Map(Input);
            var layouts = new HashSet<int> { map.Biodiversity() };

            while (true)
            {
                map.Evolve();
                if (!layouts.Add(map.Biodiversity())) break;
            }

            return map.Biodiversity();
        }

        protected override object Part2()
        {
            var map = new FractalMap(Input);

            for (var i = 0; i < 200; i++)
            {
                map.Evolve();
            }

            return map.BugCount;
        }

        private class Map
        {
            private HashSet<(int, int)> Tiles { get; set; } = new();

            public Map(string map)
            {
                var lines = map.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                for (var y = 0; y < lines.Length; y++)
                    for (var x = 0; x < lines[y].Length; x++)
                    {
                        if (lines[y][x] == '#') Tiles.Add((x, y));
                    }
            }


            public int Biodiversity()
            {
                var biodiversity = 0;
                var tileRating = 1;

                for (var y = 0; y < 5; y++)
                    for (var x = 0; x < 5; x++)
                    {
                        if (Tiles.Contains((x, y))) biodiversity += tileRating;
                        tileRating <<= 1;
                    }

                return biodiversity;
            }

            public void Evolve()
            {
                var newTiles = new HashSet<(int, int)>();

                for (var y = 0; y < 5; y++)
                    for (var x = 0; x < 5; x++)
                    {
                        var adjacentBugs = AdjacentBugs((x, y));
                        var isBug = Tiles.Contains((x, y));
                        if ((isBug && adjacentBugs == 1) || (!isBug && adjacentBugs > 0 && adjacentBugs < 3))
                        {
                            newTiles.Add((x, y));
                        }
                    }

                Tiles = newTiles;
            }

            private int AdjacentBugs((int, int) tile)
            {
                var count = 0;
                var (x, y) = tile;
                if (Tiles.Contains((x + 1, y))) count++;
                if (Tiles.Contains((x - 1, y))) count++;
                if (Tiles.Contains((x, y + 1))) count++;
                if (Tiles.Contains((x, y - 1))) count++;

                return count;
            }
        }

        private class FractalMap
        {
            private HashSet<(int, int, int)> Tiles { get; set; } = new();
            private SortedSet<int> Levels { get; set; } = new();
            public int BugCount => Tiles.Count;

            public FractalMap(string map)
            {
                var lines = map.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                for (var y = 0; y < lines.Length; y++)
                    for (var x = 0; x < lines[y].Length; x++)
                    {
                        if (x == 2 && y == 2) continue;
                        if (lines[y][x] == '#') Tiles.Add((x, y, 0));
                    }

                if (Tiles.Count > 0) Levels.Add(0);
            }

            public void Evolve()
            {
                var newTiles = new HashSet<(int, int, int)>();
                var newLevels = new SortedSet<int>();
                for (var l = Levels.Min - 1; l <= Levels.Max + 1; l++)
                    for (var y = 0; y < 5; y++)
                        for (var x = 0; x < 5; x++)
                        {
                            if (x == 2 && y == 2) continue;

                            var adjacentBugs = AdjacentBugs((x, y, l));
                            var isBug = Tiles.Contains((x, y, l));
                            if ((isBug && adjacentBugs == 1) || (!isBug && adjacentBugs > 0 && adjacentBugs < 3))
                            {
                                newTiles.Add((x, y, l));
                                newLevels.Add(l);
                            }
                        }

                Tiles = newTiles;
                Levels = newLevels;
            }

            private int AdjacentBugs((int, int, int) tile)
            {
                var count = 0;
                var (x, y, l) = tile;

                // Tiles to the right
                if (x <= 3)
                {
                    if (x == 1 && y == 2)
                    {
                        if (Tiles.Contains((0, 0, l + 1))) count++;
                        if (Tiles.Contains((0, 1, l + 1))) count++;
                        if (Tiles.Contains((0, 2, l + 1))) count++;
                        if (Tiles.Contains((0, 3, l + 1))) count++;
                        if (Tiles.Contains((0, 4, l + 1))) count++;
                    }
                    else
                    {
                        if (Tiles.Contains((x + 1, y, l))) count++;
                    }
                }
                else
                {
                    if (Tiles.Contains((3, 2, l - 1))) count++;
                }

                // Tiles up
                if (y >= 1)
                {
                    if (x == 2 && y == 3)
                    {
                        if (Tiles.Contains((0, 4, l + 1))) count++;
                        if (Tiles.Contains((1, 4, l + 1))) count++;
                        if (Tiles.Contains((2, 4, l + 1))) count++;
                        if (Tiles.Contains((3, 4, l + 1))) count++;
                        if (Tiles.Contains((4, 4, l + 1))) count++;
                    }
                    else
                    {
                        if (Tiles.Contains((x, y - 1, l))) count++;
                    }
                }
                else
                {
                    if (Tiles.Contains((2, 1, l - 1))) count++;
                }

                // Tiles to the left
                if (x >= 1)
                {
                    if (x == 3 && y == 2)
                    {
                        if (Tiles.Contains((4, 0, l + 1))) count++;
                        if (Tiles.Contains((4, 1, l + 1))) count++;
                        if (Tiles.Contains((4, 2, l + 1))) count++;
                        if (Tiles.Contains((4, 3, l + 1))) count++;
                        if (Tiles.Contains((4, 4, l + 1))) count++;
                    }
                    else
                    {
                        if (Tiles.Contains((x - 1, y, l))) count++;
                    }
                }
                else
                {
                    if (Tiles.Contains((1, 2, l - 1))) count++;
                }

                // Tiles down
                if (y <= 3)
                {
                    if (x == 2 && y == 1)
                    {
                        if (Tiles.Contains((0, 0, l + 1))) count++;
                        if (Tiles.Contains((1, 0, l + 1))) count++;
                        if (Tiles.Contains((2, 0, l + 1))) count++;
                        if (Tiles.Contains((3, 0, l + 1))) count++;
                        if (Tiles.Contains((4, 0, l + 1))) count++;
                    }
                    else
                    {
                        if (Tiles.Contains((x, y + 1, l))) count++;
                    }
                }
                else
                {
                    if (Tiles.Contains((2, 3, l - 1))) count++;
                }

                return count;
            }
        }
    }
}