using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace MobileUITests.Utils
{
    public static class ReportManager
    {
        private static ExtentReports? _extent;

        public static ExtentReports GetReporter()
        {
            if (_extent == null)
            {
                var reportDir = Path.Combine(AppContext.BaseDirectory, "Reports");
                Directory.CreateDirectory(reportDir);

                var reportPath = Path.Combine(reportDir, "TestReport.html");
                var spark = new ExtentSparkReporter(reportPath);

                _extent = new ExtentReports();
                _extent.AttachReporter(spark);
            }

            return _extent;
        }
    }
}
