using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2015
{
    internal sealed class Puzzle_2015_07 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(3176, 14710);
        }

        private Dictionary<string, string> Instructions { get; }

        public Puzzle_2015_07(string input)
        {
            Instructions = input
               .ParseLines(ParseInstruction)
               .ToDictionary(i => i.target, i => i.instructions);
        }

        protected override object Part1()
        {
            return SolveFor("a", new Dictionary<string, int>());

        }

        protected override object Part2()
        {
            return SolveFor("a", new Dictionary<string, int>{ ["b"] = 3176 });
        }

        private int SolveFor(string target, Dictionary<string, int> cache)
        {
            if (int.TryParse(target, out var value)) return value;

            if (cache.TryGetValue(target, out var cachedValue)) return cachedValue;

            var calculatedValue = 0;
            var instructionParts = Instructions[target].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (instructionParts.Length == 3)
            {
                if (instructionParts[1] == "OR")     calculatedValue = SolveFor(instructionParts[0], cache) |  SolveFor(instructionParts[2], cache);
                if (instructionParts[1] == "AND")    calculatedValue = SolveFor(instructionParts[0], cache) &  SolveFor(instructionParts[2], cache);
                if (instructionParts[1] == "LSHIFT") calculatedValue = SolveFor(instructionParts[0], cache) << SolveFor(instructionParts[2], cache);
                if (instructionParts[1] == "RSHIFT") calculatedValue = SolveFor(instructionParts[0], cache) >> SolveFor(instructionParts[2], cache);
            }
            else if (instructionParts.Length == 2)
            {
                if (instructionParts[0] == "NOT")    calculatedValue = ~SolveFor(instructionParts[1], cache);
            }
            else
            {
                calculatedValue = SolveFor(instructionParts[0], cache);
            }

            cache[target] = calculatedValue;

            return calculatedValue;
        }

        private static (string target, string instructions) ParseInstruction(string instruction)
        {
            var parts = instruction.Split(" -> ", StringSplitOptions.RemoveEmptyEntries);

            return (parts[1], parts[0]);
        }
    }
}