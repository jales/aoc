using System.Collections.Generic;
using Spectre.Cli;
using static AoC.Infrastructure.Puzzles.PuzzleRegistrar;

namespace AoC.Infrastructure.Solving
{
    public sealed class ListCommand : Command
    {
        public override int Execute(CommandContext context)
        {
            var solutions = new Dictionary<(int year, int day), int>();

            foreach (var (puzzleType, year, day, configurePuzzle, _) in GetAllPuzzles())
            {
                configurePuzzle();

                var (_, _, _, part1Solution, part2Solution) = GetOfficialRunForPuzzle(puzzleType);

                solutions[(year, day)] = (part1Solution, part2Solution) switch
                {
                    ( { },  { }) => 2,
                    ( { }, null) => 1,
                    (null,  { }) => 1,
                    _            => 0
                };
            }

            Solutions.Render(solutions);

            return 0;
        }

        public static void Configure(IConfigurator configurator)
        {
            configurator
               .AddCommand<ListCommand>("list")
               .WithDescription("List all Advent of Code puzzles completion status.")
               .WithExample(new[] {"list"});
        }
    }
}