using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2015
{
    public sealed class Puzzle_2015_11 : Puzzle
    {
        public static void Configure()
        {
            IsSequential();

            SetSolution("cqjxxyzz", "cqkaabcc");
        }

        private int[] Password { get; }

        public Puzzle_2015_11(string input)
        {
            Password = input.Select(c => c - 'a').ToArray();
        }

        protected override object Part1()
        {
            return FindNextPassword();
        }

        protected override object Part2()
        {
            return FindNextPassword();
        }

        private string FindNextPassword()
        {
            while (true)
            {
                IncrementPassword(Password);

                if (IsValidPassword(Password)) break;
            }

            return new string(Password.Select(c => (char)('a' + c)).ToArray());
        }

        private void IncrementPassword(int[] password)
        {
            for (var i = password.Length - 1; i >= 0; i--)
            {
                if(password[i] == 25)
                {
                    password[i] = 0;
                }
                else if (password[i] == 7 || password[i] == 10 || password[i] == 13)
                {
                    password[i] += 2;
                    break;
                }
                else
                {
                    password[i] += 1;
                    break;
                }
            }
        }

        private bool IsValidPassword(int[] password)
        {
            var hasStraightIncrease = false;
            var pairs = new HashSet<int>();

            for (var i = 0; i < password.Length; i++)
            {
                int c = password[i];

                if(i < password.Length - 1 && c == password[i+1]) pairs.Add(c);

                if(i < password.Length - 2 && c == (password[i+1] - 1) && c == (password[i+2] - 2)) hasStraightIncrease = true;
            }

            return hasStraightIncrease && pairs.Count >= 2;
        }
    }
}
