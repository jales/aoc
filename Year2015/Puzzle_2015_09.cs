using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2015
{
    public sealed class Puzzle_2015_09 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(207, 804);
        }

        private Dictionary<(string from, string to), int> Distances { get; }
        private HashSet<string> Locations { get; }

        public Puzzle_2015_09(string input)
        {
            Distances = input
               .ParseLines(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
               .ToDictionary(p => (p[0], p[2]), p => int.Parse(p[4]));

            Locations = Distances.Keys.Select(k => k.from).Union(Distances.Keys.Select(k => k.to)).ToHashSet();
        }

        protected override object Part1()
        {
            var shortestRouteLength = int.MaxValue;
            foreach (var route in Locations.Permutations())
            {
                var routeLength = 0;
                for (var i = 1; i < route.Count; i++)
                {
                    if (Distances.TryGetValue((route[i - 1], route[i]), out var length))
                    {
                        routeLength += length;
                    }
                    else if (Distances.TryGetValue((route[i], route[i - 1]), out length))
                    {
                        routeLength += length;
                    }
                    else
                    {
                        routeLength = int.MaxValue;
                        break;
                    }
                }

                shortestRouteLength = Math.Min(shortestRouteLength, routeLength);
            }

            return shortestRouteLength;
        }

        protected override object Part2()
        {
            var longestRouteLength = int.MinValue;
            foreach (var route in Locations.Permutations())
            {
                var routeLength = 0;
                for (var i = 1; i < route.Count; i++)
                {
                    if (Distances.TryGetValue((route[i - 1], route[i]), out var length))
                    {
                        routeLength += length;
                    }
                    else if (Distances.TryGetValue((route[i], route[i - 1]), out length))
                    {
                        routeLength += length;
                    }
                    else
                    {
                        routeLength = int.MinValue;
                        break;
                    }
                }

                longestRouteLength = Math.Max(longestRouteLength, routeLength);
            }

            return longestRouteLength;
        }
    }
}