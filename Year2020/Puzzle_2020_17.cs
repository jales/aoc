using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;
using AoC.Support;
using static System.Math;

namespace AoC.Year2020
{
	public sealed class Puzzle_2020_17 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(242, 2292);

            AddTest(@".#.
..#
###", 112, 848);
        }

        private HashSet<(int x, int y, int, int)> CurrentState { get; set; }

        public Puzzle_2020_17(string input)
        {
            CurrentState = input
               .Lines()
               .SelectMany((line, y) => line.Select((c, x) => (x, y, cube: c == '#')))
               .Where(c => c.cube)
               .Select(result => (result.x, result.y, 0, 0))
               .ToHashSet();
        }

        protected override object Part1()
        {
            for (var cycle = 0; cycle < 6; cycle++)
            {
                var newState = new HashSet<(int x, int y, int z, int w)>(CurrentState.Count);

                var (xMin, xMax, yMin, yMax, zMin, zMax, _, _) = Ranges();

                for (var x = xMin; x <= xMax; x++)
                for (var y = yMin; y <= yMax; y++)
                for (var z = zMin; z <= zMax; z++)
                {
                    var isActive = CurrentState.Contains((x, y, z, 0));
                    var activeNeighbors = (x, y, z).Neighbors().Count(c => CurrentState.Contains((c.x, c.y, c.z, 0)));

                    if(isActive && (activeNeighbors== 2 || activeNeighbors == 3))
                        newState.Add((x, y, z, 0));

                    if(!isActive && activeNeighbors == 3)
                        newState.Add((x, y, z, 0));

                }

                CurrentState = newState;
            }

            return CurrentState.Count;
        }

        protected override object Part2()
        {
            for (var cycle = 0; cycle < 6; cycle++)
            {
                var newState = new HashSet<(int x, int y, int z, int w)>(CurrentState.Count);

                var (xMin, xMax, yMin, yMax, zMin, zMax, wMin, wMax) = Ranges();

                for (var x = xMin; x <= xMax; x++)
                for (var y = yMin; y <= yMax; y++)
                for (var z = zMin; z <= zMax; z++)
                for (var w = wMin; w <= wMax; w++)
                {
                    var isActive = CurrentState.Contains((x, y, z, w));
                    var activeNeighbors = (x, y, z, w).Neighbors().Count(c => CurrentState.Contains((c.x, c.y, c.z, c.w)));

                    if(isActive && (activeNeighbors== 2 || activeNeighbors == 3))
                        newState.Add((x, y, z, w));

                    if(!isActive && activeNeighbors == 3)
                        newState.Add((x, y, z, w));
                }

                CurrentState = newState;
            }

            return CurrentState.Count;
        }

        private (int x, int X, int y, int Y, int z, int Z, int w, int W) Ranges()
        {
            var xMin = int.MaxValue;
            var xMax = int.MinValue;
            var yMin = int.MaxValue;
            var yMax = int.MinValue;
            var zMin = int.MaxValue;
            var zMax = int.MinValue;
            var wMin = int.MaxValue;
            var wMax = int.MinValue;

            foreach (var (x, y, z, w) in CurrentState)
            {
                xMin = Min(xMin, x - 1);
                xMax = Max(xMax, x + 1);

                yMin = Min(yMin, y - 1);
                yMax = Max(yMax, y + 1);

                zMin = Min(zMin, z - 1);
                zMax = Max(zMax, z + 1);

                wMin = Min(wMin, w - 1);
                wMax = Max(wMax, w + 1);
            }

            return (xMin, xMax, yMin, yMax, zMin, zMax, wMin, wMax);
        }
    }
}
