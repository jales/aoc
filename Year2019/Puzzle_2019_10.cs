using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;
using static System.Math;

namespace AoC.Year2019
{
    internal sealed class Puzzle_2019_10 : Puzzle
    {
        public static void Configure()
        {
            IsSequential();
            SetSolution(267, 1309);
        }

        private List<(int x, int y)> Asteroids { get; }

        private (int x, int y) BestAsteroid { get; set; } = (-1, -1);

        public Puzzle_2019_10(string input)
        {
            Asteroids = new List<(int, int)>(input.Length);

            var lines = input
                .Lines();

            for (var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#')
                        Asteroids.Add((x, y));
                }
            }
        }

        protected override object Part1()
        {
            var bestCount = 0;

            foreach (var currentAsteroid in Asteroids)
            {
                var count = Asteroids
                   .Where(a => a != currentAsteroid)
                   .Select(a => GetAngle(currentAsteroid, a)) // get angle to asteroid
                   .Distinct()                                // count distinct angles,
                   .Count();

                if (count > bestCount)
                {
                    BestAsteroid = currentAsteroid;
                    bestCount = count;
                }
            }

            return bestCount;
        }

        protected override object Part2()
        {
            // asteroidsByAngle is a list of all the angles for which an asteroid is visible from the center
            // for each angle it contains a list of all visible asteroids sorted from nearest to farthest

            var asteroidsByAngle = Asteroids
               .Where(a => a != BestAsteroid)
               .Select(other => (angle: GetAngle(BestAsteroid, other), distance: GetDistance(BestAsteroid, other), asteroid: other))
               .GroupBy(t => t.angle)
               .OrderBy(t => t.Key)
               .Select(t => t.OrderBy(t => t.distance).Select(t => t.asteroid).ToList())
               .ToList();


            // asteroidsByAngle can be viewed as an histogram like:
            // *   *
            // * * *   *
            // * * * * *
            // ---------
            // The 200th item is found by starting on the bottom left and counting left to right, bottom up.
            //
            // The algorithm below will count an entire row in one go, after which it removes any column that
            // does not have an asteroid on for the next row.

            var column = 199; // 0 based 200th column
            var row = 0;      // 0 based row

            while (column >= asteroidsByAngle.Count)
            {
                column -= asteroidsByAngle.Count;
                // Before moving up to the next row, remove any column that won't have an asteroid for that row
                asteroidsByAngle.RemoveAll(x => x.Count == row + 1);
                row++;
            }


            var (x, y) = asteroidsByAngle[column][row];
            return x * 100 + y;
        }

        private static double GetAngle((int x, int y) center, (int x, int y) other)
        {
            var angle = Atan2(other.x - center.x, center.y - other.y); // 0 aligns with Y axis but Y axis is inverted.
            return angle < 0 ? angle + 2 * PI : angle;
        }

        private static double GetDistance((int x, int y) center, (int x, int y) other)
        {
            return Pow(other.x - center.x, 2) + Pow(other.y - center.y, 2);  // Squared distance is enough as we just need to order.
        }
    }
}