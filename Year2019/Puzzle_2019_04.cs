using System;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2019
{
    public sealed class Puzzle_2019_04 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(1764, 1196);
        }

        private int Start { get; }
        private int End { get; }

        public Puzzle_2019_04(string input)
        {
            var parts = input.Split('-').ParseArray(int.Parse);
            Start = parts[0];
            End = parts[1];
        }

        protected override object Part1()
        {
            var validPasswords = 0;

            for(var p = Start; p <= End; p++)
            {
                var d1 = p / 100000;

                var d2 = (p / 10000) % 10;
                if(d1 > d2) continue;

                var d3 = (p / 1000) % 10;
                if (d2 > d3) continue;

                var d4 = (p / 100) % 10;
                if (d3 > d4) continue;

                var d5 = (p / 10) % 10;
                if (d4 > d5) continue;

                var d6 = p % 10;
                if (d5 > d6) continue;

                if(d1 == d2 || d2 == d3 || d3 == d4 || d4 == d5 || d5 == d6)
                    validPasswords++;
            }

            return validPasswords;
        }

        protected override object Part2()
        {
            var validPasswords = 0;

            int[] digitCount = new int[10];

            for (var p = Start; p <= End; p++)
            {
                Array.Clear(digitCount, 0, 10);

                var d1 = p / 100000;
                digitCount[d1]++;

                var d2 = (p / 10000) % 10;
                if (d1 > d2) continue;
                digitCount[d2]++;

                var d3 = (p / 1000) % 10;
                if (d2 > d3) continue;
                digitCount[d3]++;

                var d4 = (p / 100) % 10;
                if (d3 > d4) continue;
                digitCount[d4]++;

                var d5 = (p / 10) % 10;
                if (d4 > d5) continue;
                digitCount[d5]++;

                var d6 = p % 10;
                if (d5 > d6) continue;
                digitCount[d6]++;

                if (digitCount.Any(c => c == 2))
                    validPasswords++;
            }

            return validPasswords;
        }
    }
}