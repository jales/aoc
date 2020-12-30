using System;
using System.Linq;
using AoC.Infrastructure.Puzzles;
using static System.Linq.Enumerable;
using static System.Math;

namespace AoC.Year2019
{
    internal sealed class Puzzle_2019_16 : Puzzle
    {
        public static void Configure()
        {
            SetSolution("94935919", "24158285");
        }

        private int[] Sequence { get; set; }

        public Puzzle_2019_16(string input)
        {
            Sequence = input.Select(c => c - '0').ToArray();
        }

        protected override object Part1()
        {
            var length = Sequence.Length;
            var basePattern = new[] { 0, 1, 0, -1 };

            var patterns = Range(0, length)
               .Select(row => basePattern.SelectMany(d => Repeat(d, row + 1)).Repeat().Skip(1).Take(length).ToArray())
               .ToArray();


            for (var step = 0; step < 100; step++)
            {
                Sequence = Range(0, length).Select(i => Abs(Sequence.Zip(patterns[i], (x, y) => x * y).Sum()) % 10).ToArray();
            }

            var output = string.Join(null, Sequence.Take(8));

            return output;
        }

        protected override object Part2()
        {
            var sequence = Sequence.Repeat(10000).ToArray();

            var offset = int.Parse(string.Join(null, sequence.Take(7)));
            var length = sequence.Length;

            for (var step = 0; step < 100; step++)
            {
                var buffer = new int[length];
                Array.Copy(sequence, buffer, length);

                var sum = 0;
                for (var i = length - 1; i >= length / 2; i--)
                {
                    sum += sequence[i];
                    buffer[i] = sum % 10;
                }

                sequence = buffer;
            }

            var output = string.Join(null, sequence.Skip(offset).Take(8));

            return output;
        }
    }
}