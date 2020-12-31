using System;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2015
{
    public sealed class Puzzle_2015_05 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(238, 69);
        }

        private string[] Strings { get; }

        public Puzzle_2015_05(string input)
        {
            Strings = input.Lines();
        }

        protected override object Part1()
        {
            return Strings.Where(IsNicePart1).Count();
        }

        protected override object Part2()
        {
            return Strings.Where(IsNicePart2).Count();
        }

        private static bool IsNicePart1(string str)
        {
            var vowels = 0;
            var hasRepeatedLetters = false;
            var hasInvalidSubstring = false;

            var previousCharacter = str[0];

            if (previousCharacter.IsVowel()) vowels++;

            for (var i = 1; i < str.Length; i++)
            {
                var currentCharacter = str[i];
                if (currentCharacter.IsVowel()) vowels++;

                if (previousCharacter == currentCharacter) hasRepeatedLetters = true;

                if (previousCharacter == 'a' && currentCharacter == 'b') hasInvalidSubstring = true;
                if (previousCharacter == 'c' && currentCharacter == 'd') hasInvalidSubstring = true;
                if (previousCharacter == 'p' && currentCharacter == 'q') hasInvalidSubstring = true;
                if (previousCharacter == 'x' && currentCharacter == 'y') hasInvalidSubstring = true;

                previousCharacter = currentCharacter;
            }

            return vowels >= 3 && hasRepeatedLetters && !hasInvalidSubstring;
        }

        private static bool IsNicePart2(string str)
        {
            var rule1 = false;
            var rule2 = false;

            for (var i = 0; i < str.Length-2; i++)
            {
                for (var j = i+2; j < str.Length-1; j++)
                {
                    if (str[i] == str[j] && str[i+1] == str[j+1])
                    {
                        rule1 = true;
                        break;
                    }
                }

                if (str[i] == str[i+2]) rule2 = true;
            }

            return rule1 && rule2;
        }
    }
}
