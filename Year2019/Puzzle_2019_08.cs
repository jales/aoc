using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2019
{
    public sealed class Puzzle_2019_08 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(1820, "ZUKCJ");
        }

        private string Input { get; }

        public Puzzle_2019_08(string input)
        {
            Input = input;
        }

        protected override object Part1()
        {
            return Input
                .Buffer(25 * 6)                                                        // Layers are 25*6 pixels
                .ToList().MinBy(l => l.Count(d => d == '0'))                           // Get the layer with the least '0's
                .ToList().Select(l => l.Count(d => d == '1') * l.Count(d => d == '2')) // '1's * '2's
                .ToList().First();
        }

        protected override object Part2()
        {
            var layers = Input
               .Buffer(25 * 6)
               .ToList();

            Enumerable
               .Range(0, 25 * 6)                                           // Layers are 25*6 pixels
               .Select(i => layers.Select(l => l[i]).First(d => d != '2')) // For each pixel find first layer with non transparent pixel
               .Select(p => p == '0' ? ' ' : '#')                          // Readability. My console is black
               .Buffer(25)                                                 // Rows are 25 pixels
               .ForEach(row => Log.AppendLine(new string(row.ToArray()))); // Print rows

            // The result was taken from looking at the logs produced by the above query :)
            return "ZUKCJ";
        }
    }
}