using System;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2020
{
    internal sealed class Puzzle_2020_01 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(926464, 65656536);

            AddTest(@"
1721
979
366
299
675
1456", 514579, 241861950);
        }

        private int[] Values { get; }

        public Puzzle_2020_01(string input)
        {
            Values = input
                .ParseLines(int.Parse);
        }

        protected override object Part1()
        {
            for (var i = 0; i < Values.Length; i++)
            {
                for (var j = i + 1; j < Values.Length; j++)
                {
                    var x = Values[i];
                    var y = Values[j];
                    if (x + y == 2020) return x * y;
                }
            }

            return -1;
        }

        protected override object Part2()
        {
            for (var i = 0; i < Values.Length; i++)
            {
                for (var j = i + 1; j < Values.Length; j++)
                {
                    var x = Values[i];
                    var y = Values[j];
                    if (x + y >= 2020) continue;

                    for(var k = j+1; k < Values.Length; k++)
                    {
                        var z = Values[k];

                        if (x + y + z == 2020) return x * y * z;
                    }
                }
            }

            return -1;
        }
    }
}
