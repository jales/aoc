using Microsoft.Extensions.Configuration;

namespace AoC.Infrastructure.Services
{
    internal static class Configuration
    {
        private static IConfigurationRoot? Root { get; } = InitializeConfiguration();

        public static string Session => Root?["session"] ?? string.Empty;

        private static IConfigurationRoot? InitializeConfiguration() => new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", false)
           .AddJsonFile("appsettings.local.json", true)
           .Build();
    }
}
