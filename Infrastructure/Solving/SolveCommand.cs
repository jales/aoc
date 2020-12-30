using System.ComponentModel;
using System.Threading.Tasks;
using AoC.Infrastructure.Puzzles;
using Spectre.Cli;
using TextCopy;
using static AoC.Infrastructure.Services.AdventOfCode;
using static AoC.Infrastructure.Puzzles.PuzzleRunner;
using static AoC.Infrastructure.Puzzles.PuzzleRegistrar;
using static AoC.Infrastructure.Services.Renderer;
using static Spectre.Cli.ValidationResult;

namespace AoC.Infrastructure.Solving
{
    public sealed class SolveCommand : AsyncCommand<SolveCommand.Settings>
    {
        public override ValidationResult Validate(CommandContext context, Settings settings)
        {
            if (settings.InputYear.HasValue && (settings.InputYear < FirstYear || settings.InputYear > CurrentYear))
                return Error($"{settings.InputYear} is an invalid year. Please choose an year between {FirstYear} and {CurrentYear}.");

            if (settings.InputDay.HasValue && (settings.InputDay < 1 || settings.InputDay > 25))
                return Error($"{settings.InputDay} is an invalid day. Please choose an day between 1 and 25.");

            var registration = (Year: settings.InputYear, Day: settings.InputDay) switch
            {
                (null, null) => GetLatestPuzzle(),
                ({ } year, null) => GetLatestPuzzleForYear(year),
                (null, { } day) => GetLatestPuzzleForDay(day),
                ({ } year, { } day) => GetPuzzle(year, day)
            };

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

            PuzzleHeader(registration.Year, registration.Day);

            var isCopacetic = true;
            foreach (var puzzleResult in Solve(registration))
            {
                RenderPuzzleResult(puzzleResult);

                if (isCopacetic && puzzleResult.ResultIsUnknown() && puzzleResult.IsOfficial())
                {
                    await ClipboardService.SetTextAsync($"{puzzleResult.Result}");
                    var response = await Status(
                        "Submitting puzzle solution...",
                        async () => await SubmitAnswer(registration, puzzleResult.IsPart1Result ? 1 : 2, puzzleResult.Result!));
                    RenderSubmissionResponse(response);
                }

                isCopacetic &= puzzleResult.ResultIsExpected();
            }

            PuzzleFooter();

            return 0;
        }


        public static void Configure(IConfigurator configurator)
        {
            configurator
               .AddCommand<SolveCommand>("solve")
               .WithDescription("Solves an Advent of Code puzzle.")
               .WithExample(new[] {"solve"})
               .WithExample(new[] {"solve", "-y", "2019"})
               .WithExample(new[] {"solve", "-d", "10"})
               .WithExample(new[] {"solve", "-y", "2019", "-d", "10"});
        }

        public class Settings : CommandSettings
        {
            [CommandOption("-y|--year <YEAR>")]
            [Description("The year of the puzzle to solve. If no year is specified, the command will use the latest available.")]
            public int? InputYear { get; set; }

            [CommandOption("-d|--day <DAY>")]
            [Description("The day of the puzzle to solve. If no day is specified, the command will use the latest available.")]
            public int? InputDay { get; set; }

            public PuzzleRegistration Registration { get; set; } = null!;
        }
    }
}
