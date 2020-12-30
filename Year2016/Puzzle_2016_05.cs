using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2016
{
    internal sealed class Puzzle_2016_05 : Puzzle
    {
        public static void Configure()
        {
            SetSolution("4543c154", "1050cbbd");

            // Too slow
            // AddTest(@"abc", "18f47a30", "05ace8e3");
        }

        private string Input { get; }

        public Puzzle_2016_05(string input)
        {
            Input = input;
        }

        protected override object Part1()
        {
            var hasher = MD5.Create();

            var password = "";
            for (var i = 0; i < int.MaxValue; i++)
            {
                var secret = Input + i;

                var hash = hasher.ComputeHash((Encoding.UTF8.GetBytes(secret)));

                if (hash[0] == 0 && hash[1] == 0 && hash[2] < 0x10)
                {
                    password += hash[2].ToString("x");

                    if(password.Length == 8) break;
                }
            }

            return password;
        }

        protected override object Part2()
        {
            var hasher = MD5.Create();

            var password = new[] { ".", ".", ".", ".", ".", ".", ".", "." };

            for (var i = 0; i < int.MaxValue; i++)
            {
                var secret = Input + i;

                var hash = hasher.ComputeHash((Encoding.UTF8.GetBytes(secret)));

                if (hash[0] == 0 && hash[1] == 0 && hash[2] < 8 && password[hash[2]] == ".")
                {
                    password[hash[2]] = (hash[3] >> 4).ToString("x");

                    if(password.All(s => s != ".")) break;
                }
            }

            return string.Join("", password);
        }
    }
}