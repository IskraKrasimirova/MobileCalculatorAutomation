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
        protected AndroidDriver _driver;
        protected CalculatorPage _calculatorPage;
        protected AppiumSettings _settings;
        protected ExtentReports _extent;
        protected ExtentTest _test;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Directory.CreateDirectory("Screenshots");
            Directory.CreateDirectory("Reports");

            // Load settings from appsettings.json
            _settings = ConfigReader.GetAppiumSettings();

            // Disable animations (only once)
            RunAdb("shell settings put global window_animation_scale 0");
            RunAdb("shell settings put global transition_animation_scale 0");
            RunAdb("shell settings put global animator_duration_scale 0");

            // Start driver ONCE
            _driver = AppiumDriverFactory.CreateAndroidDriver();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);

            _extent = ReportManager.GetReporter(_settings);
        }

        [SetUp]
        public void SetUp()
        {
            _test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);

            // Add NUnit categories to ExtentReports
            AddCategoriesToReport();

            try
            {
                //Activate app before each test
                _driver.ActivateApp(_settings.AppPackage);

                _calculatorPage = new CalculatorPage(_driver);
                // Ensure calculator is in a clean state before each test
                _calculatorPage.TapClear();
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
                    var screenshot = _driver.GetScreenshot();
                    var filePath = $"Screenshots/{TestContext.CurrentContext.Test.Name}_{timestamp}.png";

                    screenshot.SaveAsFile(filePath);

                    TestContext.AddTestAttachment(filePath, "Screenshot on failure");
                    TestContext.Out.WriteLine($"Screenshot saved: {filePath}");

                    _test.Fail("Test failed").AddScreenCaptureFromPath(filePath);
                }
                catch (Exception ex)
                {
                    TestContext.Out.WriteLine($"Failed to capture screenshot: {ex.Message}");
                }
            }
            else if (status == TestStatus.Passed)
            {
                _test.Pass("Test passed");
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _extent.Flush();

            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
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

        private void AddCategoriesToReport()
        {
            var allCategories = new HashSet<string>();

            // 1) Categories from class
            var classCategories = GetType()
                .GetCustomAttributes(typeof(CategoryAttribute), true)
                .Cast<CategoryAttribute>()
                .Select(c => c.Name);

            allCategories.UnionWith(classCategories);

            // 2) Categories from method
            var methodName = TestContext.CurrentContext.Test.MethodName;
            var method = methodName != null ? GetType().GetMethod(methodName) : null;

            if (method != null)
            {
                var methodCategories = method
                    .GetCustomAttributes(typeof(CategoryAttribute), true)
                    .Cast<CategoryAttribute>()
                    .Select(c => c.Name);

                allCategories.UnionWith(methodCategories);
            }

            // 3) Categories from NUnit (TestCaseData)
            var nunitCategories = TestContext.CurrentContext.Test.Properties["Category"]
                .Cast<object>()
                .Select(c => c?.ToString())
                .Where(c => c is not null)!;

            allCategories.UnionWith(nunitCategories!);


            // Add to ExtentReports
            foreach (var category in allCategories)
            {
                _test.AssignCategory(category);
                _test.Info($"Category: {category}");
            }

            _test.Info($"Found {allCategories.Count} categories");
        }
    }
}
