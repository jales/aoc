using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;
using static System.Math;

namespace AoC.Year2019
{
    public sealed class Puzzle_2019_03 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(386, 6484);
        }

        private List<Segment>[] Wires { get; }

        public Puzzle_2019_03(string input)
        {
            Wires = input
               .ParseLines(PathToSegments);
        }

        protected override object Part1()
        {
            var intersections = Wires[0]
               .SelectMany(_ => Wires[1], (w1, w2) => (w1, w2))
               .SelectMany(t => t.w1.Intersect(t.w2))
               .ToList();

            return intersections.Select(i => i.Distance).Min();
        }

        protected override object Part2()
        {
            var intersections = Wires[0]
               .SelectMany(_ => Wires[1], (w1, w2) => (w1, w2))
               .SelectMany(t => t.w1.Intersect(t.w2))
               .ToList();

            return intersections.Select(i => i.Steps).Min();
        }

        private static List<Segment> PathToSegments(string wirePath)
        {
            var segments = new List<Segment>();
            var x = 0;
            var y = 0;
            var steps = 0;

            foreach (var instruction in wirePath.Split(','))
            {
                var length = int.Parse(instruction.Substring(1));

                var (xOffset, yOffset) = instruction[0] switch
                {
                    'R' => (x + length, y),
                    'L' => (x - length, y),
                    'U' => (x, y + length),
                    'D' => (x, y + -length),
                    _ => (0, 0)
                };

                segments.Add(new Segment(x, y, xOffset, yOffset, steps));
                x = xOffset;
                y = yOffset;
                steps += length;
            }

            return segments;
        }

        private class Segment
        {
            private readonly int _x1;
            private readonly int _y1;
            private readonly int _x2;
            private readonly int _y2;
            private readonly int _steps;
            private readonly bool _isHorizontal;

            public Segment(int x1, int y1, int x2, int y2, int steps)
            {
                _x1 = x1;
                _y1 = y1;
                _x2 = x2;
                _y2 = y2;
                _steps = steps;
                _isHorizontal = _x1 != _x2;
            }

            public IEnumerable<Intersection> Intersect(Segment other)
            {
                // Ignore the case where the have the same orientation for now
                // If they exist we would return all the intersections
                if (_isHorizontal == other._isHorizontal) yield break;

                var h = _isHorizontal ? this : other;
                var v = _isHorizontal ? other : this;

                var x = v._x1;
                var y = h._y1;

                if (x == 0 && y == 0) yield break;

                if (v._x1 >= Min(h._x1, h._x2) && v._x1 <= Max(h._x1, h._x2) &&
                    h._y1 >= Min(v._y1, v._y2) && h._y1 <= Max(v._y1, v._y2))
                {
                    var hSteps = h._steps + (h._x1 > x ? h._x1 - x : x - h._x1);
                    var vSteps = v._steps + (v._y1 > y ? v._y1 - y : y - v._y1);

                    yield return new Intersection(x, y, hSteps + vSteps);
                }
            }
        }

        private class Intersection
        {
            private readonly int _x;
            private readonly int _y;

            public int Distance => Abs(_x) + Abs(_y);
            public int Steps { get; }

            public Intersection(int x, int y, int steps)
            {
                _x = x;
                _y = y;
                Steps = steps;
            }
        }
    }
}