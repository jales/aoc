using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2015
{
    public sealed class Puzzle_2015_19 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(535, 212);
        }

        private string StartingMolecule { get; }
        private ILookup<string, string> Replacements { get; }
        private List<(string, string)> ReverseReplacementsByLength { get; }

        public Puzzle_2015_19(string input)
        {
            var lines = input.Lines();

            var replacements = lines[..^1]
               .ParseArray(line => line.Matches("[a-zA-Z]+").Parse(matches => (matches[0], matches[1])));

            Replacements = replacements.ToLookup(t => t.Item1, t => t.Item2);

            ReverseReplacementsByLength = replacements.OrderByDescending(t => t.Item2.Length).ToList();

            StartingMolecule = lines[^1];
        }

        protected override object Part1()
        {
            var generatedMolecules = new HashSet<string>();

            foreach (var replacementsByAtom in Replacements)
            {
                var matches = Regex.Matches(StartingMolecule, replacementsByAtom.Key);

                foreach (Match match in matches)
                foreach (var replacement in replacementsByAtom)
                {
                    var newMolecule = StartingMolecule
                       .Remove(match.Index, match.Length)
                       .Insert(match.Index, replacement);

                    generatedMolecules.Add(newMolecule);
                }
            }

            return generatedMolecules.Count;
        }

        protected override object Part2()
        {
            var startingMolecule = StartingMolecule;

            var steps = 0;
            while (startingMolecule != "e")
            {
                foreach (var (atom, replacement) in ReverseReplacementsByLength)
                {
                    var match = Regex.Match(startingMolecule, replacement);
                    if (match.Success)
                    {
                        startingMolecule = startingMolecule
                           .Remove(match.Index, match.Length)
                           .Insert(match.Index, atom);
                        break;
                    }
                }
                steps++;
            }

            return steps;
        }
    }
}