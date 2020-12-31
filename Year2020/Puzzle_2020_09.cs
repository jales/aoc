using System;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2020
{
    public sealed class Puzzle_2020_09 : Puzzle
    {
        public static void Configure()
        {
            IsSequential();

            SetSolution(542529149, 75678618);

            AddTest(@"35
20
15
25
47
40
62
55
65
95
102
117
150
182
127
219
299
277
309
576", 127, 62);
        }

        private int PreambleLength { get; set; } = 25;
        private long[] Numbers { get; }

        private int IndexOfFirstInvalid { get; set; } = -1;

        public Puzzle_2020_09(string input)
        {
            Numbers = input.ParseLines(long.Parse);
        }

        protected override object Part1()
        {
            for (var i = PreambleLength + 1; i < Numbers.Length; i++)
            {
                if (IsInvalid(i))
                {
                    IndexOfFirstInvalid = i;
                    break;
                }
            }

            return Numbers[IndexOfFirstInvalid];
        }

        private bool IsInvalid(int i)
        {
            for (var j = i - (PreambleLength + 1); j < i - 1; j++)
                for (var k = j + 1; k < i; k++)
                {
                    if (Numbers[j] + Numbers[k] == Numbers[i]) return false;
                }

            return true;
        }

        protected override object Part2()
        {
            var target = Numbers[IndexOfFirstInvalid];

            var sums = new long[IndexOfFirstInvalid];
            sums[0] = Numbers[0];

            for (var i = 1; i < IndexOfFirstInvalid; i++)
            {
                sums[i] = sums[i - 1] + Numbers[i];

                var j = Array.IndexOf(sums, sums[i] - target);

                if (j == -1) continue;

                var range = Numbers[j..i];
                return range.Min() + range.Max();
            }

            return -1;
        }

        public override void ChangePreconditionsForTests()
        {
            PreambleLength = 5;
        }
    }
}