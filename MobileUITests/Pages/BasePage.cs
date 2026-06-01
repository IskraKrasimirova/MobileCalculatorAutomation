using OpenQA.Selenium.Appium.Android;

namespace MobileUITests.Pages
{
    public class BasePage
    {
        protected AndroidDriver _driver;

        protected BasePage(AndroidDriver driver)
        {
            _driver = driver;
        }
    }
}
