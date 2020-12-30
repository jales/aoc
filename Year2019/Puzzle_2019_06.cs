using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;
using static System.Math;

namespace AoC.Year2019
{
    internal sealed class Puzzle_2019_06 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(130681, 313);
        }

        private Dictionary<string, string> DirectOrbits { get; }

        public Puzzle_2019_06(string input)
        {
            DirectOrbits = input
                .ParseLines(line => line.Split(')'))
                .ToDictionary(x => x[1], x => x[0]);
        }

        protected override object Part1()
        {
            var totalOrbits = 0;

            foreach(var start in DirectOrbits.Keys)
            {
                var tmp = start;
                do
                {
                    totalOrbits++;
                    tmp = DirectOrbits[tmp];
                }while(tmp != "COM");
            }

            return totalOrbits;
        }

        protected override object Part2()
        {
            var myPath = GetPath("YOU", DirectOrbits);
            var santaPath = GetPath("SAN", DirectOrbits);

            for (var i = 0; i < Min(myPath.Count, santaPath.Count); i++)
            {
                if (myPath[i] != santaPath[i])
                {
                    return (myPath.Count - i) + (santaPath.Count - i);
                }
            }

            return -1;
        }

        private static List<string> GetPath(string position, IReadOnlyDictionary<string, string> paths)
        {
            var path = new List<string>(400);
            var current = position;
            while (paths.TryGetValue(current, out current))
            {
                path.Insert(0, current);
            }

            return path;
        }
    }
}