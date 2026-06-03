using AventStack.ExtentReports;
using MobileUITests.Drivers;
using MobileUITests.Models;
using MobileUITests.Pages;
using MobileUITests.Utils;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium.Appium.Android;
using System.Diagnostics;

namespace MobileUITests.Tests
{
    public abstract class BaseTest
    {
        protected AndroidDriver driver;
        protected CalculatorPage calculatorPage;
        protected AppiumSettings settings;
        protected ExtentReports extent;
        protected ExtentTest test;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Directory.CreateDirectory("Screenshots");
            Directory.CreateDirectory("Reports");

            // Load settings from appsettings.json
            settings = ConfigReader.GetAppiumSettings();

            // Disable animations (only once)
            RunAdb("shell settings put global window_animation_scale 0");
            RunAdb("shell settings put global transition_animation_scale 0");
            RunAdb("shell settings put global animator_duration_scale 0");

            // Start driver ONCE
            driver = AppiumDriverFactory.CreateAndroidDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);

            extent = ReportManager.GetReporter();
        }

        [SetUp]
        public void SetUp()
        {
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);

            // Add NUnit categories to ExtentReports
            var categories = TestContext.CurrentContext.Test.Properties["Category"];

            foreach (var category in categories)
            {
                test.AssignCategory(category.ToString());
                test.AssignAuthor(category.ToString());
            }

            try
            {
                //Activate app before each test
                driver.ActivateApp(settings.AppPackage);

                calculatorPage = new CalculatorPage(driver);
                // Ensure calculator is in a clean state before each test
                calculatorPage.TapClear();
            }
            catch (Exception ex)
            {
                TestContext.Out.WriteLine($"SetUp failed: {ex.Message}");
                throw;
            }
        }

        [TearDown]
        public void TearDown()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;

            if (status == TestStatus.Failed)
            {
                try
                {
                    var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    var screenshot = driver.GetScreenshot();
                    var filePath = $"Screenshots/{TestContext.CurrentContext.Test.Name}_{timestamp}.png";

                    screenshot.SaveAsFile(filePath);

                    TestContext.AddTestAttachment(filePath, "Screenshot on failure");
                    TestContext.Out.WriteLine($"Screenshot saved: {filePath}");

                    test.Fail("Test failed").AddScreenCaptureFromPath(filePath);
                }
                catch (Exception ex)
                {
                    TestContext.Out.WriteLine($"Failed to capture screenshot: {ex.Message}");
                }
            }
            else if (status == TestStatus.Passed)
            {
                test.Pass("Test passed");
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            extent.Flush();

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
