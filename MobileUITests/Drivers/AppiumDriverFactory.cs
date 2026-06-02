using MobileUITests.Utils;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace MobileUITests.Drivers
{
    public class AppiumDriverFactory
    {
        public static AndroidDriver CreateAndroidDriver()
        {
            var settings = ConfigReader.GetAppiumSettings();

            var options = new AppiumOptions();
            options.PlatformName = settings.PlatformName;
            options.AutomationName = settings.AutomationName;
            options.DeviceName = settings.DeviceName;
            options.PlatformVersion = settings.PlatformVersion;

            options.AddAdditionalAppiumOption("appPackage", settings.AppPackage);
            options.AddAdditionalAppiumOption("appActivity", settings.AppActivity);
            options.AddAdditionalAppiumOption("noReset", settings.NoReset);

            options.AddAdditionalAppiumOption("settings[waitForIdleTimeout]", 0);
            options.AddAdditionalAppiumOption("settings[ignoreUnimportantViews]", true);

            var serverUri = new Uri(settings.ServerUrl);

            return new AndroidDriver(serverUri, options);
        }
    }
}
