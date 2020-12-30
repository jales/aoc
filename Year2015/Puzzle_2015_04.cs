using System.Security.Cryptography;
using System.Text;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2015
{
    internal sealed class Puzzle_2015_04 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(346386, 9958218);
        }

        private string Input { get; }

        public Puzzle_2015_04(string input)
        {
            Input = input;
        }

        protected override object Part1()
        {
            var hasher = MD5.Create();

            for (var i = 0; i < int.MaxValue; i++)
            {
                var secret = Input + i;

                var hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(secret));

                if(hash[0] == 0 && hash[1] == 0 && hash[2] < 0x10)
                {
                    return i;
                }
            }

            return "Can't find answer :(";
        }

        protected override object Part2()
        {
            var hasher = MD5.Create();

            for (var i = 0; i < int.MaxValue; i++)
            {
                var secret = Input + i;

                var hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(secret));

                if(hash[0] == 0 && hash[1] == 0 && hash[2] == 0)
                {
                    return i;
                }
            }

            return "Can't find answer :(";
        }
    }
}
