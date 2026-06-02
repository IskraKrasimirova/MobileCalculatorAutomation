namespace MobileUITests.Models
{
    public class AppiumSettings
    {
        public string PlatformName { get; set; } = default!;
        public string AutomationName { get; set; } = default!;
        public string DeviceName { get; set; } = default!;
        public string PlatformVersion { get; set; } = default!;
        public string AppPackage { get; set; } = default!;
        public string AppActivity { get; set; } = default!;
        public string ServerUrl { get; set; } = default!;
        public bool NoReset { get; set; }
    }
}
