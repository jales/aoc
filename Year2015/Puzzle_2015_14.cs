using System;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2015
{
    internal sealed class Puzzle_2015_14 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(2655, 1059);
        }

        private Reindeer[] Runners { get; }

        public Puzzle_2015_14(string input)
        {
            Runners = input.ParseLines(Reindeer.Parse);
        }

        protected override object Part1()
        {
            return Runners.Select(r => r.GetPositionAt(2503)).Max();
        }

        protected override object Part2()
        {
            var paths = Runners.Select(r => r.GetPositionsUntil(2503)).ToList();

            for (var second = 1; second < 2504; second++)
            {
                var maxPosition = paths.Select(p => p[second]).Max();

                foreach (var path in paths)
                {
                    path[second] = path[second - 1] + (path[second] == maxPosition ? 1 : 0);
                }
            }

            return paths.Select(p => p[2503]).Max();
        }

        private record Reindeer(int FlightSpeed, int MaxFlightTime, int MaxRestTime)
        {
            public static Reindeer Parse(string line) => line
               .Int32Matches()
               .Parse(matches => new Reindeer(matches[0], matches[1], matches[2]));

            public int GetPositionAt(int seconds)
            {
                var isFlying = true;
                var position = 0;

                while (seconds > 0)
                {
                    if (isFlying)
                    {
                        position += Math.Min(seconds, MaxFlightTime) * FlightSpeed;
                        seconds -= MaxFlightTime;
                    }
                    else
                    {
                        seconds -= MaxRestTime;
                    }

                    isFlying = !isFlying;
                }

                return position;
            }

            public int[] GetPositionsUntil(int seconds)
            {
                seconds++;

                var isFlying = true;
                var positions = new int[seconds];
                var position = 1;

                while (position < seconds)
                {
                    if (isFlying)
                    {
                        var length = Math.Min(seconds, position+MaxFlightTime);
                        for (; position < length; position++)
                        {
                            positions[position] = positions[position-1] + FlightSpeed;
                        }
                    }
                    else
                    {
                        var length = Math.Min(seconds, position+MaxRestTime);
                        for (; position < length; position++)
                        {
                            positions[position] = positions[position-1];
                        }
                    }

                    isFlying = !isFlying;
                }

                return positions;
            }
        }
    }
}