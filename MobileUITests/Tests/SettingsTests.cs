using MobileUITests.Drivers;

namespace MobileUITests.Tests
{
    public class SettingsTests : BaseTest
    {
        [Test]
        public void OpenSettingsTest()
        {
            Assert.That(driver, Is.Not.Null);
        }

        [Test]
        public void OpenCalculatorTest()
        {
            Assert.That(driver, Is.Not.Null);
        }
    }
}
