using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2020
{
    public sealed class Puzzle_2020_02 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(483, 482);

            AddTest(@"
1-3 a: abcde
1-3 b: cdefg
2-9 c: ccccccccc", 2, 1);
        }

        private List<(int min, int max, char chr, string password)> PolicyChecks { get; }

        public Puzzle_2020_02(string input)
        {
            PolicyChecks = input
                .ParseLines(line =>
                {
                    var parts = line.Split(new[] { '-', ':', ' '}, StringSplitOptions.RemoveEmptyEntries);

                    return (int.Parse(parts[0]), int.Parse(parts[1]), parts[2][0], parts[3]);
                })
                .ToList();

        }

        protected override object Part1()
        {
            var validPasswords = 0;

            foreach(var (min, max, chr, pass) in PolicyChecks)
            {
                var chrCount = pass.Count(c => c == chr);

                if (min <= chrCount && chrCount <= max)
                    validPasswords++;
            }

            return validPasswords;
        }

        protected override object Part2()
        {
            var validPasswords = 0;

            foreach (var (min, max, chr, pass) in PolicyChecks)
            {
                var chrIsAtMin = pass[min-1] == chr;
                var chrIsAtMax = pass[max-1] == chr;
                if (chrIsAtMin ^ chrIsAtMax)
                    validPasswords++;
            }

            return validPasswords;
        }
    }
}
