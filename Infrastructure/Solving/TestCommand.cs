using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using AoC.Infrastructure.Puzzles;
using Spectre.Cli;
using static AoC.Infrastructure.Services.AdventOfCode;
using static AoC.Infrastructure.Puzzles.PuzzleRunner;
using static AoC.Infrastructure.Puzzles.PuzzleRegistrar;
using static AoC.Infrastructure.Services.Renderer;
using static Spectre.Cli.ValidationResult;

namespace AoC.Infrastructure.Solving
{
    public sealed class TestCommand : AsyncCommand<TestCommand.Settings>
    {
        public override ValidationResult Validate(CommandContext context, Settings settings)
        {
            if (settings.InputYear.HasValue && (settings.InputYear < FirstYear || settings.InputYear > CurrentYear))
                return Error($"{settings.InputYear} is an invalid year. Please choose an year between {FirstYear} and {CurrentYear}.");

            var registrations = settings.InputYear.HasValue
                ? GetAllPuzzlesForYear(settings.InputYear.Value)
                : GetAllPuzzles();

            if (registrations.Count == 0)
                return Error("Could not find any puzzles to test.");

            settings.Registrations = registrations;
            return Success();
        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            var registrations = settings.Registrations;

            await Status(
                "Downloading puzzle(s) input...",
                async () =>
                {
                    foreach (var registration in registrations)
                    {
                        SetOfficialInput(registration.PuzzleType, await GetPuzzleInput(registration));
                    }
                });

            foreach (var registration in registrations)
            {
                PuzzleHeader(registration.Year, registration.Day);

                foreach (var puzzleResult in Solve(registration))
                {
                    RenderPuzzleResult(puzzleResult);

                }
            }

            PuzzleFooter();

            return 0;
        }

        public static void Configure(IConfigurator configurator)
        {
            configurator
               .AddCommand<TestCommand>("test")
               .WithDescription("Tests all Advent of Code puzzles.")
               .WithExample(new[] {"test"})
               .WithExample(new[] {"test", "-y", "2019"});
        }

        public class Settings : CommandSettings
        {
            [CommandOption("-y|--year <YEAR>")]
            [Description("The year of the puzzles to test. If no year is specified, the command will test all puzzles.")]
            public int? InputYear { get; set; }

            public List<PuzzleRegistration> Registrations { get; set; } = null!;
        }
    }
}