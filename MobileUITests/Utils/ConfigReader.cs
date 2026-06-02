using Microsoft.Extensions.Configuration;
using MobileUITests.Models;

namespace MobileUITests.Utils
{
    public class ConfigReader
    {
        private static IConfigurationRoot _config;

        static ConfigReader()
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }

        public static AppiumSettings GetAppiumSettings()
        {
            var settings = new AppiumSettings();
            _config.GetSection("Appium").Bind(settings);
            return settings;
        }
    }
}
