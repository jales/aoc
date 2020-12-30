using System.Threading.Tasks;
using Spectre.Cli;
using static AoC.Infrastructure.Services.AdventOfCode;
using static AoC.Infrastructure.Services.Renderer;

namespace AoC.Infrastructure.Persistency
{
    public sealed class CleanCommand : AsyncCommand
    {
        public override async Task<int> ExecuteAsync(CommandContext context)
        {
            var count = await Status(
                "Downloading puzzle input...",
                async () => await CleanCache());

            RenderCleanupResults(count);

            return 0;
        }


        public static void Configure(IConfigurator configurator)
        {
            configurator
               .AddCommand<CleanCommand>("clean")
               .WithDescription("Cleans the local Advent of Code puzzle input cache.")
               .WithExample(new[] {"clean"});
        }
    }
}
