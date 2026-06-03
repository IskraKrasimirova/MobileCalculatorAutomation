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
                Directory.CreateDirectory("Reports");

                var spark = new ExtentSparkReporter("Reports/TestReport.html");

                _extent = new ExtentReports();
                _extent.AttachReporter(spark);
            }

            return _extent;
        }
    }
}
