using System.ComponentModel;
using System.Threading.Tasks;
using AoC.Infrastructure.Puzzles;
using Spectre.Cli;
using static AoC.Infrastructure.Benchmarks.Statistics;
using static AoC.Infrastructure.Puzzles.PuzzleBenchmarkRunner;
using static AoC.Infrastructure.Services.AdventOfCode;
using static AoC.Infrastructure.Puzzles.PuzzleRegistrar;
using static AoC.Infrastructure.Services.Renderer;
using static Spectre.Cli.ValidationResult;

namespace AoC.Infrastructure.Benchmarks
{
    public sealed class BenchmarkCommand : AsyncCommand<BenchmarkCommand.Settings>
    {
        public override ValidationResult Validate(CommandContext context, Settings settings)
        {
            var registration = GetPuzzle(settings.InputYear, settings.InputDay);

            if (registration is null)
                return Error("Could not find a suitable puzzle.");

            settings.Registration = registration;
            return Success();
        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            var registration = settings.Registration;

            await Status(
                "Downloading puzzle input...",
                async () => SetOfficialInput(registration.PuzzleType, await GetPuzzleInput(registration)));

            var summary = await Status("Running benchmarks...", async () => await Benchmark(registration));

            Render(registration, summary);

            return 0;
        }

        public static void Configure(IConfigurator configurator)
        {
            configurator
               .AddCommand<BenchmarkCommand>("benchmark")
               .WithDescription("Benchmarks an Advent of Code puzzle.")
               .WithExample(new[] {"benchmark", "2019", "10" });
        }

        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<YEAR>")]
            [Description("The year of the puzzle to benchmark. ")]
            public int InputYear { get; set; }

            [CommandArgument(1, "<DAY>")]
            [Description("The day of the puzzle to benchmark.")]
            public int InputDay { get; set; }

            public PuzzleRegistration Registration { get; set; } = null!;
        }
    }
}
