using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure.Services;

namespace AoC.Infrastructure.Leaderboards.Model
{
    internal sealed class SolvingTimes
    {
        private int _year;

        private readonly Dictionary<(string memberId, int day), (TimeSpan? part1, TimeSpan? part2, TimeSpan? delta)> _times = new();

        private readonly Dictionary<(int, int), TimeSpan> _fastestTimesPerDay = new();
        private readonly Dictionary<(int, int), TimeSpan> _slowestTimesPerDay = new();

        private readonly Dictionary<(string, int), TimeSpan> _fastestTimesPerMember = new();
        private readonly Dictionary<(string, int), TimeSpan> _slowestTimesPerMember = new();

        public void InitializeFrom(Leaderboard leaderboard)
        {
            _year = leaderboard.Year;

            foreach (var (memberId, member) in leaderboard.MembersById)
            foreach (var (day, starTimestamps) in member.StarsByDay)
            {
                var startOfDay = AdventOfCode.StartOfDay(leaderboard.Year, day);

                var part1 = starTimestamps?.Part1Timestamp - startOfDay;
                var part2 = starTimestamps?.Part2Timestamp - startOfDay;
                var delta = part2-part1;

                _times[(memberId, day)] = (part1, part2, delta);
            }

            var part1Values = _times.Where(kvp => kvp.Value.part1.HasValue).ToList();

            var part1ValuesByDay = part1Values.GroupBy(kvp => kvp.Key.day).ToList();
            _fastestTimesPerDay.Add(part1ValuesByDay.ToDictionary(g => (g.Key, 1), g => g.Min(x => x.Value.part1!.Value)));
            _slowestTimesPerDay.Add(part1ValuesByDay.ToDictionary(g => (g.Key, 1), g => g.Max(x => x.Value.part1!.Value)));

            var part1ValuesByMember = part1Values.GroupBy(kvp => kvp.Key.memberId).ToList();
            _fastestTimesPerMember.Add(part1ValuesByMember.ToDictionary(g => (g.Key, 1), g => g.Min(x => x.Value.part1!.Value)));
            _slowestTimesPerMember.Add(part1ValuesByMember.ToDictionary(g => (g.Key, 1), g => g.Max(x => x.Value.part1!.Value)));

            var part2Values = _times.Where(kvp => kvp.Value.part2.HasValue).ToList();

            var part2ValuesByDay = part2Values.GroupBy(kvp => kvp.Key.day).ToList();
            _fastestTimesPerDay.Add(part2ValuesByDay.ToDictionary(g => (g.Key, 2), g => g.Min(x => x.Value.part2!.Value)));
            _slowestTimesPerDay.Add(part2ValuesByDay.ToDictionary(g => (g.Key, 2), g => g.Max(x => x.Value.part2!.Value)));

            var part2ValuesByMember = part2Values.GroupBy(kvp => kvp.Key.memberId).ToList();
            _fastestTimesPerMember.Add(part2ValuesByMember.ToDictionary(g => (g.Key, 2), g => g.Min(x => x.Value.part2!.Value)));
            _slowestTimesPerMember.Add(part2ValuesByMember.ToDictionary(g => (g.Key, 2), g => g.Max(x => x.Value.part2!.Value)));

            var deltaValues = _times.Where(kvp => kvp.Value.delta.HasValue).ToList();

            var deltaValuesByDay = deltaValues.GroupBy(kvp => kvp.Key.day).ToList();
            _fastestTimesPerDay.Add(deltaValuesByDay.ToDictionary(g => (g.Key, 3), g => g.Min(x => x.Value.delta!.Value)));
            _slowestTimesPerDay.Add(deltaValuesByDay.ToDictionary(g => (g.Key, 3), g => g.Max(x => x.Value.delta!.Value)));

            var deltaValuesByMember = deltaValues.GroupBy(kvp => kvp.Key.memberId).ToList();
            _fastestTimesPerMember.Add(deltaValuesByMember.ToDictionary(g => (g.Key, 3), g => g.Min(x => x.Value.delta!.Value)));
            _slowestTimesPerMember.Add(deltaValuesByMember.ToDictionary(g => (g.Key, 3), g => g.Max(x => x.Value.delta!.Value)));
        }



        public TimeSpan? FastestPart1(int day) => _fastestTimesPerDay.GetValueOrDefault((day, 1));
        public TimeSpan? FastestPart2(int day) => _fastestTimesPerDay.GetValueOrDefault((day, 2));
        public TimeSpan? FastestDelta(int day) => _fastestTimesPerDay.GetValueOrDefault((day, 3));

        public TimeSpan? SlowestPart1(int day) => _slowestTimesPerDay.GetValueOrDefault((day, 1));
        public TimeSpan? SlowestPart2(int day) => _slowestTimesPerDay.GetValueOrDefault((day, 2));
        public TimeSpan? SlowestDelta(int day) => _slowestTimesPerDay.GetValueOrDefault((day, 3));

        public TimeSpan? FastestPart1(string memberId) => _fastestTimesPerMember.GetValueOrDefault((memberId, 1));
        public TimeSpan? FastestPart2(string memberId) => _fastestTimesPerMember.GetValueOrDefault((memberId, 2));
        public TimeSpan? FastestDelta(string memberId) => _fastestTimesPerMember.GetValueOrDefault((memberId, 3));

        public TimeSpan? SlowestPart1(string memberId) => _slowestTimesPerMember.GetValueOrDefault((memberId, 1));
        public TimeSpan? SlowestPart2(string memberId) => _slowestTimesPerMember.GetValueOrDefault((memberId, 2));
        public TimeSpan? SlowestDelta(string memberId) => _slowestTimesPerMember.GetValueOrDefault((memberId, 3));

        public (TimeSpan? part1, TimeSpan? part2, TimeSpan? delta) this[string memberId, int day] => _times.GetValueOrDefault((memberId, day), (null, null, null));

        public int LateSubmissions(string memberId)
        {
            var lateSubmissions = 0;
            foreach (var (_, (part1, part2, _)) in _times.Where(kvp => kvp.Key.memberId == memberId))
            {
                if (part1.HasValue && part1.Value.Days > 0) lateSubmissions++;
                if (part2.HasValue && part2.Value.Days > 0) lateSubmissions++;
            }

            return lateSubmissions;
        }

        public TimeSpan? AverageDelta(string memberId)
        {
            var deltaTicks = _times
               .Where(kvp => kvp.Key.memberId == memberId && kvp.Value.delta.HasValue)
               .Select(kvp => kvp.Value.delta!.Value.Ticks)
               .ToList();

            if (deltaTicks.Count == 0) return null;

            return new TimeSpan((long) deltaTicks.Average());
        }

        public int Part1FastestTimes(string memberId)
        {
            var count = 0;
            foreach (var (day, _) in AdventOfCode.GetDaysForYear(_year).Where(d => d.isActive))
            {
                if(!_times.TryGetValue((memberId, day), out var times)) continue;
                if (!_fastestTimesPerDay.TryGetValue((day, 1), out var dayFastest)) continue;

                if (dayFastest == times.part1) count++;
            }

            return count;
        }

        public int Part1SlowestTimes(string memberId)
        {
            var count = 0;
            foreach (var (day, _) in AdventOfCode.GetDaysForYear(_year).Where(d => d.isActive))
            {
                if(!_times.TryGetValue((memberId, day), out var times)) continue;
                if (!_fastestTimesPerDay.TryGetValue((day, 1), out var dayFastest)) continue;
                if (!_slowestTimesPerDay.TryGetValue((day, 1), out var daySlowest)) continue;

                if (dayFastest != times.part1 && daySlowest == times.part1) count++;
            }

            return count;
        }

        public int Part2FastestTimes(string memberId)
        {
            var count = 0;
            foreach (var (day, _) in AdventOfCode.GetDaysForYear(_year).Where(d => d.isActive))
            {
                if(!_times.TryGetValue((memberId, day), out var times)) continue;
                if (!_fastestTimesPerDay.TryGetValue((day, 2), out var dayFastest)) continue;

                if (dayFastest == times.part2) count++;
            }

            return count;
        }

        public int Part2SlowestTimes(string memberId)
        {
            var count = 0;
            foreach (var (day, _) in AdventOfCode.GetDaysForYear(_year).Where(d => d.isActive))
            {
                if(!_times.TryGetValue((memberId, day), out var times)) continue;
                if (!_fastestTimesPerDay.TryGetValue((day, 2), out var dayFastest)) continue;
                if (!_slowestTimesPerDay.TryGetValue((day, 2), out var daySlowest)) continue;

                if (dayFastest != times.part2 && daySlowest == times.part2) count++;
            }

            return count;
        }
    }
}