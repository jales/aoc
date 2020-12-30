using System;
using AoC.Infrastructure.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AoC.Infrastructure.Leaderboards.Model
{
    internal sealed class StarTimestamps
    {
        [JsonProperty("1")]
        [JsonConverter(typeof(StarConverter))]
        public DateTimeOffset? Part1Timestamp { get; set; }

        [JsonProperty("2")]
        [JsonConverter(typeof(StarConverter))]
        public DateTimeOffset? Part2Timestamp { get; set; }

        private class StarConverter : JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) { }

            public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
            {
                dynamic obj = JObject.Load(reader);
                var utc = DateTimeOffset.FromUnixTimeSeconds(long.Parse((string) obj.get_star_ts));
                return TimeZoneInfo.ConvertTime(utc, AdventOfCode.TimeZone);
            }

            public override bool CanConvert(Type objectType)
            {
                return true;
            }
        }
    }
}