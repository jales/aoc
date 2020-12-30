using System.ComponentModel;
using System.Threading.Tasks;
using AoC.Infrastructure.Leaderboards.Model;
using AoC.Infrastructure.Services;
using Spectre.Cli;
using static AoC.Infrastructure.Services.AdventOfCode;
using static Spectre.Cli.ValidationResult;

namespace AoC.Infrastructure.Leaderboards
{
    public sealed class LeaderboardCommand : AsyncCommand<LeaderboardCommand.Settings>
    {
        public override ValidationResult Validate(CommandContext context, Settings settings)
        {
            if (settings.InputYear.HasValue && (settings.InputYear < FirstYear || settings.InputYear > CurrentYear))
                return Error($"{settings.InputYear} is an invalid year. Please choose an year between {FirstYear} and {CurrentYear}.");

            settings.Year = settings.InputYear ?? CurrentYear;

            return Success();
        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            var leaderboardContent = await Renderer.Status("Downloading leaderboard...",  async () => await DownloadLeaderboard(settings.Id, settings.Year));
            var leaderboard = Leaderboard.Parse(leaderboardContent);

            Standings.Render(leaderboard);

            Timings.Render(leaderboard);

            Statistics.Render(leaderboard);

            return 0;
        }

        public static void Configure(IConfigurator configurator)
        {
            configurator
               .AddCommand<LeaderboardCommand>("leaderboard")
               .WithDescription("Displays the data from an Advent of Code leaderboard.")
               .WithExample(new[] {"leaderboard", "123456"})
               .WithExample(new[] {"leaderboard", "123456", "-y", "2019"});
        }

        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<LEADERBOARD>")]
            [Description("The id of the leaderboard to display.")]
            public string Id { get; set; } = null!;

            [CommandOption("-y|--year <YEAR>")]
            [Description("The year to display the leaderboard results for. If no year is specified, the command will use the latest available data.")]
            public int? InputYear { get; set; }

            internal int Year { get; set; }
        }
    }
}
