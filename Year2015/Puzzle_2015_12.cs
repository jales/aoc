using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2015
{
    internal sealed class Puzzle_2015_12 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(191164, 87842);
        }

        private char[] Chars { get; }

        public Puzzle_2015_12(string input)
        {
            Chars = input.ToArray();
        }

        protected override object Part1()
        {
            Span<char> digits = stackalloc char[100];
            var index = 0;
            int number;
            var sum = 0;
            foreach (var c in Chars)
            {
                if (char.IsDigit(c) || c == '-')
                {
                    digits[index++] = c;
                }
                else if (index > 0)
                {
                    if (int.TryParse(digits[..index], out number))
                    {
                        sum += number;
                    }

                    index = 0;
                }
            }

            if(index > 0 && int.TryParse(digits[..index], out number))
            {
                sum += number;
            }

            return sum;
        }

        protected override object Part2()
        {
            Span<char> digits = stackalloc char[100];
            var index = 0;
            int number;
            var itemNumbers = new List<int>();
            var itemHasRed = false;
            var context = new Stack<(List<int>, bool)>();

            for (var i = 0; i < Chars.Length; i++)
            {
                var c = Chars[i];
                if (c == '[' || c == '{')
                {
                    context.Push((itemNumbers, itemHasRed));
                    index = 0;
                    itemNumbers = new List<int>();
                    itemHasRed = false;
                }
                else if (c == ']')
                {
                    if (index > 0 && int.TryParse(digits[..index], out number))
                    {
                        itemNumbers.Add(number);
                    }

                    var sum = itemNumbers.Sum();
                    (itemNumbers, itemHasRed) = context.Pop();
                    itemNumbers.Add(sum);
                    index = 0;
                }
                else if (c == '}')
                {
                    if (index > 0 && int.TryParse(digits[..index], out number))
                    {
                        itemNumbers.Add(number);
                    }
                    var sum = itemHasRed ? 0 : itemNumbers.Sum();
                    (itemNumbers, itemHasRed) = context.Pop();
                    itemNumbers.Add(sum);
                    index = 0;
                }
                else if (c == '"' && i > 4)
                {
                    var span = Chars[(i-5)..(i+1)];
                    itemHasRed |= MemoryExtensions.Equals(":\"red\"", span, StringComparison.OrdinalIgnoreCase);
                }
                else if (char.IsDigit(c) || c == '-')
                {
                    digits[index++] = c;
                }
                else if (index > 0)
                {
                    if (int.TryParse(digits[..index], out number))
                    {
                        itemNumbers.Add(number);
                    }

                    index = 0;
                }
            }

            if(index > 0 && int.TryParse(digits[..index], out number))
            {
                itemNumbers.Add(number);
            }

            return itemNumbers.Sum();
        }
    }
}
