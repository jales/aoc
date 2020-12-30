using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Leaderboards.Model;
using AoC.Infrastructure.Services;
using Spectre.Console;
using static AoC.Infrastructure.Services.Renderer;

namespace AoC.Infrastructure.Leaderboards
{
    internal static class Standings
    {
        public static void Render(Leaderboard leaderboard)
        {
            var rows = leaderboard.MembersByScore
               .Select(BuildStandingsRow);

            var table = BuildStandingsTable(leaderboard);
            table.AddRows(rows);

            RenderTable(table, " [ S T A N D I N G S ] ", Color.Red);
        }

        private static IEnumerable<string> BuildStandingsRow(Member member, int index)
        {
            // Rank
            yield return $"{index + 1})";

            // Score
            yield return member.LocalScore.ToString();

            // Stars per day
            yield return string.Join(" ", member.StarCountByDay.Select(count => count switch
            {
                2 => FullDay("*"),
                1 => HalfDay("*"),
                _ => " "
            }));

            // Total stars
            var totalStars = member.StarCountByDay.Sum();
            yield return FullDay($"{totalStars}*");

            // Name
            yield return MemberName(member.Name);
        }

        private static Table BuildStandingsTable(Leaderboard leaderboard)
        {
            return new Table()
               .SimpleBorder()
               .Centered()
               .Collapse()
                // Name
               .AddColumn("", c => c.RightAligned().NoWrap())
                // Score
               .AddColumn("", c => c.RightAligned().NoWrap())
                // Stars per day
               .AddColumn(GetDaysColumnHeader(), c => c.Width(51).NoWrap())
                // Total stars
               .AddColumn("", c => c.LeftAligned().NoWrap())
                // Name
               .AddColumn("", c => c.LeftAligned().NoWrap());

            string GetDaysColumnHeader()
            {
                var days = AdventOfCode.GetDaysForYear(leaderboard.Year)
                   .Select(x => new[]
                    {
                        x.isActive ? Day(x.day / 10) : Disabled(x.day / 10),
                        x.isActive ? Day(x.day % 10) : Disabled(x.day % 10)
                    })
                   .ToList();

                return string.Join(Environment.NewLine,
                    string.Join(" ", days.Select(d => d[0])),
                    string.Join(" ", days.Select(d => d[1])));
            }
        }
    }
}
