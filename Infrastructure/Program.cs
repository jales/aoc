using System;
using System.Text;
using System.Threading.Tasks;
using AoC.Infrastructure.Leaderboards;
using AoC.Infrastructure.Persistency;
using AoC.Infrastructure.Solving;
using Spectre.Cli;

namespace AoC.Infrastructure
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            var app = new CommandApp<SolveCommand>();

            app.Configure(configurator =>
                {
                    configurator.SetApplicationName("aoc");
                    configurator.ValidateExamples();

                    LeaderboardCommand.Configure(configurator);
                    SolveCommand.Configure(configurator);
                    TestCommand.Configure(configurator);
                    CleanCommand.Configure(configurator);
                    ListCommand.Configure(configurator);
                });

            return await app.RunAsync(args);
        }
    }
}
