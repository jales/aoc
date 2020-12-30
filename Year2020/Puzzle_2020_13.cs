using System;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2020
{
    internal sealed class Puzzle_2020_13 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(4938, 230903629977901);

            AddTest(@"939
7,13,x,x,59,x,31,19",
                295,
                1068781);
        }

        private int Timestamp { get; }
        private int[] Buses { get; }

        public Puzzle_2020_13(string input)
        {
            var numbers = input
               .ParseLines(l => l.Split(',')
                   .Select(n => n == "x" ? -1 : int.Parse(n))
                   .ToArray());

            Timestamp = numbers[0][0];
            Buses = numbers[1];
        }

        protected override object Part1()
        {
            var (bus, time) = Buses
               .Where(b => b != -1)
               .Select(b => (b, time: (Timestamp / b + 1) * b))
               .OrderBy(b => b.time)
               .First();

            return (time - Timestamp) * bus;
        }

        protected override object Part2()
        {
            var buses = Buses
               .Select((b, i) => (bus: b, index: i))
               .Where(bi => bi.bus != -1)
               .ToList();

            long timestamp = buses[0].bus;
            long increment = buses[0].bus;

            buses.RemoveAt(0);
            while (buses.Count > 0)
            {
                timestamp += increment;
                for (var i = 0; i < buses.Count; i++)
                {
                    var (bus, index) = buses[i];

                    if ((timestamp + index) % bus != 0) continue;

                    buses.RemoveAt(i);

                    increment = Lcm(increment, bus);
                    break;
                }
            }

            return timestamp;
        }
    }
}