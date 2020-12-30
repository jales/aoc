using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Leaderboards.Model;
using AoC.Infrastructure.Services;
using Spectre.Console;
using static AoC.Infrastructure.Services.Renderer;

namespace AoC.Infrastructure.Leaderboards
{
    internal static class Timings
    {
        public static void Render(Leaderboard leaderboard)
        {
            var rows = AdventOfCode.GetDaysForYear(leaderboard.Year)
               .Where(x => x.isActive)
               .Reverse()
               .Select(x => BuildTimingsRow(x.day, leaderboard));

            var table = BuildTimingsTable(leaderboard);
            table.AddRows(rows);

            RenderTable(table, " [ T I M I N G S ] ", Color.Blue);
        }

        private static IEnumerable<string> BuildTimingsRow(int day, Leaderboard leaderboard)
        {
            // Day
            yield return Day(day);

            // Members
            var dayFastestPart1 = leaderboard.SolvingTimes.FastestPart1(day);
            var dayFastestPart2 = leaderboard.SolvingTimes.FastestPart2(day);
            var dayFastestDelta = leaderboard.SolvingTimes.FastestDelta(day);

            var daySlowestPart1 = leaderboard.SolvingTimes.SlowestPart1(day);
            var daySlowestPart2 = leaderboard.SolvingTimes.SlowestPart2(day);
            var daySlowestDelta = leaderboard.SolvingTimes.SlowestDelta(day);

            foreach (var member in leaderboard.MembersByScore)
            {
                var part1Size = RenderedLength(leaderboard.SolvingTimes.SlowestPart1(member.Id));
                var part2Size = RenderedLength(leaderboard.SolvingTimes.SlowestPart2(member.Id));
                var deltaSize = RenderedLength(leaderboard.SolvingTimes.SlowestDelta(member.Id));

                var (part1, part2, delta) = leaderboard.SolvingTimes[member.Id, day];

                yield return string.Join(" | ",
                    MemberTime(part1, dayFastestPart1, daySlowestPart1, part1Size),
                    MemberTime(part2, dayFastestPart2, daySlowestPart2, part2Size),
                    MemberDelta(delta, dayFastestDelta, daySlowestDelta, deltaSize));

            }

            // Day
            yield return Day(day);
        }

        private static Table BuildTimingsTable(Leaderboard leaderboard)
        {
            return new Table()
               .MinimalBorder()
               .Centered()
               .Collapse()
                // Day
               .AddColumn("", c => c.RightAligned().NoWrap())
                // Members
               .AddColumns(leaderboard.MembersByScore.Select(m => MemberName(m.Name)), c => c.Centered().NoWrap().Padding(2, 0))
                // Day
               .AddColumn("", c => c.RightAligned().NoWrap());
        }
    }
}