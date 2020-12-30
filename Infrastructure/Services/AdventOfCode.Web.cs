using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AoC.Infrastructure.Puzzles;
using HtmlAgilityPack;

namespace AoC.Infrastructure.Services
{
    internal static partial class AdventOfCode
    {
        private static HttpClient HttpClient { get; } = InitializeClient();

        private static HttpClient InitializeClient()
        {
            var baseAddress = new Uri("https://adventofcode.com");

            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseAddress, new Cookie("session", Configuration.Session));

            var handler = new HttpClientHandler { CookieContainer = cookieContainer };

            return new HttpClient(handler, true) { BaseAddress = baseAddress };
        }

        public static async Task<string> DownloadLeaderboard(string leaderboardId, int year)
        {
            return await HttpClient.GetStringAsync($"/{year}/leaderboard/private/view/{leaderboardId}.json");
        }

        private static async Task<string> DownloadPuzzleInput(PuzzleRegistration registration)
        {
            return await HttpClient.GetStringAsync($"/{registration.Year}/day/{registration.Day}/input");
        }

        public static async Task<string> SubmitAnswer(PuzzleRegistration registration, int part, object solution)
        {
            var content = new Dictionary<string, string>
            {
                ["level"] = $"{part}",
                ["answer"] = $"{solution}",
            };

            var response = await HttpClient.PostAsync($"/{registration.Year}/day/{registration.Day}/answer", new FormUrlEncodedContent(content!));

            if (response.IsSuccessStatusCode)
            {
                var responseContent = new HtmlDocument();
                responseContent.LoadHtml(await response.Content.ReadAsStringAsync());
                return string.Join(Environment.NewLine, responseContent.DocumentNode.SelectNodes("/html/body/main/article/p").Select(p => p.InnerText));
            }

            return $"HTTP error: {response.StatusCode}";
        }

    }
}
