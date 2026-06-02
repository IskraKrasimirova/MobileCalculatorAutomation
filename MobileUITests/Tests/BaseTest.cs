using MobileUITests.Drivers;
using MobileUITests.Models;
using MobileUITests.Pages;
using MobileUITests.Utils;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium.Appium.Android;
using System.Diagnostics;

namespace MobileUITests.Tests
{
    public class BaseTest
    {
        protected AndroidDriver driver;
        protected CalculatorPage calculatorPage;
        protected AppiumSettings settings;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Load settings from appsettings.json
            settings = ConfigReader.GetAppiumSettings();

            // Disable animations (only once)
            RunAdb("shell settings put global window_animation_scale 0");
            RunAdb("shell settings put global transition_animation_scale 0");
            RunAdb("shell settings put global animator_duration_scale 0");

            // Start driver ONCE
            driver = AppiumDriverFactory.CreateAndroidDriver();
        }

        [SetUp]
        public void SetUp()
        {
            //Activate app before each test
            driver.ActivateApp(settings.AppPackage);

            calculatorPage = new CalculatorPage(driver);
            // Ensure calculator is in a clean state before each test
            calculatorPage.TapClear(); 
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                try
                {
                    Directory.CreateDirectory("Screenshots");

                    var screenshot = driver.GetScreenshot();
                    var filePath = $"Screenshots/{TestContext.CurrentContext.Test.Name}.png";

                    screenshot.SaveAsFile(filePath);

                    TestContext.AddTestAttachment(filePath, "Screenshot on failure");
                    TestContext.Out.WriteLine($"Screenshot saved: {filePath}");
                }
                catch (Exception ex)
                {
                    TestContext.Out.WriteLine($"Failed to capture screenshot: {ex.Message}");
                }
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }

        private static void RunAdb(string command)
        {
            var process = new Process();
            process.StartInfo.FileName = "adb";
            process.StartInfo.Arguments = command;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();
        }
    }
}
