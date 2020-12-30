using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace AoC.Infrastructure.Leaderboards.Model
{
    internal sealed class Member
    {
        [JsonProperty("Id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("stars")]
        public int TotalStars { get; set; }

        [JsonProperty("local_score")]
        public int LocalScore { get; set; }

        [JsonProperty("completion_day_level")]
        public Dictionary<int, StarTimestamps> StarsByDay { get; set; } = new();

        public IEnumerable<int> StarCountByDay => Enumerable.Range(1, 25)
           .Select(day => StarsByDay.GetValueOrDefault(day) switch
            {
                {Part1Timestamp: { }, Part2Timestamp: { }} => 2,
                {Part1Timestamp: { }} => 1,
                {Part2Timestamp: { }} => 1,
                _ => 0
            });

        [JsonIgnore]
        public int Part1Stars => StarsByDay.Count(kvp => kvp.Value.Part1Timestamp.HasValue);

        [JsonIgnore]
        public int Part2Stars => StarsByDay.Count(kvp => kvp.Value.Part2Timestamp.HasValue);

        [JsonIgnore]
        public DateTimeOffset? LastSubmission => StarsByDay
           .SelectMany(kvp => new[] {kvp.Value.Part1Timestamp, kvp.Value.Part2Timestamp})
           .Where(d => d.HasValue)
           .OrderByDescending(d => d)
           .FirstOrDefault();
    }
}