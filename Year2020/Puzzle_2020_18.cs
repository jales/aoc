using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2020
{
	public sealed class Puzzle_2020_18 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(6811433855019, 129770152447927);

            AddTest(@"1 + (2 * 3) + (4 * (5 + 6))", 51, 51);
        }

        private string[] Expressions { get; }

        public Puzzle_2020_18(string input)
        {
            Expressions = input.Lines();
        }

        protected override object Part1()
        {
            return Expressions.Sum(expression => Evaluate(expression, false, 0).value);
        }

        protected override object Part2()
        {
            return Expressions.Sum(expression => Evaluate(expression, true, 0).value);
        }

        private static (long value, int index) Evaluate(string expression, bool additionTakesPrecedence , int index)
        {
            var pendingValues = new List<long>();
            var currentOperation = ' ';

            var i = index;
            for (; i < expression.Length; i++)
            {
                switch (expression[i])
                {
                    case ' ':
                        break;

                    case '+':
                    case '*':
                        currentOperation = expression[i];
                        break;

                    case '(':
                        var (v, j) = Evaluate(expression, additionTakesPrecedence, i + 1);
                        if (currentOperation == '+')
                            pendingValues[^1] += v;
                        else if(currentOperation == '*' && !additionTakesPrecedence)
                            pendingValues[^1] *= v;
                        else
                            pendingValues.Add(v);
                        i = j;
                        break;

                    case ')':
                        return (pendingValues.Product(), i);

                    default:
                        if (currentOperation == '+')
                            pendingValues[^1] += expression[i] - '0';
                        else if(currentOperation == '*' && !additionTakesPrecedence)
                            pendingValues[^1] *= expression[i] - '0';
                        else
                            pendingValues.Add(expression[i] - '0');
                        currentOperation = ' ';
                        break;
                }
            }

            return (pendingValues.Product(), i);
        }

        public static string Test => @"1 + (2 * 3) + (4 * (5 + 6))";
    }
}