using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2015
{
    internal class Puzzle_2015_01 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(74, 1795);

            AddPart1Test("(())", 0);
            AddPart1Test("))(((((", 3);

            AddPart2Test(")", 1);
            AddPart2Test("()())", 5);
        }

        private string Input { get; }

        public Puzzle_2015_01(string input)
        {
            Input = input;
        }

        protected override object Part1()
        {
            return Input.Sum(c => c == '(' ? +1 : -1);
        }

        protected override object Part2()
        {
            var sum = 0;
            for (var i = 0; i < Input.Length; i++)
            {
                var c = Input[i];
                sum += c == '(' ? +1 : -1;
                if (sum < 0)
                    return i + 1;
            }

            return -1;
        }
    }
}
