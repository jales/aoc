using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace AoC.Infrastructure.Leaderboards.Model
{
    internal sealed class Leaderboard
    {
        [JsonProperty("owner_id")]
        public string OwnerId { get; set; } = string.Empty;

        [JsonProperty("event")]
        public int Year { get; set; }

        [JsonProperty("members")]
        public Dictionary<string, Member> MembersById { get; set; } = new();

        [JsonIgnore]
        public SolvingTimes SolvingTimes { get; } = new();

        [JsonIgnore]
        public IEnumerable<Member> MembersByScore => MembersById.Values
               .OrderByDescending(m => m.LocalScore)
               .ThenByDescending(m => m.Name);

        public static Leaderboard Parse(string leaderboardContent)
        {
            var leaderboard = JsonConvert.DeserializeObject<Leaderboard>(leaderboardContent);
            leaderboard.SolvingTimes.InitializeFrom(leaderboard);
            return leaderboard;
        }

        public int MinimumScore(string memberId)
        {
            var member = MembersById[memberId];
            return member.LocalScore + (50 - member.TotalStars);
        }

        public int MaximumScore(string memberId)
        {
            var maxPointsPerPart = MembersById.Count;
            var member = MembersById[memberId];

            var score = member.LocalScore;
            foreach (var (day, (part1, part2, _)) in Enumerable.Range(1, 25).Select(day => (day, SolvingTimes[memberId, day])))
            {
                if (!part1.HasValue)
                    score += maxPointsPerPart - MembersById
                       .Count(kvp => kvp.Key != memberId && kvp.Value.StarsByDay.TryGetValue(day, out var stars) && stars.Part1Timestamp.HasValue);

                if (!part2.HasValue)
                    score += maxPointsPerPart - MembersById
                       .Count(kvp => kvp.Key != memberId && kvp.Value.StarsByDay.TryGetValue(day, out var stars) && stars.Part2Timestamp.HasValue);
            }

            return score;
        }
    }
}