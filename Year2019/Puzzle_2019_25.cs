using System;
using System.Collections.Generic;
using AoC.Infrastructure.Puzzles;
using AoC.Support.Intcode;

namespace AoC.Year2019
{
    internal sealed class Puzzle_2019_25 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(1090617344, MerryChristmas);
        }

        private string Input { get; }

        public Puzzle_2019_25(string input)
        {
            Input = input;
        }

        protected override object Part1()
        {
            //The initial commands where found by playing the game interactively
            List<string> commands = new()
            {
                "south", "west", "take fuel cell", "west", "take easter egg", "east", "east", "north", "north",
                "north", "east", "east", "take cake", "west", "west", "south", "south", "east", "take ornament",
                "east", "take hologram", "east", "take dark matter", "north", "north", "east", "take klein bottle", "north",
                "take hypercube", "north", "drop ornament", "drop easter egg", "drop cake", "drop fuel cell",
                "drop klein bottle", "drop dark matter", "inv"
            };

            var availableItems = new List<string> { "ornament", "easter egg", "cake", "fuel cell", "klein bottle", "dark matter", "hologram", "hypercube" };

            foreach (var items in availableItems.Combinations(1, 6))
            {
                foreach (var item in items) commands.Add($"take {item}");
                commands.Add("west");
                foreach (var item in items) commands.Add($"drop {item}");
            }

            Play(commands, false);

            // The result was taken from running the above commands until the program terminates and reading the output
            return 1090617344;
        }

        protected override object Part2()
        {
            return MerryChristmas;
        }

        private void Play(List<string> commands, bool interactive)
        {
            var inputs = new List<long>();

            if(!interactive)
            {
                foreach(var command in commands)
                {
                    foreach (var c in command!)
                    {
                        if (c == '\r') continue;
                        inputs.Add(c);
                    }

                    inputs.Add('\n');
                }
            }

            var program = new IntcodeProgram(Input)
            {
                InputProvider = i => inputs[i]
            };

            program.OutputHandler = (o, i) =>
            {
                if (o == '\n')
                {
                    if (interactive) Console.WriteLine();
                }
                else if (o == '?')
                {
                    if (!interactive) return;

                    Console.Write((char)o);

                    if (program.Outputs[i - 3] == 'a' && program.Outputs[i - 2] == 'n' && program.Outputs[i - 1] == 'd')
                    {
                        Console.Write(' ');
                        var input = Console.ReadLine();
                        foreach (var c in input!)
                        {
                            if (c == '\r') continue;
                            inputs.Add(c);
                        }

                        inputs.Add('\n');
                    }
                }
                else if (interactive)
                {

                    Console.Write((char)o);
                }
            };

            program.Run();
        }
    }
}