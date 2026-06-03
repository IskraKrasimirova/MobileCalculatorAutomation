using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using MobileUITests.Models;

namespace MobileUITests.Utils
{
    public static class ReportManager
    {
        private static ExtentReports? _extent;

        public static ExtentReports GetReporter(AppiumSettings settings)
        {
            if (_extent == null)
            {
                var reportDir = Path.Combine(AppContext.BaseDirectory, "Reports");
                Directory.CreateDirectory(reportDir);

                var reportPath = Path.Combine(reportDir, "TestReport.html");
                var spark = new ExtentSparkReporter(reportPath);

                _extent = new ExtentReports();
                _extent.AttachReporter(spark);

                _extent.AddSystemInfo("Device Name", settings.DeviceName);
                _extent.AddSystemInfo("Platform Version", settings.PlatformVersion);
                _extent.AddSystemInfo("App Package", settings.AppPackage);
                _extent.AddSystemInfo("Automation", settings.AutomationName);
            }

            return _extent;
        }
    }
}
