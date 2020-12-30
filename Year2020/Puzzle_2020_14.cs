using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2020
{
	internal sealed class Puzzle_2020_14 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(7817357407588, 4335927555692);

            AddPart1Test(@"mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
mem[8] = 11
mem[7] = 101
mem[8] = 0", 165);
            AddPart2Test(@"mask = 000000000000000000000000000000X1001X
mem[42] = 100
mask = 00000000000000000000000000000000X0XX
mem[26] = 1", 208);
        }

        private (string, string)[] Lines { get; }

        public Puzzle_2020_14(string input)
        {
            Lines = input
                .ParseLines(l => l.Split(" = ").Parse(parts => (parts[0], parts[1])));
        }

        protected override object Part1()
        {
            var memory = new Dictionary<long, long>();

            var mask0 = 0L;
            var mask1 = 0L;

            foreach (var (instruction, value) in Lines)
            {
                if (instruction == "mask")
                {
                    mask0 = Convert.ToInt64(value.Replace('X', '1'), 2);
                    mask1 = Convert.ToInt64(value.Replace('X', '0'), 2);
                }
                else
                {
                    var address = long.Parse(instruction[4..^1]);
                    memory[address] = long.Parse(value) & mask0 | mask1;
                }

            }

            return memory.Values.Sum();
        }

        protected override object Part2()
        {
            var memory = new Dictionary<long, long>();

            var floatingBits = new List<int>();
            var mask1 = 0L;

            foreach (var (instruction, value) in Lines)
            {
                if (instruction == "mask")
                {
                    floatingBits = value.Reverse()
                       .Select((c, i) => (c, i))
                       .Where(p => p.c == 'X')
                       .Select(p => p.i)
                       .ToList();
                    mask1 = Convert.ToInt64(value.Replace('X', '0'), 2);
                }
                else
                {
                    var address = long.Parse(instruction[4..^1]) | mask1;
                    var parsedValue = long.Parse(value);
                    foreach (var maskedAddress in BuildAddressesFromMask(address, floatingBits))
                    {
                        memory[maskedAddress] = parsedValue;
                    }
                }

            }

            return memory.Values.Sum();

            static long[] BuildAddressesFromMask(long baseValue, IReadOnlyList<int> floatingBits)
            {
                var values = new long[1 << floatingBits.Count];
                values[0] = baseValue;

                for (var i = 0; i < floatingBits.Count; i++)
                {
                    var mask1 = 1L << floatingBits[i];
                    var mask0 = -1L ^ mask1;

                    var currentLength = 1 << i;
                    for (var j = 0; j < currentLength; j++)
                    {
                        values[j] &= mask0;
                        values[currentLength + j] = values[j] | mask1;
                    }
                }
                return values;
            }
        }
    }
}