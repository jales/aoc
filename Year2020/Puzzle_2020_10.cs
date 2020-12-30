using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2020
{
	internal sealed class Puzzle_2020_10 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(2080, 6908379398144);

            AddTest(@"16
10
15
5
1
11
7
19
6
12
4", 7*5, 8);
            AddTest(@"28
33
18
42
31
14
46
20
48
47
24
23
49
45
19
38
39
11
1
32
25
35
8
17
7
9
4
2
34
10
3", 22*10, 19208);
        }

        private int[] Adapters { get; }

        public Puzzle_2020_10(string input)
        {
            Adapters = input
                .ParseLines(int.Parse)
                .Sorted()
                .ToArray();
        }

        protected override object Part1()
        {
            var differences = new Dictionary<int, int>
            {
                [Adapters[0]] = 1,
                [3] = 1
            };

            for (var i = 1; i < Adapters.Length; i++)
            {
                differences[Adapters[i] - Adapters[i - 1]]++;
            }

            return differences[1]  * differences[3];
        }

        protected override object Part2()
        {
            var combinationsPerAdapter = new Dictionary<int, long>
            {
                [0] = 1
            };

            foreach (var n in Adapters)
            {
                combinationsPerAdapter[n] =
                    combinationsPerAdapter.GetValueOrDefault(n - 1) +
                    combinationsPerAdapter.GetValueOrDefault(n - 2) +
                    combinationsPerAdapter.GetValueOrDefault(n - 3);
            }

            return combinationsPerAdapter[Adapters[^1]];
        }
    }
}